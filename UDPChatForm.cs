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
	public partial class UDPChatForm : Form
	{
		public UDPChatForm()
		{
			InitializeComponent();
		}

		private void UDPChatForm_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			var msg = Encoding.ASCII.GetBytes("{\"msg_type\":\"HELLO\"}");

			try
			{
				UdpClient udpClient = new UdpClient();

				udpClient.Send(msg, msg.Length, "37.139.19.21", 9001);

				udpClient.Close();
			}
			catch (Exception exception)
			{

			}
			
		}
	}
}
