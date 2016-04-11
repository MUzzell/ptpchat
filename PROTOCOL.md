Key areas of concern

* message propagation, getting things to all recipients
* id collision.
* encryption in channels, use keys? or multi cert encryption?
* abuse, how can we reduce / mitigate DDoS attacks?
* Are we 'secure'? What flaws do we have keeping communication secure?

#ptpchat protocol

This protocol is based upon P2P CHAT, A peer-to-peer chat protocol, which can be found [here][1].

All of this is subject to change at any time, as this project is very much in the testing/prototyping phase.

###Message Format 

Messages will be transferred in JSON. At the head, a message will contain four keys, All other elements in the head should be ignored. These keys are:

1. `msg_type`, containing the verb.
2. `msg_data`, containing a JSON object holding the data relevant to the message being sent. 
3. `ttl`, containing the current time to live value as an integer for this message.
4. `flood`, describing if this message should be flooded to the whole network (i.e. all other connected nodes)

All keys, and all verb-specific keys inside `msg_data`, must be in lower-case.

###Data Types

####*ttl*

The `ttl` attribute is used to monitor how a message is broadcast around the network of nodes, its behaviour is similar to the TTL value in the Internet Protocol (IP). It is used to allow a node to re-transmit a received message to the intended node, or to the node that might be able to better handle this message (how this is decided is not figured out yet). `ttl` Is a single integer value set to define the number of permitted hops remaining for this message. The `ttl` should be reduced by one and then rebroadcast.

Certain messages require a specific `ttl` to be applied, as the message's purpose may only be for the intended neighbouring node. In other cases, the TTL can be of any number, defaulting to **32**. See the message formats to view which messages use a TTL of 1 or a TTL of any (marked as 32 in this document).

####*flood*

The `flood` attribute is used to broadcast this message to all connected nodes. It is a single boolean value of either *true* or *false* and should be used on a subset of messages. Upon receiving a message that has `flood` set to *true*, the message should be rebroadcast (and its `ttl` reduced by one) to all neighbouring nodes. 

The `flood` attribute is only set to true on a subset of messages. See the message formats to veiw which messages has this attribute set to true.

####*node_id*

The `node_id` attribute is used in multiple messages and acts as the identifier of nodes (and 'should' be unique). `node_id` Is single GUID identifier and must remain the same throughout the node's lifetime. Whilst this identifies the node in question, it does not validate the node. 

####*version*

The `version` attribute is used in the **HELLO** message and is an optional string to identify the software that the node is running. There is no specific format for this, and is purely informational. 

####*address*

The `address` attribute used in some messages specifies a socket to be used for connecting to nodes. The contents will be in the standard socket notation of <HOST>:<PORT>.

###Verbs

#####HELLO

```json
{
    "ttl" : 1,
    "flood" : false,
    
    "msg_type":"HELLO",
    "msg_data":
    {
        "node_id" : "<node_id>",
        "version" : "<version>?",
    }
}
```

* `node_id` : Identifies the sending node.
* `version` : An optional, short string to identify the software and version of this node.

This is a periodic message that is sent between nodes. Its purpose is to notify the target that it is online and available for communication. This message should be sent to known machines at an interval of between 0.5 to 2 seconds. The HELLO message does not require a response. Nodes upon receiving a HELLO message should now start sending HELLO messages in the same periodic fashion to the original node to maintain communication.

#####ACK

```json
{
    "ttl" : 1,
    "flood" : false,
    
    "msg_type":"ACK", 
    "msg_data": 
    { 
        "msg_id" : "<msg_id>" 
    }
}
```

* `msg_id` : contains the id for the message is ACK is acknowledging. 

Sent after GETCERTIFICATE, CERTIFICATE, GETKEY, KEY, MESSAGE messages, used to notify that a message has been received correctly by the immediate recipient. An ACK message *MUST* include the message ID of the received message. 

#####JOIN

```json
{
    "ttl" : 32,
    "flood" : true,
    
    "msg_type":"JOIN", 
    "msg_data": 
    { 
        "msg_id" : "<msg_id>", 
        "node_id" : "<node_id>",
        "channel" : "<channel>",
        "channel_id" : "<channel_id>" 
    }
}
```
    
* `msg_id` : identifies this message.
* `node_id` : contains the sending node id.
* `channel` : contains the channel name.
* `channel_id` : contains the channel id.

JOIN Messages are sent to notify other user of a channel that a new node is joining. A JOIN message is sent when:
* Once a node is joining an available channel (a channel in which this node has been made aware of)
* When a node is already subscribed to a channel for which a *CHANNEL* message is received which did not include this node in its listing.

