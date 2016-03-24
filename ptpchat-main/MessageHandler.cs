namespace PtpChat.Main
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using PtpChat.Base.Interfaces;
	using Base.Messages;
	using Newtonsoft.Json;

	internal class MessageHandler : IMessageHandler
	{
		private static readonly string LogCannotParseJson = "Unable to deserialise Json message, ignoring";
		private static readonly string LogUnexpectedError = "Unexpected error";

		private ILogManager logger;
		private Dictionary<MessageType, IVerbHandler> handlers;

		public MessageHandler(ILogManager logger, 
			Dictionary<MessageType, IVerbHandler> handlers)
		{
			this.logger = logger;
			this.handlers = handlers;
		}

		public void HandleMessage(string messageJson, IPEndPoint senderEndpoint)
		{
			try
			{
				var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(messageJson);

				this.handlers[baseMessage.msg_type].HandleMessage(messageJson, senderEndpoint);

			}
			catch(JsonException)
			{
				this.logger.Warning(MessageHandler.LogCannotParseJson);
			}
			catch(Exception e) // global!!
			{
				this.logger.Error(MessageHandler.LogUnexpectedError, e);
			}
		}

		public string BuildMessage(BaseMessage message)
		{
			return JsonConvert.SerializeObject(message);
		}
	}
}
