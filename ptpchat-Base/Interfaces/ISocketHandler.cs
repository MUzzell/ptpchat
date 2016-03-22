namespace PtpChat_Base.Interfaces
{
    using System;
    using System.Net;

    public interface ISocketHandler
    {
        void Start();

        void Stop();

        bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message);

        bool SendMessage(Guid dstNodeId, byte[] messsage);
    }
}