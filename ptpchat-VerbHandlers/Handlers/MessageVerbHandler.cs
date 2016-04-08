namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Text;

	using PtpChat.Base.Classes;
	using PtpChat.Base.Interfaces;
	using PtpChat.Base.Messages;

	public class MessageVerbHandler : BaseVerbHandler<MessageMessage>
    {
        private const string LogInvalidChannelId = "MESSAGE message contained invalid channel_id, ignoring message";
        private const string LogInvalidChannelName = "MESSAGE message contained invalid channel name, ignoring message";
        private const string LogInvalidMessage = "MESSAGE message contained invalid message, ignoring message";
        private const string LogInvalidNoMembers = "MESSAGE Message contained no recipients, ignoring message";
        private const string LogInvalidMemberEntry = "MESSAGE Message contained invalid recipient entry, ignoring recipient entry";


        public MessageVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler)
            : base(logger, dataManager, socketHandler)
        {
        }

        protected override bool HandleVerb(MessageMessage message, IPEndPoint senderEndpoint)
        {
            this.logger.Debug($"MESSAGE message recieved from sender: {senderEndpoint}");

            var data = message.msg_data;

            if (!this.CheckNodeId(data.node_id))
                return false;

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
			
			var newMessage = new ChatMessage
			{
				ChannelId = data.channel_id,
				DateSent = data.timestamp,
				MessageContent = data.message,
				MessageId = data.msg_id,
				SenderId = data.node_id
			};


			//is this message for us?
			if (recipientIds.Contains(this.NodeManager.LocalNode.NodeId))
            {
                this.ChannelManager.HandleMessageForChannel(newMessage);
            }
            else
            {
                //send it on?
            }

			var ackMsg = new AckMessage
			{
				msg_data = new AckData
				{
					msg_id = message.msg_data.msg_id
				}
			};

			this.SocketHandler.SendMessage(senderEndpoint, null, Encoding.ASCII.GetBytes(this.BuildMessage(ackMsg)));

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