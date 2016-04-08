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

        public HelloVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
        }

        protected override bool HandleVerb(HelloMessage message, IPEndPoint senderEndpoint)
        {
            this.logger.Debug("Hello message recieved from sender: " + senderEndpoint);

            var nodeId = message.msg_data.node_id;

            if (!this.CheckNodeId(nodeId))
            {
                return false;
            }

            var node = this.NodeManager.GetNodes(d => d.Value.NodeId == nodeId).FirstOrDefault();

            if (node != null) // Existing Node
            {
                this.NodeManager.Update(
                    node.NodeId,
                    n =>
                        {
                            n.LastRecieve = DateTime.Now;
                            n.Version = node.Version ?? message.msg_data.version;
                            node.IsConnected = true;
                        });
            }
            else //New Node
            {
                this.NodeManager.Add(
                    new Node
                        {
                            NodeId = nodeId,
                            Added = DateTime.Now,
                            LastRecieve = DateTime.Now,
                            IpAddress = senderEndpoint.Address,
                            Port = senderEndpoint.Port,
                            Version = message.msg_data.version,
                            IsConnected = true
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