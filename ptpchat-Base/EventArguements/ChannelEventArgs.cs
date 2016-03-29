namespace PtpChat.Base.EventArguements
{
	using System;

	using PtpChat.Base.Classes;

	public class ChannelEventArgs : EventArgs
	{
		public Channel Channel { get; set; }
	}
}