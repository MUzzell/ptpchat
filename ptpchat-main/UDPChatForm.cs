namespace PtpChat.Main
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Windows.Forms;

    using PtpChat.Main.Client_Class;
    using PtpChat.Main.Util;
    using PtpChat.Net.Socket_Manager;

    using Timer = System.Timers.Timer;

    public partial class UDPChatForm : Form
    {
        private PTPClient PtpClient { get; }


        public UDPChatForm(ConfigManager manager)
            : this()
        {

        }

        public UDPChatForm()
        {
            this.InitializeComponent();
            //read the server ip addresses from the config
            var serverIps = this.GetServerIpsFromConfig();

            this.PtpClient = new PTPClient(serverIps);

            //setup the ui manager
            UI.Initialize(this);

            UI.Invoke(() => { this.lbl_NodeId.Text = "My Node Id:" + this.PtpClient.ThisNodeId; });

            this.PtpClient.ServerSocketManagers.OnAdd += this.ServerSocketManagers_OnAdd;
            this.PtpClient.ClientSocketManagers.OnAdd += this.ClientSocketManagers_OnAdd;
            this.PtpClient.ErrorMessages.OnAdd += this.ErrorMessages_OnAdd;
        }




        //send a hello using an already existing socketManager, 
        //private static void ClientSendHello(PTPClient ptpClient, SocketManager socketManager)
        //{
        //    if (ptpClient == null) //no ptpclient, dont even try
        //    {
        //        return;
        //    }

        //    var sendSuccessful = ptpClient.SendHello(ref socketManager);

        //    if (!sendSuccessful)
        //    {
        //        //something went wrong with the hello
        //    }
        //}

        //private static void ClientSendConnect(PTPClient ptpClient, SocketManager socketManager)
        //{
        //    if (ptpClient == null) //no ptpclient, dont even try
        //    {
        //        return;
        //    }

        //    var sendSuccessful = ptpClient.SendConnect(ref socketManager);

        //    if (!sendSuccessful)
        //    {
        //        //something went wrong with the hello
        //    }
        //}

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

                                    socketManager.LastHelloRecieved != DateTime.MinValue
                                        ? socketManager.LastHelloRecieved.ToLocalTime().ToShortTimeString()
                                        : "--",

                                    socketManager.LastHelloRecieved != DateTime.MinValue 
                                        ? "Connectable" 
                                        : "Not Connectable");
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

                            this.grid_Clients.Rows[rowToupdate.Index].Cells[4].Value = socketManager.LastHelloRecieved != DateTime.MinValue 
                                ? "Connectable" 
                                : "Not Connectable";

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