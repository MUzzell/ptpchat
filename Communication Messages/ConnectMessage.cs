namespace ptpchat.Communication_Messages
{
    using System;

    using ptpchat.Class_Definitions;

    internal class ConnectMessage : BaseMessage
    {
        public new ConnectData msg_data { get; set; }
    }

    internal class ConnectData
    {
        public Guid src_node_id { get; set; }

        public Guid dst_node_id { get; set; }

        string dst { get; set; }

        string src { get; set; }
    }
}