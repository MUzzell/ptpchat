namespace PtpChat.Main.Subforms
{
    using System.Windows.Forms;

    public partial class ClientsForm : Form
    {
        public ClientsForm(PTPClient ptpclient)
        {
            this.ptpClient = ptpclient;

            this.InitializeComponent();
        }

        private PTPClient ptpClient;
    }
}