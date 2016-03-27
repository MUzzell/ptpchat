namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;

	using Base.Classes;
	using Base.Interfaces;

	public class ChannelVerbHandler : BaseVerbHandler<ChannelMessage>
	{
		public ChannelVerbHandler(ILogManager logger, IDataManager nodeManager, ISocketHandler socketHandler)
            : base(logger, nodeManager, socketHandler)
        {
		}

		protected override bool HandleVerb(ChannelMessage message, IPEndPoint senderEndpoint)
		{
			throw new NotImplementedException();
		}
	}
}
