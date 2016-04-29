namespace PtpChat.Base.Messages
{
    using System.Collections.Generic;

    public class RoutingMessage : BaseMessage
    {
        public new RoutingData msg_data { get; set; }
    }

    public class RoutingData
    {
        public List<RoutingNodeData> nodes { get; set; }
    }

	public class RoutingNodeData
	{
		public string node_id { get; set; }
		public int ttl { get; set; }
	}
}