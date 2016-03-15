namespace ptpchat.VerbHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Newtonsoft.Json;

    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;

    internal class RoutingVerbHandler : IVerbHandler
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

        public bool HandleMessage(ref SocketManager socketManager, ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers)
        {
            if (!this.Nodes.Any())
            {
                throw new Exception("ROUTING MESSAGE: no nodes");
            }

            foreach (var node in this.Nodes)
            {
                var manager = new SocketManager { NodeId = node.NodeId, LocalEndpoint = new IPEndPoint(node.IpAddress, node.Port) };

                clientSocketManagers.Add(manager);
            }

            //return 'dont stop listening' after message has been handled

            return false;
        }
    }
}