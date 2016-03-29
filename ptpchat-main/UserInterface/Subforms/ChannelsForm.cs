namespace PtpChat.Main.UserInterface.Subforms
{
    using System.Windows.Forms;

    public partial class ChannelsForm : Form
    {
        public ChannelsForm(PTPClient ptpclient)
        {
            this.ptpClient = ptpclient;

            this.InitializeComponent();
        }

        public PTPClient ptpClient;
    }
}