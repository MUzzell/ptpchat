﻿namespace PtpChat.Base.Messages
{
    using System;

    public class ConnectMessage : BaseMessage
    {
        public new MessageType msg_type => MessageType.CONNECT;

        public new ConnectData msg_data { get; set; }
    }

    public class ConnectData
    {
        public string dst { get; set; }

        public Guid dst_node_id { get; set; }

        public string src { get; set; }

        public Guid src_node_id { get; set; }
    }
}