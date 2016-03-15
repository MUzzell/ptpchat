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
            this.servers_NodeIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.servers_IpAddressCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.server_PortCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.servers_LastHelloCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.servers_IsListeningCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.socketManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lbl_Clients = new System.Windows.Forms.Label();
            this.grid_Clients = new System.Windows.Forms.DataGridView();
            this.clients_NodeIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clients_IPAddressCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clients_PortCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clients_LastHelloCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clients_IsListeningCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_ClientConnect = new System.Windows.Forms.Button();
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
            this.listBox_ErrorLog.Location = new System.Drawing.Point(925, 25);
            this.listBox_ErrorLog.Name = "listBox_ErrorLog";
            this.listBox_ErrorLog.Size = new System.Drawing.Size(425, 472);
            this.listBox_ErrorLog.TabIndex = 4;
            // 
            // lbl_log
            // 
            this.lbl_log.AutoSize = true;
            this.lbl_log.Location = new System.Drawing.Point(922, 9);
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
            this.grid_Servers.Size = new System.Drawing.Size(661, 150);
            this.grid_Servers.TabIndex = 6;
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
            // lbl_Clients
            // 
            this.lbl_Clients.AutoSize = true;
            this.lbl_Clients.Location = new System.Drawing.Point(37, 335);
            this.lbl_Clients.Name = "lbl_Clients";
            this.lbl_Clients.Size = new System.Drawing.Size(41, 13);
            this.lbl_Clients.TabIndex = 7;
            this.lbl_Clients.Text = "Clients:";
            // 
            // grid_Clients
            // 
            this.grid_Clients.AllowUserToAddRows = false;
            this.grid_Clients.AllowUserToDeleteRows = false;
            this.grid_Clients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Clients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clients_NodeIdCol,
            this.clients_IPAddressCol,
            this.clients_PortCol,
            this.clients_LastHelloCol,
            this.clients_IsListeningCol});
            this.grid_Clients.Location = new System.Drawing.Point(40, 351);
            this.grid_Clients.Name = "grid_Clients";
            this.grid_Clients.ReadOnly = true;
            this.grid_Clients.Size = new System.Drawing.Size(661, 150);
            this.grid_Clients.TabIndex = 8;
            this.grid_Clients.SelectionChanged += new System.EventHandler(this.ClientSocketManagers_SelectionChanged);
            // 
            // clients_NodeIdCol
            // 
            this.clients_NodeIdCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clients_NodeIdCol.HeaderText = "NodeID";
            this.clients_NodeIdCol.Name = "clients_NodeIdCol";
            this.clients_NodeIdCol.ReadOnly = true;
            // 
            // clients_IPAddressCol
            // 
            this.clients_IPAddressCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clients_IPAddressCol.HeaderText = "IP Address";
            this.clients_IPAddressCol.Name = "clients_IPAddressCol";
            this.clients_IPAddressCol.ReadOnly = true;
            // 
            // clients_PortCol
            // 
            this.clients_PortCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clients_PortCol.HeaderText = "Local Port";
            this.clients_PortCol.Name = "clients_PortCol";
            this.clients_PortCol.ReadOnly = true;
            this.clients_PortCol.Width = 50;
            // 
            // clients_LastHelloCol
            // 
            this.clients_LastHelloCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clients_LastHelloCol.HeaderText = "Last Recieve time";
            this.clients_LastHelloCol.Name = "clients_LastHelloCol";
            this.clients_LastHelloCol.ReadOnly = true;
            this.clients_LastHelloCol.Width = 150;
            // 
            // clients_IsListeningCol
            // 
            this.clients_IsListeningCol.HeaderText = "Is Listening?";
            this.clients_IsListeningCol.Name = "clients_IsListeningCol";
            this.clients_IsListeningCol.ReadOnly = true;
            // 
            // btn_ClientConnect
            // 
            this.btn_ClientConnect.Location = new System.Drawing.Point(725, 351);
            this.btn_ClientConnect.Name = "btn_ClientConnect";
            this.btn_ClientConnect.Size = new System.Drawing.Size(182, 23);
            this.btn_ClientConnect.TabIndex = 9;
            this.btn_ClientConnect.Text = "Connect to client";
            this.btn_ClientConnect.UseVisualStyleBackColor = true;
            this.btn_ClientConnect.Visible = false;
            // 
            // UDPChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1365, 597);
            this.Controls.Add(this.btn_ClientConnect);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn clients_NodeIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn clients_IPAddressCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn clients_PortCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn clients_LastHelloCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn clients_IsListeningCol;
        private System.Windows.Forms.Button btn_ClientConnect;
    }
}