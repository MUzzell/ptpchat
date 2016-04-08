namespace PtpChat.Base.Classes
{
    using System;

    public class ChatMessage
    {
        public Guid ChannelId { get; set; }

        public DateTime DateSent { get; set; }

        public string MessageContent { get; set; }

        public Guid MessageId { get; set; }

        public Guid SenderId { get; set; }
    }
}