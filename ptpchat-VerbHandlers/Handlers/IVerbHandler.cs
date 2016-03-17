namespace PtpChat.VerbHandlers.Handlers
{
    using System.Net;

    using PtpChat.Net.Socket_Manager;
    using PtpChat.UtilityClasses;

    public interface IVerbHandler
    {
        void ParseBaseMessage(string messageJson);

        bool HandleMessage(IPEndPoint senderEndpoint, ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers);
    }
}