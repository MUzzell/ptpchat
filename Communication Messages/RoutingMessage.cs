using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptpchat.Class_Definitions
{
    class RoutingMessage :BaseMessage
    {
        public RoutingMessage(){ }

        public string msg_type { get; set; }

        public List<Dictionary<string, string>> msg_data { get; set; }

    }
}
