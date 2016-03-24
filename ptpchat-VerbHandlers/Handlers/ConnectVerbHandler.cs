namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;

	using PtpChat.Net;
	using PtpChat.Utility;
	using Base.Interfaces;
	using Base.Messages;

	public class ConnectVerbHandler : BaseVerbHandler<ConnectMessage>
    {
		private ConnectMessage Message { get; set; }

		public ConnectVerbHandler(ref ILogManager logger, ref INodeManager nodeManager, ref ISocketHandler socketHandler) : base( ref logger, ref nodeManager, ref socketHandler) { }

		protected override bool HandleVerb(ConnectMessage message, IPEndPoint senderEndpoint)
		{
			throw new NotImplementedException();
		}
	}
}