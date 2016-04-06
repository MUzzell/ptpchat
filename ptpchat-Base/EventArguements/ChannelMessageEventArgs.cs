using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtpChat.Base.EventArguements
{
    using PtpChat.Base.Classes;

    public class ChannelMessageEventArgs : EventArgs
    {
        public ChatMessage ChatMessage { get; set; }
        public Channel Channel { get; set; }
    }
}
