namespace PtpChat.UI.Subforms
{
    partial class NodesForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objList_Nodes = new BrightIdeasSoftware.ObjectListView();
            this.olvCol_NodeId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCol_IpAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCol_Port = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCol_Added = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCol_LastRecieve = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCol_IsConnected = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCol_IsStartUpNode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.objList_Nodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objList_Nodes
            // 
            this.objList_Nodes.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.objList_Nodes.AllColumns.Add(this.olvCol_NodeId);
            this.objList_Nodes.AllColumns.Add(this.olvCol_IpAddress);
            this.objList_Nodes.AllColumns.Add(this.olvCol_Port);
            this.objList_Nodes.AllColumns.Add(this.olvCol_Added);
            this.objList_Nodes.AllColumns.Add(this.olvCol_LastRecieve);
            this.objList_Nodes.AllColumns.Add(this.olvCol_IsConnected);
            this.objList_Nodes.AllColumns.Add(this.olvCol_IsStartUpNode);
            this.objList_Nodes.AllowColumnReorder = true;
            this.objList_Nodes.CellEditUseWholeCell = false;
            this.objList_Nodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvCol_NodeId,
            this.olvCol_IpAddress,
            this.olvCol_Port,
            this.olvCol_Added,
            this.olvCol_LastRecieve,
            this.olvCol_IsConnected,
            this.olvCol_IsStartUpNode});
            this.objList_Nodes.Cursor = System.Windows.Forms.Cursors.Default;
            this.objList_Nodes.FullRowSelect = true;
            this.objList_Nodes.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.objList_Nodes.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.objList_Nodes.Location = new System.Drawing.Point(12, 12);
            this.objList_Nodes.Name = "objList_Nodes";
            this.objList_Nodes.Size = new System.Drawing.Size(878, 165);
            this.objList_Nodes.SortGroupItemsByPrimaryColumn = false;
            this.objList_Nodes.TabIndex = 2;
            this.objList_Nodes.UseCompatibleStateImageBehavior = false;
            this.objList_Nodes.UseNotifyPropertyChanged = true;
            this.objList_Nodes.View = System.Windows.Forms.View.Details;
            // 
            // olvCol_NodeId
            // 
            this.olvCol_NodeId.AspectName = "NodeId";
            this.olvCol_NodeId.MaximumWidth = 200;
            this.olvCol_NodeId.MinimumWidth = 1;
            this.olvCol_NodeId.Text = "NodeId";
            this.olvCol_NodeId.Width = 200;
            // 
            // olvCol_IpAddress
            // 
            this.olvCol_IpAddress.AspectName = "IpAddress";
            this.olvCol_IpAddress.Text = "IpAddress";
            this.olvCol_IpAddress.Width = 169;
            // 
            // olvCol_Port
            // 
            this.olvCol_Port.AspectName = "Port";
            this.olvCol_Port.Text = "Port";
            this.olvCol_Port.Width = 70;
            // 
            // olvCol_Added
            // 
            this.olvCol_Added.AspectName = "Added";
            this.olvCol_Added.AspectToStringFormat = "{0:d}";
            this.olvCol_Added.Text = "Added On";
            this.olvCol_Added.Width = 114;
            // 
            // olvCol_LastSeen
            // 
            this.olvCol_LastRecieve.AspectName = "LastRecieve";
            this.olvCol_LastRecieve.Text = "LastRecieve";
            this.olvCol_LastRecieve.Width = 129;
            // 
            // olvCol_IsConnected
            // 
            this.olvCol_IsConnected.AspectName = "IsConnected";
            this.olvCol_IsConnected.Text = "IsConnected";
            this.olvCol_IsConnected.Width = 107;
            // 
            // olvCol_IsStartUpNode
            // 
            this.olvCol_IsStartUpNode.AspectName = "IsStartUpNode";
            this.olvCol_IsStartUpNode.Text = "StartUp Node";
            this.olvCol_IsStartUpNode.Width = 80;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 210);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 3;
            // 
            // NodesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 656);
            this.ControlBox = false;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.objList_Nodes);
            this.Name = "NodesForm";
            ((System.ComponentModel.ISupportInitialize)(this.objList_Nodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objList_Nodes;
        private BrightIdeasSoftware.OLVColumn olvCol_NodeId;
        private BrightIdeasSoftware.OLVColumn olvCol_IpAddress;
        private BrightIdeasSoftware.OLVColumn olvCol_Port;
        private BrightIdeasSoftware.OLVColumn olvCol_Added;
        private BrightIdeasSoftware.OLVColumn olvCol_LastRecieve;
        private BrightIdeasSoftware.OLVColumn olvCol_IsConnected;
        private BrightIdeasSoftware.OLVColumn olvCol_IsStartUpNode;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}