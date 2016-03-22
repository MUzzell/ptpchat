namespace PtpChat.Base.Interfaces
{
	using System;
	using System.Net;

	public abstract class BaseVerbHandler
	{

		private ILogManager logManager {get;set;}
		private INodeManager nodeManager { get; set; }
		private ISocketHandler socketHandler { get; set; }

		public BaseVerbHandler(ref ILogManager logger, ref INodeManager nodeManager, ref ISocketHandler socketHandler)
		{
			this.logManager = logger;
			this.nodeManager = nodeManager;
			this.socketHandler = socketHandler;
		}
		
		public abstract bool HandleMessage(string messageJson, IPEndPoint senderEndpoint);
		
    }
}