namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using Base.Interfaces;
	using Base.Messages;
	using Base.Classes;
	using System.Text;
	public class ChannelVerbHandler : BaseVerbHandler<ChannelMessage>
	{
		
		private const string LogInvalidMembersEntry = "Members list in CHANNEL message contained invalid node_id, ignoring";
		private const string LogInvalidChannelId = "CHANNEL message contained invalid channel_id, ignoring";
		private const string LogInvalidChannelName = "CHANNEL message contained invalid channel, ignoring";
		private const string LogInvalidMemebersList = "Members list on CHANNEL message was invalid, ignoring";

		public ChannelVerbHandler(ILogManager logger, IDataManager nodeManager, ISocketHandler socketHandler)
            : base(logger, nodeManager, socketHandler)
        {
        }

		protected override bool HandleVerb(ChannelMessage message, IPEndPoint senderEndpoint)
		{
			this.logger.Debug($"CHANNEL message recieved from sender: {senderEndpoint}");

			var nodeId = message.msg_data.node_id;

			if (!CheckNodeId(nodeId))
				return false;

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

			if (data.members == null)
			{
				this.logger.Warning(LogInvalidMemebersList);
				return false;
			}

			var memberIds = ParseMemberList(message.msg_data.members);

			//Add any unknown nodes to our list, but do not connect yet
			var unknownIds = memberIds.Except(this.NodeManager.GetNodes(n => !memberIds.Contains(n.Key)).Select<Node, Guid>(n => n.NodeId));

			foreach (Guid unknownId in unknownIds)
			{
				this.NodeManager.Add(new Node
				{
					NodeId = unknownId,
					LastRecieve = null,
					Added = DateTime.Now,
					SeenThrough = nodeId
				});

			}

			Channel channel;

			var channels = this.ChannelManager.GetChannels(c => c.Key == message.msg_data.channel_id);

			if (!channels.Any())
			{
				channel = new Channel
				{
					ChannelId = data.channel_id,
					ChannelName = data.channel,
					Closed = data.closed,
					IsUpToDate = true,
					LastTransmission = DateTime.Now,
					Nodes = memberIds.ToList(),
				};
				this.ChannelManager.Add(channel);
			}
			else
			{
				channel = channels.First();
			}
			

			//Are we in this channel?
			if (this.ChannelManager.IsNodeInChannel(this.NodeManager.LocalNode.NodeId, channel.ChannelId))
			{
				//Are we listed?
				if(!memberIds.Contains(this.NodeManager.LocalNode.NodeId)) //send JOIN
				{
					throw new NotImplementedException();
				}
				else //connect to other nodes.
				{
					var connectToNodes = this.NodeManager.GetNodes(kv => memberIds.Contains(kv.Value.NodeId) && !kv.Value.IsConnected);

					foreach (Node node in connectToNodes)
					{
						var connect = new ConnectMessage
						{
							msg_data = new ConnectData
							{
								src_node_id = this.NodeManager.LocalNode.NodeId,
								dst_node_id = node.NodeId,
								src = $"{this.SocketHandler.GetPortForNode(node.NodeId)}",
								dst = null
							}
						};
						this.SocketHandler.SendMessage(node.SeenThrough.Value, Encoding.ASCII.GetBytes(this.BuildMessage(connect)));
					}
				}
			}

			var ackMsg = new AckMessage
			{
				msg_data = new AckData
				{
					msg_id = message.msg_data.channel_id
				}
			};

			this.SocketHandler.SendMessage(senderEndpoint, null, Encoding.ASCII.GetBytes(this.BuildMessage(ackMsg)));

			return true;
		}

		private IEnumerable<Guid> ParseMemberList(List<Dictionary<string, string>> members)
		{
			var memberList = new List<Guid>();

			
			foreach (Dictionary<string, string> member in members)
			{
				Guid memberId;
				if (!Guid.TryParse(member["node_id"], out memberId))
				{
					this.logger.Warning(LogInvalidMembersEntry);
					continue;
				}

				if (!CheckNodeId(memberId))
					continue;

				memberList.Add(memberId);
			}

			return memberList;
		}
	}
}
