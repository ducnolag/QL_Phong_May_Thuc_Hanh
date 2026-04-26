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
            this.pnlSidebarMenu = new System.Windows.Forms.Panel();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnUserManage = new System.Windows.Forms.Button();
            this.btnScheduleManage = new System.Windows.Forms.Button();
            this.btnComputerManage = new System.Windows.Forms.Button();
            this.btnRoomManage = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.lblSection = new System.Windows.Forms.Label();
            this.pnlProfile = new System.Windows.Forms.Panel();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblAvatar = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Label();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlTopbar = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            this.pnlSidebarMenu.SuspendLayout();
            this.pnlProfile.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
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
            
            // pnlSidebarMenu
            this.pnlSidebarMenu.BackColor = System.Drawing.Color.Transparent;
            this.pnlSidebarMenu.Controls.Add(this.btnReports);
            this.pnlSidebarMenu.Controls.Add(this.btnUserManage);
            this.pnlSidebarMenu.Controls.Add(this.btnScheduleManage);
            this.pnlSidebarMenu.Controls.Add(this.btnComputerManage);
            this.pnlSidebarMenu.Controls.Add(this.btnRoomManage);
            this.pnlSidebarMenu.Controls.Add(this.btnDashboard);
            this.pnlSidebarMenu.Controls.Add(this.lblSection);
            this.pnlSidebarMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSidebarMenu.Location = new System.Drawing.Point(0, 70);
            this.pnlSidebarMenu.Name = "pnlSidebarMenu";
            this.pnlSidebarMenu.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlSidebarMenu.Size = new System.Drawing.Size(250, 585);
            
            // btnReports
            this.btnReports.BackColor = System.Drawing.Color.Transparent;
            this.btnReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 45, 80);
            this.btnReports.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(25, 35, 65);
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReports.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnReports.Location = new System.Drawing.Point(10, 240);
            this.btnReports.Name = "btnReports";
            this.btnReports.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnReports.Size = new System.Drawing.Size(230, 40);
            this.btnReports.TabIndex = 6;
            this.btnReports.Tag = "Reports";
            this.btnReports.Text = "  📈   Báo Cáo";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.UseVisualStyleBackColor = false;
            
            // btnUserManage
            this.btnUserManage.BackColor = System.Drawing.Color.Transparent;
            this.btnUserManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserManage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUserManage.FlatAppearance.BorderSize = 0;
            this.btnUserManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 45, 80);
            this.btnUserManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(25, 35, 65);
            this.btnUserManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnUserManage.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnUserManage.Location = new System.Drawing.Point(10, 200);
            this.btnUserManage.Name = "btnUserManage";
            this.btnUserManage.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnUserManage.Size = new System.Drawing.Size(230, 40);
            this.btnUserManage.TabIndex = 5;
            this.btnUserManage.Tag = "UserManage";
            this.btnUserManage.Text = "  👥   Người Dùng";
            this.btnUserManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserManage.UseVisualStyleBackColor = false;
            
            // btnScheduleManage
            this.btnScheduleManage.BackColor = System.Drawing.Color.Transparent;
            this.btnScheduleManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScheduleManage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnScheduleManage.FlatAppearance.BorderSize = 0;
            this.btnScheduleManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 45, 80);
            this.btnScheduleManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(25, 35, 65);
            this.btnScheduleManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnScheduleManage.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnScheduleManage.Location = new System.Drawing.Point(10, 160);
            this.btnScheduleManage.Name = "btnScheduleManage";
            this.btnScheduleManage.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnScheduleManage.Size = new System.Drawing.Size(230, 40);
            this.btnScheduleManage.TabIndex = 4;
            this.btnScheduleManage.Tag = "ScheduleManage";
            this.btnScheduleManage.Text = "  📅   Lịch Thực Hành";
            this.btnScheduleManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnScheduleManage.UseVisualStyleBackColor = false;
            
            // btnComputerManage
            this.btnComputerManage.BackColor = System.Drawing.Color.Transparent;
            this.btnComputerManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComputerManage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnComputerManage.FlatAppearance.BorderSize = 0;
            this.btnComputerManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 45, 80);
            this.btnComputerManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(25, 35, 65);
            this.btnComputerManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComputerManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnComputerManage.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnComputerManage.Location = new System.Drawing.Point(10, 120);
            this.btnComputerManage.Name = "btnComputerManage";
            this.btnComputerManage.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnComputerManage.Size = new System.Drawing.Size(230, 40);
            this.btnComputerManage.TabIndex = 3;
            this.btnComputerManage.Tag = "ComputerManage";
            this.btnComputerManage.Text = "  💻   Máy Tính";
            this.btnComputerManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComputerManage.UseVisualStyleBackColor = false;
            
            // btnRoomManage
            this.btnRoomManage.BackColor = System.Drawing.Color.Transparent;
            this.btnRoomManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRoomManage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRoomManage.FlatAppearance.BorderSize = 0;
            this.btnRoomManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 45, 80);
            this.btnRoomManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(25, 35, 65);
            this.btnRoomManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoomManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRoomManage.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnRoomManage.Location = new System.Drawing.Point(10, 80);
            this.btnRoomManage.Name = "btnRoomManage";
            this.btnRoomManage.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnRoomManage.Size = new System.Drawing.Size(230, 40);
            this.btnRoomManage.TabIndex = 2;
            this.btnRoomManage.Tag = "RoomManage";
            this.btnRoomManage.Text = "  🏢   Phòng Máy";
            this.btnRoomManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRoomManage.UseVisualStyleBackColor = false;
            
            // btnDashboard
            this.btnDashboard.BackColor = System.Drawing.Color.Transparent;
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 45, 80);
            this.btnDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(25, 35, 65);
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnDashboard.Location = new System.Drawing.Point(10, 40);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(230, 40);
            this.btnDashboard.TabIndex = 1;
            this.btnDashboard.Tag = "Dashboard";
            this.btnDashboard.Text = "  📊   Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            
            // lblSection
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSection.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblSection.ForeColor = System.Drawing.Color.FromArgb(90, 255, 255, 255);
            this.lblSection.Location = new System.Drawing.Point(10, 8);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(230, 32);
            this.lblSection.TabIndex = 0;
            this.lblSection.Text = "  MENU CHÍNH";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            
            // pnlProfile
            this.pnlProfile.BackColor = System.Drawing.Color.FromArgb(22, 30, 60);
            this.pnlProfile.Controls.Add(this.lblRole);
            this.pnlProfile.Controls.Add(this.lblUsername);
            this.pnlProfile.Controls.Add(this.lblAvatar);
            this.pnlProfile.Controls.Add(this.btnLogout);
            this.pnlProfile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlProfile.Location = new System.Drawing.Point(0, 655);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Size = new System.Drawing.Size(250, 65);
            
            // lblRole
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.lblRole.Location = new System.Drawing.Point(58, 32);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(78, 15);
            this.lblRole.TabIndex = 3;
            this.lblRole.Text = "Quản trị viên";
            
            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.White;
            this.lblUsername.Location = new System.Drawing.Point(58, 12);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(95, 17);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Administrator";
            
            // lblAvatar
            this.lblAvatar.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lblAvatar.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblAvatar.ForeColor = System.Drawing.Color.White;
            this.lblAvatar.Location = new System.Drawing.Point(12, 13);
            this.lblAvatar.Name = "lblAvatar";
            this.lblAvatar.Size = new System.Drawing.Size(38, 38);
            this.lblAvatar.TabIndex = 1;
            this.lblAvatar.Text = "A";
            this.lblAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.Transparent;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            this.btnLogout.Location = new System.Drawing.Point(210, 18);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(30, 30);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "🚪";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // pnlLogo
            this.pnlLogo.BackColor = System.Drawing.Color.Transparent;
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(250, 70);
            
            // lblLogo
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.lblLogo.Size = new System.Drawing.Size(250, 70);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "🎓  LabManager";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // pnlTopbar
            this.pnlTopbar.BackColor = System.Drawing.Color.White;
            this.pnlTopbar.Controls.Add(this.lblDate);
            this.pnlTopbar.Controls.Add(this.lblPageTitle);
            this.pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopbar.Location = new System.Drawing.Point(250, 0);
            this.pnlTopbar.Name = "pnlTopbar";
            this.pnlTopbar.Size = new System.Drawing.Size(1030, 60);
            
            // lblDate
            this.lblDate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            this.lblDate.Location = new System.Drawing.Point(850, 22);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(100, 15);
            this.lblDate.TabIndex = 1;
            this.lblDate.Text = "Monday, 01/01/2026";
            
            // lblPageTitle
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(40, 45, 60);
            this.lblPageTitle.Location = new System.Drawing.Point(22, 14);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(131, 31);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Dashboard";
            
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
            this.pnlSidebarMenu.ResumeLayout(false);
            this.pnlProfile.ResumeLayout(false);
            this.pnlProfile.PerformLayout();
            this.pnlLogo.ResumeLayout(false);
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
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
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnUserManage;
        private System.Windows.Forms.Button btnScheduleManage;
        private System.Windows.Forms.Button btnComputerManage;
        private System.Windows.Forms.Button btnRoomManage;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblAvatar;
        private System.Windows.Forms.Label lblRole;
    }
}
