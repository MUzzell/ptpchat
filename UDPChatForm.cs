using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ptpchat
{
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Threading;

    using Newtonsoft.Json;

    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;

    public partial class UDPChatForm : Form
    {
        private PTPClient PTPClient {get;set;}

        public delegate string ServerHelloAsyncCaller(List<IPAddress> IpList);

        public UDPChatForm()
        {
            InitializeComponent();
            this.PTPClient = new PTPClient();

            var loadServerInfoThread = new Thread(this.ReadServerInfoAndSendHello) { IsBackground = true };

            loadServerInfoThread.Start();
        }

        private void ReadServerInfoAndSendHello()
        {
            var ipListCsv = ConfigurationManager.AppSettings["Server_Ips"];

            if (string.IsNullOrWhiteSpace(ipListCsv))
            {
                //no server
            }

            var ipStrings = ipListCsv.Contains(';') 
                                 ? ipListCsv.Split(';').Where(ipString => !string.IsNullOrWhiteSpace(ipString)).Distinct().ToList() 
                                 : new List<string>() { ipListCsv };


            if (!ipStrings.Any())
            {
                //no server ip? dont need to do server connections
                return;
            }

            var ipAddresses = new List<IPAddress>();

            ipStrings.ForEach(
                ip =>
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(ip))
                        {
                            ipAddresses.Add(IPAddress.Parse(ip));
                        }
                    }
                    catch
                    {
                        //ip string couldn't be parsed as an Ip address
                    }
                });

            this.PTPClient.SendHelloToServer(ipAddresses);
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            //var username = this.txt_MessageBox.Text;

            //if (string.IsNullOrWhiteSpace(username))
            //{
            //    //not valid
            //    return;
            //}

            //var registerJson = "{" + "\"msg_type\":\"HELLO\", \"msg_data\":{\"username\":\"" + username + "\"" + "}}";
            //var msg = Encoding.ASCII.GetBytes(registerJson);

            //try
            //{
            //    this.udpClient.Send(msg, msg.Length, "37.139.19.21", 9001);
            //}
            //catch (Exception ex)
            //{
            //    //broke!
            //    var err = ex.ToString();
            //}

            //try
            //{
            //    this.udpClient.BeginReceive(new AsyncCallback(RecieveHelloCallback), null);
            //}
            //catch (Exception ex)
            //{
            //    var err = ex.ToString();
            //}
        }

    }
}
