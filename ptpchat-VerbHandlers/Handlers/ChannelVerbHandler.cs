namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Net;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;

    public class ChannelVerbHandler : BaseVerbHandler<ChannelMessage>
    {
        public ChannelVerbHandler(ILogManager logger, IDataManager nodeManager, ISocketHandler socketHandler)
            : base(logger, nodeManager, socketHandler)
        {
        }

        protected override bool HandleVerb(ChannelMessage message, IPEndPoint senderEndpoint)
        {
            throw new NotImplementedException();
        }
    }
}