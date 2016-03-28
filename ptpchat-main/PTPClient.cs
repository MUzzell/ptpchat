﻿namespace PtpChat.Main
{
    using System;
    using System.Collections.Generic;

    using PtpChat.Base.Classes;
    using PtpChat.Base.EventArguements;
    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;
    using PtpChat.Main.Managers;
    using PtpChat.Net;
    using PtpChat.Utility;
    using PtpChat.VerbHandlers.Handlers;

    public class PTPClient
    {
        public PTPClient(ConfigManager config)
        {
            this.logger = new Logger(config, "ptpchat");

            this.nodeManager = new NodeManager(this.logger);

            //TODO: remove this when we get a response from the server
            this.nodeManager.Add(new Node { IpAddress = config.InitialServerAddress, NodeId = Guid.Empty, Port = 9001, Added = DateTime.Now, LastSeen = DateTime.Now });

            this.nodeManager.NodeAdd += this.NodeManager_NodesChanged;
            this.nodeManager.NodeUpdate += this.NodeManager_NodesChanged;
            this.nodeManager.NodeDelete += this.NodeManager_NodesChanged;

            this.messageHandler = new MessageHandler(this.logger);

            //socket handler here used to create its UDP socket thread handling in the ctor
            this.socketHandler = new SocketHandler(this.logger, this.nodeManager, this.messageHandler);

            this.messageHandler.AddHandler(MessageType.HELLO, new HelloVerbHandler(this.logger, this.nodeManager, this.socketHandler));
            this.messageHandler.AddHandler(MessageType.ROUTING, new RoutingVerbHandler(this.logger, this.nodeManager, this.socketHandler));

            this.socketHandler.Start();
        }

        private readonly ILogManager logger;

        private readonly MessageHandler messageHandler;

        private readonly INodeManager nodeManager;

        private readonly ISocketHandler socketHandler;

        public event EventHandler OnNodesChange;

        public IEnumerable<Node> GetNodes() => this.nodeManager?.GetNodes();

        private void NodeManager_NodesChanged(object sender, EventArgs e)
        {
            this.OnNodesChange?.Invoke(sender, (NodeEventArgs)e);
        }

        /**
            this.ErrorMessages = new PtpList<string>();
            this.ThisNodeId = Guid.NewGuid();

            //this.LocalEndpoint = new IPEndPoint(IPAddress.Any, new Random().Next(10000, 65535));
            this.LocalEndpoint = new IPEndPoint(IPAddress.Any, 23456);

            this.LocalUdpClient = new UdpClient(this.LocalEndpoint);

            this.ServerSocketManagers = new PtpList<SocketManager>();
            this.ClientSocketManagers = new PtpList<SocketManager>();

            var worker = new BackgroundWorker();

            // what to do in the background thread
            worker.DoWork += async delegate
                {
                    foreach (var ip in ServerIps)
                    {
                        //send the hello, 
                        await this.SendNewHello(ip, 9001, true);
                    }
                };

            worker.RunWorkerCompleted += delegate { this.StartPeriodicHelloTimer(); };

            worker.RunWorkerAsync();

            this.StartListening();
        }

        private readonly Timer periodicHelloTimer = new Timer { Interval = 10000 };

        public PtpList<SocketManager> ClientSocketManagers;

        public PtpList<SocketManager> ServerSocketManagers;

        public Guid ThisNodeId { get; }

        public IPEndPoint LocalEndpoint { get; }

        public UdpClient LocalUdpClient { get; }

        public PtpList<string> ErrorMessages { get; }

        private void StopPeriodicHelloTimer()
        {
            this.periodicHelloTimer.Enabled = false;
        }

        private void StartPeriodicHelloTimer()
        {
            //once the startup hello to the server has finished, we need to start periodically sending the hello messages
            //setup the periodicHelloTimer function that will send the hello messages on a new thread
            this.periodicHelloTimer.Elapsed += async (sender, args) =>
                {
                    if (this.ServerSocketManagers.Any())
                    {
                        foreach (var serverSocket in this.ServerSocketManagers)
                        {
                            await this.SendHello(serverSocket);
                        }
                    }

                    if (!this.ClientSocketManagers.Any())
                    {
                        return;
                    }

                    foreach (var clientSocket in this.ClientSocketManagers)
                    {
                        await this.SendHello(clientSocket);
                    }
                };

            this.periodicHelloTimer.Enabled = true;
        }

        //send a hello to an ip address
        //create a new socket manager and send the hello, then return the socket manager
        public async Task SendNewHello(IPAddress ip, int port, bool isServer)
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

            await this.SendHello(socketManager);

            if (isServer && !this.ServerSocketManagers.Contains(socketManager))
            {
                this.ServerSocketManagers.Add(socketManager);
            }

            if (!isServer && !this.ClientSocketManagers.Contains(socketManager))
            {
                this.ClientSocketManagers.Add(socketManager);
            }
        }

        //send a hello using an existing socketManager
        public async Task SendHello(SocketManager socketManager)
        {
            try
            {
                //check that we dont hello ourselves
                if (socketManager?.DestinationEndpoint == null)
                {
                    return;
                }

                //var helloJson = "{" + string.Format(JsonDefinitions.HelloJson, this.ThisNodeId, string.Empty) + "}";
				var helloJson = "wew lad";
                //create the byte array for the Hello JSON message data
                var encodedHelloMsg = Encoding.ASCII.GetBytes(helloJson);

                //send the hello msg to the server at the IP
                await
                    this.LocalUdpClient.SendAsync(
                        encodedHelloMsg,
                        encodedHelloMsg.Length,
                        socketManager.DestinationEndpoint.Address.ToString(),
                        socketManager.DestinationEndpoint.Port > 0 ? socketManager.DestinationEndpoint.Port : 9001);
            }
            catch (Exception ex)
            {
                this.ErrorMessages.Add(ex.ToString());
            }
        }

        //start listening on a socket in a while loop
        public void StartListening()
        {
            Task.Run(
                async () =>
                    {
                        //constantly listen 
                        while (true)
                        {
                            try
                            {
                                //wait for an incoming message
                                var asyncResult = await this.LocalUdpClient.ReceiveAsync();

                                //read and parse the json
                                var messageJson = Encoding.ASCII.GetString(asyncResult.Buffer);

                                //cast the message to a BaseMessage so we can use the message type
                                var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(messageJson);

                                //get a verb handler for the message
                                var verbHandlerForMessage = this.GetVerbHandlerForMessage(baseMessage.msg_type);

                                //pass the message to the verb handler, and have it parse the message
                                verbHandlerForMessage.ParseBaseMessage(messageJson);

                                //and call the handle method on the verb handler, passing in the things it will need to do stuff
                                var doReturn = verbHandlerForMessage.HandleMessage(asyncResult.RemoteEndPoint, ref this.ServerSocketManagers, ref this.ClientSocketManagers);

                                if (doReturn)
                                {
                                    break;
                                }
                            }
                            catch (SocketException ex)
                            {
                                this.ErrorMessages.Add("Error receiving data.");
                            }
                            catch (Exception ex)
                            {
                                //catch any exceptions, log the error, and discard the message
                                this.ErrorMessages.Add(ex.Message);
                            }
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

        //#########
        //Delegates
        //#########
        private delegate void SendHelloByIpDelegate(List<IPAddress> ipAddresses);

        private delegate void SendHelloBySocketManagerDelegate(SocketManager socketManager);*/
    }
}