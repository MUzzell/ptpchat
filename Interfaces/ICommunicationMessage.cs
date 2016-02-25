using System.Collections.Generic;

namespace ptpchat.Class_Definitions
{
    interface ICommunicationMessage
    {
        Dictionary<string, object> msg_data { get; set; }

        string msg_type { get; set; }
    }
}