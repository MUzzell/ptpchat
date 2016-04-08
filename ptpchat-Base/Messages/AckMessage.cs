namespace PtpChat.Base.Messages
{
    using System;

    public class AckMessage : BaseMessage
    {
        public AckData msg_data;

        public new MessageType msg_type => MessageType.ACK;
    }

    public class AckData
    {
        public Guid msg_id;
    }
}