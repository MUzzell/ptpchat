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
			this.Messages = new List<Message>();
		}

        public Guid NodeId { get; set; }

        public IPAddress IpAddress { get; set; }

        public int Port { get; set; }

        public IPEndPoint IpEndPoint => new IPEndPoint(this.IpAddress, this.Port);

        public DateTime? LastRecieve { get; set; }

        public DateTime? Added { get; set; }

        public DateTime? LastSend { get; set; }

        public string Version { get; set; }

        public IList<Channel> Channels { get; }

		public IList<Message> Messages { get; }

		public Guid? SeenThrough { get; set; }
	}
}