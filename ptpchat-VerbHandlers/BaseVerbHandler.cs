namespace PtpChat.VerbHandlers
{
	using Base.Interfaces;
	using Base.Messages;
	using Newtonsoft.Json;
	using System.Net;

	public abstract class BaseVerbHandler<T> : IVerbHandler
    {
		private static readonly string LogCannotParseJson = "Unable to deserialise Json message, ignoring";

		protected BaseVerbHandler(ref ILogManager logger, ref INodeManager nodeManager, ref ISocketHandler socketHandler)
        {
            this.logger = logger;
            this.NodeManager = nodeManager;
            this.SocketHandler = socketHandler;
        }

        protected ILogManager logger { get; set; }

        protected INodeManager NodeManager { get; set; }

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