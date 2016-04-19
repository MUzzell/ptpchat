namespace PtpChat.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using PtpChat.Base.Interfaces;

    public class SocketHandler : ISocketHandler
    {
        private const string LogPortBound = "SocketHandler has bound to 0.0.0.0:{0}";
        private const string EndpointRefused = "Could not connect to {0} because the target machine refused";

        private readonly UdpClient localClient;

        //im holding onto this until i figure out how to do the rest of the socket stuff.
        private readonly int localPort;

        private readonly ILogManager logger;
        private readonly INodeManager nodeManager;
        private readonly TcpListener localListener;

        //threads are organised by their port.
        public IDictionary<int, SocketThread> InternalThreads { get; }

        public SocketHandler(ILogManager logger, IDataManager dataManager, IMessageHandler messageHandler)
        {
            this.logger = logger;
            this.logger.Info(string.Format(LogPortBound, this.localPort));

            this.InternalThreads = new Dictionary<int, SocketThread>();

            //What if it's in use??
            this.localPort = new Random().Next(10000, 65535);
            this.localClient = new UdpClient(this.localPort, AddressFamily.InterNetwork);

            dataManager.NodeManager.LocalNode.Port = this.localPort;
            this.nodeManager = dataManager.NodeManager;

            this.localListener = new TcpListener(IPAddress.Any, this.localPort) { ExclusiveAddressUse = false };

            this.InternalThreads.Add(this.localPort, new SocketThread(this.localClient, this.localListener, messageHandler, this.logger));

            this.StartListening();
        }

        public bool SendMessage(Guid dstNodeId, byte[] message)
        {
            var node = this.nodeManager.GetNodes(a => a.Value.NodeId.Id == dstNodeId).FirstOrDefault();

            return node != null && this.SendMessage(node.IpEndPoint, null, message);
        }

        public bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message)
        {
            try
            {
                this.InternalThreads[this.localPort].Send(dst, message);
            }
            catch (SocketException ex)
            {
                this.logger.Error(string.Format(EndpointRefused, dst));

                return false;
            }

            return true;
        }

        public void StartListening()
        {
            foreach (var thread in this.InternalThreads.Values)
            {
                //start the listeners
                new Task(() => thread.Listen()).Start();
            }
        }

        public void StopListening()
        {
            foreach (var thread in this.InternalThreads.Values)
            {
                //stop the listeners
                new Task(() => thread.Stop()).Start();
            }
        }
    }
}