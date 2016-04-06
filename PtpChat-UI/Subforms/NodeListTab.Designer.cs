namespace PtpChat.UI.Subforms
{
	partial class NodeListTab
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
			this.NodesTab_DataListView = new BrightIdeasSoftware.DataListView();
			this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			((System.ComponentModel.ISupportInitialize)(this.NodesTab_DataListView)).BeginInit();
			this.SuspendLayout();
			// 
			// NodesTab_DataListView
			// 
			this.NodesTab_DataListView.AllColumns.Add(this.olvColumn1);
			this.NodesTab_DataListView.AllColumns.Add(this.olvColumn2);
			this.NodesTab_DataListView.CellEditUseWholeCell = false;
			this.NodesTab_DataListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2});
			this.NodesTab_DataListView.Cursor = System.Windows.Forms.Cursors.Default;
			this.NodesTab_DataListView.DataSource = null;
			this.NodesTab_DataListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NodesTab_DataListView.EmptyListMsg = "No Nodes";
			this.NodesTab_DataListView.FullRowSelect = true;
			this.NodesTab_DataListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.NodesTab_DataListView.HighlightBackgroundColor = System.Drawing.Color.Empty;
			this.NodesTab_DataListView.HighlightForegroundColor = System.Drawing.Color.Empty;
			this.NodesTab_DataListView.Location = new System.Drawing.Point(0, 0);
			this.NodesTab_DataListView.Name = "NodesTab_DataListView";
			this.NodesTab_DataListView.Size = new System.Drawing.Size(415, 362);
			this.NodesTab_DataListView.TabIndex = 0;
			this.NodesTab_DataListView.UseCompatibleStateImageBehavior = false;
			this.NodesTab_DataListView.View = System.Windows.Forms.View.Details;
			// 
			// olvColumn1
			// 
			this.olvColumn1.AspectName = "Status";
			this.olvColumn1.Text = "Status";
			// 
			// olvColumn2
			// 
			this.olvColumn2.AspectName = "Name";
			this.olvColumn2.Text = "Name";
			// 
			// NodeListTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.NodesTab_DataListView);
			this.Name = "NodeListTab";
			this.Size = new System.Drawing.Size(415, 362);
			((System.ComponentModel.ISupportInitialize)(this.NodesTab_DataListView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BrightIdeasSoftware.DataListView NodesTab_DataListView;
		private BrightIdeasSoftware.OLVColumn olvColumn1;
		private BrightIdeasSoftware.OLVColumn olvColumn2;
	}
}
