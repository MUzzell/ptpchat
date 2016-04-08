namespace PtpChat.Main.Managers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using PtpChat.Base.Classes;
    using PtpChat.Base.EventArguements;
    using PtpChat.Base.Interfaces;
    using PtpChat.Utility;

    public class ChannelManager : IChannelManager
    {
        private const string LogAddedChannel = "Added new channel, Channel ID: {0}";
        private const string LogDeletedChannel = "Deleted channel, Channel ID: {0}";
        private const string LogMessageRecieved = "message recieved, Channel ID: {0}";
        private const string LogUpdatedChannel = "Updated channel, Channel ID: {0}";

        private static readonly object updateLock = new object();

        private readonly TimeSpan ChannelCutoff;

        private readonly ConcurrentDictionary<Guid, Channel> Channels = new ConcurrentDictionary<Guid, Channel>();

        private readonly ILogManager logger;

        private Timer ProcessTimer;

        public ChannelManager(ILogManager logger, ConfigManager config)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger), @"Is Null");
            }

            this.logger = logger;
            this.ChannelCutoff = config.ChannelCutoff;
        }

        public event EventHandler ChannelAdd;
        public event EventHandler ChannelDelete;
        public event EventHandler ChannelUpdate;
        public event EventHandler MessageRecieved;

        public bool IsNodeInChannel(Guid channelId, Guid nodeId)
        {
            if (!this.Channels.ContainsKey(channelId))
            {
                return false;
            }

            return this.Channels[channelId].Nodes.Contains(nodeId);
        }

        public void Add(Channel channel)
        {
            if (!this.Channels.TryAdd(channel.ChannelId, channel))
            {
                throw new InvalidOperationException("Add, Channel is already present");
            }

            this.ChannelAdd?.Invoke(this, new ChannelEventArgs { Channel = channel });

            this.logger.Info(string.Format(LogAddedChannel, channel.ChannelId));
        }

        public Channel Delete(Channel channel)
        {
            if (channel?.ChannelId == null)
            {
                throw new ArgumentNullException(nameof(channel), @"channel or its ID is null");
            }

            return this.Delete(channel.ChannelId);
        }

        public Channel Delete(Guid channelId)
        {
            if (channelId == null || channelId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(channelId), @"Invalid channelId");
            }

            Channel outChannel;
            if (!this.Channels.TryRemove(channelId, out outChannel))
            {
                throw new InvalidOperationException("Delete, NodeID not present");
            }

            this.logger.Info(string.Format(LogDeletedChannel, outChannel.ChannelId));

            this.ChannelDelete?.Invoke(this, new ChannelEventArgs { Channel = outChannel });

            return outChannel;
        }

        public void Update(Guid channelId, Action<Channel> updateFunc)
        {
            if (channelId == Guid.Empty || updateFunc == null)
            {
                throw new ArgumentNullException(@"channelId or updateFunc is null");
            }

            Channel currentChannel, channel;

            if (!this.Channels.TryGetValue(channelId, out currentChannel))
            {
                throw new InvalidOperationException("Update, could not find Node");
            }

            lock (updateLock)
            {
                channel = this.Channels[channelId];
                updateFunc(channel);

                if (!this.Channels.TryUpdate(channelId, channel, currentChannel))
                {
                    throw new InvalidOperationException("Update, unable to update node");
                }
            }

            this.ChannelUpdate?.Invoke(this, new ChannelEventArgs { Channel = channel });

            this.logger.Info(string.Format(LogUpdatedChannel, channelId));
        }

        public void HandleMessageForChannel(ChatMessage message)
        {
            if (message == null || message.MessageContent == null || message.MessageId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(message), @"Invalid message object");
            }

            var channel = this.Channels.FirstOrDefault(c => c.Value.ChannelId == message.ChannelId).Value;

            if (channel == null)
            {
                throw new InvalidOperationException("Sending message for unknown Channel");
            }

            this.Update(message.ChannelId, c => c.AddMessage(message));

            this.MessageRecieved?.Invoke(this, new ChannelMessageEventArgs { Channel = channel, ChatMessage = message });

            this.logger.Info(string.Format(LogMessageRecieved, channel.ChannelId));
        }

        public IEnumerable<Channel> GetChannels(Func<KeyValuePair<Guid, Channel>, bool> filter) => this.Channels.Where(filter).Select(n => n.Value);

        public IEnumerable<Channel> GetChannels() => this.Channels.Select(n => n.Value);

        private void ProcessChannels(object state)
        {
            foreach (var channel in this.GetChannels(n => n.Value.LastTransmission < DateTime.Now.Subtract(this.ChannelCutoff)))
            {
                channel.IsUpToDate = false;
            }
        }
    }
}