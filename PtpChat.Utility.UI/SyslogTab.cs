namespace PtpChat.Utility.UI
{
	using System.Windows.Forms;

	using NLog.Windows.Forms;
	using NLog;

	using Utility;

	public partial class SyslogTab : UserControl
	{
		public SyslogTab()
		{
			InitializeComponent();
		}

		public void SetupLogging(PtpChat.Utility.Logger logger)
		{
			logger.FormTarget.TargetRichTextBox = this.SyslogTab_RTBLog;
			logger.FormTarget.LinkClicked += SysLogTab_RTBLog_LinkClicked;
		}

		private void SysLogTab_RTBLog_LinkClicked(RichTextBoxTarget sender, string linkText, LogEventInfo logEvent)
		{
			MessageBox.Show(logEvent.Exception.ToString(), "Exception details", MessageBoxButtons.OK);
		}

	}
}
