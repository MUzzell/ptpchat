﻿namespace PtpChat.Base.Interfaces
{
    using System;
    using System.Net;

    public interface ISocketHandler
    {
		event EventHandler SocketConnected;
		event EventHandler SocketDisconnected;
		event EventHandler SocketReset;

        bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message);

        bool SendMessage(Guid dstNodeId, byte[] messsage);

        void AddSocketThread(IPEndPoint destination, IMessageHandler messageHandler);

        void StartListening();

        void StopListening();
    }
}