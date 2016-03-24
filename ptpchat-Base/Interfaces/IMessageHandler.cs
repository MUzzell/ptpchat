namespace PtpChat.Base.Interfaces
{
    using System.Net;

    using PtpChat.Base.Messages;

    public interface IMessageHandler
    {
        void HandleMessage(string messageJson, IPEndPoint senderEndpoint);

        string BuildMessage(BaseMessage message);
    }
}