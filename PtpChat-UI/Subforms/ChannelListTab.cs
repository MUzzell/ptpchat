namespace PtpChat.UI.Subforms
{
	using System;
	using System.Windows.Forms;

	using Base.Interfaces;
	using Base.EventArguements;

	public partial class ChannelListTab : UserControl, IEventManager
	{
		private IChannelManager channelManager { get; set; }

		public ChannelListTab()
		{
			InitializeComponent();
		}

		public IDataManager DataManager
		{
			set
			{
				this.channelManager = value.ChannelManager;
			}
		}

		private void dataTreeListView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
