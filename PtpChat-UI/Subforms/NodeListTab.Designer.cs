namespace PtpChat.UI.Subforms
{
    using BrightIdeasSoftware;

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
			this.components = new System.ComponentModel.Container();
			this.RightClickContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ughToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.memesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NodeListTab_Nodes = new BrightIdeasSoftware.ObjectListView();
			this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.RightClickContextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NodeListTab_Nodes)).BeginInit();
			this.SuspendLayout();
			// 
			// RightClickContextMenu
			// 
			this.RightClickContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailsToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.ughToolStripMenuItem});
			this.RightClickContextMenu.Name = "RightClickContextMenu";
			this.RightClickContextMenu.Size = new System.Drawing.Size(110, 70);
			// 
			// detailsToolStripMenuItem
			// 
			this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
			this.detailsToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
			this.detailsToolStripMenuItem.Text = "Details";
			this.detailsToolStripMenuItem.Click += new System.EventHandler(this.RightClickMenuClick_DetailsClick);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.RightClickMenuClick_DeleteClick);
			// 
			// ughToolStripMenuItem
			// 
			this.ughToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.memesToolStripMenuItem});
			this.ughToolStripMenuItem.Name = "ughToolStripMenuItem";
			this.ughToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
			this.ughToolStripMenuItem.Text = "Ugh";
			// 
			// memesToolStripMenuItem
			// 
			this.memesToolStripMenuItem.Name = "memesToolStripMenuItem";
			this.memesToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
			this.memesToolStripMenuItem.Text = "Memes";
			// 
			// NodeListTab_Nodes
			// 
			this.NodeListTab_Nodes.AllColumns.Add(this.olvColumn1);
			this.NodeListTab_Nodes.AllColumns.Add(this.olvColumn2);
			this.NodeListTab_Nodes.CellEditUseWholeCell = false;
			this.NodeListTab_Nodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2});
			this.NodeListTab_Nodes.Cursor = System.Windows.Forms.Cursors.Default;
			this.NodeListTab_Nodes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NodeListTab_Nodes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.NodeListTab_Nodes.HighlightBackgroundColor = System.Drawing.Color.Empty;
			this.NodeListTab_Nodes.HighlightForegroundColor = System.Drawing.Color.Empty;
			this.NodeListTab_Nodes.Location = new System.Drawing.Point(0, 0);
			this.NodeListTab_Nodes.Name = "NodeListTab_Nodes";
			this.NodeListTab_Nodes.Size = new System.Drawing.Size(415, 362);
			this.NodeListTab_Nodes.TabIndex = 1;
			this.NodeListTab_Nodes.UseCompatibleStateImageBehavior = false;
			this.NodeListTab_Nodes.View = System.Windows.Forms.View.Details;
			// 
			// olvColumn1
			// 
			this.olvColumn1.AspectName = "Status";
			this.olvColumn1.Text = "Status";
			// 
			// olvColumn2
			// 
			this.olvColumn2.AspectName = "NodeId";
			this.olvColumn2.FillsFreeSpace = true;
			this.olvColumn2.Groupable = false;
			this.olvColumn2.Text = "Name";
			// 
			// NodeListTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.NodeListTab_Nodes);
			this.Name = "NodeListTab";
			this.Size = new System.Drawing.Size(415, 362);
			this.RightClickContextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.NodeListTab_Nodes)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
        private System.Windows.Forms.ContextMenuStrip RightClickContextMenu;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ughToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memesToolStripMenuItem;
		private ObjectListView NodeListTab_Nodes;
		private OLVColumn olvColumn1;
		private OLVColumn olvColumn2;
	}
}
