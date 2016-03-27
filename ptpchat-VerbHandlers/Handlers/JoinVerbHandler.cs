namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;

	using PtpChat.Base.Messages;
	using Base.Interfaces;

	public class JoinVerbHandler : BaseVerbHandler<JoinMessage>
	{
		public JoinVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler) :
			base(logger, dataManager, socketHandler)
		{ }

		protected override bool HandleVerb(JoinMessage message, IPEndPoint senderEndpoint)
		{
			throw new NotImplementedException();
		}
	}
}
