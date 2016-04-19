namespace PtpChat.Net
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using PtpChat.Base.Interfaces;

    public class SocketThread
    {
        private const string LogArgumentException = "Argument Exception recieved whilst parsing recieved data: {0}";
        private const string LogSocketException = "Socket Exception recieved whilst operating on socket: {0}";

        private readonly ILogManager logger;
        private readonly IMessageHandler messageHandler;

        private volatile bool running = true;

        public TcpListener listener { get; }

        private UdpClient Socket { get; }

        private TcpClient Socket_tcp { get; set; }

        public SocketThread(UdpClient socket, TcpListener list, IMessageHandler messageHandler, ILogManager logger)
        {
            this.Socket = socket;
            this.messageHandler = messageHandler;
            this.logger = logger;

            this.listener = list;
        }

        public SocketThread(IPEndPoint local, IMessageHandler messageHandler, ILogManager logger)
        {
            this.Socket = new UdpClient(local);
            this.messageHandler = messageHandler;
            this.logger = logger;
        }

        public async void Listen()
        {
            this.logger.Info($"New SocketThread listening on endpoint: {this.Socket.Client.LocalEndPoint}");
            this.listener.Start();

            try
            {
                while (this.running)
                {
                    try
                    {
                        if (this.listener.Pending())
                        {
                            //return or queue
                        }

                        // Create a TCP socket. 
                        // If you ran this server on the desktop, you could use 
                        // Socket socket = tcpListener.AcceptSocket() 
                        // for greater flexibility.
                        var tcpClient = await this.listener.AcceptTcpClientAsync();

                        //SslStream sslStream = new SslStream(tcpClient.GetStream(), false);

                        var stream = tcpClient.GetStream();

                        var messageLength = new byte[4];
                        await stream.ReadAsync(messageLength, 0, messageLength.Length);

                        var messageData = new byte[BitConverter.ToInt32(messageLength, 0)];
                        await stream.ReadAsync(messageData, 0, messageData.Length);

                        //do stuff with message data hopefully!

                        //Socket client = listener.AcceptSocket();

                        //int size = client.Receive(bytes);

                        //for (int i = 0; i < size; i++)
                        //    Console.Write(Convert.ToChar(bytes[i]));

                        //client.Close();

                        //var asyncResult = await this.Socket.ReceiveAsync();
                        //var message = Encoding.ASCII.GetString(asyncResult.Buffer);

                        //this.logger.Debug($"endpoint:{this.Socket.Client.LocalEndPoint} > Incoming message ");

                        //this.messageHandler.HandleMessage(message, asyncResult.RemoteEndPoint);
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

        public void Send(IPEndPoint dst, byte[] msg)
        {
            this.Socket_tcp = new TcpClient(dst.Address.ToString(), dst.Port);

            var sendArgs = new SocketAsyncEventArgs();
            sendArgs.SetBuffer(msg, 0, msg.Length);

            this.Socket_tcp.Client.SendAsync(sendArgs);

            this.Socket.SendAsync(msg, msg.Length, dst.Address.ToString(), dst.Port);
        }

        public void Stop() => this.running = false;
    }
}