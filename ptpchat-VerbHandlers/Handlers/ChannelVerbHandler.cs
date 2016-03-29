namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using Base.Interfaces;
	using Base.Messages;
	using Base.Classes;

	public class ChannelVerbHandler : BaseVerbHandler<ChannelMessage>
	{
		
		private const string LogInvalidMembersEntry = "Members list in CHANNEL message contained invalid node_id, ignoring";

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

			var memberIds = ParseMemberList(message.msg_data.members);

			//Are we in this channel?
			if (this.ChannelManager.IsNodeInChannel(this.NodeManager.LocalNode.NodeId, message.msg_data.channel_id))
			{
				//Are we listed?


				var unknownIds = memberIds.Except(this.NodeManager.GetNodes(n => !memberIds.Contains(n.Key)).Select<Node, Guid>(n => n.NodeId));

				throw new NotImplementedException();

				/*
				var connectNode = this.NodeManager.GetNodeForConnect(unknownId);
				this.SocketHandler.SendMessage(connectNode, new ConnectMessage
				{
					msg_data = new ConnectData
					{
						dst_node_id = unknownId,
						src_node_id = this.NodeManager.LocalNode.NodeId,
						src = this.SocketHandler.GetPortForNode(unknownId)
					}
				});
				*/
			}
			else
			{
				//Add any unknown nodes to our list, but do not connect
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
				
			}

			
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
