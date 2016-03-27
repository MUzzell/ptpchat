namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Net;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public class ConnectVerbHandler : BaseVerbHandler<ConnectMessage>
    {
        public ConnectVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler)
            : base(logger, dataManager, socketHandler)
        {
        }

        private ConnectMessage Message { get; set; }

        protected override bool HandleVerb(ConnectMessage message, IPEndPoint senderEndpoint)
        {
            throw new NotImplementedException();
        }
    }
}