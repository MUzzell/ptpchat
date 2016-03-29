namespace PtpChat.Net
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	using PtpChat.Base.Interfaces;
	using PtpChat.Base.Messages;
	using Base.Classes;
	using Utility;

	public class SocketHandler : ISocketHandler
    {
		private const string LogHandlerStarting = "SocketHandler starting";

		private const string LogHandlerStopping = "SocketHandler stopping";

		private const string LogPortBound = "SocketHandler has bound to 0.0.0.0:{0}";

		//threads are organised by their port.
		private readonly IDictionary<int, SocketThread> internalThreads;

		private readonly UdpClient localClient;

		//im holding onto this until i figure out how to do the rest of the socket stuff.
		private readonly int localPort;

		private readonly ILogManager logger;

		private readonly INodeManager nodeManager;
		private readonly IChannelManager channelManager;

		private Timer heartBeatTimer;

		private IMessageHandler MessageHandler { get; }

		private readonly HelloMessage NodeHello;

		public SocketHandler(ILogManager logger, IDataManager dataManager, IMessageHandler messageHandler)
        {
            this.logger = logger;
            this.MessageHandler = messageHandler;
		    this.nodeManager = dataManager.NodeManager;

            this.internalThreads = new Dictionary<int, SocketThread>();

            //What if it's in use??
            this.localPort = new Random().Next(10000, 65535);
            this.localClient = new UdpClient(this.localPort, AddressFamily.InterNetwork);

            this.logger.Info(string.Format(LogPortBound, this.localPort));

		    this.nodeManager.LocalNode.Port = this.localPort;

            this.nodeManager = dataManager.NodeManager;
			this.channelManager = dataManager.ChannelManager;

            this.internalThreads.Add(this.localPort, new SocketThread(this.localClient, messageHandler, this.logger));

			this.NodeHello = new HelloMessage
			{
				msg_type = MessageType.HELLO,
				msg_data = new HelloData
				{
					node_id = this.nodeManager.LocalNode.NodeId,
					version = this.nodeManager.LocalNode.Version
				}
			};
		}
		
        public bool SendMessage(Guid dstNodeId, byte[] messsage)
        {
            var node = this.nodeManager.GetNodes(a => a.Value.NodeId == dstNodeId).FirstOrDefault();

            if (node == null)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message)
        {
            this.internalThreads[this.localPort].Send(dst, message);

            return true;
        }

        public void Start()
        {
            this.logger.Info(LogHandlerStarting);

            foreach (var thread in this.internalThreads.Values)
            {
                //start the listeners
                new Task(() => thread.Listen()).Start();
            }

            this.heartBeatTimer = new Timer(this.HeartBeat, null, 0, 3000);
        }

        public void Stop()
        {
            this.logger.Info(LogHandlerStopping);

            foreach (var thread in this.internalThreads.Values)
            {
                thread.Stop();
            }
        }

		private void HeartBeat(object state)
		{
			this.SendHellos();
			this.SendChannels();
		}

        private void SendHellos()
        {

            var nodes = this.nodeManager.GetNodes(node => node.Value.IsConnected || node.Value.IsStartUpNode).ToList();
			
            var msg = Encoding.ASCII.GetBytes(this.MessageHandler.BuildMessage(this.NodeHello));

            this.logger.Debug($"Sending Hellos to {nodes.Count} nodes");

            foreach (var node in nodes)
            {
                this.SendMessage(node.IpEndPoint, null, msg);
				this.nodeManager.Update(node.NodeId, n => n.LastSend = DateTime.Now);
            }
        }

		private void SendChannels()
		{

			var channels = this.channelManager.GetChannels(c => c.Value.IsUpToDate).ToList();

			if (!channels.Any())
				return;

			foreach (Channel channel in channels)
			{
				var chanMsg = new ChannelMessage
				{
					msg_data = new ChannelData
					{
						channel_id = channel.ChannelId,
						channel = channel.ChannelName,
						closed = channel.Closed,
						members = ExtensionMethods.BuildNodeIdList(channel.Nodes),
						msg_id = Guid.NewGuid(),
						node_id = this.nodeManager.LocalNode.NodeId
					}
				};
				var msg = Encoding.ASCII.GetBytes(this.MessageHandler.BuildMessage(chanMsg));

				this.logger.Debug($"Broadcasting Channel Message ({channel.ChannelName} : {channel.ChannelId}) to {channel.Nodes.Count} nodes");

                foreach (var node in this.nodeManager.GetNodes(n => channel.Nodes.Contains(n.Key)))
				{
					node.LastSend = DateTime.Now;
					this.SendMessage(node.IpEndPoint, null, msg);
				}

				this.channelManager.Update(channel.ChannelId, c => c.LastTransmission = DateTime.Now);
			}
			
		}

		public int GetPortForNode(Guid unknownId)
		{
			throw new NotImplementedException();
		}

        public void SendConnect()
        {

        }
    }
}