namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Net;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public class MessageVerbHandler : BaseVerbHandler<MessageMessage>
    {
        public MessageVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler)
            : base(logger, dataManager, socketHandler)
        {
        }

        protected override bool HandleVerb(MessageMessage message, IPEndPoint senderEndpoint)
        {
            throw new NotImplementedException();
        }
    }
}