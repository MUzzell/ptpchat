namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Net;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;
    using PtpChat.Utility;

    public class ConnectVerbHandler : BaseVerbHandler<ConnectMessage>
    {
        public ConnectVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler)
            : base(logger, dataManager, socketHandler)
        {
        }

        private ConnectMessage Message { get; set; }

        protected override bool HandleVerb(ConnectMessage message, IPEndPoint senderEndpoint)
        {
            //somebody connecting to us

            //sender => A
            var senderId = message.msg_data.src_node_id;

            //A's complete endpoint
            var senderIpEndpoint = message.msg_data.src;

            //recipient => B
            var recipientid = message.msg_data.dst_node_id;

            //B's IP, no port
            var recipientIp = message.msg_data.dst;

            //S's Ip
            var serverip = senderEndpoint;

            this.logger.Debug($"Connect message recieved from: {senderIpEndpoint}");

            //connect logic

            return true;
        }
    }
}