namespace ptpchat.Communication_Messages
{
    using System;

    using ptpchat.Class_Definitions;

    public class HelloMessage : BaseMessage
    {
        public new HelloData msg_data { get; set; }
    }

    public class HelloData
    {
        public Guid node_id { get; set; }

        public string version { get; set; }
    }
}