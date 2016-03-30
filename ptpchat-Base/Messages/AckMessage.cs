using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtpChat.Base.Messages
{
	public class AckMessage : BaseMessage
	{
		public new MessageType msg_type => MessageType.ACK;
		public AckData msg_data;
	}

	public class AckData
	{
		public Guid msg_id;
	}
}
