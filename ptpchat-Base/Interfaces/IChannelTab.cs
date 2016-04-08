namespace PtpChat.Base.Interfaces
{
    using PtpChat.Base.Classes;

    public interface IChannelTab
    {
        void MessageRecieved(ChatMessage message);
    }
}