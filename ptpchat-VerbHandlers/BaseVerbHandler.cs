namespace PtpChat.VerbHandlers
{
	using System.Net;

	using Newtonsoft.Json;

	using PtpChat.Base.Interfaces;
	using PtpChat.Base.Messages;
	using System;
	public abstract class BaseVerbHandler<T> : IVerbHandler
    {
		private const string LogInvalidNodeId = "Invalid Node ID in this message, ignoring";
		private const string LogSameNodeId = "Recieved message presented this Node's ID! ignoring";

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

		/// <summary>
		/// Checks that the given NodeId is:
		///		a) Not Empty
		///		b) Not our NodeId
		///	Will log the error (if applicable) and return a bool noting the result.
		/// </summary>
		/// <param name="nodeId">The NodeId to check</param>
		/// <returns></returns>
		protected bool CheckNodeId(Guid nodeId)
		{
			if (nodeId == Guid.Empty)
			{
				this.logger.Warning(LogInvalidNodeId);
				return false;
			}

			if (nodeId == this.NodeManager.LocalNode.NodeId)
			{
				this.logger.Error(LogSameNodeId);
				return false;
			}

			return true;
		}

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