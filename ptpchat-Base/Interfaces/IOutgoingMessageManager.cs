namespace PtpChat.Base.Interfaces
{
	using System;
	using System.Net;

	using PtpChat.Base.Classes;
	using PtpChat.Base.Messages;

	public interface IOutgoingMessageManager
    {
        void SendHello(Node node, HelloMessage helloMessage);

        void SendHello(IPEndPoint endpoint, HelloMessage connectMessage);

        void SendConnect(Node node, Guid targetNodeId);

        void SendConnect(IPEndPoint endpoint, Guid targetNodeId);

        void SendMessage(Node node, MessageMessage messageMessage);

        void SendMessage(IPEndPoint endpoint, MessageMessage messageMessage);

        void SendChannel(Node node, ChannelMessage channelMessage);

        void SendChannel(IPEndPoint endpoint, ChannelMessage messageMessage);

        void SendAck(Node node, Guid msgId);

        void SendAck(IPEndPoint endpoint, Guid msgId);

		void Send(IPEndPoint endpoint, BaseMessage message);

        void DoHeartBeat(object state);
    }
}