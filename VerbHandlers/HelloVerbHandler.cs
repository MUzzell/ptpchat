namespace ptpchat.VerbHandlers
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;

    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;
    using ptpchat.Communication_Messages;

    internal class HelloVerbHandler : IVerbHandler
    {
        private HelloMessage Message { get; set; }

        public void ParseBaseMessage(string messageJson)
        {
            this.Message = JsonConvert.DeserializeObject<HelloMessage>(messageJson);
            // { "msg_data": { "node_id": "5f715c17-4a41-482a-ab1f-45fa2cdd702b", "version": "ptpchat-server; 0.0"}, "msg_type": "HELLO"}
        }

        public bool HandleMessage(ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers)
        {
            var socketManager = serverSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id)
                                ?? clientSocketManagers.FirstOrDefault(a => a.DestinationNodeId == this.Message.msg_data.node_id);

            if (socketManager == null)
            {
                //no socket manager for the message?
                throw new Exception("Hello MESSAGE: no known socketmanager. Id = " + this.Message.msg_data.node_id);
            }

            socketManager.LastHelloRecieved = DateTime.Now;

            //return 'dont stop listening' after message has been handled
            return false;
        }
    }
}