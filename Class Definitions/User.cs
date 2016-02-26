using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptpchat.Class_Definitions
{
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    internal class User
    {
        public string username { get; set; }

        public string address { get; set; }

        //this should be an int, we'll need to change it once we get the json!
        private string port;
        public string Port
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.port))
                {
                    return this.port;
                }

                try
                {
                    return string.IsNullOrWhiteSpace(this.address) 
                        ? null 
                        : this.address.Split(':')[1];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                port = value;
            }
        }

    }
}
