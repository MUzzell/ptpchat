namespace PtpChat.Main
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    internal class MessageHandler : IMessageHandler
    {
        private static readonly string LogCannotParseJson = "Unable to deserialise Json message, ignoring";

        private static readonly string LogUnexpectedError = "Unexpected error";

        private readonly Dictionary<MessageType, IVerbHandler> handlers;

        private readonly ILogManager logger;

        public MessageHandler(ILogManager logger)
        {
            this.logger = logger;
            this.handlers = new Dictionary<MessageType, IVerbHandler>();

            //required to put the MessageType enum as a string and not a value.
            JsonConvert.DefaultSettings = () =>
                {
                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
                    return settings;
                };
        }

        public void HandleMessage(string messageJson, IPEndPoint senderEndpoint)
        {
            try
            {
                var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(messageJson);

                this.handlers[baseMessage.msg_type].HandleMessage(messageJson, senderEndpoint);
            }
            catch (JsonException)
            {
                this.logger.Warning(LogCannotParseJson);
            }
            catch (Exception e) // global!!
            {
                this.logger.Error(LogUnexpectedError, e);
            }
        }

        public string BuildMessage(BaseMessage message) => JsonConvert.SerializeObject(message);

        public void AddHandler(MessageType type, IVerbHandler handler) => this.handlers.Add(type, handler);
    }
}