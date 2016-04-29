namespace PtpChat.Base.Interfaces
{
	using Messages;
	using System.Net;

	public interface IVerbHandler<T> where T : BaseMessage
    {
        bool HandleMessage(T msgData, IPEndPoint senderEndpoint);
    }
}