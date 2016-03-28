namespace PtpChat.Main.Managers
{
    using System;
    using System.Collections.Concurrent;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;

    public class ChannelManager : IChannelManager
    {
        public ChannelManager(ILogManager logger)
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