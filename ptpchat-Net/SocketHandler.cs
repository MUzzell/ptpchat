using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtpChat.Net
{
	using System.Net;
	using PtpChat.Base.Interfaces;

	public class SocketHandler : ISocketHandler
	{
		public bool SendMessage(Guid dstNodeId, byte[] messsage)
		{
			throw new NotImplementedException();
		}

		public bool SendMessage(IPEndPoint dst, IPEndPoint src, byte[] message)
		{
			throw new NotImplementedException();
		}

		public void Start()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
	}
}
