namespace PtpChat.Base.Interfaces
{
    using System;
    using System.Net;

    public interface ISocketHandler
    {
        void StartPeriodicHello();

        void StopPeriodicHello();

        bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message);

        bool SendMessage(Guid dstNodeId, byte[] messsage);

        void SendConnect();
    }
}