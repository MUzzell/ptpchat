namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;
	using System.Linq;

	using PtpChat.Base.Interfaces;
	using PtpChat.Base.Messages;
	using Base.Classes;

	public class LeaveVerbHandler : BaseVerbHandler<LeaveMessage>
	{
		private const string LogInvalidChannelId = "LEAVE message contained invalid channel_id, ignoring";

		private const string LogInvalidChannelName = "LEAVE message contained invalid channel name, ignoring";

		private const string LogAddingNodeToChannel = "LEAVE, Adding node {0} to channel {1}";

		public LeaveVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
			: base(logger, dataManager, outgoingMessageManager)
		{
		}

		protected override bool HandleVerb(LeaveMessage message, IPEndPoint senderEndpoint)
		{
			this.logger.Debug($"LEAVE message recieved from sender: {senderEndpoint}");

			var nodeId = message.msg_data.node_id;

			if (!this.CheckNodeId(nodeId))
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
				this.logger.Debug("LEAVE message for unlisted channel, ignoring");
				return true;
			}

			var channel = channels.First();

			//are we aware of this node?
			var nodes = this.NodeManager.GetNodes(kv => kv.Key == nodeId);

			//no -> add
			if (!nodes.Any())
			{
				this.NodeManager.Add(new Node
				{
					Added = DateTime.Now,
					SeenThrough = null,
					IpAddress = null,
					NodeId = nodeId,
					LastRecieve = null,
					LastSend = null
				});
			}

			//is this node already part of the channel?
			if (!channel.Nodes.Contains(data.node_id))
			{
				this.logger.Info(string.Format(LogAddingNodeToChannel, nodeId, channel.ChannelId));
				this.ChannelManager.Update(channel.ChannelId, c => c.Nodes.Remove(data.node_id));
			}

			return true;
		}
	}
}