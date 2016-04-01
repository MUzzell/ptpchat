namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;
	using System.Collections.Generic;

	using PtpChat.Base.Messages;
	using Base.Interfaces;

	public class AckVerbHandler : BaseVerbHandler<AckMessage>
	{
		private const string LogMsgIdNotFound = "Msg Id {0} was not found, ignoring";

		public AckVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler) 
			: base(logger, dataManager, socketHandler)
		{
		}

		protected override bool HandleVerb(AckMessage message, IPEndPoint senderEndpoint)
		{
			if (message.msg_data.msg_id == Guid.Empty)
			{
				logger.Warning(LogInvalidMsgId);
				return false;
			}

			try
			{
				this.ResponseManager.AckRecieved(message.msg_data.msg_id);
			}
			catch (KeyNotFoundException)
			{
				logger.Warning(string.Format(LogMsgIdNotFound, message.msg_data.msg_id));
				return false;
			}
			
			return true;
		}
	}
}
