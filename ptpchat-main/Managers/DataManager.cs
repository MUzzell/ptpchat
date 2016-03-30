namespace PtpChat.Main.Managers
{
    using System;

    using PtpChat.Base.Interfaces;

    public class DataManager : IDataManager
    {
        public DataManager(IChannelManager channelManager, INodeManager nodeManager, IResponseManager postMaster)
        {
            if (channelManager == null || nodeManager == null)
            {
                throw new ArgumentNullException("Given node or channel manager(s) were null");
            }

            this.channelManager = channelManager;
            this.nodeManager = nodeManager;
			this.postMaster = postMaster;
        }

        private IChannelManager channelManager { get; }

        private INodeManager nodeManager { get; }

		private IResponseManager postMaster { get; }

        public IChannelManager ChannelManager => this.channelManager;

        public INodeManager NodeManager => this.nodeManager;

		public IResponseManager ResponseManager => this.postMaster;
		
	}
}