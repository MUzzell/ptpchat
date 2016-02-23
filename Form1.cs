using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ptpchat
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

        private void btn_Register_Click(object sender, EventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://1.2.3.4:9001/msg");

            var postData = "msg_type=hello";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentLength = data.Length;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch
            {
                //couldn't connect to the server, bail out and drop error messages
                return;
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //read response and do stuff with it
        }
    }
}
