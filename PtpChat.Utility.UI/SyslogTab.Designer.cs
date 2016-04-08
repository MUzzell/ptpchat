namespace PtpChat.Utility.UI
{
	partial class SyslogTab
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SyslogTab_RTBLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // SyslogTab_RTBLog
            // 
            this.SyslogTab_RTBLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SyslogTab_RTBLog.Location = new System.Drawing.Point(0, 0);
            this.SyslogTab_RTBLog.Name = "SyslogTab_RTBLog";
            this.SyslogTab_RTBLog.ReadOnly = true;
            this.SyslogTab_RTBLog.Size = new System.Drawing.Size(582, 584);
            this.SyslogTab_RTBLog.TabIndex = 0;
            this.SyslogTab_RTBLog.Text = "";
            // 
            // SyslogTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SyslogTab_RTBLog);
            this.Name = "SyslogTab";
            this.Size = new System.Drawing.Size(582, 584);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox SyslogTab_RTBLog;
	}
}
