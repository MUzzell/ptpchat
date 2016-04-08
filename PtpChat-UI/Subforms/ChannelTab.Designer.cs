namespace PtpChat.UI.Subforms
{
	partial class ChannelTab
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
			this.ChannelTab_TextPanel = new System.Windows.Forms.Panel();
			this.ChannelTab_TextEntry = new System.Windows.Forms.TextBox();
			this.ChannelTab_BtnSubmit = new System.Windows.Forms.Button();
			this.ChannelTab_Messages = new BrightIdeasSoftware.ObjectListView();
			this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.ChannelTab_TextPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ChannelTab_Messages)).BeginInit();
			this.SuspendLayout();
			// 
			// ChannelTab_TextPanel
			// 
			this.ChannelTab_TextPanel.Controls.Add(this.ChannelTab_TextEntry);
			this.ChannelTab_TextPanel.Controls.Add(this.ChannelTab_BtnSubmit);
			this.ChannelTab_TextPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ChannelTab_TextPanel.Location = new System.Drawing.Point(0, 392);
			this.ChannelTab_TextPanel.Name = "ChannelTab_TextPanel";
			this.ChannelTab_TextPanel.Size = new System.Drawing.Size(591, 48);
			this.ChannelTab_TextPanel.TabIndex = 1;
			// 
			// ChannelTab_TextEntry
			// 
			this.ChannelTab_TextEntry.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChannelTab_TextEntry.Location = new System.Drawing.Point(0, 0);
			this.ChannelTab_TextEntry.Multiline = true;
			this.ChannelTab_TextEntry.Name = "ChannelTab_TextEntry";
			this.ChannelTab_TextEntry.Size = new System.Drawing.Size(525, 48);
			this.ChannelTab_TextEntry.TabIndex = 2;
			// 
			// ChannelTab_BtnSubmit
			// 
			this.ChannelTab_BtnSubmit.Dock = System.Windows.Forms.DockStyle.Right;
			this.ChannelTab_BtnSubmit.Location = new System.Drawing.Point(525, 0);
			this.ChannelTab_BtnSubmit.Name = "ChannelTab_BtnSubmit";
			this.ChannelTab_BtnSubmit.Size = new System.Drawing.Size(66, 48);
			this.ChannelTab_BtnSubmit.TabIndex = 1;
			this.ChannelTab_BtnSubmit.Text = "Send";
			this.ChannelTab_BtnSubmit.UseVisualStyleBackColor = true;
			this.ChannelTab_BtnSubmit.Click += new System.EventHandler(this.ChannelTab_BtnSubmit_Click);
			// 
			// ChannelTab_Messages
			// 
			this.ChannelTab_Messages.AllColumns.Add(this.olvColumn1);
			this.ChannelTab_Messages.AllColumns.Add(this.olvColumn2);
			this.ChannelTab_Messages.AllColumns.Add(this.olvColumn3);
			this.ChannelTab_Messages.CellEditUseWholeCell = false;
			this.ChannelTab_Messages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3});
			this.ChannelTab_Messages.Cursor = System.Windows.Forms.Cursors.Default;
			this.ChannelTab_Messages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChannelTab_Messages.EmptyListMsg = "No Messages";
			this.ChannelTab_Messages.HighlightBackgroundColor = System.Drawing.Color.Empty;
			this.ChannelTab_Messages.HighlightForegroundColor = System.Drawing.Color.Empty;
			this.ChannelTab_Messages.Location = new System.Drawing.Point(0, 0);
			this.ChannelTab_Messages.Name = "ChannelTab_Messages";
			this.ChannelTab_Messages.Size = new System.Drawing.Size(591, 392);
			this.ChannelTab_Messages.TabIndex = 2;
			this.ChannelTab_Messages.UseCompatibleStateImageBehavior = false;
			this.ChannelTab_Messages.View = System.Windows.Forms.View.Details;
			// 
			// olvColumn1
			// 
			this.olvColumn1.AspectName = "Member";
			this.olvColumn1.Groupable = false;
			this.olvColumn1.Text = "Name";
			// 
			// olvColumn2
			// 
			this.olvColumn2.AspectName = "Time";
			this.olvColumn2.Groupable = false;
			this.olvColumn2.Text = "Time";
			// 
			// olvColumn3
			// 
			this.olvColumn3.AspectName = "Message";
			this.olvColumn3.FillsFreeSpace = true;
			this.olvColumn3.Groupable = false;
			this.olvColumn3.Text = "Message";
			// 
			// ChannelTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.Controls.Add(this.ChannelTab_Messages);
			this.Controls.Add(this.ChannelTab_TextPanel);
			this.Name = "ChannelTab";
			this.Size = new System.Drawing.Size(591, 440);
			this.ChannelTab_TextPanel.ResumeLayout(false);
			this.ChannelTab_TextPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ChannelTab_Messages)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel ChannelTab_TextPanel;
		private System.Windows.Forms.Button ChannelTab_BtnSubmit;
		private System.Windows.Forms.TextBox ChannelTab_TextEntry;
		private BrightIdeasSoftware.ObjectListView ChannelTab_Messages;
		private BrightIdeasSoftware.OLVColumn olvColumn1;
		private BrightIdeasSoftware.OLVColumn olvColumn2;
		private BrightIdeasSoftware.OLVColumn olvColumn3;
	}
}
