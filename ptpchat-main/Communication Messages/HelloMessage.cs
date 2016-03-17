namespace ptpchat.Communication_Messages
{
    using System;

    using ptpchat.Class_Definitions;

    internal class HelloMessage : BaseMessage
    {
        public new HelloData msg_data { get; set; }
    }

    internal class HelloData
    {
        public Guid node_id { get; set; }

        public string version { get; set; }
    }
}