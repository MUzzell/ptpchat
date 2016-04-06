namespace PtpChat.UI.Subforms
{
	partial class ChannelListTab
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
			this.components = new System.ComponentModel.Container();
			this.dataTreeListView1 = new BrightIdeasSoftware.DataTreeListView();
			((System.ComponentModel.ISupportInitialize)(this.dataTreeListView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataTreeListView1
			// 
			this.dataTreeListView1.CellEditUseWholeCell = false;
			this.dataTreeListView1.DataSource = null;
			this.dataTreeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTreeListView1.EmptyListMsg = "No Channels";
			this.dataTreeListView1.HighlightBackgroundColor = System.Drawing.Color.Empty;
			this.dataTreeListView1.HighlightForegroundColor = System.Drawing.Color.Empty;
			this.dataTreeListView1.Location = new System.Drawing.Point(0, 0);
			this.dataTreeListView1.Name = "dataTreeListView1";
			this.dataTreeListView1.RootKeyValueString = "";
			this.dataTreeListView1.ShowGroups = false;
			this.dataTreeListView1.Size = new System.Drawing.Size(285, 285);
			this.dataTreeListView1.TabIndex = 0;
			this.dataTreeListView1.UseCompatibleStateImageBehavior = false;
			this.dataTreeListView1.View = System.Windows.Forms.View.Details;
			this.dataTreeListView1.VirtualMode = true;
			// 
			// ChannelListTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataTreeListView1);
			this.Name = "ChannelListTab";
			this.Size = new System.Drawing.Size(285, 285);
			((System.ComponentModel.ISupportInitialize)(this.dataTreeListView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BrightIdeasSoftware.DataTreeListView dataTreeListView1;
	}
}
