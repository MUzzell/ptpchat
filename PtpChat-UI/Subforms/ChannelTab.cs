namespace PtpChat.UI.Subforms
{
	using System;
	using System.Windows.Forms;
	using System.Collections.Generic;

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
			var msg = new RenderMessage
			{
				Member = message.MessageId.ToString(),
				Message = message.MessageContent,
				Time = message.DateSent.ToShortTimeString()
			};
			UI.Invoke(() => this.ChannelTab_Messages.AddObject(msg));
		}

		private void ChannelTab_BtnSubmit_Click(object sender, EventArgs e)
		{
			handler.SendMessage(this.channel.ChannelId, this.ChannelTab_TextEntry.Text);
			this.ChannelTab_TextEntry.Clear();
		}
		
	}

	struct RenderMessage
	{
		public string Member;
		public string Message;
		public string Time;
	}
}
