namespace PtpChat.Main
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PtpChat.Base.Classes;
    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;

    internal class ChannelTabHandler : IChannelTabHandler
    {
        private readonly IChannelManager ChannelManager;

        private readonly IDictionary<Guid, IChannelTab> ChannelTabs;

        private readonly ILogManager Logger;

        private readonly IMessageHandler MessageHandler;

        private readonly INodeManager NodeManager;

        private readonly ISocketHandler SocketHandler;

        public ChannelTabHandler(ILogManager logger, IDataManager dataManager, IMessageHandler messageHandler, ISocketHandler socketHandler)
        {
            this.ChannelTabs = new Dictionary<Guid, IChannelTab>();
            this.Logger = logger;
            this.SocketHandler = socketHandler;
            this.ChannelManager = dataManager.ChannelManager;
            this.MessageHandler = messageHandler;
            this.NodeManager = dataManager.NodeManager;
        }

        public void AddChannelTab(Guid channelId, IChannelTab channelTab)
        {
            this.ChannelTabs.Add(channelId, channelTab);
        }

        public void SendMessage(Guid channelId, string message)
        {
            var chatMessage = new ChatMessage { ChannelId = channelId, DateSent = DateTime.Now, MessageContent = message, MessageId = Guid.NewGuid(), SenderId = this.NodeManager.LocalNode.NodeId };

            this.ChannelManager.HandleMessageForChannel(chatMessage);

            var channel = this.ChannelManager.GetChannels(c => c.Value.ChannelId == channelId).First();

            var recipientList = new List<Dictionary<string, string>>();

            foreach (var nodeId in channel.Nodes)
            {
                var recipient = new Dictionary<string, string>();
                recipient.Add("node_id", nodeId.ToString());
                recipientList.Add(recipient);
            }

            var outMsg = new MessageMessage
                             {
                                 msg_data =
                                     new MessageData
                                         {
                                             attachment = null,
                                             channel = channel.ChannelName,
                                             channel_id = channel.ChannelId,
                                             message = chatMessage.MessageContent,
                                             timestamp = chatMessage.DateSent,
                                             msg_id = chatMessage.MessageId,
                                             node_id = this.NodeManager.LocalNode.NodeId,
                                             recipient = recipientList
                                         }
                             };

            var encodedMsg = Encoding.ASCII.GetBytes(this.MessageHandler.BuildMessage(outMsg));

            foreach (var nodeId in channel.Nodes)
            {
                this.SocketHandler.SendMessage(nodeId, encodedMsg);
            }
        }

        public void MessageRecieved(ChatMessage message)
        {
            if (message == null || message.ChannelId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(message), @"Invalid ChatMessage object");
            }

            if (!this.ChannelTabs.ContainsKey(message.ChannelId))
            {
                throw new InvalidOperationException($"There is no tab that can handle this message for channel {message.ChannelId}");
            }

            this.ChannelTabs[message.ChannelId].MessageRecieved(message);
        }
    }
}