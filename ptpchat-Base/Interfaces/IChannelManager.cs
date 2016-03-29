namespace PtpChat.Base.Interfaces
{
	using System;

	public interface IChannelManager
	{
		/// <summary>
		/// Checks that the given NodeId is in the given Channel, according to this ChannelManager
		/// </summary>
		/// <param name="nodeId">Target NodeId</param>
		/// <param name="channel_id">Target Channel</param>
		/// <returns>bool determining if this node is in the channel.</returns>
		bool IsNodeInChannel(Guid nodeId, Guid channelId);
	}
}