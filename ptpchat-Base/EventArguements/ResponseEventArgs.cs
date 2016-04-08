namespace PtpChat.Base.EventArguements
{
    using System;

    public class ResponseEventArgs : EventArgs
    {
        public Guid MsgId { get; set; }
    }
}