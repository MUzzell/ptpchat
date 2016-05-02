namespace PtpChat.Base.Messages
{
	using Classes;
	using System;
	using Newtonsoft.Json;

	public class BaseMessage
    {
        public bool flood { get; set; }

        public object msg_data { get; set; }

        public MessageType msg_type { get; set; }

		public Guid msg_id { get; set; }

        public int ttl { get; set; }

        public string sender_id { get; set; }

        public string target_id { get; set; }

		[JsonIgnore]
		public NodeId SenderId { get; set; }

		[JsonIgnore]
		public NodeId TargetId { get; set; }

    }

    public enum MessageType
    {
        HELLO,

        ROUTING,

        CONNECT,

        MESSAGE,

        JOIN,

        LEAVE,

        CHANNEL,

        ACK
    }
}