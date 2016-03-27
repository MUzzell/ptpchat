using System;
using System.Linq;

namespace PtpChat.Utility
{
    using System.Net;
    using System.Text.RegularExpressions;

    public class ExtensionMethods
    {
        public static IPEndPoint ParseEndpoint(string endpointstring)
        {
            if (string.IsNullOrWhiteSpace(endpointstring))
            {
                throw new FormatException("Endpoint descriptor may not be empty.");
            }

            string[] values = endpointstring.Split(':');

            IPAddress ipaddress = null;
            int port = 0;

            if (values.Length < 2)
            {
                throw new FormatException($"Invalid endpoint ipaddress '{endpointstring}'");
            }

            //check if we have an IPv6 or ports
            if (values.Length == 2) // ipv4 
            {
                var ipaddressStr = values[0];

                if (!IPAddress.TryParse(ipaddressStr, out ipaddress))
                {
                    throw new FormatException($"IPv4: Invalid endpoint ip '{ipaddressStr}'");
                }

                if (!int.TryParse(values[1], out port) || port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                {
                    throw new FormatException($"IPv4: Invalid endpoint port '{values[1]}'");
                }
            }

            if (values.Length > 2) //ipv6
            {
                //[a:b:c:d:e:f:g:h]:port
                if (!values[0].StartsWith("["))
                {
                    //has no brackets, and thus no port?
                    throw new FormatException($"IPv6: Invalid endpoint ip '{endpointstring}'");
                }

                //\[             # starts with a '[' character (delimited)
                //  (            # capture 
                //   [^\]]       # Any character that is not a ']'
                //        *      # Zero or more occurrences 
                //         )     # Close the capture
                //          \]   # Ends with a ']'

                //basically, grab everything inside the square brackets
                var addressRegex = Regex.Match(endpointstring, @"\[([^\]]*)\]");

                //no matches?
                if (addressRegex.Groups.Count == 0)
                {
                    throw new FormatException($"IPv6: Invalid endpoint '{endpointstring}'");
                }

                string ipaddressStr = addressRegex.Groups[0].Value;

                if (!IPAddress.TryParse(ipaddressStr, out ipaddress))
                {
                    throw new FormatException($"IPv6: Invalid endpoint ip '{ipaddressStr}'");
                }

                if (!int.TryParse(values[values.Length - 1], out port) || port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                {
                    throw new FormatException($"IPv6: Invalid endpoint port '{values[1]}'");
                }
            }

            if (ipaddress != null && port != 0)
                return new IPEndPoint(ipaddress, port);

            return null;
        }
    }
}
