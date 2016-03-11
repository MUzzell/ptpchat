namespace ptpchat.Class_Definitions
{
    using System.Collections.Generic;

    internal class RoutingMessage : BaseMessage
    {
        public new RoutingData msg_data { get; set; }
    }

    internal class RoutingData
    {
        public List<Dictionary<string, string>> nodes { get; set; }
    }
}