namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Net;

	using PtpChat.Net;
	using PtpChat.Utility;
	using Base.Interfaces;
	using Base.Messages;

	public class ConnectVerbHandler : BaseVerbHandler
    {
		private ConnectMessage Message { get; set; }

		public ConnectVerbHandler(ref ILogManager logger, ref INodeManager nodeManager, ref ISocketHandler socketHandler) : base( ref logger, ref nodeManager, ref socketHandler) { }

		public override bool HandleMessage(string messageJson, IPEndPoint senderEndpoint)
		{
			throw new NotImplementedException();
		}
	}
}