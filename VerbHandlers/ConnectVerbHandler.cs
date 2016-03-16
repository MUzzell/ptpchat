namespace ptpchat.VerbHandlers
{
    using System;

    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;
    using ptpchat.Communication_Messages;

    internal class ConnectVerbHandler : IVerbHandler
    {
        private ConnectMessage Message { get; set; }

        public void ParseBaseMessage(string messageJson)
        {
            throw new NotImplementedException();
        }

        public bool HandleMessage(ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers)
        {
            throw new NotImplementedException();
        }
    }
}