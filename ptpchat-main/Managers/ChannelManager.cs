namespace PtpChat.Main
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

        private readonly ILogManager logger;

        private readonly ConcurrentDictionary<Guid, ChannelMessage> Messages = new ConcurrentDictionary<Guid, ChannelMessage>();
    }
}