namespace PtpChat.Main.Managers
{
    using System;
    using System.Collections.Concurrent;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;
    using PtpChat.Utility;

    public class ChannelManager : IChannelManager
    {
        public ChannelManager(ILogManager logger, ConfigManager config)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger), @"Is Null");
            }

            this.logger = logger;
        }

        private readonly ConcurrentDictionary<Guid, Channel> Channels = new ConcurrentDictionary<Guid, Channel>();

		private readonly ConcurrentDictionary<Guid, Message> Messages = new ConcurrentDictionary<Guid, Message>();

        private readonly ILogManager logger;

		public bool IsNodeInChannel(Guid nodeId, Guid channelId)
		{
			throw new NotImplementedException();
		}
	}
}
