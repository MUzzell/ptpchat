namespace PtpChat.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public class SocketHandler : ISocketHandler
    {
        public SocketHandler(ILogManager logger, INodeManager nodeManager)
        {
            this.logger = logger;

            this.internalThreads = new Dictionary<int, SocketThread>();

            //What if it's in use??
            this.localPort = new Random().Next(10000, 65535);
            this.localClient = new UdpClient(this.localPort, AddressFamily.InterNetwork);

            this.logger.Info(string.Format(LogPortBound, this.localPort));

            nodeManager.LocalNode.Port = this.localPort;
            this.nodeManager = nodeManager;

            this.internalThreads.Add(this.localPort, new SocketThread(this.localClient, null, this.logger));
        }

        public void SetMessageHandler(IMessageHandler handler)
        {
            this.MessageHandler = handler;

            foreach (var socketThread in this.internalThreads)
            {
                socketThread.Value.SetMessageHandler(handler);
            }
        }

        private const string LogHandlerStarting = "SocketHandler starting";
        private const string LogHandlerStopping = "SocketHandler stopping";
        private const string LogPortBound = "SocketHandler has bound to 0.0.0.0:{0}";

        private Timer helloTimer;

        //threads are organised by their port.
        private readonly IDictionary<int, SocketThread> internalThreads;

        private readonly UdpClient localClient;

        //im holding onto this until i figure out how to do the rest of the socket stuff.
        private readonly int localPort;

        private readonly ILogManager logger;
        private readonly INodeManager nodeManager;

        private IMessageHandler MessageHandler { get; set; }

        public bool SendMessage(Guid dstNodeId, byte[] messsage)
        {
            var node = this.nodeManager.GetNodes(a => a.Value.NodeId == dstNodeId).FirstOrDefault();

            if (node == null) return false;

            throw new NotImplementedException();
        }

        public bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message)
        {
            this.internalThreads[this.localPort].Send(dst, message);

            return true;
        }

        public void Start()
        {
            this.logger.Info(LogHandlerStarting);

            foreach (var thread in this.internalThreads.Values)
            {
                new Task(() => thread.Run()).Start();
            }

            this.helloTimer = new Timer(this.SendHellos, null, 0, 3000);
        }

        public void Stop()
        {
            this.logger.Info(LogHandlerStopping);

            foreach (var thread in this.internalThreads.Values)
            {
                thread.Stop();
            }
        }

        private void SendHellos(object state)
        {
            var nodes = this.nodeManager.GetNodes().ToList();

            var hello = new HelloMessage { msg_type = MessageType.HELLO, msg_data = new HelloData { node_id = this.nodeManager.LocalNode.NodeId, version = string.Empty } };

            var msg = Encoding.ASCII.GetBytes(this.MessageHandler.BuildMessage(hello));

            this.logger.Debug($"Sending Hellos to {nodes.Count} nodes");

            foreach (var node in nodes)
            {
                this.SendMessage(node.IpEndPoint, null, msg);
            }
        }
    }
}