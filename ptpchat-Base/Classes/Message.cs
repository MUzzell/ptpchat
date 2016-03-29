namespace PtpChat.Base.Classes
{
	using System;

	public class Message
	{ 
		public Guid MessageId { get; set; }

		public string MessageContent { get; set; }

		public DateTime DateSent { get; set; }

		public Guid ChannelId { get; set; }

		public Guid SenderId { get; set; }
	}
}
