namespace PtpChat.UI.Subforms
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Linq;

	using PtpChat.Base.EventArguements;
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

        private NodeView GetSelectedNodeView() => this.NodeListTab_Nodes?.FocusedObject as NodeView;

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

        private void NodesTab_DataListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.RightClickContextMenu.Show(this.NodeListTab_Nodes, new Point(e.X, e.Y));
            }
        }

        private void NodesTab_DataListView_DoubleClick(object sender, EventArgs e)
        {
            //open connect for selected node
            var clickedNodeView = this.GetSelectedNodeView();

            var node = this.NodeManager.GetNodes(a => a.Value.NodeId == Guid.Parse(clickedNodeView.Name));

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