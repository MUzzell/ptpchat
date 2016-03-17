using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ptpchat.util
{
	class ConfigManager
	{

		IPAddress InitialServerAddress
		{
			get
			{
				return IPAddress.Parse(Properties.Settings.Default.InitialServerAddress_Host);
			}
			

		}

	}
}
