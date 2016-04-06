namespace PtpChat.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using PtpChat.Utility;
    using PtpChat.UI.Subforms;
	using PtpChat.Main;

    public partial class MainForm : Form
    {
		
		private PTPClient PtpClient { get; }

		public MainForm(ConfigManager manager)
            : this()
        { }

        private MainForm()
        {
            this.InitializeComponent();
            this.IsMdiContainer = true;

            //setup the ui manager
            UI.Initialize(this);
			
            this.PtpClient = new PTPClient(new ConfigManager());
            this.PtpClient.NodeChanged += this.PtpClient_OnNodesChange;
        }

        private void Nodes_RefreshNodesView(NodesForm existingNodeForm)
        {
            existingNodeForm?.RefreshNodes(this.PtpClient);
        }

        private void PtpClient_OnNodesChange(object sender, EventArgs e)
        {
            UI.Invoke(() =>
            {
				LeftTabControl_NodeList.RefreshNodeList();
            });
        }

        //private void PtpClient_OnNodesChange(object sender, EventArgs e)
        //{
        //    var eventArgs = (NodeEventArgs)e;
        //    var node = eventArgs.Node;

        //    UI.Invoke(() =>
        //    {
        //        var castList = this.dgv_new.Rows.Cast<DataGridViewRow>().ToList();

        //        var rowToupdate = castList.FirstOrDefault(r => r.Cells["servers_NodeIdCol"].Value.Equals(node.NodeId))
        //                          ?? castList.FirstOrDefault(r => r.Cells["servers_NodeIdCol"].Value.Equals(Guid.Empty));

        //        if (rowToupdate == null)
        //        {
        //            this.dgv_new.Rows.Add(
        //                node.NodeId,
        //                node.IpAddress,
        //                node.Port,
        //                node.LastSeen.Value != null ? node.LastSeen.Value.ToLocalTime().ToShortTimeString() : "--",
        //                node.LastSeen.Value != null ? "Connectable" : "Not Connectable");
        //        }
        //        else
        //        {
        //            this.grid_Clients.Rows[rowToupdate.Index].Cells[0].Value = node.NodeId;
        //            this.grid_Clients.Rows[rowToupdate.Index].Cells[1].Value = node.IpAddress;
        //            this.grid_Clients.Rows[rowToupdate.Index].Cells[2].Value = node.Port;
        //            this.grid_Clients.Rows[rowToupdate.Index].Cells[3].Value = node.LastSeen.Value != null ? node.LastSeen.Value.ToLocalTime().ToString() : "--";
        //            this.grid_Clients.Rows[rowToupdate.Index].Cells[4].Value = node.LastSeen.Value != null ? "Connectable" : "Not Connectable";
        //        }
        //    });
        //}

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


        private void btn_Nodes_Click(object sender, EventArgs e)
        {
            UI.Invoke(() =>
                {
					/*
                    this.pnl_SubForm.Visible = true;
                    this.pnl_SubForm.Controls.Clear();

                    var existingNodeForm = this.Forms.OfType<NodesForm>().ToList().FirstOrDefault();
                    this.Nodes_RefreshNodesView(existingNodeForm);

                    this.pnl_SubForm.Controls.Add(existingNodeForm);
					*/
                });
        }

        private void btn_Channels_Click(object sender, EventArgs e)
        {
            UI.Invoke(() =>
               {
				   /*
                   this.pnl_SubForm.Visible = true;

                   this.pnl_SubForm.Controls.Clear();

                   var serversForm = this.Forms.OfType<ChannelsForm>().FirstOrDefault();

                   if (serversForm != null)
                   {
                       this.pnl_SubForm.Controls.Add(serversForm);
                   }
				   */
               });
        }

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		private void LeftTabControl_ChannelList_Load(object sender, EventArgs e)
		{

		}


		//}

		////#########
		////Event Handlers
		////#########
		//private void ErrorMessages_OnAdd(object sender, string error)
		//{

		//    UI.Invoke(() => this.listBox_ErrorLog.Items.Add(error));

		//private void ClientSocketManagers_OnAdd(object sender, SocketManager socketManager)
		//{
		//    //once we've added them to our clients list, we set up the onchange event
		//    //so the UI will update with any changes to the socket manager
		//    socketManager.PropertyChanged += this.ClientSocketManagers_PropertyChanged;

		//    UI.Invoke(
		//        () =>
		//            {
		//                try
		//                {
		//                    var castList = this.grid_Clients.Rows.Cast<DataGridViewRow>().ToList();

		//                    var rowToupdate = castList.FirstOrDefault(r => r.Cells["clients_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId));

		//                    if (rowToupdate == null)
		//                    {
		//                        this.grid_Clients.Rows.Add(
		//                            socketManager.DestinationNodeId,
		//                            socketManager.DestinationEndpoint.Address,
		//                            socketManager.DestinationEndpoint.Port,
		//                            socketManager.LastHelloRecieved != DateTime.MinValue ? socketManager.LastHelloRecieved.ToLocalTime().ToShortTimeString() : "--",
		//                            socketManager.LastHelloRecieved != DateTime.MinValue ? "Connectable" : "Not Connectable");
		//                    }

		//                }
		//                catch (Exception ex)
		//                {
		//                }
		//            });
		//}

		//private void ServerSocketManagers_OnAdd(object sender, SocketManager socketManager)
		//{
		//    //once we've added them to our server list, we set up the onchange event
		//    //so the UI will update with any changes to the socket manager
		//    socketManager.PropertyChanged += this.ServerSocketManagers_PropertyChanged;

		//    UI.Invoke(
		//        () =>
		//            {
		//                try
		//                {
		//                    var castList = this.grid_Servers.Rows.Cast<DataGridViewRow>().ToList();

		//                    var rowToupdate = castList.FirstOrDefault(r => r.Cells["servers_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId));

		//                    if (rowToupdate == null)
		//                    {
		//                        this.grid_Servers.Rows.Add(
		//                            socketManager.DestinationNodeId,
		//                            socketManager.DestinationEndpoint.Address,
		//                            socketManager.DestinationEndpoint.Port,
		//                            socketManager.LastHelloRecieved.ToShortTimeString());
		//                    }
		//                }
		//                catch (Exception ex)
		//                {
		//                }
		//            });
		//}

		//private void ClientSocketManagers_PropertyChanged(object sender, PropertyChangedEventArgs args)
		//{
		//    var socketManager = (SocketManager)sender;

		//    UI.Invoke(
		//        () =>
		//            {
		//                try
		//                {
		//                    var castList = this.grid_Clients.Rows.Cast<DataGridViewRow>().ToList();

		//                    var rowToupdate = castList.FirstOrDefault(r => r.Cells["clients_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId))
		//                                      ?? castList.FirstOrDefault(r => r.Cells["clients_IpAddressCol"].Value.Equals(socketManager.DestinationEndpoint.Address));

		//                    if (rowToupdate == null)
		//                    {
		//                        return;
		//                    }

		//                    this.grid_Clients.Rows[rowToupdate.Index].Cells[0].Value = socketManager.DestinationNodeId;
		//                    this.grid_Clients.Rows[rowToupdate.Index].Cells[1].Value = socketManager.DestinationEndpoint.Address;
		//                    this.grid_Clients.Rows[rowToupdate.Index].Cells[2].Value = socketManager.DestinationEndpoint.Port;
		//                    this.grid_Clients.Rows[rowToupdate.Index].Cells[3].Value = socketManager.LastHelloRecieved.ToLocalTime();

		//                    this.grid_Clients.Rows[rowToupdate.Index].Cells[4].Value = socketManager.LastHelloRecieved != DateTime.MinValue ? "Connectable" : "Not Connectable";
		//                }
		//                catch (Exception ex)
		//                {
		//                }
		//            });
		//}

		//private void ClientSocketManagers_SelectionChanged(object sender, EventArgs e)
		//{
		//    //if client socket manager is selected, show connect button, else hide it
		//}

		//private void ServerSocketManagers_PropertyChanged(object sender, PropertyChangedEventArgs args)
		//{
		//    var socketManager = (SocketManager)sender;

		//    UI.Invoke(
		//        () =>
		//            {
		//                try
		//                {
		//                    var castList = this.grid_Servers.Rows.Cast<DataGridViewRow>().ToList();

		//                    var rowToupdate = castList.FirstOrDefault(r => r.Cells["servers_NodeIdCol"].Value.Equals(socketManager.DestinationNodeId))
		//                                      ?? castList.FirstOrDefault(r => r.Cells["servers_IpAddressCol"].Value.Equals(socketManager.DestinationEndpoint.Address));

		//                    if (rowToupdate == null)
		//                    {
		//                        return;
		//                    }

		//                    this.grid_Servers.Rows[rowToupdate.Index].Cells[0].Value = socketManager.DestinationNodeId;
		//                    this.grid_Servers.Rows[rowToupdate.Index].Cells[1].Value = socketManager.DestinationEndpoint.Address;
		//                    this.grid_Servers.Rows[rowToupdate.Index].Cells[2].Value = socketManager.DestinationEndpoint.Port;
		//                    this.grid_Servers.Rows[rowToupdate.Index].Cells[3].Value = socketManager.LastHelloRecieved.ToLocalTime();
		//                }
		//                catch (Exception ex)
		//                {
		//                }
		//            });
		//}
	}
}