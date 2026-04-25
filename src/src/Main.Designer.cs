namespace src
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlTopbar = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.pnlProfile = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Label();
            this.pnlSidebarMenu = new System.Windows.Forms.Panel();
            this.lblSection = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            
            this.pnlSidebar.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
            this.pnlProfile.SuspendLayout();
            this.pnlSidebarMenu.SuspendLayout();
            this.SuspendLayout();
            
            // pnlSidebar
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(18, 25, 50);
            this.pnlSidebar.Controls.Add(this.pnlSidebarMenu);
            this.pnlSidebar.Controls.Add(this.pnlProfile);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(250, 720);
            
            // pnlLogo
            this.pnlLogo.BackColor = System.Drawing.Color.Transparent;
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(250, 70);
            
            // pnlProfile
            this.pnlProfile.BackColor = System.Drawing.Color.FromArgb(22, 30, 60);
            this.pnlProfile.Controls.Add(this.btnLogout);
            this.pnlProfile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlProfile.Location = new System.Drawing.Point(0, 655);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Size = new System.Drawing.Size(250, 65);
            
            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.Transparent;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnLogout.Location = new System.Drawing.Point(210, 18);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(30, 30);
            this.btnLogout.Text = "🚪";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // pnlSidebarMenu
            this.pnlSidebarMenu.BackColor = System.Drawing.Color.Transparent;
            this.pnlSidebarMenu.Controls.Add(this.lblSection);
            this.pnlSidebarMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSidebarMenu.Location = new System.Drawing.Point(0, 70);
            this.pnlSidebarMenu.Name = "pnlSidebarMenu";
            this.pnlSidebarMenu.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlSidebarMenu.Size = new System.Drawing.Size(250, 585);
            
            // lblSection
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSection.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblSection.ForeColor = System.Drawing.Color.FromArgb(90, 255, 255, 255);
            this.lblSection.Location = new System.Drawing.Point(10, 8);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(230, 32);
            this.lblSection.Text = "  MENU CHÍNH";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            // pnlTopbar
            this.pnlTopbar.BackColor = System.Drawing.Color.White;
            this.pnlTopbar.Controls.Add(this.lblDate);
            this.pnlTopbar.Controls.Add(this.lblPageTitle);
            this.pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopbar.Location = new System.Drawing.Point(250, 0);
            this.pnlTopbar.Name = "pnlTopbar";
            this.pnlTopbar.Size = new System.Drawing.Size(1030, 60);
            
            // lblPageTitle
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(40, 45, 60);
            this.lblPageTitle.Location = new System.Drawing.Point(22, 14);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(131, 31);
            this.lblPageTitle.Text = "Dashboard";
            
            // lblDate
            this.lblDate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            this.lblDate.Location = new System.Drawing.Point(850, 22);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(100, 15);
            this.lblDate.Text = "Monday, 01/01/2026";
            
            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(250, 60);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(15);
            this.pnlContent.Size = new System.Drawing.Size(1030, 660);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTopbar);
            this.Controls.Add(this.pnlSidebar);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(1100, 650);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Phòng Máy Thực Hành";
            
            this.pnlSidebar.ResumeLayout(false);
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
            this.pnlProfile.ResumeLayout(false);
            this.pnlSidebarMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlTopbar;
        public System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlSidebarMenu;
        public System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Panel pnlProfile;
        private System.Windows.Forms.Label btnLogout;
        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.Label lblDate;
    }
}
