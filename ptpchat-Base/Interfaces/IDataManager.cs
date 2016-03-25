using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtpChat.Base.Interfaces
{
	public interface IDataManager
	{
		INodeManager NodeManager { get; }
		IChannelManager ChannelManager { get; }
	}
}
