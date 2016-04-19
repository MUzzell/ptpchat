namespace PtpChat.UI.Subforms
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using BrightIdeasSoftware;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;

    public partial class NodeListTab : UserControl, IEventManager
    {
        private readonly ILogManager Logger;
        private readonly ISocketHandler SocketHandler;

        private INodeManager NodeManager { get; set; }

        public NodeListTab()
        {
            this.InitializeComponent();
        }

        public IDataManager DataManager
        {
            set
            {
                this.NodeManager = value.NodeManager;
                this.NodeManager.NodeAdd += this.RefreshNodeList;
                this.NodeManager.NodeUpdate += this.RefreshNodeList;

                var nodes = this.NodeManager.GetNodes().ToList();

                UI.Invoke(() => this.NodeListTab_Nodes.SetObjects(nodes));
            }
        }

        private Node GetSelectedNode() => this.NodeListTab_Nodes?.FocusedObject as Node;

        public void NewNode(object sender, EventArgs e)
        {
            var nodes = this.NodeManager.GetNodes().ToList();

            UI.Invoke(() => this.NodeListTab_Nodes.UpdateObjects(nodes));
        }

        public void RefreshNodeList(object sender, EventArgs e)
        {
            var nodes = this.NodeManager.GetNodes().ToList();

            UI.Invoke(() => this.NodeListTab_Nodes.UpdateObjects(nodes));
        }

        //public object OnlineImageGetter(object rowObject)
        //{
        //    NodeView s = (NodeView)rowObject;

        //    return s.Status == " " ? Image.FromFile(@"P:\Repos\ptpchat\PtpChat-UI\green.png") : Image.FromFile(@"P:\Repos\ptpchat\PtpChat-UI\red.png");
        //}

        private void NodeListTab_Nodes_RightMouseClick(object sender, CellRightClickEventArgs e)
        {
            this.RightClickContextMenu.Show(this.NodeListTab_Nodes, new Point(e.Location.X, e.Location.Y));
        }

        private void NodeListTab_Nodes_DoubleMouseClick(object sender, EventArgs e)
        {
            //open connect for selected node
            var clickedNodeView = this.GetSelectedNode();

            var node = this.NodeManager.GetNodes(a => a.Value.NodeId == clickedNodeView.NodeId);

            //this.DataManager.ChannelManager.Add(new Channel() {});
            //this.DataManager.NodeManager.
        }

        private void RightClickMenuClick_DetailsClick(object sender, EventArgs e)
        {
            //details for selected node
            var clickedNode = this.GetSelectedNode();
        }

        private void RightClickMenuClick_DeleteClick(object sender, EventArgs e)
        {
        }
    }

    internal class NodeView
    {
        public string Name;

        public string Status;
    }
}