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
            this.btn_Register = new System.Windows.Forms.Button();
            this.txt_MessageBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_Register
            // 
            this.btn_Register.Location = new System.Drawing.Point(182, 12);
            this.btn_Register.Name = "btn_Register";
            this.btn_Register.Size = new System.Drawing.Size(75, 23);
            this.btn_Register.TabIndex = 0;
            this.btn_Register.Text = "Register User";
            this.btn_Register.UseVisualStyleBackColor = true;
            this.btn_Register.Click += new System.EventHandler(this.btn_Register_Click);
            // 
            // txt_MessageBox
            // 
            this.txt_MessageBox.Location = new System.Drawing.Point(12, 12);
            this.txt_MessageBox.Name = "txt_MessageBox";
            this.txt_MessageBox.Size = new System.Drawing.Size(164, 20);
            this.txt_MessageBox.TabIndex = 1;
            // 
            // UDPChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 363);
            this.Controls.Add(this.txt_MessageBox);
            this.Controls.Add(this.btn_Register);
            this.Name = "UDPChatForm";
            this.Text = "UDPChatForm";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Button btn_Register;
        private System.Windows.Forms.TextBox txt_MessageBox;
    }
}