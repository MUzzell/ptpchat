namespace PtpChat.Base.Interfaces
{
	using System;

	public interface IChannelEventHandler
	{
		void SendMessage(Guid channelId, string message);
	}
}
