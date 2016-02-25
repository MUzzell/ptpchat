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

        public UDPChatForm()
        {
            InitializeComponent();

            this.endPoint = new IPEndPoint(IPAddress.Any, new Random().Next(10000, 65535));
            this.udpClient = new UdpClient(this.endPoint);
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

            var responseStrings = new List<string>();

            try
            {
                byte[] bytes = this.udpClient.Receive(ref this.endPoint);

                responseStrings.Add($"Received message from {this.endPoint.ToString()} :\n {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}\n");
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
            }
            finally
            {
                this.udpClient.Close();
            }

            int a = 1;
            // CommunicationMessage message = JsonConvert.DeserializeObject<CommunicationMessage>(responseString);
        }
    }
}
