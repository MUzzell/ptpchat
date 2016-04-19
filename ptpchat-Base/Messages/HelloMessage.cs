namespace PtpChat.Base.Messages
{
    public class HelloMessage : BaseMessage
    {
        public new HelloData msg_data { get; set; }
    }

    public class HelloData
    {
        public string node_id { get; set; }

        public string version { get; set; }
    }
}