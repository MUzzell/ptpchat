﻿namespace PtpChat.Main
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Threading;

	using Base.Interfaces;
	using Utility;
	using Base.EventArguements;
	using Base.Classes;

	internal class ResponseManager : IResponseManager
	{
		private const string LogMaxAttemptsExceeded = "Message {0} has exceeded the maximum resend attempts.";

		public event EventHandler OnAckRecieved;
		public event EventHandler OnMessageAdded;
		public event EventHandler OnMessageSendFail;

		private readonly ILogManager logger;

		private readonly ConcurrentDictionary<Guid, ResponseMessage> messages;

		private readonly TimeSpan messageCutoff;
		private readonly int maxAttempts;

		private readonly Timer ProcessTimer;
		

		public ResponseManager(ILogManager logger, ConfigManager config)
		{
			this.logger = logger;
			this.messageCutoff = config.MessageCutoff;
			this.maxAttempts = config.MaxMessageResendAttempts;

			this.messages = new ConcurrentDictionary<Guid, ResponseMessage>();

			this.ProcessTimer = new Timer(this.ProcessMessages, null, 10000, 5000);
		}

		public bool AddOrUpdate(Guid msgId, Guid targetNodeId, byte[] message)
		{
			ResponseMessage currentMessage;
			if (this.messages.TryGetValue(msgId, out currentMessage))
			{
				if (currentMessage.Attempts == this.maxAttempts)
				{
					
					if (!this.messages.TryRemove(msgId, out currentMessage))
					{
						throw new InvalidOperationException("AddOrUpdate, could not remove message which had exceeded its max attempts");
					}
					this.logger.Warning(string.Format(LogMaxAttemptsExceeded, msgId));
					this.OnMessageSendFail.Invoke(this, new ResponseEventArgs { MsgId = msgId });
					return false;
				}

				currentMessage.Attempts += 1;
				currentMessage.NeedsResend = false;
				currentMessage.LastSent = DateTime.Now;
				if (!this.messages.TryUpdate(msgId, currentMessage, currentMessage))
					throw new InvalidOperationException("AddOrUpdate, could not update message");
			}
			else // 1st attempt
			{
				currentMessage = new ResponseMessage
				{
					Added = DateTime.Now,
					Attempts = 1,
					LastSent = DateTime.Now,
					Msg = message,
					MsgId = msgId,
					TargetNodeId = targetNodeId,
					NeedsResend = false
				};
				this.messages.TryAdd(msgId, currentMessage);

				this.logger.Info($"Added message (id {msgId.ToString()} to ResponseManager");
				this.OnMessageAdded.Invoke(this, new ResponseEventArgs { MsgId = msgId });
			}
			return true;
		}

		public IEnumerable<ResponseMessage> GetMessages() => this.messages.Values;

		public IEnumerable<ResponseMessage> GetOutstandingMessages()
		{
			return this.messages.Where(kv => kv.Value.NeedsResend).Select<KeyValuePair<Guid, ResponseMessage>, ResponseMessage>(kv => kv.Value);
		}

		private void ProcessMessages(object state)
		{
			foreach (ResponseMessage message in this.messages.Where(kv => kv.Value.LastSent < DateTime.Now - this.messageCutoff).Select<KeyValuePair<Guid, ResponseMessage>, ResponseMessage>(kv => kv.Value))
			{
				message.NeedsResend = true;
				this.messages.TryUpdate(message.MsgId, message, message);
			}
		}

		public void AckRecieved(Guid msgId)
		{
			if (msgId == Guid.Empty)
				throw new ArgumentException("AckRecieved, invalid msgId");

			ResponseMessage msg;

			if (!this.messages.TryRemove(msgId, out msg))
				throw new KeyNotFoundException("AckRecieved, cannot find given msgId");
			
			this.logger.Info($"ACK recieved for message {msgId.ToString()}.");

			this.OnAckRecieved.Invoke(this, new ResponseEventArgs { MsgId = msgId });
		}
		
	}

	
}
