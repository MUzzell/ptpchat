namespace PtpChat.Base.Classes
{
    using System;
    using System.Net;

    public class Node
    {
        public Guid NodeId { get; set; }

        public IPAddress IpAddress { get; set; }

        public int Port { get; set; }

		public IPEndPoint IpEndPoint { get { return new IPEndPoint(this.IpAddress, this.Port); } }

		public DateTime LastSeen { get; set; }

		public string Version { get; set; }
	}
}