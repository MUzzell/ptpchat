namespace PtpChat.Base.Messages
{
    using System.Collections.Generic;

    public class HelloMessage : BaseMessage
    {
		public new MessageType msg_type => MessageType.HELLO;

        public new HelloData msg_data { get; set; }
    }

    public class HelloData
    {
        public string version { get; set; }
        public Dictionary<string, string> attributes { get; set; }
    }
}