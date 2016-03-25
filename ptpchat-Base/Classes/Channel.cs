namespace PtpChat.Base.Classes
{
	using System;
	using System.Collections.Generic;

	public class Channel
	{
		public Guid ChannelId { get; set; }

		public IList<Guid> Nodes { get; set; }

		public string ChannelName { get; set; }

		public bool Closed { get; set; }

		public IList<Guid> Messages { get; set; }
	}
}
