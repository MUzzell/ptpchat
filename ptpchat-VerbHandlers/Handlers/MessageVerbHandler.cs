namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;

	using Base.Interfaces;
	using Base.Messages;

	public class MessageVerbHandler : BaseVerbHandler<MessageMessage>
	{

		public MessageVerbHandler(ILogManager logger, IDataManager dataManager, ISocketHandler socketHandler)
            : base(logger, dataManager, socketHandler)
        {
		}
		
		protected override bool HandleVerb(MessageMessage message, IPEndPoint senderEndpoint)
		{
			throw new NotImplementedException();
		}
	}
}
