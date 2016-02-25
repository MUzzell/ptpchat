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
        public UDPChatForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var msg = Encoding.ASCII.GetBytes("{\"msg_type\":\"HELLO\"}");
            UdpClient udpClient = new UdpClient();

            try
            {
                udpClient.Send(msg, msg.Length, "37.139.19.21", 9001);
            }
            catch (Exception ex)
            {
                //broke!
                var err = ex.ToString();
                return;
            }

            var endPoint = new IPEndPoint(IPAddress.Any, new Random().Next(10000, 65535));

            var responseStrings = new List<string>();

            try
            {
                byte[] bytes = udpClient.Receive(ref endPoint);

                responseStrings.Add($"Received message from {endPoint.ToString()} :\n {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}\n");
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
            }
            finally
            {
                udpClient.Close();
            }

            int a = 1;

            // CommunicationMessage message = JsonConvert.DeserializeObject<CommunicationMessage>(responseString);
        }
    }
}
