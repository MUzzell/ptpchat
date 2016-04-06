namespace PtpChat.UI.Subforms
{
	using System;
	using System.Windows.Forms;
	using System.Collections.Generic;

	using Base.EventArguements;
	using Base.Interfaces;

	public partial class NodeListTab : UserControl, IEventManager
	{
		private INodeManager nodeManager { get; set; }

		public NodeListTab()
		{
			InitializeComponent();
		}

		public IDataManager DataManager
		{
			set
			{
				this.nodeManager = value.NodeManager;
				nodeManager.NodeAdd += RefreshNodeList;
				nodeManager.NodeUpdate += RefreshNodeList;
			}
		}

		public void RefreshNodeList(object sender, EventArgs e)
		{
			var nodes = this.nodeManager.GetNodes();
			IList<NodeView> nodeList = new List<NodeView>();
			foreach (var node in nodes)
			{
				nodeList.Add(new NodeView
				{
					Name = node.NodeId.ToString(),
					Status = node.IsConnected ? "Online" : "Offline"
				});
			}

			UI.Invoke(() => NodesTab_DataListView.SetObjects(nodeList));
			
		}
	}

	struct NodeView
	{
		public string Name;
		public string Status;
	}
}
