namespace PtpChat.VerbHandlers.Communication_Messages
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

        string dst { get; set; }

        string src { get; set; }
    }
}