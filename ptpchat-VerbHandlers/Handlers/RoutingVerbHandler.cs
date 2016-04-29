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
        private const string LogInvalidIP = "NodeList in Routing message contained IP address that didn't parse, ignoring entry";

        private const string LogInvalidNodeList = "Recieved Invalid nodes list in ROUTING message, ignoring";

        private const string LogInvalidNodesEntry = "NodeList in Routing message contained invalid NodeId, ignoring entry";

        private const string LogInvalidSenderId = "Invalid Sender Node ID in ROUTING message, ignoring";

        private const string LogSameNodeId = "Recieved ROUTING sender's Node ID presented this Node's ID! ignoring";

		private const string LogInvalidRouteAttributes = "Invalid ttl or flood for ROUTING message, ignoring";

		public RoutingVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
        }

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

			if (message.flood || message.ttl != 1) //ROUTING is strictly a 1 to 1 transmission.
			{
				this.logger.Warning(LogInvalidRouteAttributes);
				return false;
			}

			var nodes = message.msg_data.nodes;

            if (nodes == null)
            {
                this.logger.Warning(LogInvalidNodeList);
                return false;
            }

			foreach (var node in nodes)
			{
				NodeId nodeId = null;

				if (!ExtensionMethods.TryParseNodeId(node.node_id, out nodeId))
				{ 
					this.logger.Debug("Invalid node_id in ROUTING message, skipping");
					continue;
				}
				
                if (nodeId == this.NodeManager.LocalNode.NodeId)
                {
                    this.logger.Debug("Ignoring our entry in ROUTING message");
                    continue;
                }

				var currentNodes = this.NodeManager.GetNodes(d => d.Key == nodeId.Id);

				if (!currentNodes.Any())
                {
                    // not seen, add.
                    this.NodeManager.Add(
                        new Node(nodeId)
                            {
                                SeenThrough = message.SenderId.Id,
                                Ttl = node.ttl + 1,
                                Version = null,
                                Added = DateTime.Now,
                                LastRecieve = null
                            });
				}
				else //update ttl
				{
					var currentNode = currentNodes.First();
					if (node.ttl + 1 >= currentNode.Ttl) //ignore if we have a better or equal ttl
						continue;

					currentNode.SeenThrough = message.SenderId.Id;
					currentNode.Ttl = node.ttl + 1;
				}
			}

			return true;
        }
    }
}