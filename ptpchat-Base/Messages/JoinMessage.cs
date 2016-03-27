namespace PtpChat.Base.Messages
{
	using System;

	public class JoinMessage : BaseMessage
	{
		public new JoinData msg_data { get; set; }
	}

	public class JoinData
	{
		public Guid msg_id { get; set; }

		public Guid node_id { get; set; }

		public string channel { get; set; }

		public Guid channel_id { get; set; }
	}
}
