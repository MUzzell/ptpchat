namespace PtpChat.Main.Managers
{
    using System;

    using PtpChat.Base.Interfaces;

    public class DataManager : IDataManager
    {
        public DataManager(IChannelManager channelManager, INodeManager nodeManager)
        {
            if (channelManager == null || nodeManager == null)
            {
                throw new ArgumentNullException("Given node or channel manager(s) were null");
            }

            this.channelManager = channelManager;
            this.nodeManager = nodeManager;
        }

        private IChannelManager channelManager { get; }

        private INodeManager nodeManager { get; }

        public IChannelManager ChannelManager => this.channelManager;

        public INodeManager NodeManager => this.nodeManager;
    }
}