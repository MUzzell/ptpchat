namespace PtpChat.Base.Messages
{
    using System;

    public class ConnectMessage : BaseMessage
    {
        public new ConnectData msg_data { get; set; }
    }

    public class ConnectData
    {
        public Guid src_node_id { get; set; }

        public Guid dst_node_id { get; set; }

        public string dst { get; set; }

        public string src { get; set; }
    }
}