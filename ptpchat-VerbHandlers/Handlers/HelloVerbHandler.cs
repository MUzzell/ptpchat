namespace PtpChat.VerbHandlers.Handlers
{
	using System;
	using System.Linq;
	using System.Net;

	using Newtonsoft.Json;

	using PtpChat.Base.Messages;
	using PtpChat.Net;
	using PtpChat.Utility;
	using Base.Interfaces;

	public class HelloVerbHandler : BaseVerbHandler
    {
        private HelloMessage Message { get; set; }

		public HelloVerbHandler(ref ILogManager logger, ref INodeManager nodeManager, ref ISocketHandler socketHandler) : base(ref logger, ref nodeManager, ref socketHandler) { }
		
        public void ParseBaseMessage(string messageJson)
        {
            this.Message = JsonConvert.DeserializeObject<HelloMessage>(messageJson);
            // { "msg_data": { "node_id": "5f715c17-4a41-482a-ab1f-45fa2cdd702b", "version": "ptpchat-server; 0.0"}, "msg_type": "HELLO"}
        }

        
		/*
        var socketManager = serverSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id)
                            ?? clientSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id);

        if (socketManager == null)
        {
            socketManager = serverSocketManagers.FirstOrDefault(a => Equals(a.DestinationEndpoint.Address, senderEndpoint.Address))
                            ?? clientSocketManagers.FirstOrDefault(a => Equals(a.DestinationEndpoint.Address, senderEndpoint.Address));

            if (socketManager != null)
            {
                socketManager.DestinationNodeId = this.Message.msg_data.node_id;
            }
        }

        if (socketManager == null)
        {
            //no socket manager for the message?
            throw new Exception("Hello MESSAGE: no known socketmanager. Id = " + this.Message.msg_data.node_id);
        }

        socketManager.LastHelloRecieved = DateTime.Now;

        //return 'dont stop listening' after message has been handled
        return false;
		*/
        

		public override bool HandleMessage(string messageJson, IPEndPoint senderEndpoint)
		{
			throw new NotImplementedException();
		}
	}
}