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

        private readonly UdpClient localClient;

        //im holding onto this until i figure out how to do the rest of the socket stuff.
        private readonly int localPort;

        private readonly ILogManager logger;
        private readonly INodeManager nodeManager;

        //threads are organised by their port.
        public IDictionary<int, SocketThread> InternalThreads { get; }

        public SocketHandler(ILogManager logger, IDataManager dataManager, IMessageHandler messageHandler)
        {
            this.logger = logger;

            this.InternalThreads = new Dictionary<int, SocketThread>();

            //What if it's in use??
            this.localPort = new Random().Next(10000, 65535);
            this.localClient = new UdpClient(this.localPort, AddressFamily.InterNetwork);

            this.logger.Info(string.Format(LogPortBound, this.localPort));

            dataManager.NodeManager.LocalNode.Port = this.localPort;
            this.nodeManager = dataManager.NodeManager;

            this.InternalThreads.Add(this.localPort, new SocketThread(this.localClient, messageHandler, this.logger));

            this.StartListening();
        }

        public bool SendMessage(Guid dstNodeId, byte[] message)
        {
            var node = this.nodeManager.GetNodes(a => a.Value.NodeId == dstNodeId).FirstOrDefault();

            return node != null && this.SendMessage(node.IpEndPoint, null, message);
        }

        public bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message)
        {
            this.InternalThreads[this.localPort].Send(dst, message);

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