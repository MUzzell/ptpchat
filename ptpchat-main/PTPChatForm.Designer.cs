﻿namespace PtpChat.Main
{
	partial class PTPChatForm
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
            this.btn_Register = new System.Windows.Forms.Button();
            this.txt_Username = new System.Windows.Forms.TextBox();
            this.lbl_Username = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Register
            // 
            this.btn_Register.Location = new System.Drawing.Point(33, 65);
            this.btn_Register.Name = "btn_Register";
            this.btn_Register.Size = new System.Drawing.Size(242, 93);
            this.btn_Register.TabIndex = 0;
            this.btn_Register.Text = "Register me with the server!";
            this.btn_Register.UseVisualStyleBackColor = true;
            this.btn_Register.Click += new System.EventHandler(this.btn_Register_Click);
            // 
            // txt_Username
            // 
            this.txt_Username.Location = new System.Drawing.Point(12, 28);
            this.txt_Username.Name = "txt_Username";
            this.txt_Username.Size = new System.Drawing.Size(302, 20);
            this.txt_Username.TabIndex = 1;
            // 
            // lbl_Username
            // 
            this.lbl_Username.AutoSize = true;
            this.lbl_Username.Location = new System.Drawing.Point(30, 9);
            this.lbl_Username.Name = "lbl_Username";
            this.lbl_Username.Size = new System.Drawing.Size(95, 13);
            this.lbl_Username.TabIndex = 2;
            this.lbl_Username.Text = "Enter a Username:";
            // 
            // PTPChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 444);
            this.Controls.Add(this.lbl_Username);
            this.Controls.Add(this.txt_Username);
            this.Controls.Add(this.btn_Register);
            this.Name = "PTPChatForm";
            this.Text = "PTP Chat";
            this.Load += new System.EventHandler(this.PTPChatForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Button btn_Register;
        private System.Windows.Forms.TextBox txt_Username;
        private System.Windows.Forms.Label lbl_Username;
    }
}

