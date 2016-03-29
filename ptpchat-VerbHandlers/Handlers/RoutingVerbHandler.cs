namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Linq;
    using System.Net;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;
    using PtpChat.Utility;

    public class RoutingVerbHandler : BaseVerbHandler<RoutingMessage>
    {

        public RoutingVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler)
            : base(logger, dataManager, socketHandler)
        {
        }

        private const string LogInvalidSenderId = "Invalid Sender Node ID in ROUTING message, ignoring";

        private const string LogSameNodeId = "Recieved ROUTING sender's Node ID presented this Node's ID! ignoring";

        private const string LogInvalidNodeList = "Recieved Invalid nodes list in ROUTING message, ignoring";

        private const string LogInvalidNodesEntry = "NodeList in Routing message contained invalid NodeId, ignoring entry";

        private const string LogInvalidIP = "NodeList in Routing message contained IP address that didn't parse, ignoring entry";

        /*
        public void ParseBaseMessage(string messageJson)
        {
            this.Message = JsonConvert.DeserializeObject<RoutingMessage>(messageJson);
            this.Nodes = new List<Node>();

            this.Message.msg_data.nodes.ForEach(
                node =>
                    {
                        try
                        {
                            var nodeId = node.Where(p => p.Key == "node_id").Select(p => p.Value).FirstOrDefault();

                            if (string.IsNullOrWhiteSpace(nodeId))
                            {
                                throw new Exception("ROUTING MESSAGE: missing node_id");
                            }

                            var wholeAddress = node.Where(p => p.Key == "address").Select(p => p.Value).FirstOrDefault();

                            if (string.IsNullOrWhiteSpace(wholeAddress))
                            {
                                throw new Exception("ROUTING MESSAGE: no address");
                            }

                            var splitAddress = wholeAddress.Split(':');

                            this.Nodes.Add(new Node { NodeId = Guid.Parse(nodeId), IpAddress = IPAddress.Parse(splitAddress[0]), Port = int.Parse(splitAddress[1]) });
                        }
                        catch (Exception ex)
                        {
                        }
                    });
        }

        public bool HandleMessage(IPEndPoint senderEndpoint, ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers)
        {
            var socketManager = serverSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id)
                                ?? clientSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id);

            if (socketManager == null)
            {
                socketManager = serverSocketManagers.FirstOrDefault(a => Equals(a.DestinationEndpoint.Address, senderEndpoint.Address))
                                ?? clientSocketManagers.FirstOrDefault(a => Equals(a.DestinationEndpoint.Address, senderEndpoint.Address));
            }

            if (socketManager == null)
            {
                //no socket manager for the message?
                throw new Exception("ROUTING MESSAGE: no known socketmanager");
            }

            if (!this.Nodes.Any())
            {
                throw new Exception("ROUTING MESSAGE: no nodes");
            }

            foreach (var node in this.Nodes)
            {
                var manager = new SocketManager
                                  {
                                      DestinationNodeId = node.NodeId,
                                      DestinationEndpoint = new IPEndPoint(node.IpAddress, node.Port),

                                      //using the server's socket manager to handle routing, so we can get our local info from there
                                      LocalEndpoint = socketManager.LocalEndpoint,
                                      LocalNodeId = socketManager.LocalNodeId,

                                      //UdpClient = socketManager.UdpClient,
                                      IsServerConnection = false
                                  };

                if (clientSocketManagers.All(a => a.DestinationNodeId != manager.DestinationNodeId))
                {
                    clientSocketManagers.Add(manager);
                }
            }

            //return 'dont stop listening' after message has been handled

            return false;
        }
		*/

        protected override bool HandleVerb(RoutingMessage message, IPEndPoint senderEndpoint)
        {
            this.logger.Debug($"Routing message recieved from: {senderEndpoint}");

            var senderId = message.msg_data.node_id;

            var nodes = message.msg_data.nodes;

			if (!CheckNodeId(senderId))
				return false;

			if (nodes == null)
            {
                this.logger.Warning(LogInvalidNodeList);
                return false;
            }

            foreach (var node in nodes)
            {
                var nodeAddress = node["address"].Trim();

                Guid nodeId;
                if (!Guid.TryParse(node["node_id"], out nodeId))
                {
                    this.logger.Warning(LogInvalidNodesEntry);
                    continue;
                }

                if (nodeId == Guid.Empty)
                {
                    this.logger.Warning(LogInvalidNodesEntry);
                    continue;
                }

                if (nodeId == this.NodeManager.LocalNode.NodeId)
                {
                    this.logger.Debug("Ignoring our entry in ROUTING message");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(nodeAddress))
                {
                    this.logger.Warning(LogInvalidNodesEntry);
                    continue;
                }

                IPEndPoint address;

                try
                {
                    address = ExtensionMethods.ParseEndpoint(nodeAddress);
                }
                catch (Exception e)
                {
                    this.logger.Warning(LogInvalidIP);
                    continue;
                }

                if (!this.NodeManager.GetNodes(d => d.Value.NodeId == nodeId).Any())
                {
					// not seen, add. else, ignore
					this.NodeManager.Add(new Node
					{
						NodeId = nodeId,
						SeenThrough = senderId,
						IpAddress = address.Address,
						Port = address.Port,
						Version = null,
						Added = DateTime.Now,
						LastRecieve = null
                    });
                }
            }

            return true;
        }
    }
}