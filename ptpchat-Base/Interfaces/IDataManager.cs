namespace PtpChat.Base.Interfaces
{
    public interface IDataManager
    {
        INodeManager NodeManager { get; }

        IChannelManager ChannelManager { get; }
    }
}