namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Linq;
    using System.Net;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;
    using PtpChat.Utility;

    public class HelloVerbHandler : BaseVerbHandler<HelloMessage>
    {
		private const string LogInvalidRouteAttributes = "Invalid ttl or flood for HELLO message, ignoring";
        private const string LogInvalidNodeId = "Invalid Node ID in HELLO message, ignoring";

        private const string LogSameNodeId = "Recieved Hello presented this Node's ID! ignoring";

        public HelloVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        { }

        protected override bool HandleVerb(HelloMessage message, IPEndPoint senderEndpoint)
        {
            this.logger.Debug("Hello message recieved from sender: " + senderEndpoint);

			if (message.flood || message.ttl != 1) //HELLO is strictly a 1 to 1 transmission.
			{
				this.logger.Warning(LogInvalidRouteAttributes);
				return false;
			}
			
            var node = this.NodeManager.GetNodes(d => d.Key == message.SenderId.Id).FirstOrDefault();

			var attributes = message.msg_data.attributes;
			if (attributes != null)
			{

			}

            if (node != null) // Existing Node
            {
                this.NodeManager.Update(
                    node.NodeId.Id,
                    n =>
                        {
							n.UpdateName(node.NodeId.Name);
                            n.LastRecieve = DateTime.Now;
                            n.Version = node.Version ?? message.msg_data.version;
                            n.IsConnected = true;
							n.SeenThrough = null;
							n.Ttl = 1;
                        });
            }
            else //New Node
            {
				this.NodeManager.Add(
					new Node(message.SenderId)
					{
							Added = DateTime.Now,
							LastRecieve = DateTime.Now,
							IpAddress = senderEndpoint.Address,
							Port = senderEndpoint.Port,
							Version = message.msg_data.version,
							IsConnected = true,
							Attributes = attributes
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