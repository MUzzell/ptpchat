namespace ptpchat.VerbHandlers
{
    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;

    internal interface IVerbHandler
    {
        void ParseBaseMessage(string messageJson);

        bool HandleMessage(ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers);
    }
}