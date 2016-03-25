namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Linq;
    using System.Net;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public class HelloVerbHandler : BaseVerbHandler<HelloMessage>
    {
		private const string LogInvalidNodeId = "Invalid Node ID in HELLO message, ignoring";
		private const string LogSameNodeId = "Recieved Hello presented this Node's ID! ignoring";
		
		public HelloVerbHandler(ILogManager logger, INodeManager nodeManager, ISocketHandler socketHandler)
            : base( logger, nodeManager, socketHandler)
        {
        }
		
        protected override bool HandleVerb(HelloMessage message, IPEndPoint senderEndpoint)
        {
            this.logger.Debug("Hello message recieved from sender: " + senderEndpoint);

            var nodeId = message.msg_data.node_id;

            if (nodeId == Guid.Empty)
            {
                this.logger.Warning(LogInvalidNodeId);
                return false;
            }

            if (nodeId == this.NodeManager.LocalNode.NodeId)
            {
                this.logger.Error(LogSameNodeId);
                return false;
            }

            var nodes = this.NodeManager.GetNodes(d => d.Value.NodeId == nodeId).ToList();

            if (nodes.Count > 0) // Existing Node
            {
                var node = nodes[0];
                node.LastSeen = DateTime.Now;
				node.Version = node.Version ?? message.msg_data.version;
                this.NodeManager.Update(node);
            }
            else //New Node
            {
                this.NodeManager.Add(new Node {
					NodeId = nodeId,
					LastSeen = DateTime.Now,
					IpAddress = senderEndpoint.Address,
					Port = senderEndpoint.Port,
					Version = message.msg_data.version
				});
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