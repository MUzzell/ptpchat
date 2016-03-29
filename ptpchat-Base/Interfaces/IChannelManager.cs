namespace PtpChat.Base.Interfaces
{
	using System.Collections.Generic;

	using Classes;
	using System;
	
	public interface IChannelManager
	{
		/// <summary>
		/// Checks that the given ChannelId is in the given Channel, according to this ChannelManager
		/// </summary>
		/// <param name="nodeId">Target ChannelId</param>
		/// <param name="channelId">Target Channel</param>
		/// <returns>bool determining if this node is in the channel.</returns>
		bool IsNodeInChannel(Guid nodeId, Guid channelId);

		event EventHandler ChannelAdd;

		event EventHandler ChannelDelete;

		event EventHandler ChannelUpdate;

		/// <summary>
		/// Attempt to add the given Channel.
		/// Throws an ArgumentException if unable to do so, such as if the ChannelId already exists.
		/// <param name="channel">The new channel to be added, will be compared against its ChannelId.</param>
		/// </summary>
		void Add(Channel channel);

		/// <summary>
		/// Attempt to delete the given Channel.
		/// Throws an ArgumentException if unable to do so, such as if the ChannelId does not exists.
		/// <param name="channel">The channel to be deleted, will be compared against its ChannelId.</param>
		/// </summary>
		Channel Delete(Channel channel);

		/// <summary>
		/// Attempt to delete the given Channel, using the given channelId.
		/// Throws an ArgumentException if unable to do so, such as if the ChannelId does not exists.
		/// <param name="channelId">The channelId of the channel to be deleted.</param>
		/// </summary>
		Channel Delete(Guid channelId);

		/// <summary>
		/// Updates the channel which has the given channelId, using the funtion provided to edit its attributs.
		/// Throws an ArgumentException if unable to do so, such as if the Channel does not exists.
		/// <param name="channelId">The id of the channel to be updated.</param>
		/// <param name="updateFunc">The function that will edit the parameters of the target channel.</param>
		/// </summary>
		void Update(Guid channelId, Action<Channel> updateFunc);

		IEnumerable<Channel> GetChannels();

		IEnumerable<Channel> GetChannels(Func<KeyValuePair<Guid, Channel>, bool> filter);
	}
}