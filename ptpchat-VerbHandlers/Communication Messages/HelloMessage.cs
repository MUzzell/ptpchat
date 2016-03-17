namespace PtpChat.VerbHandlers.Communication_Messages
{
    using System;

    public class HelloMessage : BaseMessage
    {
        public new HelloData msg_data { get; set; }
    }

    public class HelloData
    {
        public Guid node_id { get; set; }

        public string version { get; set; }
    }
}