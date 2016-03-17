namespace ptpchat.VerbHandlers
{
    using System.Net;

    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;

    public interface IVerbHandler
    {
        void ParseBaseMessage(string messageJson);

        bool HandleMessage(IPEndPoint senderEndpoint, ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers);
    }
}