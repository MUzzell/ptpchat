namespace PtpChat.Base.Classes
{
	using System;

	public class ResponseMessage
	{
		public Guid MsgId { get; set; }
		public Guid TargetNodeId { get; set; }
		public byte[] Msg { get; set; }
		public DateTime Added { get; set; }
		public DateTime LastSent { get; set; }
		public bool NeedsResend { get; set; }
		public int Attempts { get; set; }
	}
}
