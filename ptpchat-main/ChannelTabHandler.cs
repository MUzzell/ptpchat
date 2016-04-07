namespace PtpChat.Main
{
	using System;
	using Base.Interfaces;
	using Base.Classes;
	using System.Collections.Generic;

	internal class ChannelTabHandler : IChannelTabHandler
	{

		private readonly IDictionary<Guid, IChannelTab> ChannelTabs;

		private readonly ILogManager Logger;
		private readonly IChannelManager ChannelManager;
		private readonly ISocketHandler SocketHandler;

		public ChannelTabHandler(ILogManager logger, IChannelManager channelManager, ISocketHandler socketHandler)
		{
			this.ChannelTabs = new Dictionary<Guid, IChannelTab>();
			this.Logger = logger;
			this.SocketHandler = socketHandler;
			this.ChannelManager = channelManager;
		}

		public void AddChannelTab(Guid channelId, IChannelTab channelTab)
		{
			this.ChannelTabs.Add(channelId, channelTab);
		}

		public void SendMessage(Guid channelId, string message)
		{
			throw new NotImplementedException();
		}

		public void MessageRecieved(ChatMessage message)
		{
			throw new NotImplementedException();
		}
	}
}
