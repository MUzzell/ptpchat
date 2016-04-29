using PtpChat.Base.Classes;
using PtpChat.Base.Messages;
using System;


namespace PtpChat.Base.Exceptions
{
	public class MessageTtlExpired : Exception
	{
		public MessageType MsgType { get; private set; }
		public NodeId SenderId { get; private set; }
		public Guid MsgId { get; private set; }

		public MessageTtlExpired(MessageType msgType, NodeId senderId, Guid msgId) : base($"{msgType} message from {senderId} TTL reached; ID: {msgId}.")
		{
			MsgType = msgType;
			SenderId = senderId;
			MsgId = msgId;
		}
	}
}
