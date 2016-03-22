namespace PtpChat_Base.Interfaces
{
    using System.Net;

    public abstract class BaseVerbHandler
    {
        protected BaseVerbHandler(ref ILogManager logger, ref INodeManager nodeManager, ref ISocketHandler socketHandler)
        {
            this.LogManager = logger;
            this.NodeManager = nodeManager;
            this.SocketHandler = socketHandler;
        }

        private ILogManager LogManager { get; set; }

        private INodeManager NodeManager { get; set; }

        private ISocketHandler SocketHandler { get; set; }

        public abstract bool HandleMessage(string messageJson, IPEndPoint senderEndpoint);
    }
}