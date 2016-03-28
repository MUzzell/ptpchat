namespace PtpChat.Main.Subforms
{
    using System.Windows.Forms;

    public partial class ServersForm : Form
    {
        public ServersForm(PTPClient ptpclient)
        {
            this.ptpClient = ptpclient;

            this.InitializeComponent();
        }

        private PTPClient ptpClient;
    }
}