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
    using System.IO;
    using System.Net;

    using Newtonsoft.Json;

    using ptpchat.Class_Definitions;

    public partial class UDPChatForm : Form
    {
        private IPEndPoint endPoint;
        private readonly UdpClient udpClient;

        private List<User> knownUsers { get; set; }

        public UDPChatForm()
        {
            InitializeComponent();

            endPoint = new IPEndPoint(IPAddress.Any, new Random().Next(10000, 65535));
            udpClient = new UdpClient(this.endPoint);
            knownUsers = new List<User>();
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            var username = this.txt_MessageBox.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                //not valid
                return;
            }

            var registerJson = "{" + "\"msg_type\":\"HELLO\", \"msg_data\":{\"username\":\"" + username + "\"" + "}}";
            var msg = Encoding.ASCII.GetBytes(registerJson);

            try
            {
                this.udpClient.Send(msg, msg.Length, "37.139.19.21", 9001);
            }
            catch (Exception ex)
            {
                //broke!
                var err = ex.ToString();
            }

            try
            {
                this.udpClient.BeginReceive(new AsyncCallback(RecieveHelloCallback), null);
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
            }
        }


        private void RecieveHelloCallback(IAsyncResult asyncResult)
        {
            CommunicationMessage message = new CommunicationMessage();

            try
            {
                Byte[] receiveBytes = this.udpClient.EndReceive(asyncResult, ref this.endPoint);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                message = JsonConvert.DeserializeObject<CommunicationMessage>(receiveString);
            }
            catch (Exception ex)
            {
                //is the server alive?
                var err = ex.ToString();
            }

            this.knownUsers = message.msg_data.Values.First();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = this.knownUsers;
            //do known users stuff here and wherever else o
        }
    }



}
