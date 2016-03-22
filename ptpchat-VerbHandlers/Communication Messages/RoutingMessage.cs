﻿namespace PtpChat.VerbHandlers.Communication_Messages
{
    using System;
    using System.Collections.Generic;

    public class RoutingMessage : BaseMessage
    {
        public new RoutingData msg_data { get; set; }
    }

    public class RoutingData
    {
        public List<Dictionary<string, string>> nodes { get; set; }

        public Guid node_id { get; set; }
    }
}