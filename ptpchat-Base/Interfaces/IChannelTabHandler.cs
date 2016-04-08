namespace PtpChat.Base.Interfaces
{
    using System;

    using PtpChat.Base.Classes;

    public interface IChannelTabHandler
    {
        void AddChannelTab(Guid channelId, IChannelTab channelTab);

        void SendMessage(Guid channelId, string message);

        void MessageRecieved(ChatMessage message);
    }
}