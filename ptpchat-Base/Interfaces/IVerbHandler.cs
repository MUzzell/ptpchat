namespace PtpChat.Base.Interfaces
{
    using System.Net;

    public interface IVerbHandler
    {
        bool HandleMessage(string msgData, IPEndPoint senderEndpoint);
    }
}