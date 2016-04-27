﻿namespace PtpChat.Base.Messages
{
    using System.Collections.Generic;

    public class RoutingMessage : BaseMessage
    {
        public new RoutingData msg_data { get; set; }
    }

    public class RoutingData
    {
        public List<Dictionary<string, string>> nodes { get; set; }
    }
}