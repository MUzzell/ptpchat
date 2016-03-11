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
    using ptpchat.VerbHandlers;

    public class PTPClient
    {
        public PTPClient()
        {
            this.ErrorMessages = new PtpList<string>();
            this.ThisNodeId = Guid.NewGuid();

            this.ServerSocketManagers = new PtpList<SocketManager>();
            this.ClientSocketManagers = new PtpList<SocketManager>();
        }

        public Guid ThisNodeId;

        public PtpList<SocketManager> ClientSocketManagers;
        public PtpList<SocketManager> ServerSocketManagers;

        public PtpList<string> ErrorMessages { get; }

        //send a hello to an ip address
        //create a new socket manager and send the hello, then return the socket manager
        public SocketManager SendHello(IPAddress ip)
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
                                        NodeId = this.ThisNodeId,
                                        DestinationEndpoint = new IPEndPoint(ip, 0),
                                        LocalEndpoint = new IPEndPoint(IPAddress.Any, new Random().Next(10000, 65535))
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
                if (socketManager == null)
                {
                    //send a hello directly via IP instead of this
                    return false;
                }

                if (socketManager.UdpClient == null)
                {
                    //bind the udp client to the (random) local endpoint created above
                    socketManager.UdpClient = new UdpClient(socketManager.LocalEndpoint);
                }

                var helloJson = "{" + string.Format(JsonDefinitions.HelloJson, this.ThisNodeId, string.Empty) + "}";

                //create the byte array for the Hello JSON message data
                var encodedHelloMsg = Encoding.ASCII.GetBytes(helloJson);

                //send the hello msg to the server at the IP
                socketManager.UdpClient.SendAsync(encodedHelloMsg, encodedHelloMsg.Length, socketManager.DestinationEndpoint.Address.ToString(), 9001);

                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessages.Add(ex.ToString());
                return false;
            }
        }

        //start listening on a socket in a while loop
        public void StartListening(SocketManager socketManager)
        {
            Task.Run(
                async () =>
                    {
                        //the client for socket we want to listen on
                        using (var udpClient = socketManager.UdpClient)
                        {
                            //constantly listen 
                            while (true)
                            {
                                try
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
                                    var doReturn = verbHandlerForMessage.HandleMessage(ref socketManager, ref this.ServerSocketManagers, ref this.ClientSocketManagers);

                                    if (doReturn) break;
                                }
                                catch (Exception ex)
                                {
                                    //catch any exceptions, log the error, and discard the message
                                    this.ErrorMessages.Add(ex.Message);
                                }
                            }
                        }
                    });
        }

        private IVerbHandler GetVerbHandlerForMessage(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.HELLO:
                    return new HelloVerbHandler();

                case MessageType.ROUTING:
                    return new RoutingVerbHandler();

                default:
                    throw new Exception("No known verb handler");
            }
        }
    }
}