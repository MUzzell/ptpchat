using System;
using System.Linq;

namespace PtpChat.Utility
{
    using System.Net;

    public class ExtensionMethods
    {
        public static IPEndPoint ParseEndpoint(string endpointstring)
        {
            if (!endpointstring.Contains(':'))
            {
                throw new FormatException("Invalid format: no port");
            }

            var values = endpointstring.Split(':');

            if (values.Length != 2)
            {
                throw new FormatException("Invalid format: Incorrect number of values");
            }

            var ipaddressStr = values[0];

            IPAddress ipaddress;
            if (!IPAddress.TryParse(ipaddressStr, out ipaddress))
            {
                throw new FormatException($"Invalid endpoint ip '{ipaddressStr}'");
            }

            int port;
            if (!int.TryParse(values[1], out port) || port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new FormatException($"Invalid endpoint port '{values[1]}'");
            }

            return new IPEndPoint(ipaddress, port);
        }
    }
}
