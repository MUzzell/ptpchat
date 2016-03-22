namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Net;

    using PtpChat.Net;
    using PtpChat.UtilityClasses;

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