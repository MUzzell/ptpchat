namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;

	using PtpChat.Base.Messages;
	using Base.Interfaces;

	public class LeaveVerbHandler : BaseVerbHandler<LeaveMessage>
	{
		public LeaveVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler) :
			base(logger, dataManager, socketHandler)
		{ }

		protected override bool HandleVerb(LeaveMessage message, IPEndPoint senderEndpoint)
		{
			throw new NotImplementedException();
		}
	}
}
