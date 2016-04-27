namespace PtpChat.Base.Messages
{
    using System;
    using System.Collections.Generic;

    public class MessageMessage : BaseMessage
    {
        public new MessageData msg_data { get; set; }
    }

    public class MessageData
    {
        public List<Dictionary<string, string>> attachment { get; set; }

        public string channel { get; set; }

        public Guid channel_id { get; set; }

        public string message { get; set; }

        public Guid msg_id { get; set; }

        public List<Dictionary<string, string>> recipient { get; set; }

        public DateTime timestamp { get; set; }
    }
}