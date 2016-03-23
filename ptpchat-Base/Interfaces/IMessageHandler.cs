namespace PtpChat.Base.Interfaces
{
	using Messages;
	using System.Net;

	public interface IMessageHandler
	{
		void HandleMessage(string messageJson, IPEndPoint senderEndpoint);

		string BuildMessage(BaseMessage message);
	}
}
