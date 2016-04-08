namespace PtpChat.VerbHandlers.Handlers
{
    using System.Net;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    public class ConnectVerbHandler : BaseVerbHandler<ConnectMessage>
    {
        private ConnectMessage Message { get; set; }

        public ConnectVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
        }

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