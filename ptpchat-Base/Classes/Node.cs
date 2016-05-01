namespace PtpChat.Base.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class Node
    {
        public bool IsConnected = false;

        public bool IsStartUpNode = false;

        public IList<Channel> Channels { get; }

        public IPEndPoint IpEndPoint
		{
			get
			{
				if (this.IpAddress == null || !this.Port.HasValue)
					throw new InvalidOperationException("Node: Unable to create enpoint; port or address is null");

				return new IPEndPoint(this.IpAddress, this.Port.Value);
			}
		}

        public IList<ChatMessage> Messages { get; }

        public string Status => this.IsConnected ? "Online" : "Offline";

        public DateTime? Added { get; set; }

        public IPAddress IpAddress { get; set; }

        public DateTime? LastRecieve { get; set; }

        public DateTime? LastSend { get; set; }

        //public Guid NodeId { get; set; }
        public NodeId NodeId { get; private set; }

        public int? Port { get; set; }

		public int Ttl { get; set; }

        public Guid? SeenThrough { get; set; }

        public string Version { get; set; }

		public IDictionary<string, string> Attributes { get; set; }

        public Node(NodeId nodeId)
        {
			if (nodeId == null)
			{
				throw new ArgumentNullException();
			}
			this.NodeId = nodeId;
            this.Channels = new List<Channel>();
            this.Messages = new List<ChatMessage>();
        }

		public void UpdateName(string name)
		{
			this.NodeId = new NodeId(name, this.NodeId.Id);
		}
	}
}