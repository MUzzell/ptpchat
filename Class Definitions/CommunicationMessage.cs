using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptpchat.Class_Definitions
{
    class CommunicationMessage : ICommunicationMessage
    {
        public CommunicationMessage(){ }

        public string msg_type { get; set; }
        public Dictionary<string, List<User>> msg_data { get; set; }
    }
}
