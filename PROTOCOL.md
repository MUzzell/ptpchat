#ptpchat protocol

Most of the protocol is loosely based upon P2P CHAT, A peer-to-peer chat protocol, which can be found [here][1].

All of this is subject to change at any time, as this project is very much in the testing/prototyping phase.

###Message Format 

Messages will be transferred in JSON. At the head, a message will contain one or two keys, dependent on the verb being sent. Other keys in the head should be ignored. These two keys are:

1. `msg_type`, containing the verb.
2. `msg_data`, containing a JSON object holding the data relevant to the message being sent. 

Example:

```json
{"msg_type":"HELLO"}
{"msg_type":"MESSAGE", "msg_data":{"channel_id":"<channel_guid>", "message":"<message>"}}
```

###Verbs

*. HELLO
 * This is the initial message that is sent when the client becomes active to the target machine. Its purpose is to notify the target that it is online and available for communication. This message should be sent periodically to other known machines that it is still active. 
*. ACK
 * Sent after all messages, used to notify that a message has been received correctly.
*. JOIN
 * Sent to a target machine to join a channel 
*. CHANNEL
 * (might remove)
*. LEAVE
 * Sent to all mebers of a channel that the sending user is leaving the channel
*. GETCERTIFICATE
*. CERTIFICATE
*. GETKEY
*. KEY
*. MESSAGE
*. RELAY


[1]: https://tools.ietf.org/html/draft-strauss-p2p-chat-08