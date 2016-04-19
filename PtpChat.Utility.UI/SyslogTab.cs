namespace PtpChat.Utility.UI
{
    using System.Windows.Forms;

    using NLog;
    using NLog.Windows.Forms;

    using Logger = PtpChat.Utility.Logger;

    public partial class SyslogTab : UserControl
    {
        public SyslogTab()
        {
            this.InitializeComponent();
        }

        public void SetupLogging(Logger logger)
        {
            this.SyslogTab_RTBLog.TextChanged += SyslogTab_RTBLog_TextChanged;

            logger.FormTarget.TargetRichTextBox = this.SyslogTab_RTBLog;

            logger.FormTarget.LinkClicked += this.SysLogTab_RTBLog_LinkClicked;
        }

        private void SyslogTab_RTBLog_TextChanged(object sender, System.EventArgs e)
        {
            this.SyslogTab_RTBLog.SelectionStart = this.SyslogTab_RTBLog.Text.Length;
            this.SyslogTab_RTBLog.ScrollToCaret();
        }

        private void SysLogTab_RTBLog_LinkClicked(RichTextBoxTarget sender, string linkText, LogEventInfo logEvent)
        {
            MessageBox.Show(logEvent.Exception.ToString(), "Exception details", MessageBoxButtons.OK);
        }
    }
}