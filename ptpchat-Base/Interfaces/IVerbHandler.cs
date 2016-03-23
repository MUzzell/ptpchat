using System.Net;

namespace PtpChat.Base.Interfaces
{
	public interface IVerbHandler
	{
		bool HandleMessage(string msgData, IPEndPoint senderEndpoint);
	}
}
