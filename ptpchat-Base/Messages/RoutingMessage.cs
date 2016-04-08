namespace PtpChat.Base.Messages
{
    using System;
    using System.Collections.Generic;

    public class RoutingMessage : BaseMessage
    {
        public new RoutingData msg_data { get; set; }
    }

    public class RoutingData
    {
        public Guid node_id { get; set; }

        public List<Dictionary<string, string>> nodes { get; set; }
    }
}