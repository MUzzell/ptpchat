namespace PtpChat.Main.Managers
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Collections.Generic;
	using System.Collections.Concurrent;

	using PtpChat.Base.Classes;
	using PtpChat.Base.Interfaces;
	using PtpChat.Utility;
	using Base.EventArguements;

	public class ChannelManager : IChannelManager
    {
		private const string LogAddedChannel = "Added new channel, Channel ID: {0}";
		private const string LogDeletedChannel = "Deleted channel, Channel ID: {0}";
		private const string LogUpdatedChannel = "Updated channel, Channel ID: {0}";

		private readonly ConcurrentDictionary<Guid, Channel> Channels = new ConcurrentDictionary<Guid, Channel>();

		private readonly ConcurrentDictionary<Guid, ChatMessage> Messages = new ConcurrentDictionary<Guid, ChatMessage>();

		private readonly ILogManager logger;

		private readonly TimeSpan ChannelCutoff;

		private Timer ProcessTimer;

		public event EventHandler ChannelAdd;
		public event EventHandler ChannelDelete;
		public event EventHandler ChannelUpdate;

		private static object updateLock = new object();

		public ChannelManager(ILogManager logger, ConfigManager config)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger), @"Is Null");
            }

            this.logger = logger;
			this.ChannelCutoff = config.ChannelCutoff;
        }
		
		private void ProcessChannels(object state)
		{
			foreach (Channel channel in this.GetChannels(n => n.Value.LastTransmission < DateTime.Now.Subtract(this.ChannelCutoff)))
			{
				channel.IsUpToDate = false;
			}
		}
		
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

			lock (ChannelManager.updateLock)
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

		public IEnumerable<Channel> GetChannels(Func<KeyValuePair<Guid, Channel>, bool> filter) => this.Channels.Where(filter).Select(n => n.Value);

		public IEnumerable<Channel> GetChannels() => this.Channels.Select(n => n.Value);
		
	}
}
