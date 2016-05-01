namespace PtpChat.VerbHandlers
{
	using System;
	using System.Net;

	using Newtonsoft.Json;

	using PtpChat.Base.Interfaces;
	using PtpChat.Base.Messages;
	using Utility;
	using Base.Classes;
	using Base.Exceptions;
	public abstract class BaseVerbHandler<T> : IVerbHandler where T : BaseMessage
    {
        protected const string LogInvalidMsgId = "Recieved message with invalid msg_id, ignoring";

        private static readonly string LogCannotParseJson = "Unable to deserialise Json message, ignoring";

        private const string LogInvalidNodeId = "Invalid Node ID in this message, ignoring";

        private const string LogSameNodeId = "Recieved message presented this Node's ID! ignoring";

        protected IChannelManager ChannelManager { get; }

        protected ILogManager logger { get; }

        protected INodeManager NodeManager { get; }

        protected IResponseManager ResponseManager { get; }

        protected IOutgoingMessageManager OutgoingMessageManager { get; set; }

        protected BaseVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outMessageManager)
        {
            this.logger = logger;
            this.NodeManager = dataManager.NodeManager;
            this.ChannelManager = dataManager.ChannelManager;
            this.OutgoingMessageManager = outMessageManager;
            this.ResponseManager = dataManager.ResponseManager;
        }

        public bool HandleMessage(string msg, IPEndPoint senderEndpoint)
        {
            try
            {
				var message = JsonConvert.DeserializeObject<T>(msg);

				if (message.msg_id == Guid.Empty)
				{
					return false;
				}

				if (message.ttl <= 0)
				{
					return false;
				}

				NodeId senderId = null;
				var successful = ExtensionMethods.TryParseNodeId(message.sender_id, out senderId);

				if (!successful || !this.CheckNodeId(senderId.Id))
				{
					return false;
				}
				message.SenderId = senderId;

				NodeId targetId = null;
				if (message.target_id != null && ExtensionMethods.TryParseNodeId(message.target_id, out targetId))
				{
					return false;
				}
				else
				{
					message.TargetId = targetId;
				}

				if (targetId == null && message.ttl == 1) // this node
					return this.HandleVerb(message, senderEndpoint);

				if (message.flood)
					return this.HandleVerb(message, senderEndpoint);

				if (targetId != null && targetId == this.NodeManager.LocalNode.NodeId) //targets us directly
					return this.HandleVerb(message, senderEndpoint);

				message.ttl -= 1;

				if (message.ttl == 0) // not for us, but ttl as expired
					throw new MessageTtlExpired(message.msg_type, senderId, message.msg_id);
				

				return this.HandleVerb(message, senderEndpoint);
            }
            catch (JsonException)
            {
                this.logger.Warning(BaseVerbHandler<BaseMessage>.LogCannotParseJson);
                return false;
            }
        }

        /// <summary>
        /// Checks that the given NodeId is:
        ///		a) Not Empty
        ///		b) Not our NodeId
        ///	Will log the error (if applicable) and return a bool noting the result.
        /// </summary>
        /// <param name="nodeId">The NodeId to check</param>
        /// <returns></returns>
        public bool CheckNodeId(Guid nodeId)
        {
            if (nodeId == Guid.Empty)
            {
                this.logger.Warning(LogInvalidNodeId);
                return false;
            }

            if (nodeId == this.NodeManager.LocalNode.NodeId.Id)
            {
                this.logger.Error(LogSameNodeId);
                return false;
            }

            return true;
        }

        protected string BuildMessage(BaseMessage message) => JsonConvert.SerializeObject(message);

        protected abstract bool HandleVerb(T message, IPEndPoint senderEndpoint);
    }
}