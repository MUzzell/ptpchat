namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;
    using PtpChat.Utility;

    public class MessageVerbHandler : BaseVerbHandler<MessageMessage>
    {
        private const string LogInvalidChannelId = "MESSAGE message contained invalid channel_id, ignoring message";

        private const string LogInvalidChannelName = "MESSAGE message contained invalid channel name, ignoring message";

        private const string LogInvalidMemberEntry = "MESSAGE Message contained invalid recipient entry, ignoring recipient entry";

        private const string LogInvalidMessage = "MESSAGE message contained invalid message, ignoring message";

        private const string LogInvalidNoMembers = "MESSAGE Message contained no recipients, ignoring message";

        public MessageVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
        }

        protected override bool HandleVerb(MessageMessage message, IPEndPoint senderEndpoint)
        {
            this.logger.Debug($"MESSAGE message recieved from sender: {senderEndpoint}");

            var data = message.msg_data;

            var longId = data.node_id;

            string senderName;
            Guid senderId;
            var successful = ExtensionMethods.SplitNodeId(longId, out senderName, out senderId);

            if (!successful || !this.CheckNodeId(senderId))
            {
                return false;
            }

            if (data.msg_id == Guid.Empty)
            {
                this.logger.Warning(LogInvalidMsgId);
                return false;
            }

            if (data.channel_id == Guid.Empty)
            {
                this.logger.Warning(LogInvalidChannelId);
                return false;
            }

            if (string.IsNullOrWhiteSpace(data.channel))
            {
                this.logger.Warning(LogInvalidChannelName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(data.message))
            {
                this.logger.Warning(LogInvalidMessage);
                return false;
            }

            if (data.recipient == null || !data.recipient.Any())
            {
                this.logger.Warning(LogInvalidNoMembers);
                return false;
            }

            var recipientIds = this.ParseRecipientList(data.recipient);

            var newMessage = new ChatMessage { ChannelId = data.channel_id, DateSent = data.timestamp, MessageContent = data.message, MessageId = data.msg_id, SenderId = senderId };

            //is this message for us?
            if (recipientIds.Contains(this.NodeManager.LocalNode.NodeId.Id))
            {
                this.ChannelManager.HandleMessageForChannel(newMessage);
            }

            var ackMsg = new AckMessage { msg_data = new AckData { msg_id = message.msg_data.msg_id } };

            this.OutgoingMessageManager.SendAck(senderEndpoint, ackMsg);

            return true;
        }

        private IEnumerable<Guid> ParseRecipientList(IEnumerable<Dictionary<string, string>> members)
        {
            var memberList = new List<Guid>();

            foreach (var member in members)
            {
                Guid memberId;

                if (!Guid.TryParse(member["node_id"], out memberId) || !this.CheckNodeId(memberId))
                {
                    this.logger.Warning(LogInvalidMemberEntry);
                    continue;
                }

                memberList.Add(memberId);
            }

            return memberList;
        }
    }
}