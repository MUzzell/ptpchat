#ptpchat protocol

Most of the protocol is loosely based upon P2P CHAT, A peer-to-peer chat protocol, which can be found [here][1].

All of this is subject to change at any time, as this project is very much in the testing/prototyping phase.

###Message Format 

Messages will be transferred in JSON. At the head, a message will contain one or two keys, dependent on the verb being sent. `msg_type` is required for all messges, `msg_data` is not required for all messages. Should a recieved JSON message not contain a `msg_data` element, it should be treated as null (or None). All other elements in the head should be ignored. These two keys are:

1. `msg_type`, containing the verb.
2. `msg_data`, containing a JSON object holding the data relevant to the message being sent. 

Example:

```json
{"msg_type":"HELLO"}
{"msg_type":"MESSAGE", "msg_data":{"channel_id":"<channel_guid>", "message":"<message>"}}
```

###Verbs

* HELLO
    * This is a periodic message that is sent between nodes. Its purpose is to notify the target that it is online and available for communication. This message should be sent to known machines at an interval of between 0.5 to 2 seconds. The HELLO message does not include a msg_data attribute (or its value is set to none).
* ACK
    * Sent after GETCERTIFICATE, CERTIFICATE, GETKEY, KEY, MESSAGE messages, used to notify that a message has been received correctly by the immediate recipient.
* JOIN
    * Sent to a target machine to join a channel.
* CHANNEL
    * (might remove)
* LEAVE
    * Sent to all mebers of a channel that the sending user is leaving the channel
* GETCERTIFICATE
* CERTIFICATE
* GETKEY
* KEY
* MESSAGE
* RELAY
* CONNECT
    * Used to open a path between two nodes, whom have been made aware of each other through another node, but cannot communicate directly. This is used to allow nodes that are present behind NAT devices to directly communicate with each other. A client would need to respond to this message in a very specific order to facilitate a successful operation. As an example, two nodes, A and B, want to talk to each other and will use node S to achieve it. 
        1. *A* opens an available socket and sends the following to *S*:
            * ```
            {"msg_type":"CONNECT", "msg_data":{"dst":"<B's IP>", "src":"<A's Port>"}
            ```
        2. *S* forwards the message to *B*, applying *A's* IP and swapping the addresses arround:
            * ```
            {"msg_type":"CONNECT", "msg_data":{"dst":"<A's IP>:<A's Port>", "src":"<B's IP>"}
            ```
        3. *B* opens an available socket for communication, and returns the message to *S* **replacing** its IP with the newly opened port (at this time, *B* should start trying to communicate with *A*, to begin getting through the NAT):
            * ```json
            {"msg_type":"CONNECT", "msg_data":{"dst":"<A's IP>:<A's Port>", "src":"<B's Port>"}
            ```
        4. *S* forwards the message, adding *B's* IP to the packet and swappin them arround:
            * ```json
            {"msg_type":"CONNECT", "msg_data":{"dst":"<B's IP>:<B's Port>", "src":"<A's IP>:<A's Port>"}
            ```
        5. *A* now has a full **CONNECT** message and can now start communicating with B. 
* ROUTING
    * A periodic message sent by a node to neighbouring nodes that it can communicate to. 


[1]: https://tools.ietf.org/html/draft-strauss-p2p-chat-08
