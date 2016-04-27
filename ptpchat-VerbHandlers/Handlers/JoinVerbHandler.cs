namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;
	using System.Linq;

	using PtpChat.Base.Interfaces;
	using PtpChat.Base.Messages;
	using Base.Classes;

	using PtpChat.Utility;

    public class JoinVerbHandler : BaseVerbHandler<JoinMessage>
    {
		private const string LogInvalidChannelId = "JOIN message contained invalid channel_id, ignoring";
		
		private const string LogInvalidChannelName = "JOIN message contained invalid channel name, ignoring";

		private const string LogAddingNodeToChannel = "JOIN, Adding node {0} to channel {1}";
		
		public JoinVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
        }

        protected override bool HandleVerb(JoinMessage message, IPEndPoint senderEndpoint)
        {
			this.logger.Debug($"JOIN message recieved from sender: {senderEndpoint}");

            var longId = message.sender_id;

            string senderName;
            Guid senderId;
            var successful = ExtensionMethods.SplitNodeId(longId, out senderName, out senderId);

            if (!successful || !this.CheckNodeId(senderId))
            {
                return false;
            }

            var data = message.msg_data;

			if (data.channel_id == Guid.Empty)
			{
				this.logger.Warning(LogInvalidChannelId);
				return false;
			}

			if (data.msg_id == Guid.Empty)
			{
				this.logger.Warning(LogInvalidMsgId);
				return false;
			}

			if (string.IsNullOrWhiteSpace(data.channel))
			{
				this.logger.Warning(LogInvalidChannelName);
				return false;
			}

			//are we aware of this channel?
			var channels = this.ChannelManager.GetChannels(kv => kv.Key == data.channel_id);

			//no -> ignore (we'll need more than just the channel id and name)
			if (!channels.Any())
			{
				this.logger.Debug("JOIN message for unlisted channel, ignoring");
				return true;
			}

			var channel = channels.First();

			//are we aware of this node?
			var nodes = this.NodeManager.GetNodes(kv => kv.Key == senderId);
			
			//no -> add
			if (!nodes.Any())
			{
				this.NodeManager.Add(new Node
				{
					Added = DateTime.Now,
					SeenThrough = null,
					IpAddress = null,
					NodeId = new NodeId(senderName, senderId),
					LastRecieve = null,
					LastSend = null
				});
			}

			//is this node already part of the channel?
			if (!channel.Nodes.Contains(senderId))
			{
				this.logger.Info(string.Format(LogAddingNodeToChannel, senderId, channel.ChannelId));
				this.ChannelManager.Update(channel.ChannelId, c => c.Nodes.Add(senderId));

				//connect to node
				if (this.ChannelManager.IsNodeInChannel(this.NodeManager.LocalNode.NodeId.Id, channel.ChannelId))
				{
					this.OutgoingMessageManager.SendConnect(senderEndpoint, senderId);
				}
			}

			return true;
		}
    }
}