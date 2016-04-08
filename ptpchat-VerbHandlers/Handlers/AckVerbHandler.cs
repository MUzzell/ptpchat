namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public class AckVerbHandler : BaseVerbHandler<AckMessage>
    {
        private const string LogMsgIdNotFound = "Msg Id {0} was not found, ignoring";

        public AckVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
        }

        protected override bool HandleVerb(AckMessage message, IPEndPoint senderEndpoint)
        {
            if (message.msg_data.msg_id == Guid.Empty)
            {
                this.logger.Warning(LogInvalidMsgId);
                return false;
            }

            try
            {
                this.ResponseManager.AckRecieved(message.msg_data.msg_id);
            }
            catch (KeyNotFoundException)
            {
                this.logger.Warning(string.Format(LogMsgIdNotFound, message.msg_data.msg_id));
                return false;
            }

            return true;
        }
    }
}