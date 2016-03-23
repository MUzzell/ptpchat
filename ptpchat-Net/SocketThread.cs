namespace PtpChat.Net
{
	using Base.Interfaces;
	using System;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;

	internal class SocketThread
	{
		private static readonly string LogArgumentException = "Argument Exception recieved whilst parsing recieved data: {0}";
		private static readonly string LogSocketException = "Socket Exception recieved whilst operating on socket: {0}";

		public UdpClient socket { get; }

		private IMessageHandler messageHandler;
		private ILogManager logger;

		private volatile bool running = true;

		public SocketThread(UdpClient socket, 
			IMessageHandler messageHandler,
			ILogManager logger)
		{
			this.logger = logger;
			this.socket = socket;
			this.messageHandler = messageHandler;
		}

		public SocketThread(IPEndPoint local,
			IMessageHandler messageHandler,
			ILogManager logger)
		{
			this.logger = logger;
			this.socket = new UdpClient(local);
			this.messageHandler = messageHandler;
		}

		public async void Run()
		{
			this.logger.Info(string.Format("New SocketThread starting, endpoint: {0}", this.socket.Client.LocalEndPoint));
			try
			{
				while (this.running)
				{
					try
					{
						this.logger.Debug("listening on SocketThread");
						
						var asyncResult = await this.socket.ReceiveAsync();

						var message = Encoding.ASCII.GetString(asyncResult.Buffer);

						this.messageHandler.HandleMessage(message, asyncResult.RemoteEndPoint);

					}
					catch (ArgumentException ae)
					{
						this.logger.Warning(string.Format(SocketThread.LogArgumentException, ae.Message));
					}
					
				}
			}
			catch (SocketException se)
			{
				this.logger.Error(string.Format(SocketThread.LogArgumentException, se.Message));
			}
			
		}

		// will it work? Who knows, thats the fuuunn.
		public void Send(IPEndPoint dst, byte[] msg)
		{
			this.socket.Send(msg, msg.Length);
		}

		public void Stop()
		{
			this.running = false;
		}

	}
}
