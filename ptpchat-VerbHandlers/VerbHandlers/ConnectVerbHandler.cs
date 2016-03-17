namespace ptpchat.VerbHandlers
{
    using System;
    using System.Net;

    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;
    using ptpchat.Communication_Messages;

    public class ConnectVerbHandler : IVerbHandler
    {
        private ConnectMessage Message { get; set; }

        public void ParseBaseMessage(string messageJson)
        {
            throw new NotImplementedException();
        }

        public bool HandleMessage(IPEndPoint senderEndpoint, ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers)
        {
            throw new NotImplementedException();
        }
    }
}