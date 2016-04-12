namespace PtpChat.Main.Managers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;

    using Newtonsoft.Json;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;
    using PtpChat.Utility;

    public class OutgoingMessageManager : IOutgoingMessageManager
    {
        private readonly IChannelManager channelManager;

        private readonly ILogManager logger;

        private readonly INodeManager nodeManager;

        private readonly IResponseManager responseManager;

        private ISocketHandler SocketHandler { get; }

        public int DefaultTimeToLive { get; }

        public OutgoingMessageManager(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler, int defaultTtl)
        {
            this.SocketHandler = socketHandler;
            this.logger = logger;
            this.channelManager = dataManager.ChannelManager;
            this.responseManager = dataManager.ResponseManager;
            this.nodeManager = dataManager.NodeManager;

            this.DefaultTimeToLive = defaultTtl > 0 ? defaultTtl : 32;
        }

        public void SendHello(Node node, HelloMessage helloMessage) => this.SendHello(node.IpEndPoint, helloMessage);

        public void SendHello(IPEndPoint endpoint, HelloMessage connectMessage)
        {
            this.MessageTTLOne(connectMessage);
            this.Send(endpoint, connectMessage);
        }

        public void SendConnect(Node node, Guid targetNodeId) => this.SendConnect(node.IpEndPoint, targetNodeId);

		public void SendConnect(IPEndPoint endpoint, Guid targetNodeId)
		{
			ConnectMessage connectMessage = new ConnectMessage()
			{
				ttl = 32,
				flood = false,

				msg_data = new ConnectData
				{
					src = $"{this.nodeManager.LocalNode.IpAddress}:{this.nodeManager.LocalNode.Port}",
					src_node_id = this.nodeManager.LocalNode.NodeId,
					dst = null,
					dst_node_id = targetNodeId
				}
			};

			this.Send(endpoint, connectMessage)
				
		}

        public void SendMessage(Node node, MessageMessage messageMessage) => this.SendMessage(node.IpEndPoint, messageMessage);

        public void SendMessage(IPEndPoint endpoint, MessageMessage messageMessage)
        {
            this.MessageTTLDefault(messageMessage);
            this.Send(endpoint, messageMessage);
        }

        public void SendChannel(Node node, ChannelMessage channelMessage) => this.SendChannel(node.IpEndPoint, channelMessage);

        public void SendChannel(IPEndPoint endpoint, ChannelMessage channelMessage)
        {
            this.SetMessageFlood(channelMessage);
            this.Send(endpoint, channelMessage);
        }

        public void SendAck(Node node, Guid msgId) => this.SendAck(node.IpEndPoint, msgId);

        public void SendAck(IPEndPoint endpoint, Guid msgId)
        {
			var ackMessage = new AckMessage
			{
				ttl = 1,
				flood = false,
				msg_data = new AckData
				{
					msg_id = msgId
				}
			};

            this.Send(endpoint, ackMessage);
        }

        private void SendHeartBeatHelloToNodes()
        {
            var nodes = this.nodeManager.GetNodes(node => node.Value.IsConnected || node.Value.IsStartUpNode).ToList();

            this.logger.Debug($"Sending Hellos to {nodes.Count} nodes");

            var msg = new HelloMessage
			{
				msg_data = new HelloData
				{
					node_id = this.nodeManager.LocalNode.NodeId,
					version = this.nodeManager.LocalNode.Version
				}
			};

            foreach (var node in nodes)
            {
                this.SendHello(node.IpEndPoint, msg);
                this.nodeManager.Update(node.NodeId, n => n.LastSend = DateTime.Now);
            }
        }

        public void DoHeartBeat(object state)
        {
            this.SendHeartBeatHelloToNodes();
            this.SendChannels();
            this.ResendMessages();
        }

        public void Send(IPEndPoint endpoint, BaseMessage message)
        {
            var msg = Encoding.ASCII.GetBytes(this.SerialiseObject(message));
            this.SocketHandler.SendMessage(endpoint, null, msg);
        }

        private void ResendMessages()
        {
            foreach (var message in this.responseManager.GetOutstandingMessages())
            {
                this.responseManager.AddOrUpdate(message.MsgId, message.TargetNodeId, message.Msg);
                this.SocketHandler.SendMessage(message.TargetNodeId, message.Msg);
            }
        }

        private void SendChannels()
        {
            var channels = this.channelManager.GetChannels(c => c.Value.IsUpToDate).ToList();

            if (!channels.Any())
            {
                return;
            }

            foreach (var channel in channels)
            {
                var chanMsg = new ChannelMessage
				{
					msg_data =
					new ChannelData
					{
						channel_id = channel.ChannelId,
						channel = channel.ChannelName,
						closed = channel.Closed,
						members = ExtensionMethods.BuildNodeIdList(channel.Nodes),
						msg_id = Guid.NewGuid(),
						node_id = this.nodeManager.LocalNode.NodeId
					}
				};

                this.logger.Debug($"Broadcasting Channel Message ({channel.ChannelName} : {channel.ChannelId}) to connected nodes");

                foreach (var node in this.nodeManager.GetNodes(n => n.Value.IsConnected))
                {
                    this.SendChannel(node, chanMsg);
                    this.nodeManager.Update(node.NodeId, n => n.LastSend = DateTime.Now);
                }

                this.channelManager.Update(channel.ChannelId, c => c.LastTransmission = DateTime.Now);
            }
        }

        public int GetPortForNode(Guid unknownId)
        {
			//42 TCP UDP (ARPA Host Name Server Protocol Official)
			return 42;
        }

        private void MessageTTLOne(BaseMessage message)
        {
            message.ttl = 1;
            message.flood = false;
        }

        private void MessageTTLDefault(BaseMessage message)
        {
            message.ttl = this.DefaultTimeToLive;
            message.flood = false;
        }

        private void SetMessageFlood(BaseMessage message)
        {
            message.ttl = this.DefaultTimeToLive;
            message.flood = true;
        }

        private string SerialiseObject(BaseMessage message) => JsonConvert.SerializeObject(message);
    }
}