#####CHANNEL

```json
{
    "ttl" : 32,
    "flood" : true,
    
    "msg_type" : "CHANNEL", 
    "msg_data" : 
    { 
        "msg_id" : "<msg_id>", 
        "node_id" : "<node_id>", 
        "channel" : "<channel>",  
        "channel_id" : "<channel_id>",
        "members" : [ {"node_id" : "<node_id>" } ],
        "closed" : "<closed:FALSE>?"
    }
}
```

* `msg_id` : identifies this message.
* `channel` : contains the channel name.
* `channel_id` : contains the channel id.
* `node_id` : contains the sending node id.
* `members` : a list of `node_id` elements.
* `closed` : an optional flag that marks this channel as closed (default false).  

CHANNEL messages are used to maintain available channels between nodes. A CHANNEL message is sent when:
* A node creates a channel, to all neighbouring nodes.
* To maintain channels by periodically sending a CHANNEL message if this node has not received/sent a CHANNEL message for a channel this node is subscribed to. 
* Optionally, a node may send CHANNEL messages may be sent to newly connected nodes.

#####LEAVE

```json
{
    "ttl" : 32,
    "flood" : true,
    
    "msg_type":"JOIN", 
    "msg_data": 
    { 
        "msg_id" : "<msg_id>", 
        "node_id" : "<node_id>",
        "channel" : "<channel>",
        "channel_id" : "<channel_id>" 
    }
}
```

* `msg_id` : identifies this message.
* `node_id` : contains the sending node id.
* `channel` : contains the channel name.
* `channel_id` : contains the channel id.
    
Sent to all members of a channel that the sending user is leaving the channel. A LEAVE message is sent when:
* Once a node leaves a channel it is subscribed to.
* When a node received a CHANNEL message which list this node which this node is not part of. 

#####GETCERTIFICATE

```json
{
    "ttl" : 32,
    "flood" : false,
    
    "msg_type":"GETCERTIFICATE",
    "msg_data":
    {
        "msg_id" : "<msg_id>",
        "node_id" : "<node_id>"
    }
}
```

* `msg_id` : identifies this message,
* `node_id` : contains the **sending** node id. 

This message is used to request the certificate of the recipient, as to identify this node. The `msg_id` should also be the same in any responsive CERTIFICATE messages. Nodes *should* keep a record of certs against `node_id's` and store them locally if possible for future reference. 

#####CERTIFICATE

```json
{
    "ttl" : 32,
    "flood" : false,
    
    "msg_type" : "CERTIFICATE",
    "msg_data" :
    {
        "msg_id" : "<msg_id>",
        "node_id" : "<msg_id>",
        "recipient_id" : "<recipient_id>",
        "certificate" : "<certificate>"
    }
}
```
* `msg_id` : Identifies this message, must be the same as the `msg_id` from a GETCERTIFICATE message this message is responding to.
* `node_id` : contains this node's id.
* `recipient_id` : Contains the `node_id` that was present in the GETCERTIFICATE message this message is responding to.
* `certificate` : Contains the certificate as a base64 encoded string.
This message contains the certificate for a given node, used to identify this certificate. A CERTIFICATE message is sent when:
* In response to a GETCERTIFICATE message. In this case, msg_id **must** be the same as the id in the received GETCERTIFICATE message.
* In advance of a GETCERTIFICATE message when this node has joined an encrypted channel. 
Nodes *should* keep a record of certs against `node_id's` and store them locally if possible for future reference. 

#####GETKEY

```json
{
    "ttl" : 32,
    "flood" : false,
    
    "msg_type" : "GETKEY",
    "msg_data" : 
    {
        "msg_id" : "<msg_id>",
        "node_id" : "<node_id>",
        "channel" : "<channel>",
        "channel_id" : "<channel_id>",
    }
}
```

* `msg_id` : identifies this message.
* `node_id` : contains the sending node id.
* `channel` : contains the channel name.
* `channel_id` : contains the channel id.

This message is sent to request a key to a closed channel.

#####KEY

```json
{
    "ttl" : 32,
    "flood" : false,
    
    "msg_type" : "KEY",
    "msg_data" : 
    {
        "msg_id" : "<msg_id>",
        "node_id" : "<node_id>",
        "recipient_id" : "<recipient_id>",
        "channel" : "<channel>",
        "channel_id" : "<channel_id>",
        "cipher" : "<cipher>",
        "key" : "<key>"
    }
}
```

* `msg_id` : identifies this message.
* `node_id` : contains the sending node id.
* `channel` : contains the channel name.
* `channel_id` : contains the channel id.
* `cipher` : defines the cipher used for the channel's encryption.
* `key` : contains the key as a base64 encoded string.

