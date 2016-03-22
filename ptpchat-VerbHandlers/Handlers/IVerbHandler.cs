namespace PtpChat_VerbHandlers.Handlers
{
    using System.Net;

    using PtpChat_Net;

    using PtpChat_UtilityClasses;

    public interface IVerbHandler
    {
        void ParseBaseMessage(string messageJson);

        bool HandleMessage(IPEndPoint senderEndpoint, ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers);
    }
}