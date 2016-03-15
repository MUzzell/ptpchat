﻿namespace ptpchat.Class_Definitions
{
    public class BaseMessage
    {
        public MessageType msg_type { get; set; }

        public object msg_data { get; set; }
    }

    public enum MessageType
    {
        HELLO,

        ROUTING,

        CONNECT
    }
}