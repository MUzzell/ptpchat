namespace PtpChat.Base.Classes
{
    using System;
    using System.Collections.Generic;

    public class Channel
    {
        private readonly IList<ChatMessage> Messages;

        public DateTime Added { get; }

        public Guid ChannelId { get; set; }

        public string ChannelName { get; set; }

        public bool Closed { get; set; }

        public bool IsUpToDate { get; set; }

        public DateTime LastTransmission { get; set; }

        public IList<Guid> Nodes { get; set; }

        public Channel()
        {
            this.Messages = new List<ChatMessage>();
            this.Added = DateTime.Now;
        }

        public void AddMessage(ChatMessage message)
        {
            this.Messages.Add(message);
        }

        public IList<ChatMessage> GetMessages() => this.Messages;
    }
}