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
        private readonly IPEndPoint endPoint;
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

            UdpState udpstate = new UdpState() { endpoint = this.endPoint, udpclient = this.udpClient};

            try
            {
                this.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback) , udpstate);
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
            }
        }

        private static void ReceiveCallback(IAsyncResult asyncResult)
        {
            UdpClient _udpclient = (UdpClient)((UdpState)(asyncResult.AsyncState)).udpclient;
            IPEndPoint _endpoint = (IPEndPoint)((UdpState)(asyncResult.AsyncState)).endpoint;
            CommunicationMessage message = new CommunicationMessage();

            try
            {
                Byte[] receiveBytes = _udpclient.EndReceive(asyncResult, ref _endpoint);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                message = JsonConvert.DeserializeObject<CommunicationMessage>(receiveString);
            }
            catch
            {
                //is the server alive?
            }

            //hopefully use message.stuff !
            var users = message.msg_data.Values;

        }
    }

    public class UdpState
    {
        public IPEndPoint endpoint;
        public UdpClient udpclient;
    }

}
