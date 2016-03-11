namespace ptpchat
{
	partial class UDPChatForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.lbl_Servers = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.listBox_ErrorLog = new System.Windows.Forms.ListBox();
            this.lbl_log = new System.Windows.Forms.Label();
            this.grid_Servers = new System.Windows.Forms.DataGridView();
            this.socketManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lbl_Clients = new System.Windows.Forms.Label();
            this.servers_NodeIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.servers_IpAddressCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.server_PortCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.servers_LastHelloCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.servers_IsListeningCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grid_Clients = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Servers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.socketManagerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Clients)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Servers
            // 
            this.lbl_Servers.AutoSize = true;
            this.lbl_Servers.Location = new System.Drawing.Point(37, 135);
            this.lbl_Servers.Name = "lbl_Servers";
            this.lbl_Servers.Size = new System.Drawing.Size(46, 13);
            this.lbl_Servers.TabIndex = 2;
            this.lbl_Servers.Text = "Servers:";
            // 
            // listBox_ErrorLog
            // 
            this.listBox_ErrorLog.FormattingEnabled = true;
            this.listBox_ErrorLog.Location = new System.Drawing.Point(757, 29);
            this.listBox_ErrorLog.Name = "listBox_ErrorLog";
            this.listBox_ErrorLog.Size = new System.Drawing.Size(282, 472);
            this.listBox_ErrorLog.TabIndex = 4;
            // 
            // lbl_log
            // 
            this.lbl_log.AutoSize = true;
            this.lbl_log.Location = new System.Drawing.Point(754, 13);
            this.lbl_log.Name = "lbl_log";
            this.lbl_log.Size = new System.Drawing.Size(28, 13);
            this.lbl_log.TabIndex = 5;
            this.lbl_log.Text = "Log:";
            // 
            // grid_Servers
            // 
            this.grid_Servers.AllowUserToAddRows = false;
            this.grid_Servers.AllowUserToDeleteRows = false;
            this.grid_Servers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Servers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.servers_NodeIdCol,
            this.servers_IpAddressCol,
            this.server_PortCol,
            this.servers_LastHelloCol,
            this.servers_IsListeningCol});
            this.grid_Servers.Location = new System.Drawing.Point(40, 151);
            this.grid_Servers.Name = "grid_Servers";
            this.grid_Servers.ReadOnly = true;
            this.grid_Servers.Size = new System.Drawing.Size(650, 150);
            this.grid_Servers.TabIndex = 6;
            // 
            // lbl_Clients
            // 
            this.lbl_Clients.AutoSize = true;
            this.lbl_Clients.Location = new System.Drawing.Point(37, 335);
            this.lbl_Clients.Name = "lbl_Clients";
            this.lbl_Clients.Size = new System.Drawing.Size(41, 13);
            this.lbl_Clients.TabIndex = 7;
            this.lbl_Clients.Text = "Clients:";
            // 
            // servers_NodeIdCol
            // 
            this.servers_NodeIdCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.servers_NodeIdCol.HeaderText = "NodeID";
            this.servers_NodeIdCol.Name = "servers_NodeIdCol";
            this.servers_NodeIdCol.ReadOnly = true;
            // 
            // servers_IpAddressCol
            // 
            this.servers_IpAddressCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.servers_IpAddressCol.HeaderText = "IP Address";
            this.servers_IpAddressCol.Name = "servers_IpAddressCol";
            this.servers_IpAddressCol.ReadOnly = true;
            // 
            // server_PortCol
            // 
            this.server_PortCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.server_PortCol.HeaderText = "Local Port";
            this.server_PortCol.Name = "server_PortCol";
            this.server_PortCol.ReadOnly = true;
            this.server_PortCol.Width = 50;
            // 
            // servers_LastHelloCol
            // 
            this.servers_LastHelloCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.servers_LastHelloCol.HeaderText = "Last Recieve time";
            this.servers_LastHelloCol.Name = "servers_LastHelloCol";
            this.servers_LastHelloCol.ReadOnly = true;
            this.servers_LastHelloCol.Width = 150;
            // 
            // servers_IsListeningCol
            // 
            this.servers_IsListeningCol.HeaderText = "Is Listening?";
            this.servers_IsListeningCol.Name = "servers_IsListeningCol";
            this.servers_IsListeningCol.ReadOnly = true;
            // 
            // grid_Clients
            // 
            this.grid_Clients.AllowUserToAddRows = false;
            this.grid_Clients.AllowUserToDeleteRows = false;
            this.grid_Clients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Clients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.grid_Clients.Location = new System.Drawing.Point(40, 351);
            this.grid_Clients.Name = "grid_Clients";
            this.grid_Clients.ReadOnly = true;
            this.grid_Clients.Size = new System.Drawing.Size(650, 150);
            this.grid_Clients.TabIndex = 8;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "NodeID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "IP Address";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "Local Port";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.HeaderText = "Last Recieve time";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Is Listening?";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // UDPChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 550);
            this.Controls.Add(this.grid_Clients);
            this.Controls.Add(this.lbl_Clients);
            this.Controls.Add(this.grid_Servers);
            this.Controls.Add(this.lbl_log);
            this.Controls.Add(this.listBox_ErrorLog);
            this.Controls.Add(this.lbl_Servers);
            this.Name = "UDPChatForm";
            this.Text = "UDPChatForm";
            ((System.ComponentModel.ISupportInitialize)(this.grid_Servers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.socketManagerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Clients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion
        private System.Windows.Forms.Label lbl_Servers;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox listBox_ErrorLog;
        private System.Windows.Forms.Label lbl_log;
        private System.Windows.Forms.DataGridView grid_Servers;
        private System.Windows.Forms.DataGridViewTextBoxColumn nodeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn destinationEndpointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn localEndpointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastHelloRecievedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isSocketListeningDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource socketManagerBindingSource;
        private System.Windows.Forms.Label lbl_Clients;
        private System.Windows.Forms.DataGridViewTextBoxColumn servers_NodeIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn servers_IpAddressCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn server_PortCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn servers_LastHelloCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn servers_IsListeningCol;
        private System.Windows.Forms.DataGridView grid_Clients;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}