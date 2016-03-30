namespace PtpChat.Base.Interfaces
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// This is designed to track all messages that need to guarantee transmission (i.e. have a msg_id element)
	/// When this program needs to send a message of this nature, it should add it to this manager, which will do
	///		a) Track when we sent the message
	///		b) Record the ACK or NACK (well, delete it from this manager)
	///		c) Mark the message for resend if it has not recieved an ACK or NACK after a period of time.
	/// </summary>
	public interface IResponseManager
	{
		event EventHandler OnAckRecieved;

		event EventHandler OnNackRecieved;

		event EventHandler OnMessageAdded;

		event EventHandler OnMessageSendFail;

		/// <summary>
		/// Adds or update a message to this PostMaster.
		/// Messages that are being re-sent should also call this method, which updates the internal tracker.
		/// Messages should be sent after calling this method.
		/// </summary>
		/// <param name="msgId">the messageId of the message.</param>
		/// <param name="message">The raw message to be sent.</param>
		/// <returns>Indicates the operation was successful, if false, the message should not be sent.</returns>
		bool AddOrUpdate(Guid msgId, byte[] message);

		/// <summary>
		/// Get all messageIds currently stored.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Guid> GetMessages();

		/// <summary>
		/// Get all outstanding messages (i.e. have passed the response deadline).
		/// </summary>
		/// <returns>A collection of messages that are outstanding.</returns>
		IEnumerable<byte[]> GetOutstandingMessage();

		/// <summary>
		/// This message has been delivered correctly. Remove it and call the event handler.
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if the given msgId is invalid.</exception>
		/// <exception cref="KeyNotFoundException">Thrown if the given msgId does not point to a message.</exception>
		/// <param name="msgId"></param>
		void AckRecieved(Guid msgId);

		/// <summary>
		/// This message has not been delivered correctly. Remove it and call the event handler.
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if the given msgId is invalid.</exception>
		/// <exception cref="KeyNotFoundException">Thrown if the given msgId does not point to a message.</exception>
		/// <param name="msgId"></param>
		void NackRecieved(Guid msgId);
	}
}
