using System.Collections.Generic;

namespace ptpchat.Class_Definitions
{
    public class BaseMessage
    {
        public string msg_type { get; set; }
        public object msg_data { get; set; }
    }
}