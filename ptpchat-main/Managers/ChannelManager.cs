namespace PtpChat.Main
{
	using System;
	using System.Collections.Concurrent;

	using Base.Classes;
	using Base.Interfaces;
	using Utility;

	public class ChannelManager : IChannelManager
	{
		private readonly ILogManager logger;

		private readonly ConcurrentDictionary<Guid, Channel> Channels = new ConcurrentDictionary<Guid, Channel>();

		private readonly ConcurrentDictionary<Guid, ChannelMessage> Messages = new ConcurrentDictionary<Guid, ChannelMessage>();

		public ChannelManager(ILogManager logger, ConfigManager config)
		{
			if (logger == null)
			{
				throw new ArgumentNullException(nameof(logger), @"Is Null");
			}

			this.logger = logger;
		}


	}
}
