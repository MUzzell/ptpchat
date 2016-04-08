namespace PtpChat.Base.EventArguements
{
    using System;

    using PtpChat.Base.Classes;

    public class ChannelMessageEventArgs : EventArgs
    {
        public Channel Channel { get; set; }

        public ChatMessage ChatMessage { get; set; }
    }
}