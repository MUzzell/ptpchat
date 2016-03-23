namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;

	using PtpChat.Base.Messages;
	using Base.Interfaces;
	using VerbHandlers;
	using System.Collections.Generic;
	using Base.Classes;

	public class HelloVerbHandler : BaseVerbHandler<HelloMessage>
    {
		private static readonly string LogInvalidNodeId = "Invalid Node ID in HELLO message, ignoring";
		private static readonly string LogSameNodeId = "Recieved Hello presented this Node's ID! ignoring";

        private HelloMessage Message { get; set; }

		public HelloVerbHandler(ref ILogManager logger, ref INodeManager nodeManager, ref ISocketHandler socketHandler) : base(ref logger, ref nodeManager, ref socketHandler) { }

		protected override bool HandleVerb(HelloMessage message, IPEndPoint senderEndpoint)
		{
			this.logger.Debug("Hello message recieved from sender: " + senderEndpoint);

			Guid nodeId = message.msg_data.node_id;

			if (nodeId == null || nodeId == Guid.Empty)
			{
				this.logger.Warning(HelloVerbHandler.LogInvalidNodeId);
				return false;
			}

			if (nodeId == this.NodeManager.LocalNode.NodeId)
			{
				this.logger.Error(HelloVerbHandler.LogSameNodeId);
				return false;
			}

			var filter = new Dictionary<NodeFilterType, object>();
			filter.Add(NodeFilterType.NodeId, nodeId);

			var nodes = this.NodeManager.GetNodes(filter);

			if (nodes.Count > 0) // Existing Node
			{
				var node = nodes[0];
				node.LastSeen = DateTime.Now;
				this.NodeManager.Update(node);
			}
			else //New Node
			{
				var node = new Node();

				node.NodeId = nodeId;
				node.LastSeen = DateTime.Now;
				this.NodeManager.Add(node);
			}

			return true;
		}


		/*
        var socketManager = serverSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id)
                            ?? clientSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id);

        if (socketManager == null)
        {
            socketManager = serverSocketManagers.FirstOrDefault(a => Equals(a.DestinationEndpoint.Address, senderEndpoint.Address))
                            ?? clientSocketManagers.FirstOrDefault(a => Equals(a.DestinationEndpoint.Address, senderEndpoint.Address));

            if (socketManager != null)
            {
                socketManager.DestinationNodeId = this.Message.msg_data.node_id;
            }
        }

        if (socketManager == null)
        {
            //no socket manager for the message?
            throw new Exception("Hello MESSAGE: no known socketmanager. Id = " + this.Message.msg_data.node_id);
        }

        socketManager.LastHelloRecieved = DateTime.Now;

        //return 'dont stop listening' after message has been handled
        return false;
		*/


	}
}