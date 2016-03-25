namespace PtpChat.Base.Classes
{
	using System;

	public class ChannelMessage
	{
		public Guid MessageId { get; set; }

		public string Message { get; set; }

		public DateTime DateSent { get; set; }

		public Guid ChannelId { get; set; }

		public Guid SenderId { get; set; }
	}
}