This message transmits the key to the target node. The key **must** be encrypted with the target's public key (which means that the sending node already has the recipient node's certificate). Whether to send a KEY message is at the discretion of the sender, and holds the responsibility to determine if the recipient should receive the key.

#####MESSAGE

```json
{
    "ttl" : 32,
    "flood" : false,
    
    "msg_type" : "MESSAGE",
    "msg_data" : 
    {
        "msg_id" : "<msg_id>",
        "node_id" : "<msg_id>",
        "recipient" : [ { "node_id" : "<node_id>" } ],
        "timestamp" : "<timestamp>",
        "channel" : "<channel>",
        "channel_id" : "<channel_id>",
        "message" : "<message>",
        "attachment" : [ { "attachment" : "<attachment>", "attachment_type" : "<attachment_type>" } ]
    }
}
```

* `msg_id` : identifies this message.
* `node_id` : contains the sending node id.
* `channel` : contains the channel name.
* `channel_id` : contains the channel id.
* `recipient` : contains a list of recipients for this message.
* `timestamp` : a timestamp for this message.
* `message` : a base64 encoded string that contains the signed message from the sender.
* `attachment` : An optional list of attachments to this message. `attachment_type` contains the MIME Type for the attachment and `attachment` contains the actual attachment as a base64 encoded string. 

This message is the actual text message to be sent between nodes. The `message` element **must** be signed by the sender's private key, to be verified by recipient node's using the senders public key (which they may or may not have, which may trigger GETCERTIFICATE messages to be sent).

#####CONNECT

```json
{
    "ttl" : 32,
    "flood" : false,
    
    "msg_type" : "CONNECT",
    "msg_data" : 
    {
        "src_node_id" : "<src_node_id>",
        "dst_node_id" : "<dst_node_id>",
        "dst" : "<dst>",
        "src" : "<src>"
    }
}
```

* `src_node_id` : The node_id of the node initiating this connect process.
* `dst_node_id` : The node_id of the target for this connect process. 
* `dst` : The target of this CONNECT message, contains a full or partial socket.
* `src` : The origin of this CONNECT message, contains a full or partial socket. 

Used to open a path between two nodes, whom have been made aware of each other through another node, but cannot communicate directly. This is used to allow nodes that are present behind NAT devices to establish a connection. A client would need to respond to this message in a very specific manner to ensure the connection is established successfully. 

As an example, two nodes, *A* and *B*, want to talk to each other and will use node *S* to achieve it. 
    
1. *A* opens an available socket and sends the following to *S*:
    `{"msg_type":"CONNECT", "msg_data":{"src_node_id": "<src_node_id>", "dst_node_id": "<dst_node_id>", "dst":"<B's IP>", "src":"<A's Port>"}`
2. *S* forwards the message to *B*, applying *A's* IP:
    `{"msg_type":"CONNECT", "msg_data":{"src_node_id": "<src_node_id>", "dst_node_id": "<dst_node_id>", "dst":"<B's IP>", "src":"<A's IP>:<A's Port>"}`
3. *B* opens an available socket for communication, and returns the message to *S* adding the newly opened port (at this time, *B* should start trying to communicate with *A*, to begin getting through the NAT):
    `{"msg_type":"CONNECT", "msg_data":{"src_node_id": "<src_node_id>", "dst_node_id": "<dst_node_id", "dst":"<B's IP>:<B's Port>", "src":"<A's IP>:<A's Port>"}`
4. *S* forwards the message, adding *B's* IP to the packet:
    `{"msg_type":"CONNECT", "msg_data":{"src_node_id": "<src_node_id>", "dst_node_id": "<dst_node_id>", "dst":"<B's IP>:<B's Port>", "src":"<A's IP>:<A's Port>"}`
5. *A* now has a full CONNECT message and can now start communicating with B. 
    
#####ROUTING

```json
{
    "ttl" : 1,
    "flood" : false,
    
    "msg_type" : "ROUTING",
    "msg_data" : 
    {
        "node_id" : "<node_id>",
        "nodes" : [ { "node_id" : "<node_id>", "address" : "<address>" } ]
    }
}
```

* `node_id` : contains the sending node id.
* `nodes` : A list of available nodes that this node is aware of.

A periodic message sent by a node to neighbouring nodes which lists nodes it can communicate to. ROUTING Messages should be sent to all nodes that this node can communicate with directly, but a node may chose to omit results or to not send this message should it decide to. 

#####RELAY

*to be, well, figured out*


[1]: https://tools.ietf.org/html/draft-strauss-p2p-chat-08
