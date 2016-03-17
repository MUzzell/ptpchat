namespace PtpChat.VerbHandlers.Serialisation_Classes
{
    using System;
    using System.Net;

    public class Node
    {
        public Guid NodeId { get; set; }

        public IPAddress IpAddress { get; set; }

        public int Port { get; set; }
    }
}