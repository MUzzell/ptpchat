namespace PtpChat.UI.Subforms
{
	using System;
	using System.Windows.Forms;

	using Base.Interfaces;
	using Base.Classes;

	public partial class ChannelTab : UserControl, IChannelTab
	{
		private readonly Channel channel;
		private readonly IChannelTabHandler handler;

		public ChannelTab(Channel channel, IChannelTabHandler handler)
		{
			this.channel = channel;
			this.handler = handler;

			InitializeComponent();
		}

		public void MessageRecieved(ChatMessage message)
		{
			throw new NotImplementedException();
		}

		private void ChannelTab_BtnSubmit_Click(object sender, EventArgs e)
		{
			handler.SendMessage(this.channel.ChannelId, this.ChannelTab_TextEntry.Text);
			this.ChannelTab_TextEntry.Clear();
		}
		
	}
}
