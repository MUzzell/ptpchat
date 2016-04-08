namespace PtpChat.Base.Classes
{
    using System;

    public class ResponseMessage
    {
        public DateTime Added { get; set; }

        public int Attempts { get; set; }

        public DateTime LastSent { get; set; }

        public byte[] Msg { get; set; }

        public Guid MsgId { get; set; }

        public bool NeedsResend { get; set; }

        public Guid TargetNodeId { get; set; }
    }
}