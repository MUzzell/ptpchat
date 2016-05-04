namespace PtpChat.Net
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;

	using PtpChat.Base.Interfaces;
	using Base.EventArguements;
	using System.Threading;
	using System.IO;
	public class SocketThread
    {
        private const string LogArgumentException = "Argument Exception recieved whilst parsing recieved data: {0}";
        private const string LogSocketException = "Socket Exception recieved whilst operating on socket: {0}";

        private readonly ILogManager logger;
        private readonly IMessageHandler messageHandler;

        private volatile bool running = true;
		
        private TcpClient Socket { get; set; }

		public IPEndPoint Destination { get; private set; }

		public event EventHandler SocketConnected;
		public event EventHandler SocketDisconnected;
		public event EventHandler SocketReset;

		private Timer KeepAliveTimer;

        public SocketThread(IPEndPoint destination, IMessageHandler messageHandler, ILogManager logger)
        {
            this.messageHandler = messageHandler;
            this.logger = logger;
			
			this.Destination = destination;

            this.Socket = new TcpClient { ExclusiveAddressUse = false };
            //this.Socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            this.Socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

			this.KeepAliveTimer = new Timer(this.SendKeepAlive, null, Timeout.Infinite, 5000);
            
        }

        //public static ManualResetEvent allDone = new ManualResetEvent(false);

        public async void Listen()
        {
            this.logger.Info($"New SocketThread connecting to target: {this.Destination}");
			//this.listener.Start();
			
			try
			{
				this.Socket.Connect(this.Destination.Address.ToString(), this.Destination.Port);
				this.SocketConnected?.Invoke(this, new SocketThreadEventArgs { Destination = this.Destination });
				this.KeepAliveTimer.Change(5000, 5000);
			}
			catch(SocketException se)
			{
				this.logger.Error(string.Format(LogSocketException, se.Message), se);
				this.SocketReset?.Invoke(this, new SocketThreadEventArgs { Destination = this.Destination, Exception = se, ErrorCode = se.ErrorCode });
#if DEBUG
				throw se;
#endif
			}

			var stream = this.Socket.GetStream();
			var messageLengthBytes = new byte[4];
			int read;
			try
            {
				
				while (this.running && this.CheckConnection())
                {
                    try
                    {

                        read = await stream.ReadAsync(messageLengthBytes, 0, 4);

						if (read < 4)
							continue;

                        messageLengthBytes = messageLengthBytes.Reverse().ToArray();
                        int messageLength = BitConverter.ToInt32(messageLengthBytes, 0);

						if (messageLength <= 0 || messageLength > this.Socket.ReceiveBufferSize)
							continue;

                        var messageData = new byte[messageLength];

                        //var messageData = new byte[BitConverter.ToInt32(messageLength, 0)];
                        read = await stream.ReadAsync(messageData, 0, messageData.Length);

						if (read < messageLength)
							continue;

						//==================================================


						var message = Encoding.ASCII.GetString(messageData);

                        this.logger.Debug($"endpoint:{this.Destination} > Incoming message ");

                        this.messageHandler.HandleMessage(message, (IPEndPoint)this.Socket.Client.RemoteEndPoint);
                    }
					catch (IOException ioe)
					{
						this.logger.Error(string.Format(LogArgumentException, ioe.Message));
						this.SocketReset?.Invoke(this, new SocketThreadEventArgs { Destination = this.Destination});
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
				this.SocketReset?.Invoke(this, new SocketThreadEventArgs { Destination = this.Destination, Exception = se, ErrorCode = se.ErrorCode });
			}
			finally
            {
				this.running = false;
				this.KeepAliveTimer.Change(Timeout.Infinite, 5000);
				this.Socket.Close();
				this.SocketDisconnected?.Invoke(this, new SocketThreadEventArgs() { Destination = this.Destination });
            }
        }

		private bool CheckConnection()
		{
			try
			{
				return !(this.Socket.Client.Poll(1, SelectMode.SelectRead) && Socket.Available == 0);
			}
			catch (SocketException se)
			{
				this.logger.Debug($"Exception checking connection to {this.Destination}; {se.Message}");
				return false;
			}
		}

		private async void SendKeepAlive(object state)
		{
			try
			{
				if (this.running)
					await this.Socket.GetStream().WriteAsync(new byte[4], 0, 4);
			}
			catch (SocketException se)
			{
				this.logger.Debug($"Exception sending heartbeat to {this.Destination}; {se.Message}");
			}
		}
		
        public void Send(byte[] msg)
        {

            //reverse the array because reasons that are important and not to be underestimated
            byte[] messageSize = BitConverter.GetBytes(msg.Length).Reverse().ToArray();

            byte[] finishedMessage = new byte[messageSize.Length + msg.Length];

            Buffer.BlockCopy(messageSize, 0, finishedMessage, 0, messageSize.Length);
            Buffer.BlockCopy(msg, 0, finishedMessage, messageSize.Length, msg.Length);

            var sendArgs = new SocketAsyncEventArgs { RemoteEndPoint = this.Destination };
            sendArgs.SetBuffer(finishedMessage, 0, finishedMessage.Length);

            this.Socket.Client.SendAsync(sendArgs);

            //this.Socket.SendAsync(msg, msg.Length, dst.Address.ToString(), dst.Port);
        }

        public void Stop() => this.running = false;
    }
}