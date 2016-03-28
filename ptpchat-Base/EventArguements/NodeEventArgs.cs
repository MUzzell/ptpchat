namespace PtpChat.Base.EventArguements
{
    using System;

    using PtpChat.Base.Classes;

    public class NodeEventArgs : EventArgs
    {
        public Node Node { get; set; }
    }
}