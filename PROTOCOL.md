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

Messages will be transferred in JSON. At the head, a message will contain one or two keys, dependent on the verb being sent. `msg_type` is required for all messges, `msg_data` is not required for all messages. Should a recieved JSON message not contain a `msg_data` element, it should be treated as null (or None). All other elements in the head should be ignored. These two keys are:

1. `msg_type`, containing the verb.
2. `msg_data`, containing a JSON object holding the data relevant to the message being sent. 


###Verbs

####HELLO

`msg_data` is not required and should be ignored.

This is a periodic message that is sent between nodes. Its purpose is to notify the target that it is online and available for communication. This message should be sent to known machines at an interval of between 0.5 to 2 seconds. The HELLO message does not include a msg_data attribute (or its value is set to none), and does not require a response. Nodes upon receiving a HELLO message should now start sending HELLO messages in the same periodic fashion to the original node to maintain communication.

####ACK

```json
{
    "msg_type":"ACK", 
    "msg_data": 
    { 
        "msg_id" : <msg_id> 
    }
}
```

* `msg_id` : contains the id for the message is ACK is acknowledging. 

Sent after GETCERTIFICATE, CERTIFICATE, GETKEY, KEY, MESSAGE messages, used to notify that a message has been received correctly by the immediate recipient. An ACK message *MUST* include the message ID of the received message. 

####JOIN

    ```json
    {
        "msg_type":"JOIN", 
        "msg_data": 
        { 
            "msg_id" : <msg_id>, 
            "node_id" : <node_id>,
            "channel" : <channel>,
            "channel_id" : <channel_id> 
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

####CHANNEL

```json
{
    "msg_type" : "CHANNEL", 
    "msg_data" : 
    { 
        "msg_id" : <msg_id>, 
        "node_id" : <node_id>, 
        "channel" : <channel>,  
        "channel_id" : <channel_id>,
        "members" : [ {"node_id" : <node_id> } ],
        ("closed" : <closed:FALSE>)?
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

####LEAVE

```json
{
    "msg_type":"JOIN", 
    "msg_data": 
    { 
        "msg_id" : <msg_id>, 
        "node_id" : <node_id>,
        "channel" : <channel>,
        "channel_id" : <channel_id> 
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

####GETCERTIFICATE

```json
{
    "msg_type":"GETCERTIFICATE",
    "msg_data":
    {
        "msg_id" : <msg_id>,
        "node_id" : <node_id>
    }
}
```

* `msg_id` : identifies this message,
* `node_id` : contains the **sending** node id. 

This message is used to request the certificate of the recipient, as to identify this node. The `msg_id` should also be the same in any responsive CERTIFICATE messages. Nodes *should* keep a record of certs against `node_id's` and store them locally if possible for future reference. 

####CERTIFICATE

```json
{
    "msg_type" : "CERTIFICATE",
    "msg_data" :
    {
        "msg_id" : <msg_id>,
        "node_id" : <msg_id>,
        "recipient_id" : <recipient_id>,
        "certificate" : <certificate>
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

####GETKEY

```json
{
    "msg_type" : "GETKEY",
    "msg_data" : 
    {
        "msg_id" : <msg_id>
        "node_id" : <node_id>,
        "channel" : <channel>,
        "channel_id" : <channel_id>,
    }
}
```

* `msg_id` : identifies this message.
* `node_id` : contains the sending node id.
* `channel` : contains the channel name.
* `channel_id` : contains the channel id.

This message is sent to request a key to a closed channel.

####KEY

```json
{
    "msg_type" : "KEY",
    "msg_data" : 
    {
        "msg_id" : <msg_id>,
        "node_id" : <node_id>,
        "recipient_id" : <recipient_id>,
        "channel" : <channel>,
        "channel_id" : <channel_id>,
        "cipher" : <cipher>,
        "key" : <key>
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

####MESSAGE

```json
{
    "msg_type" : "MESSAGE",
    "msg_data" : 
    {
        "msg_id" : <msg_id>,
        "node_id" : <msg_id>,
        "recipient" : [ { "node_id" : <node_id> } ],
        "timestamp" : <timestamp>,
        "channel" : <channel>,
        "channel_id" : <channel_id>,
        "message" : "message"
        "attachment" : [ { "attachment" : <attachment>, "attachment_type" : <attachment_type> } ]
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

####CONNECT

```json
{
    "msg_type" : "CONNECT",
    "msg_data" : 
    {
        "dst" : <dst>,
        "src" : <src>
    }
}
```

* `dst` : The target of this CONNECT message, contains a full or partial socket.
* `src` : The origin of this CONNECT message, contains a full or partial socket. 

Used to open a path between two nodes, whom have been made aware of each other through another node, but cannot communicate directly. This is used to allow nodes that are present behind NAT devices to establish a connection. A client would need to respond to this message in a very specific manner to ensure the connection is established successfully. 

As an example, two nodes, *A* and *B*, want to talk to each other and will use node *S* to achieve it. 
    
1. *A* opens an available socket and sends the following to *S*:
    `{"msg_type":"CONNECT", "msg_data":{"dst":"<B's IP>", "src":"<A's Port>"}`
2. *S* forwards the message to *B*, applying *A's* IP and swapping the addresses around:
    `{"msg_type":"CONNECT", "msg_data":{"dst":"<A's IP>:<A's Port>", "src":"<B's IP>"}`
3. *B* opens an available socket for communication, and returns the message to *S* **replacing** its IP with the newly opened port (at this time, *B* should start trying to communicate with *A*, to begin getting through the NAT):
    `{"msg_type":"CONNECT", "msg_data":{"dst":"<A's IP>:<A's Port>", "src":"<B's Port>"}`
4. *S* forwards the message, adding *B's* IP to the packet and swapping them around:
    `{"msg_type":"CONNECT", "msg_data":{"dst":"<B's IP>:<B's Port>", "src":"<A's IP>:<A's Port>"}`
5. *A* now has a full CONNECT message and can now start communicating with B. 
    
####ROUTING

```json
{
    "msg_type" : "ROUTING",
    "msg_data" : 
    {
        "nodes" : [ { "node_id" : <node_id>, "address" : <address> } ]
    }
}
```

* `nodes` : A list of available nodes that this node is aware of.

A periodic message sent by a node to neighbouring nodes which lists nodes it can communicate to. ROUTING Messages should be sent to all nodes that this node can communicate with directly, but a node may chose to omit results or to not send this message should it chose to. 

####RELAY

*to be, well, figured out*


[1]: https://tools.ietf.org/html/draft-strauss-p2p-chat-08
