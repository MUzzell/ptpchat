namespace PtpChat.Base.Messages
{
    using System;
    using System.Collections.Generic;

    public class ChannelMessage : BaseMessage
    {
        public new MessageType msg_type => MessageType.CHANNEL;

        public new ChannelData msg_data { get; set; }
    }

    public class ChannelData
    {
        public string channel { get; set; }

        public Guid channel_id { get; set; }

        public bool closed { get; set; }

        public List<Dictionary<string, string>> members { get; set; }

        public Guid msg_id { get; set; }
    }
}