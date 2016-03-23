namespace PtpChat.Net
{
	using System.Net;
	using System.Net.Sockets;
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Threading.Tasks;
	using System.Threading;

	using PtpChat.Base.Interfaces;
	using Base.Classes;
	using Base.Messages;
	public class SocketHandler : ISocketHandler
	{

		private static readonly string LogHandlerStarting = "SocketHandler starting";
		private static readonly string LogHandlerStopping = "SocketHandler stopping";
		private static readonly string LogPortBound = "SocketHandler has bound to 0.0.0.0:{0}";

		private ILogManager logger;
		private INodeManager nodeManager;
		private IMessageHandler messageHandler;

		private Random internalRand;
		private Timer helloTimer;

		//threads are organised by their port.
		private IDictionary<int, SocketThread> internalThreads;

		//im holding onto this until i figure out how to do the rest of the socket stuff.
		private int localPort;

		public SocketHandler(
			ILogManager logger, 
			INodeManager nodeManager, 
			IMessageHandler handler)
		{
			this.logger = logger;
			this.nodeManager = nodeManager;
			this.messageHandler = handler;

			this.internalThreads = new Dictionary<int, SocketThread>();
			this.internalRand = new Random();

			//What if it's in use??
			this.localPort = this.internalRand.Next(10000,65535);

			UdpClient newClient = new UdpClient(port, AddressFamily.InterNetwork);

			this.logger.Info(string.Format(SocketHandler.LogPortBound, this.localPort));

			this.internalThreads.Add(this.localPort, new SocketThread(newClient, messageHandler, logger));
			
		}

		private void SendHellos(object state)
		{
			var nodes = nodeManager.GetNodes();

			var hello = new HelloMessage()
			{
				msg_type = MessageType.HELLO,
				msg_data = new HelloData
				{
					node_id = nodeManager.LocalNode.NodeId,
					version = nodeManager.LocalNode.Version
				}
			};

			//i hope this works!
			var msg = Encoding.ASCII.GetBytes(this.messageHandler.BuildMessage(hello));

			this.logger.Debug(string.Format("Sending Hellos to {0} nodes", nodes.Count));



			foreach (Node node in nodes)
				this.SendMessage(node.IpEndPoint, null, msg);
		}

		public bool SendMessage(Guid dstNodeId, byte[] messsage)
		{
			var filter = new Dictionary<NodeFilterType, object>();
			filter[NodeFilterType.NodeId] = dstNodeId;

			throw new NotImplementedException();
			
			
		}

		public bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message)
		{
			this.internalThreads[this.localPort].Send(dst, message);

			return true;
		}

		public void Start()
		{
			this.logger.Info(SocketHandler.LogHandlerStarting);
			
			foreach (SocketThread thread in this.internalThreads.Values)
				new Task(() => thread.Run()).Start();

			this.helloTimer = new Timer(new TimerCallback(SendHellos), null, 0, 3000);
		}

		public void Stop()
		{
			this.logger.Info(SocketHandler.LogHandlerStopping);

			foreach (SocketThread thread in this.internalThreads.Values)
				thread.Stop();
		}
		
	}
}
