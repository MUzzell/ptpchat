namespace ptpchat.Class_Definitions
{
    using System.Net;

    internal class Node
    {
        public int NodeId { get; set; }

        public IPAddress IpAddress { get; set; }

        public int Port { get; set; }
    }
}