using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptpchat.Client_Class
{
    using System.Net;
    using System.Net.Sockets;

    using Newtonsoft.Json;

    using ptpchat.Class_Definitions;

    public class PTPClient
    {
        private List<string> ErrorMessages { get; }

        private List<SocketManager> ServerSocketManagers { get; set; }
        //private List<SocketManager> ClientSocketManagers { get; set; }

        public PTPClient()
        {
            this.ErrorMessages = new List<string>();
            this.ServerSocketManagers = new List<SocketManager>();
        }

        public void SendHelloToServer(List<IPAddress> IpAddresses)
        {
            //sending hello messages out to a number of Ip addresses, foreach ip
            IpAddresses.ForEach(
                ip =>
                    {
                        try
                        {
                            SocketManager socketManager;

                            //do we have a socket manager for this ip address? 
                            if (this.ServerSocketManagers.Any(a => Equals(a.DestinationIp, ip)))
                            {
                                //if so, use it
                                socketManager = this.ServerSocketManagers.First(a => Equals(a.DestinationIp, ip));
                            }
                            else
                            {
                                //else create the socket manager instance for this ip address as we've not seen it before
                                socketManager = new SocketManager
                                {
                                    DestinationIp = ip,
                                    LocalEndpoint = new IPEndPoint(IPAddress.Any, new Random().Next(10000, 65535)),

                                };
                            }

                            //bind the udp client to the (random) local endpoint created above
                            socketManager.UdpClient = new UdpClient(socketManager.LocalEndpoint);

                            //create the byte array for the Hello JSON message data
                            var msg = Encoding.ASCII.GetBytes(JsonDefinitions.HelloJson);

                            //send the hello msg to the server at the IP
                            socketManager.UdpClient.SendAsync(msg, msg.Length, ip.ToString(), 9001);

                            //and store the socket manager instance now that we've created a connection
                            this.ServerSocketManagers.Add(socketManager);

                            //start this thread listening for incoming connections
                            this.StartListening(socketManager);
                        }
                        catch (Exception ex)
                        {
                            this.ErrorMessages.Add(ex.ToString());
                        }
                    });

        }

        private void StartListening(SocketManager socketManager)
        {
            Task.Run(async () =>
            {
                using (var udpClient = socketManager.UdpClient)
                {
                    while (true)
                    {
                        var asyncResult = await udpClient.ReceiveAsync();
                        var messageJson = Encoding.ASCII.GetString(asyncResult.Buffer);

                        try
                        {
                            BaseMessage baseMessage = JsonConvert.DeserializeObject<BaseMessage>(messageJson);
                        }
                        catch (Exception ex)
                        {
                            
                        }


                        //var verbHandlerForMessage = GetVerbHandlerForMessage();
                    }
                }
            });
        }

        private void GetVerbHandlerForMessage(BaseMessage baseMessage)
        {
            
        }
    }
}
