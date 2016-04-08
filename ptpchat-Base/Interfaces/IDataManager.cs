namespace PtpChat.Base.Interfaces
{
    public interface IDataManager
    {
        IChannelManager ChannelManager { get; }

        INodeManager NodeManager { get; }

        IResponseManager ResponseManager { get; }
    }
}