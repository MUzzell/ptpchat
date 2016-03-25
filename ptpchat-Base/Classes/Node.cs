namespace PtpChat.Base.Classes
{
	using System;
	using System.Collections.Generic;
	using System.Net;

	public class Node
    {
		public Node()
		{
			this.Channels = new List<Channel>();
			this.Messages = new List<ChannelMessage>();
		}

        public Guid NodeId { get; set; }

        public IPAddress IpAddress { get; set; }

        public int Port { get; set; }

        public IPEndPoint IpEndPoint { get { return new IPEndPoint(this.IpAddress, this.Port); } }

        public DateTime? LastSeen { get; set; }

		public DateTime? Added { get; set; }

		public DateTime? LastSend { get; set; }

        public string Version { get; set; }

		public IList<Channel> Channels { get; }

		public IList<ChannelMessage> Messages { get; }
    }
}