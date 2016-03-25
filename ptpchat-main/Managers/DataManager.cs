namespace PtpChat.Main
{
	using System;
	using PtpChat.Base.Interfaces;

	public class DataManager : IDataManager
	{
		private IChannelManager channelManager { get; set; }

		public IChannelManager ChannelManager { get { return this.channelManager; } }

		private INodeManager nodeManager { get; set; }

		public INodeManager NodeManager { get { return this.nodeManager; } }

		public DataManager(IChannelManager channelManager, INodeManager nodeManager)
		{

			if (channelManager == null || nodeManager == null)
			{
				throw new ArgumentNullException("Given node or channel manager(s) were null");
			}

			this.channelManager = channelManager;
			this.nodeManager = nodeManager;
		}
	}
}
