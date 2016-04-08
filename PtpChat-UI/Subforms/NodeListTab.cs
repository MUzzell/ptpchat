namespace PtpChat.UI.Subforms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using PtpChat.Base.EventArguements;
    using PtpChat.Base.Interfaces;

    public partial class NodeListTab : UserControl, IEventManager
    {
        private readonly ILogManager Logger;
        private readonly ISocketHandler SocketHandler;

        private INodeManager nodeManager { get; set; }

        public NodeListTab()
        {
            this.InitializeComponent();
        }

        public IDataManager DataManager
        {
            set
            {
                this.nodeManager = value.NodeManager;
                this.nodeManager.NodeAdd += this.RefreshNodeList;
                this.nodeManager.NodeUpdate += this.RefreshNodeList;

                var nodes = this.nodeManager.GetNodes();
                IList<NodeView> nodeList = new List<NodeView>();

                foreach (var node in nodes)
                {
                    nodeList.Add(new NodeView { Name = node.NodeId.ToString(), Status = node.IsConnected ? " " : "  " });
                }

                UI.Invoke(() => this.NodesTab_DataListView.SetObjects(nodeList));
            }
        }

        private NodeView GetSelectedNodeView() => this.NodesTab_DataListView?.FocusedObject as NodeView;

        public void RefreshNodeList(object sender, EventArgs e)
        {
            var node = (e as NodeEventArgs).Node;

            var nodeView = new NodeView { Name = node.NodeId.ToString(), Status = node.IsConnected ? " " : "  " };

            this.NodesTab_DataListView.UpdateObject(nodeView);
        }

        //public object OnlineImageGetter(object rowObject)
        //{
        //    NodeView s = (NodeView)rowObject;

        //    return s.Status == " " ? Image.FromFile(@"P:\Repos\ptpchat\PtpChat-UI\green.png") : Image.FromFile(@"P:\Repos\ptpchat\PtpChat-UI\red.png");
        //}

        private void NodesTab_DataListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.RightClickContextMenu.Show(this.NodesTab_DataListView, new Point(e.X, e.Y));
            }
        }

        private void NodesTab_DataListView_DoubleClick(object sender, EventArgs e)
        {
            //open connect for selected node
            var clickedNodeView = this.GetSelectedNodeView();

            var node = this.nodeManager.GetNodes(a => a.Value.NodeId == Guid.Parse(clickedNodeView.Name));

            //this.DataManager.NodeManager.
        }

        private void RightClickMenuClick_DetailsClick(object sender, EventArgs e)
        {
            //details for selected node
            var clickedNode = this.GetSelectedNodeView();
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