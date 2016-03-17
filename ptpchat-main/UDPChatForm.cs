namespace PtpChat.Main
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Windows.Forms;

    using ptpchat_Net.Socket_Manager;

    using PtpChat.Main.Client_Class;

    using Timer = System.Timers.Timer;

    public partial class UDPChatForm : Form
    {
        public UDPChatForm()
        {
            this.InitializeComponent();

            this.PtpClient = new PTPClient();

            //setup the ui manager
            UI.Initialize(this);

            UI.Invoke(() => { this.lbl_NodeId.Text = "My Node Id:" + this.PtpClient.ThisNodeId; });

            this.PtpClient.ServerSocketManagers.OnAdd += this.ServerSocketManagers_OnAdd;
            this.PtpClient.ClientSocketManagers.OnAdd += this.ClientSocketManagers_OnAdd;
            this.PtpClient.ErrorMessages.OnAdd += this.ErrorMessages_OnAdd;

            //read the server ip addresses from the config
            var serverIps = this.GetServerIpsFromConfig();

            SendHelloByIpDelegate hello = SendStartUpHello;
            hello.BeginInvoke(this.PtpClient, serverIps, this.StartPeriodicHelloTimer, null);
        }

        private readonly Timer periodicHelloTimer = new Timer { Interval = 10000 };

        private PTPClient PtpClient { get; }

        private void StartPeriodicHelloTimer(IAsyncResult result)
        {
            //once the startup hello to the server has finished, we need to start periodically sending the hello messages
            //setup the periodicHelloTimer function that will send the hello messages on a new thread
            this.periodicHelloTimer.Elapsed += (sender, args) =>
                {
                    var servers = this.PtpClient.ServerSocketManagers;

                    if (servers.Any())
                    {
                        servers.ForEach(
                            serverSocket =>
                                {
                                    SendHelloBySocketManagerDelegate hello = ClientSendHello;
                                    var ar = hello.BeginInvoke(this.PtpClient, serverSocket, null, null);
                                });
                    }

                    var clients = this.PtpClient.ClientSocketManagers;

                    if (clients.Any())
                    {
                        clients.ForEach(
                            clientSocket =>
                                {
                                    SendHelloBySocketManagerDelegate hello = ClientSendHello;
                                    var ar = hello.BeginInvoke(this.PtpClient, clientSocket, null, null);
                                });
                    }
                };

            this.periodicHelloTimer.Enabled = true;
        }

        private static void SendStartUpHello(PTPClient ptpClient, object ipAddresses)
        {
            if (ptpClient == null) //no ptpclient, dont even try
            {
                return;
            }

            var addresses = ipAddresses as List<IPAddress>;

            if (addresses == null) //ip addresses dont parse
            {
                return;
            }

            addresses.ForEach(
                ip =>
                    {
                        //send the hello, and return the socket manager create in doing so
                        var socketManager = ptpClient.SendHello(ip, 9001, true);

                        if (socketManager == null)
                        {
                            //something went wrong with the hello
                            return;
                        }

                        ptpClient.StartListening();

                        if (!ptpClient.ServerSocketManagers.Contains(socketManager))
                        {
                            //and store the socket manager instance now that we've created a connection
                            ptpClient.ServerSocketManagers.Add(socketManager);
                        }
                    });
        }

        //send a hello using an already existing socketManager, 
        private static void ClientSendHello(PTPClient ptpClient, SocketManager socketManager)
        {
            if (ptpClient == null) //no ptpclient, dont even try
            {
                return;
            }

            var sendSuccessful = ptpClient.SendHello(ref socketManager);

            if (!sendSuccessful)
            {
                //something went wrong with the hello
            }
        }

        private static void ClientSendConnect(PTPClient ptpClient, SocketManager socketManager)
        {
            if (ptpClient == null) //no ptpclient, dont even try
            {
                return;
            }

            var sendSuccessful = ptpClient.SendConnect(ref socketManager);

            if (!sendSuccessful)
            {
                //something went wrong with the hello
            }
        }

        private List<IPAddress> GetServerIpsFromConfig()
        {
            var ipListCsv = ConfigurationManager.AppSettings["Server_Ips"];

            if (string.IsNullOrWhiteSpace(ipListCsv))
            {
                //no string in config file
                return new List<IPAddress>();
            }

            var ipStrings = ipListCsv.Contains(';') ? ipListCsv.Split(';').Where(ipString => !string.IsNullOrWhiteSpace(ipString)).Distinct().ToList() : new List<string> { ipListCsv };

            if (!ipStrings.Any())
            {
                //no valid strings
                return new List<IPAddress>();
            }

            var ipAddresses = new List<IPAddress>();

            ipStrings.ForEach(
                ip =>
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(ip))
                            {
                                ipAddresses.Add(IPAddress.Parse(ip));
                            }
                        }
                        catch
                        {
                            //ip string couldn't be parsed as an Ip address, ignore it
                        }
                    });

            return ipAddresses;
        }

        //#########
        //Event Handlers
        //#########
        private void ErrorMessages_OnAdd(object sender, string error)
        {
            UI.Invoke(() => this.listBox_ErrorLog.Items.Add(error));
        }

        private void ClientSocketManagers_OnAdd(object sender, SocketManager socketManager)
        {
            //once we've added them to our clients list, we set up the onchange event
            //so the UI will update with any changes to the socket manager
            socketManager.PropertyChanged += this.ClientSocketManagers_PropertyChanged;

            UI.Invoke(
                () =>
                    {
                        try
                        {
                            var castList = this.grid_Clients.Rows.Cast<DataGridViewRow>().ToList();

                            var rowToupdate = castList.FirstOrDefault(r => r.Cells["clients_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId));

                            if (rowToupdate == null)
                            {
                                this.grid_Clients.Rows.Add(
                                    socketManager.DestinationNodeId,
                                    socketManager.DestinationEndpoint.Address,
                                    socketManager.DestinationEndpoint.Port,
                                    socketManager.LastHelloRecieved.ToShortTimeString());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    });
        }

        private void ServerSocketManagers_OnAdd(object sender, SocketManager socketManager)
        {
            //once we've added them to our server list, we set up the onchange event
            //so the UI will update with any changes to the socket manager
            socketManager.PropertyChanged += this.ServerSocketManagers_PropertyChanged;

            UI.Invoke(
                () =>
                    {
                        try
                        {
                            var castList = this.grid_Servers.Rows.Cast<DataGridViewRow>().ToList();

                            var rowToupdate = castList.FirstOrDefault(r => r.Cells["servers_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId));

                            if (rowToupdate == null)
                            {
                                this.grid_Servers.Rows.Add(
                                    socketManager.DestinationNodeId,
                                    socketManager.DestinationEndpoint.Address,
                                    socketManager.DestinationEndpoint.Port,
                                    socketManager.LastHelloRecieved.ToShortTimeString());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    });
        }

        //#########
        //Delegates
        //#########
        private delegate void SendHelloByIpDelegate(PTPClient ptpClient, object ipAddresses);

        private delegate void SendHelloBySocketManagerDelegate(PTPClient ptpClient, SocketManager socketManager);

        private void ClientSocketManagers_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var socketManager = (SocketManager)sender;

            UI.Invoke(
                () =>
                    {
                        try
                        {
                            var castList = this.grid_Clients.Rows.Cast<DataGridViewRow>().ToList();

                            var rowToupdate = castList.FirstOrDefault(r => r.Cells["clients_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId))
                                              ?? castList.FirstOrDefault(r => r.Cells["clients_IpAddressCol"].Value.Equals(socketManager.DestinationEndpoint.Address));

                            if (rowToupdate == null)
                            {
                                return;
                            }

                            this.grid_Clients.Rows[rowToupdate.Index].Cells[0].Value = socketManager.DestinationNodeId;
                            this.grid_Clients.Rows[rowToupdate.Index].Cells[1].Value = socketManager.DestinationEndpoint.Address;
                            this.grid_Clients.Rows[rowToupdate.Index].Cells[2].Value = socketManager.DestinationEndpoint.Port;
                            this.grid_Clients.Rows[rowToupdate.Index].Cells[3].Value = socketManager.LastHelloRecieved.ToLocalTime();
                            this.grid_Clients.Rows[rowToupdate.Index].Cells[4].Value = socketManager.LastHelloRecieved != DateTime.MinValue;
                        }
                        catch (Exception ex)
                        {
                        }
                    });
        }

        private void ClientSocketManagers_SelectionChanged(object sender, EventArgs e)
        {
            //if client socket manager is selected, show connect button, else hide it
        }

        private void ServerSocketManagers_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var socketManager = (SocketManager)sender;

            UI.Invoke(
                () =>
                    {
                        try
                        {
                            var castList = this.grid_Servers.Rows.Cast<DataGridViewRow>().ToList();

                            var rowToupdate = castList.FirstOrDefault(r => r.Cells["servers_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId))
                                              ?? castList.FirstOrDefault(r => r.Cells["servers_IpAddressCol"].Value.Equals(socketManager.DestinationEndpoint.Address));

                            if (rowToupdate == null)
                            {
                                return;
                            }

                            this.grid_Servers.Rows[rowToupdate.Index].Cells[0].Value = socketManager.DestinationNodeId;
                            this.grid_Servers.Rows[rowToupdate.Index].Cells[1].Value = socketManager.DestinationEndpoint.Address;
                            this.grid_Servers.Rows[rowToupdate.Index].Cells[2].Value = socketManager.DestinationEndpoint.Port;
                            this.grid_Servers.Rows[rowToupdate.Index].Cells[3].Value = socketManager.LastHelloRecieved.ToLocalTime();
                        }
                        catch (Exception ex)
                        {
                        }
                    });
        }
    }
}