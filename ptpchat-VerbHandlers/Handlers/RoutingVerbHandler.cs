namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Newtonsoft.Json;

    using PtpChat.Base.Communication_Messages;
    using PtpChat.Base.General_Classes;
    using PtpChat.Net;
    using PtpChat.UtilityClasses;

    public class RoutingVerbHandler : IVerbHandler
    {
        private RoutingMessage Message { get; set; }

        private List<Node> Nodes { get; set; }

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

                if (!clientSocketManagers.Any(a => a.DestinationNodeId == manager.DestinationNodeId))
                {
                    clientSocketManagers.Add(manager);
                }
            }

            //return 'dont stop listening' after message has been handled

            return false;
        }
    }
}