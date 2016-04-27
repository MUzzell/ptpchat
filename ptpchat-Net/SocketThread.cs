namespace PtpChat.Net
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    using PtpChat.Base.Interfaces;

    public class SocketThread
    {
        private const string LogArgumentException = "Argument Exception recieved whilst parsing recieved data: {0}";
        private const string LogSocketException = "Socket Exception recieved whilst operating on socket: {0}";

        private readonly ILogManager logger;
        private readonly IMessageHandler messageHandler;

        private volatile bool running = true;

        public TcpListener listener { get; }

        private TcpClient Socket { get; set; }
        private Socket listenerSocket { get; set; }

        public SocketThread(IPEndPoint destination, IPEndPoint local, TcpListener list, IMessageHandler messageHandler, ILogManager logger)
        {
            this.messageHandler = messageHandler;
            this.logger = logger;

            this.listener = list;

            this.Socket = new TcpClient { ExclusiveAddressUse = false };
            this.Socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            this.Socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            this.Socket.Connect(destination.Address.ToString(), destination.Port);

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenerSocket.Bind(local);
        }

        //public static ManualResetEvent allDone = new ManualResetEvent(false);

        public async void Listen()
        {
            this.logger.Info($"New SocketThread listening on endpoint: {this.listener.LocalEndpoint}");
            //this.listener.Start();

            listenerSocket.Listen(100);

            try
            {
                while (this.running)
                {
                    try
                    {
                        var messageLengthBytes = new byte[4];

                        //var tcpClient = await this.listener.AcceptTcpClientAsync();

                        ////SslStream sslStream = new SslStream(tcpClient.GetStream(), false);

                        //================================================

                        //var recieveArgs = new SocketAsyncEventArgs();
                        //recieveArgs.SetBuffer(messageLengthBytes, 0, 4);//Receive bytes from x to total - x, x is the number of bytes already recieved

                        //this.Socket.Client.ReceiveAsync(recieveArgs);

                        //messageLengthBytes = messageLengthBytes.Reverse().ToArray();
                        //int messageLength = BitConverter.ToInt32(messageLengthBytes, 0);


                        //var messageData = new byte[messageLength];

                        //recieveArgs = new SocketAsyncEventArgs();
                        //recieveArgs.SetBuffer(messageData, 4, messageLength);//Receive bytes from x to total - x, x is the number of bytes already recieved
                        //this.Socket.Client.ReceiveAsync(recieveArgs);

                        //==================================================

                        var stream = this.Socket.GetStream();
                        await stream.ReadAsync(messageLengthBytes, 0, 4);

                        messageLengthBytes = messageLengthBytes.Reverse().ToArray();
                        int messageLength = BitConverter.ToInt32(messageLengthBytes, 0);

                        var messageData = new byte[messageLength];

                        //var messageData = new byte[BitConverter.ToInt32(messageLength, 0)];
                        await stream.ReadAsync(messageData, 0, messageData.Length);

                        //==================================================


                        var message = Encoding.ASCII.GetString(messageData);

                        this.logger.Debug($"endpoint:{this.listener.LocalEndpoint} > Incoming message ");

                        this.messageHandler.HandleMessage(message, (IPEndPoint)this.Socket.Client.RemoteEndPoint);
                    }
                    catch (ArgumentException ae)
                    {
                        this.logger.Warning(string.Format(LogArgumentException, ae.Message));
                    }
                }
            }
            catch (SocketException se)
            {
                this.logger.Error(string.Format(LogArgumentException, se.Message));
            }
            finally
            {
                this.listener.Stop();
            }
        }

        //private void AcceptCallback(IAsyncResult ar)
        //{
        //    // Signal the main thread to continue.
        //    allDone.Set();

        //    // Get the socket that handles the client request.
        //    Socket listener = (Socket)ar.AsyncState;
        //    Socket handler = listener.EndAccept(ar);

        //    byte[] messageSize = new byte[4];


        //    var recieveArgs = new SocketAsyncEventArgs();

        //    recieveArgs.SetBuffer(messageSize, 0,4);//Receive bytes from x to total - x, x is the number of bytes already recieved

        //    handler.ReceiveAsync(recieveArgs);



        //    //byte[] finishedMessage = new byte[messageSize.Length + msg.Length];

        //    //Buffer.BlockCopy(messageSize, 0, finishedMessage, 0, messageSize.Length);
        //    //Buffer.BlockCopy(msg, 0, finishedMessage, messageSize.Length, msg.Length);

            
        //}

        public void Send(IPEndPoint dst, byte[] msg)
        {

            //reverse the array because reasons that are important and not to be underestimated
            byte[] messageSize = BitConverter.GetBytes(msg.Length).Reverse().ToArray();

            byte[] finishedMessage = new byte[messageSize.Length + msg.Length];

            Buffer.BlockCopy(messageSize, 0, finishedMessage, 0, messageSize.Length);
            Buffer.BlockCopy(msg, 0, finishedMessage, messageSize.Length, msg.Length);

            var sendArgs = new SocketAsyncEventArgs { RemoteEndPoint = dst };
            sendArgs.SetBuffer(finishedMessage, 0, finishedMessage.Length);

            this.Socket.Client.SendAsync(sendArgs);

            //this.Socket.SendAsync(msg, msg.Length, dst.Address.ToString(), dst.Port);
        }

        public void Stop() => this.running = false;
    }
}