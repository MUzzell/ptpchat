#ptpchat protocol

Most of the protocol is loosely based upon P2P CHAT, A peer-to-peer chat protocol, which can be found [here][1].

All of this is subject to change at any time, as this project is very much in the testing phase.

###Verbs

1. HELLO
 * This is the initial message that is sent when the client becomes active to the target broker server. Its purpose is to notify the broker that it is online and available for communication. 
2. ACK
 * Sent after all messages, used to notify that a message has been recieved correctly.
4. JOIN
5. CHANNEL
6. LEAVE
7. GETCERTIFICATE
8. CERTIFICATE
9. GETKEY
10. KEY
11. MESSAGE

### Message Format 

Messages will be transferred in JSON. At the head, a message will contain one or two keys, dependent on the verb being sent. Other keys in the head should be ignored. These two keys are:

1. msg_type, containing the verb.
2. msg_data, containing a JSON object that contains the data relevant to the message being sent. 

{"msg_type":"HELLO"}
{"msg_type":"MESSAGE", "msg_data":{"channel_id":"<channel_guid>", "message":"<message>"}}

[1]: https://tools.ietf.org/html/draft-strauss-p2p-chat-08