namespace PtpChat.UI.Subforms
{
    using System;
    using System.Windows.Forms;

    using PtpChat.Base.Interfaces;

    public partial class ChannelListTab : UserControl, IEventManager
    {
        private IChannelManager channelManager { get; set; }

        public ChannelListTab()
        {
            this.InitializeComponent();
        }

        public IDataManager DataManager { set { this.channelManager = value.ChannelManager; } }

        private void dataTreeListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}