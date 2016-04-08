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

        public IPEndPoint IpEndPoint => new IPEndPoint(this.IpAddress, this.Port);

        public IList<ChatMessage> Messages { get; }

        public DateTime? Added { get; set; }

        public IPAddress IpAddress { get; set; }

        public DateTime? LastRecieve { get; set; }

        public DateTime? LastSend { get; set; }

        public Guid NodeId { get; set; }

        public int Port { get; set; }

        public Guid? SeenThrough { get; set; }

        public string Version { get; set; }

        public Node()
        {
            this.Channels = new List<Channel>();
            this.Messages = new List<ChatMessage>();
        }
    }
}