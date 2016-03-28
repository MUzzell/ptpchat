namespace PtpChat.Main
{
    using PtpChat.Main.Ribbon.Classes.Enums;
    using PtpChat.Main.Ribbon.Component_Classes;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UDPChatForm));
            this.lbl_NodeId = new System.Windows.Forms.Label();
            this.ribbon1 = new Ribbon.Component_Classes.Ribbon();
            this.ribbonButton1 = new RibbonButton();
            this.ribbonButton4 = new RibbonButton();
            this.ribbonButton5 = new RibbonButton();
            this.ribbonButton7 = new RibbonButton();
            this.ribbonButton8 = new RibbonButton();
            this.ribbonButton9 = new RibbonButton();
            this.ribbonButton10 = new RibbonButton();
            this.rTab_ThisNode = new RibbonTab();
            this.rTab_Servers = new RibbonTab();
            this.ribbonPanel1 = new RibbonPanel();
            this.ribbonButton3 = new RibbonButton();
            this.rTab_Clients = new RibbonTab();
            this.ribbonPanel2 = new RibbonPanel();
            this.ribbonButton6 = new RibbonButton();
            this.ribbonButton2 = new RibbonButton();
            this.pnl_SubForm = new System.Windows.Forms.Panel();
            this.ribbonButton11 = new RibbonButton();
            this.SuspendLayout();
            // 
            // lbl_NodeId
            // 
            this.lbl_NodeId.AutoSize = true;
            this.lbl_NodeId.Location = new System.Drawing.Point(37, 25);
            this.lbl_NodeId.Name = "lbl_NodeId";
            this.lbl_NodeId.Size = new System.Drawing.Size(0, 13);
            this.lbl_NodeId.TabIndex = 10;
            // 
            // ribbon1
            // 
            this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonButton1);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonButton4);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonButton5);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(0, 72);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.OrbImage = null;
            this.ribbon1.OrbStyle = RibbonOrbStyle.Office_2010;
            this.ribbon1.OrbText = "Menu";
            // 
            // 
            // 
            this.ribbon1.QuickAcessToolbar.Items.Add(this.ribbonButton7);
            this.ribbon1.QuickAcessToolbar.Items.Add(this.ribbonButton9);
            this.ribbon1.QuickAcessToolbar.Items.Add(this.ribbonButton11);
            this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbon1.Size = new System.Drawing.Size(1365, 107);
            this.ribbon1.TabIndex = 0;
            this.ribbon1.Tabs.Add(this.rTab_ThisNode);
            this.ribbon1.Tabs.Add(this.rTab_Servers);
            this.ribbon1.Tabs.Add(this.rTab_Clients);
            this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(12, 26, 20, 0);
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.Image")));
            this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
            // 
            // ribbonButton4
            // 
            this.ribbonButton4.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton4.Image")));
            this.ribbonButton4.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton4.SmallImage")));
            // 
            // ribbonButton5
            // 
            this.ribbonButton5.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton5.Image")));
            this.ribbonButton5.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton5.SmallImage")));
            // 
            // ribbonButton7
            // 
            this.ribbonButton7.DropDownItems.Add(this.ribbonButton8);
            this.ribbonButton7.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton7.Image")));
            this.ribbonButton7.MaxSizeMode = RibbonElementSizeMode.Compact;
            this.ribbonButton7.MinSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton7.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton7.SmallImage")));
            this.ribbonButton7.Text = "I cant seem to get rid of these buttons annoyingly..";
            // 
            // ribbonButton8
            // 
            this.ribbonButton8.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton8.Image")));
            this.ribbonButton8.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton8.SmallImage")));
            this.ribbonButton8.Text = "ribbonButton8";
            // 
            // ribbonButton9
            // 
            this.ribbonButton9.DropDownItems.Add(this.ribbonButton10);
            this.ribbonButton9.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton9.Image")));
            this.ribbonButton9.MaxSizeMode = RibbonElementSizeMode.Compact;
            this.ribbonButton9.MinSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton9.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton9.SmallImage")));
            this.ribbonButton9.Text = "so we might as well come up with a use for them..";
            this.ribbonButton9.Value = "";
            // 
            // ribbonButton10
            // 
            this.ribbonButton10.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton10.Image")));
            this.ribbonButton10.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton10.SmallImage")));
            this.ribbonButton10.Text = "ribbonButton10";
            // 
            // rTab_ThisNode
            // 
            this.rTab_ThisNode.Text = "This Node";
            // 
            // rTab_Servers
            // 
            this.rTab_Servers.Panels.Add(this.ribbonPanel1);
            this.rTab_Servers.Text = "Servers";
            this.rTab_Servers.ActiveChanged += new System.EventHandler(this.rTab_Servers_OnSelect);
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.Items.Add(this.ribbonButton3);
            this.ribbonPanel1.Text = "ribbonPanel1";
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.Image")));
            this.ribbonButton3.MaxSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton3.MinSizeMode = RibbonElementSizeMode.Compact;
            this.ribbonButton3.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.SmallImage")));
            this.ribbonButton3.Text = "Add Server";
            // 
            // rTab_Clients
            // 
            this.rTab_Clients.Panels.Add(this.ribbonPanel2);
            this.rTab_Clients.Text = "Clients";
            this.rTab_Clients.ActiveChanged += new System.EventHandler(this.rTab_Clients_OnSelect);
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.Items.Add(this.ribbonButton6);
            this.ribbonPanel2.Text = "ribbonPanel2";
            // 
            // ribbonButton6
            // 
            this.ribbonButton6.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton6.Image")));
            this.ribbonButton6.MaxSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton6.MinSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton6.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton6.SmallImage")));
            this.ribbonButton6.Text = "Connect";
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.Image")));
            this.ribbonButton2.MaxSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton2.MinSizeMode = RibbonElementSizeMode.Compact;
            this.ribbonButton2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.SmallImage")));
            this.ribbonButton2.Tag = "";
            this.ribbonButton2.Text = "Connect";
            this.ribbonButton2.ToolTip = "";
            this.ribbonButton2.Value = "";
            // 
            // pnl_SubForm
            // 
            this.pnl_SubForm.Location = new System.Drawing.Point(12, 113);
            this.pnl_SubForm.Name = "pnl_SubForm";
            this.pnl_SubForm.Size = new System.Drawing.Size(1341, 472);
            this.pnl_SubForm.TabIndex = 12;
            // 
            // ribbonButton11
            // 
            this.ribbonButton11.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton11.Image")));
            this.ribbonButton11.MaxSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton11.MinSizeMode = RibbonElementSizeMode.Medium;
            this.ribbonButton11.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton11.SmallImage")));
            this.ribbonButton11.Text = "Assuming we wanna stick with this ribbon thing?";
            // 
            // UDPChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1365, 597);
            this.Controls.Add(this.pnl_SubForm);
            this.Controls.Add(this.ribbon1);
            this.Controls.Add(this.lbl_NodeId);
            this.Name = "UDPChatForm";
            this.Text = "UDPChatForm";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Label lbl_NodeId;
        private Ribbon.Component_Classes.Ribbon ribbon1;
        private RibbonTab rTab_Servers;
        private RibbonTab rTab_ThisNode;
        private RibbonTab rTab_Clients;
        private RibbonButton ribbonButton2;
        private RibbonPanel ribbonPanel1;
        private RibbonButton ribbonButton3;
        private RibbonButton ribbonButton1;
        private RibbonButton ribbonButton4;
        private RibbonButton ribbonButton5;
        private RibbonButton ribbonButton7;
        private RibbonButton ribbonButton8;
        private RibbonButton ribbonButton9;
        private RibbonButton ribbonButton10;
        private RibbonPanel ribbonPanel2;
        private RibbonButton ribbonButton6;
        private System.Windows.Forms.Panel pnl_SubForm;
        private RibbonButton ribbonButton11;
    }
}