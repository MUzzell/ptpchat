namespace PtpChat.VerbHandlers
{
    using System.Net;

    using Newtonsoft.Json;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public abstract class BaseVerbHandler<T> : IVerbHandler
    {
        protected BaseVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler)
        {
            this.logger = logger;
            this.NodeManager = dataManager.NodeManager;
            this.ChannelManager = dataManager.ChannelManager;
            this.SocketHandler = socketHandler;
        }

        private static readonly string LogCannotParseJson = "Unable to deserialise Json message, ignoring";

        protected ILogManager logger { get; }

        protected INodeManager NodeManager { get; }

        protected IChannelManager ChannelManager { get; }

        protected ISocketHandler SocketHandler { get; set; }

        public bool HandleMessage(string msgJson, IPEndPoint senderEndpoint)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<T>(msgJson);
                return this.HandleVerb(message, senderEndpoint);
            }
            catch (JsonException)
            {
                this.logger.Warning(BaseVerbHandler<BaseMessage>.LogCannotParseJson);
                return false;
            }
        }

        protected abstract bool HandleVerb(T message, IPEndPoint senderEndpoint);
    }
}