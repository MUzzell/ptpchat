namespace ptpchat
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Windows.Forms;

    using Newtonsoft.Json;

    using ptpchat.Class_Definitions;

    public partial class PTPChatForm : Form
    {
        public PTPChatForm()
        {
            this.InitializeComponent();
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            var enteredUsername = this.txt_Username.Text;

            if (string.IsNullOrWhiteSpace(enteredUsername)) //will need a register validator class
            {
                //invalid => error mesages
                return;
            }

            //var registerJson = "{\"msg_type\":\"hello\"}";
            var registerJson = "{ " + @"""msg_type"":""register"",""username"":""{enteredUsername}""" + " }";
            var data = Encoding.ASCII.GetBytes(registerJson);

            var request = (HttpWebRequest)WebRequest.Create("http://37.139.19.21:9001/msg");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
            catch
            {
                //couldn't connect to the server, bail out and drop error messages
                return;
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //read response and do stuff with it
        }

        private void PTPChatForm_Load(object sender, EventArgs e)
        {
            //create the hello json string
            var helloJson = "{\"msg_type\":\"HELLO\"}";
            var data = Encoding.ASCII.GetBytes(helloJson);

            //create the hello request to the server
            var request = (HttpWebRequest)WebRequest.Create("http://37.139.19.21:9001/msg");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    //write the data to the server & close the stream once done
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
            catch
            {
                //couldn't connect to the server, bail out and drop error messages
                return;
            }

            //get the server response
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //read response and hopefully, mnessage now has the json values
            var message = JsonConvert.DeserializeObject<RoutingMessage>(responseString);
        }
    }
}