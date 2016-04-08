namespace PtpChat.Base.Interfaces
{
    using System.Net;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Messages;

    public interface IOutgoingMessageManager
    {
        void SendHello(Node node, HelloMessage helloMessage);

        void SendHello(IPEndPoint endpoint, HelloMessage connectMessage);

        void SendConnect(Node node, ConnectMessage connectMessage);

        void SendConnect(IPEndPoint endpoint, ConnectMessage connectMessage);

        void SendMessage(Node node, MessageMessage messageMessage);

        void SendMessage(IPEndPoint endpoint, MessageMessage messageMessage);

        void SendChannel(Node node, ChannelMessage channelMessage);

        void SendChannel(IPEndPoint endpoint, ChannelMessage messageMessage);

        void SendAck(Node node, AckMessage ackMessage);

        void SendAck(IPEndPoint endpoint, AckMessage ackMessage);

        void SendHeartBeatHelloToNodes();

        void DoHeartBeat(object state);
    }
}