namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Net;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public class LeaveVerbHandler : BaseVerbHandler<LeaveMessage>
    {
        public LeaveVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
        }

        protected override bool HandleVerb(LeaveMessage message, IPEndPoint senderEndpoint)
        {
            throw new NotImplementedException();
        }
    }
}