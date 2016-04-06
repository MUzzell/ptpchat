namespace PtpChat.UI
{
	partial class MainForm
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
			this.lbl_NodeId = new System.Windows.Forms.Label();
			this.pnl_SubForm = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.btn_Channels = new System.Windows.Forms.Button();
			this.btn_Nodes = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbl_NodeId
			// 
			this.lbl_NodeId.AutoSize = true;
			this.lbl_NodeId.Location = new System.Drawing.Point(37, 25);
			this.lbl_NodeId.Name = "lbl_NodeId";
			this.lbl_NodeId.Size = new System.Drawing.Size(0, 13);
			this.lbl_NodeId.TabIndex = 13;
			// 
			// pnl_SubForm
			// 
			this.pnl_SubForm.Location = new System.Drawing.Point(205, 5);
			this.pnl_SubForm.Name = "pnl_SubForm";
			this.pnl_SubForm.Size = new System.Drawing.Size(1030, 672);
			this.pnl_SubForm.TabIndex = 16;
			this.pnl_SubForm.Visible = false;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(0, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(200, 683);
			this.splitter1.TabIndex = 17;
			this.splitter1.TabStop = false;
			// 
			// btn_Channels
			// 
			this.btn_Channels.Location = new System.Drawing.Point(12, 116);
			this.btn_Channels.Name = "btn_Channels";
			this.btn_Channels.Size = new System.Drawing.Size(178, 56);
			this.btn_Channels.TabIndex = 18;
			this.btn_Channels.Text = "Channels";
			this.btn_Channels.UseVisualStyleBackColor = true;
			this.btn_Channels.Click += new System.EventHandler(this.btn_Channels_Click);
			// 
			// btn_Nodes
			// 
			this.btn_Nodes.Location = new System.Drawing.Point(12, 41);
			this.btn_Nodes.Name = "btn_Nodes";
			this.btn_Nodes.Size = new System.Drawing.Size(178, 56);
			this.btn_Nodes.TabIndex = 15;
			this.btn_Nodes.Text = "Nodes";
			this.btn_Nodes.UseVisualStyleBackColor = true;
			this.btn_Nodes.Click += new System.EventHandler(this.btn_Nodes_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1242, 683);
			this.Controls.Add(this.btn_Channels);
			this.Controls.Add(this.btn_Nodes);
			this.Controls.Add(this.pnl_SubForm);
			this.Controls.Add(this.lbl_NodeId);
			this.Controls.Add(this.splitter1);
			this.Name = "MainForm";
			this.Text = "UDPChatForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbl_NodeId;
		private System.Windows.Forms.Panel pnl_SubForm;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button btn_Channels;
		private System.Windows.Forms.Button btn_Nodes;
	}
}