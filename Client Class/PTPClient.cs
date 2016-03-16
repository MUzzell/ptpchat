namespace ptpchat.Client_Class
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using ptpchat.Class_Definitions;
    using ptpchat.Communication_Messages;
    using ptpchat.VerbHandlers;

    public class PTPClient
    {
        public PTPClient()
        {
            this.ErrorMessages = new PtpList<string>();
            this.ThisNodeId = Guid.NewGuid();

            this.LocalEndpoint = new IPEndPoint(IPAddress.Any, new Random().Next(10000, 65535));
            this.LocalUdpClient = new UdpClient(this.LocalEndpoint);

            this.ServerSocketManagers = new PtpList<SocketManager>();
            this.ClientSocketManagers = new PtpList<SocketManager>();
        }

        public PtpList<SocketManager> ClientSocketManagers;

        public PtpList<SocketManager> ServerSocketManagers;

        public Guid ThisNodeId { get; }

        public IPEndPoint LocalEndpoint { get; }

        public UdpClient LocalUdpClient { get; }

        public PtpList<string> ErrorMessages { get; }

        //send a hello to an ip address
        //create a new socket manager and send the hello, then return the socket manager
        public SocketManager SendHello(IPAddress ip, int port, bool isServer)
        {
            SocketManager socketManager;

            //do we have a socket manager for this ip address saved? 
            if (this.ServerSocketManagers.Any(a => Equals(a.DestinationEndpoint.Address, ip)))
            {
                //if so, use it
                socketManager = this.ServerSocketManagers.First(a => Equals(a.DestinationEndpoint.Address, ip));
            }
            else
            {
                //else create the socket manager instance for this ip address as we've not seen it before
                socketManager = new SocketManager
                                    {
                                        LocalNodeId = this.ThisNodeId,
                                        LocalEndpoint = this.LocalEndpoint,
                                        DestinationNodeId = Guid.Empty,
                                        DestinationEndpoint = new IPEndPoint(ip, port),

                                        //UdpClient = this.LocalUdpClient,
                                        IsServerConnection = isServer
                                    };
            }

            var sendSuccessful = this.SendHello(ref socketManager);

            return sendSuccessful ? socketManager : null;
        }

        //send a hello using an existing socketManager
        public bool SendHello(ref SocketManager socketManager)
        {
            try
            {
                //check that we dont hello ourselves
                if (socketManager?.DestinationEndpoint == null)
                {
                    return false;
                }

                var helloJson = "{" + string.Format(JsonDefinitions.HelloJson, this.ThisNodeId, string.Empty) + "}";

                //create the byte array for the Hello JSON message data
                var encodedHelloMsg = Encoding.ASCII.GetBytes(helloJson);

                //send the hello msg to the server at the IP
                this.LocalUdpClient.SendAsync(
                    encodedHelloMsg,
                    encodedHelloMsg.Length,
                    socketManager.DestinationEndpoint.Address.ToString(),
                    socketManager.DestinationEndpoint.Port > 0 ? socketManager.DestinationEndpoint.Port : 9001);

                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessages.Add(ex.ToString());
                return false;
            }
        }

        //start listening on a socket in a while loop
        public void StartListening()
        {
            Task.Run(
                async () =>
                    {
                        try
                        {
                            //the client for socket we want to listen on
                            using (var udpClient = this.LocalUdpClient)
                            {
                                //constantly listen 
                                while (true)
                                {
                                    //wait for an incoming message
                                    var asyncResult = await udpClient.ReceiveAsync();

                                    //read and parse the json
                                    var messageJson = Encoding.ASCII.GetString(asyncResult.Buffer);

                                    //cast the message to a BaseMessage so we can use the message type
                                    var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(messageJson);

                                    //get a verb handler for the message
                                    var verbHandlerForMessage = this.GetVerbHandlerForMessage(baseMessage.msg_type);

                                    //pass the message to the verb handler, and have it parse the message
                                    verbHandlerForMessage.ParseBaseMessage(messageJson);

                                    //and call the handle method on the verb handler, passing in the things it will need to do stuff
                                    var doReturn = verbHandlerForMessage.HandleMessage(ref this.ServerSocketManagers, ref this.ClientSocketManagers);

                                    if (doReturn)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //catch any exceptions, log the error, and discard the message
                            this.ErrorMessages.Add(ex.Message);
                        }
                    });
        }

        //send a hello using an existing socketManager
        public bool SendConnect(ref SocketManager socketManager)
        {
            try
            {
                var message = new ConnectMessage { msg_type = MessageType.CONNECT, msg_data = new ConnectData { dst_node_id = socketManager.LocalNodeId, src_node_id = this.ThisNodeId } };

                var connectJson = JsonConvert.SerializeObject(message);

                var encodedConnectMsg = Encoding.ASCII.GetBytes(connectJson);

                this.LocalUdpClient.SendAsync(encodedConnectMsg, encodedConnectMsg.Length, socketManager.DestinationEndpoint.Address.ToString(), 9001);

                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessages.Add(ex.ToString());
                return false;
            }
        }

        private IVerbHandler GetVerbHandlerForMessage(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.HELLO:
                    return new HelloVerbHandler();

                case MessageType.ROUTING:
                    return new RoutingVerbHandler();

                case MessageType.CONNECT:
                    return new ConnectVerbHandler();

                default:
                    throw new Exception("No known verb handler");
            }
        }
    }
}