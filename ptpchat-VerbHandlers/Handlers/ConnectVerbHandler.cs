namespace PtpChat.VerbHandlers.Handlers
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Linq;

    using PtpChat.Base.Interfaces;
    using PtpChat.Base.Messages;
    using Base.Classes;
    public class ConnectVerbHandler : BaseVerbHandler<ConnectMessage>
    {
        private const string LogInvalidSenderId = "CONNECT message contains invalid src_node_id, ignoring";
        private const string LogInvalidRecipientId = "CONNECT message contains invalid dst_node_id, ignoring";
        private const string LogInvalidSrc = "CONNECT message contains invalid src element, ignoring";
        private const string LogNotForThisNode = "CONNECT message was not for this node, ignoring";

        private const string ipv4Pattern = @"^((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?\:?([0-9]{1,5})?$";

        private ConnectMessage Message { get; set; }

        private Regex ipv4Regex;

        public ConnectVerbHandler(ILogManager logger, IDataManager dataManager, IOutgoingMessageManager outgoingMessageManager)
            : base(logger, dataManager, outgoingMessageManager)
        {
            ipv4Regex = new Regex(ConnectVerbHandler.ipv4Pattern);
        }

        protected override bool HandleVerb(ConnectMessage message, IPEndPoint senderEndpoint)
        {
            //         //somebody connecting to us

            //         //sender => A
            //         var senderId = message.msg_data.src_node_id;

            //if (senderId != Guid.Empty)
            //{
            //	this.logger.Warning(LogInvalidSenderId);
            //	return false;
            //}

            //var senderNode = this.NodeManager.GetNodes(kv => kv.Key == senderId).FirstOrDefault();

            //         //A's complete endpoint
            //         var sender = string.IsNullOrWhiteSpace(message.msg_data.src) ? null : message.msg_data.src;

            ////sender should never be null
            //if (sender == null)
            //{
            //	this.logger.Warning(LogInvalidSrc);
            //	return false;
            //}

            //         //recipient => B
            //         var recipientId = message.msg_data.dst_node_id;

            //if (recipientId != Guid.Empty)
            //{
            //	this.logger.Warning(LogInvalidRecipientId);
            //	return false;
            //}

            //var recipientNode = this.NodeManager.GetNodes(kv => kv.Key == recipientId).FirstOrDefault();

            ////do nothing! we're connected!
            //if (senderId == this.NodeManager.LocalNode.NodeId.Id && (recipientNode != null && recipientNode.IsConnected))
            //{
            //	return false;
            //}

            ////do nothing! 
            //if (recipientId == this.NodeManager.LocalNode.NodeId.Id && (senderNode != null && senderNode.IsConnected))
            //{

            //}

            //var recipient = string.IsNullOrWhiteSpace(message.msg_data.dst) ? null : message.msg_data.dst;

            //var senderMatch = ipv4Regex.Match(sender);
            //var recipientMatch = recipient != null ? ipv4Regex.Match(recipient) : null;

            //         this.logger.Debug($"Connect message recieved from: {senderId}");

            ////connect logic
            ////type 1: dst has nothing, src has port. Add src IP and forward to dst node.
            //if (recipientMatch == null && (!senderMatch.Groups[0].Success && senderMatch.Groups[1].Success))
            //{
            //	//TODO: this case;
            //}

            ////type 2: dst has nothing, src has socket. Open port, start talking, add port to dst and return to sender.
            //if (recipientMatch == null && (senderMatch.Groups[0].Success && senderMatch.Groups[1].Success))
            //{
            //	if (recipientId != this.NodeManager.LocalNode.NodeId.Id)
            //	{
            //		this.logger.Warning(LogNotForThisNode);
            //		return false;
            //	}

            //	if (senderNode == null)
            //	{
            //		senderNode = new Node
            //		{
            //			Added = DateTime.Now,
            //			IpAddress = IPAddress.Parse(senderMatch.Groups[0].Value),
            //			Port = int.Parse(senderMatch.Groups[1].Value),
            //			NodeId = new NodeId("", senderId),
            //			LastRecieve = DateTime.Now //this is abit hacky.
            //		};

            //		this.NodeManager.Add(senderNode);
            //	}
            //	else
            //	{
            //		//this does not feel right..
            //		//what if C sent me a connect pretending to be B?
            //		//S might not let you?
            //		this.NodeManager.Update(senderId, n =>
            //		{
            //			n.IpAddress = IPAddress.Parse(senderMatch.Groups[0].Value);
            //			n.Port = int.Parse(senderMatch.Groups[1].Value);
            //			n.LastRecieve = DateTime.Now;
            //		});
            //	}
            //	message.msg_data.dst = "" + this.NodeManager.LocalNode.Port;
            //	this.OutgoingMessageManager.Send(senderEndpoint, message);
            //	return true;

            //}

            ////type 3: dst contains port, src contains socket. add dst ip and forward to src.
            //if ((recipientMatch != null && !recipientMatch.Groups[0].Success && recipientMatch.Groups[1].Success) && 
            //	(senderMatch.Groups[0].Success && senderMatch.Groups[1].Success))
            //{
            //	//TODO: this case;
            //}

            ////type 4: dst contains socket, src contains socket. start talking to dst.
            //if ((recipientMatch != null && recipientMatch.Groups[0].Success && recipientMatch.Groups[1].Success) &&
            //	(senderMatch.Groups[0].Success && senderMatch.Groups[1].Success))
            //{
            //	if (senderId != this.NodeManager.LocalNode.NodeId.Id)
            //	{
            //		this.logger.Warning(LogNotForThisNode);
            //		return false;
            //	}

            //	if (recipientNode == null)
            //	{
            //		recipientNode = new Node
            //		{
            //			Added = DateTime.Now,
            //			IpAddress = IPAddress.Parse(recipientMatch.Groups[0].Value),
            //			Port = int.Parse(recipientMatch.Groups[1].Value),
            //			NodeId = recipientId,
            //			LastRecieve = DateTime.Now
            //		};
            //		this.NodeManager.Add(recipientNode);
            //	}
            //	else
            //	{
            //		//this does not feel right..
            //		//what if C sent me a connect pretending to be B?
            //		//S might not let you?
            //		this.NodeManager.Update(recipientId, n =>
            //		{
            //			n.IpAddress = IPAddress.Parse(recipientMatch.Groups[0].Value);
            //			n.Port = int.Parse(recipientMatch.Groups[1].Value);
            //			n.LastRecieve = DateTime.Now;
            //		});
            //	}
            //	return true;
            //}
            return false;
        }
    }
}