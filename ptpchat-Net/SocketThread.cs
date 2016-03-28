namespace PtpChat.Net
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    using PtpChat.Base.Interfaces;

    internal class SocketThread
    {
        public SocketThread(UdpClient socket, IMessageHandler messageHandler, ILogManager logger)
        {
            this.Socket = socket;
            this.messageHandler = messageHandler;
            this.logger = logger;
        }

        public SocketThread(IPEndPoint local, IMessageHandler messageHandler, ILogManager logger)
        {
            this.Socket = new UdpClient(local);
            this.messageHandler = messageHandler;
            this.logger = logger;
        }

        private const string LogArgumentException = "Argument Exception recieved whilst parsing recieved data: {0}";

        private const string LogSocketException = "Socket Exception recieved whilst operating on socket: {0}";

        private readonly ILogManager logger;

        private IMessageHandler messageHandler;

        private volatile bool running = true;

        private UdpClient Socket { get; }

        public async void Run()
        {
            this.logger.Info($"New SocketThread starting, endpoint: {this.Socket.Client.LocalEndPoint}");
            try
            {
                while (this.running)
                {
                    try
                    {
                        this.logger.Debug("listening on SocketThread");

                        var asyncResult = await this.Socket.ReceiveAsync();

                        //nothing ever turns up here? are we getting any messages from the server? nothing on wireshark...
                        var message = Encoding.ASCII.GetString(asyncResult.Buffer);

                        this.messageHandler.HandleMessage(message, asyncResult.RemoteEndPoint);
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
        }

        public void SetMessageHandler(IMessageHandler handler) => this.messageHandler = handler;

        // will it work? Who knows, thats the fuuunn.
        // turns out no it wont.
        public void Send(IPEndPoint dst, byte[] msg)
        {
            this.Socket.SendAsync(msg, msg.Length, dst.Address.ToString(), dst.Port);
        }

        public void Stop() => this.running = false;
    }
}