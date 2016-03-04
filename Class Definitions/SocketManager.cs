namespace ptpchat.Class_Definitions
{
    using System.Net;
    using System.Net.Sockets;

    class SocketManager
    {
        public UdpClient UdpClient { get; set; }

        public IPAddress DestinationIp { get; set; }
        //public IPEndPoint DestinationEndpoint { get; set; }

        public IPEndPoint LocalEndpoint { get; set; }

        public SocketManager()
        {
        }

        public SocketManager(IPEndPoint localEndpoint, UdpClient updClient)
        {
            this.LocalEndpoint = localEndpoint;
            this.UdpClient = updClient;
        }

    }
}
