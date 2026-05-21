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
            this.btnScheduleManage = new System.Windows.Forms.Button();
            this.btnComputerManage = new System.Windows.Forms.Button();
            this.btnRoomManage = new System.Windows.Forms.Button();
            this.btnUserManage = new System.Windows.Forms.Button();
            this.btnCatalog = new System.Windows.Forms.Button();
            this.pnlProfile = new System.Windows.Forms.Panel();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblAvatar = new System.Windows.Forms.Label();
            this.pnlLogout = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblLogoIcon = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSeparator = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            this.pnlSidebarMenu.SuspendLayout();
            this.pnlProfile.SuspendLayout();
            this.pnlLogout.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.SuspendLayout();

            // ─── pnlSidebar ── Sidebar trắng, độ rộng cố định 240px ──
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.Controls.Add(this.pnlSidebarMenu);
            this.pnlSidebar.Controls.Add(this.pnlSeparator);
            this.pnlSidebar.Controls.Add(this.pnlProfile);
            this.pnlSidebar.Controls.Add(this.pnlLogout);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(240, 720);
            this.pnlSidebar.TabIndex = 0;

            // ─── pnlLogo ── Logo phía trên (65px) ──
            this.pnlLogo.BackColor = System.Drawing.Color.White;
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.Controls.Add(this.lblLogoIcon);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(240, 65);
            this.pnlLogo.TabIndex = 3;

            // ─── lblLogoIcon ── Icon vuông bo tròn ──
            this.lblLogoIcon.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
            this.lblLogoIcon.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblLogoIcon.ForeColor = System.Drawing.Color.White;
            this.lblLogoIcon.Location = new System.Drawing.Point(16, 14);
            this.lblLogoIcon.Name = "lblLogoIcon";
            this.lblLogoIcon.Size = new System.Drawing.Size(36, 36);
            this.lblLogoIcon.TabIndex = 0;
            this.lblLogoIcon.Text = "🖥";
            this.lblLogoIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ─── lblLogo ── Tên ứng dụng ──
            this.lblLogo.AutoSize = false;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblLogo.Location = new System.Drawing.Point(60, 18);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(170, 28);
            this.lblLogo.TabIndex = 1;
            this.lblLogo.Text = "PC Room System";

            // ─── pnlProfile ── Thông tin user (75px) ──
            this.pnlProfile.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlProfile.Controls.Add(this.lblRole);
            this.pnlProfile.Controls.Add(this.lblUsername);
            this.pnlProfile.Controls.Add(this.lblAvatar);
            this.pnlProfile.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProfile.Location = new System.Drawing.Point(0, 65);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Size = new System.Drawing.Size(240, 72);
            this.pnlProfile.TabIndex = 2;

            // ─── lblAvatar ── Chữ cái đầu tên user ──
            this.lblAvatar.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
            this.lblAvatar.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblAvatar.ForeColor = System.Drawing.Color.White;
            this.lblAvatar.Location = new System.Drawing.Point(16, 16);
            this.lblAvatar.Name = "lblAvatar";
            this.lblAvatar.Size = new System.Drawing.Size(40, 40);
            this.lblAvatar.TabIndex = 0;
            this.lblAvatar.Text = "A";
            this.lblAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ─── lblUsername ──
            this.lblUsername.AutoSize = false;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblUsername.Location = new System.Drawing.Point(64, 14);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(164, 22);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "admin";

            // ─── lblRole ──
            this.lblRole.AutoSize = false;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblRole.Location = new System.Drawing.Point(64, 37);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(164, 18);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Admin";

            // ─── pnlSeparator ── Đường kẻ ngăn cách mỏng ──
            this.pnlSeparator.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeparator.Location = new System.Drawing.Point(0, 137);
            this.pnlSeparator.Name = "pnlSeparator";
            this.pnlSeparator.Size = new System.Drawing.Size(240, 1);
            this.pnlSeparator.TabIndex = 4;

            // ─── pnlSidebarMenu ── Khu vực menu (vị trí cố định để Designer hoạt động) ──
            this.pnlSidebarMenu.BackColor = System.Drawing.Color.Transparent;
            this.pnlSidebarMenu.Controls.Add(this.btnUserManage);
            this.pnlSidebarMenu.Controls.Add(this.btnRoomManage);
            this.pnlSidebarMenu.Controls.Add(this.btnComputerManage);
            this.pnlSidebarMenu.Controls.Add(this.btnCatalog);
            this.pnlSidebarMenu.Controls.Add(this.btnScheduleManage);
            this.pnlSidebarMenu.Controls.Add(this.btnReports);
            this.pnlSidebarMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSidebarMenu.Location = new System.Drawing.Point(0, 138);
            this.pnlSidebarMenu.Name = "pnlSidebarMenu";
            this.pnlSidebarMenu.Size = new System.Drawing.Size(240, 527);
            this.pnlSidebarMenu.TabIndex = 1;


            // ─── btnUserManage ──
            this.btnUserManage.BackColor = System.Drawing.Color.Transparent;
            this.btnUserManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserManage.FlatAppearance.BorderSize = 0;
            this.btnUserManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnUserManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnUserManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnUserManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnUserManage.Location = new System.Drawing.Point(12, 64);
            this.btnUserManage.Name = "btnUserManage";
            this.btnUserManage.Size = new System.Drawing.Size(216, 44);
            this.btnUserManage.TabIndex = 1;
            this.btnUserManage.Tag = "UserManage";
            this.btnUserManage.Text = "👤   User Management";
            this.btnUserManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserManage.UseVisualStyleBackColor = false;

            // ─── btnRoomManage ──
            this.btnRoomManage.BackColor = System.Drawing.Color.Transparent;
            this.btnRoomManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRoomManage.FlatAppearance.BorderSize = 0;
            this.btnRoomManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnRoomManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnRoomManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoomManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRoomManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnRoomManage.Location = new System.Drawing.Point(12, 116);
            this.btnRoomManage.Name = "btnRoomManage";
            this.btnRoomManage.Size = new System.Drawing.Size(216, 44);
            this.btnRoomManage.TabIndex = 2;
            this.btnRoomManage.Tag = "RoomManage";
            this.btnRoomManage.Text = "🏢   Room Management";
            this.btnRoomManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRoomManage.UseVisualStyleBackColor = false;

            // ─── btnComputerManage ──
            this.btnComputerManage.BackColor = System.Drawing.Color.Transparent;
            this.btnComputerManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComputerManage.FlatAppearance.BorderSize = 0;
            this.btnComputerManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnComputerManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnComputerManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComputerManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnComputerManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnComputerManage.Location = new System.Drawing.Point(12, 168);
            this.btnComputerManage.Name = "btnComputerManage";
            this.btnComputerManage.Size = new System.Drawing.Size(216, 44);
            this.btnComputerManage.TabIndex = 3;
            this.btnComputerManage.Tag = "ComputerManage";
            this.btnComputerManage.Text = "💻   Computers";
            this.btnComputerManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComputerManage.UseVisualStyleBackColor = false;

            // ─── btnScheduleManage ──
            this.btnScheduleManage.BackColor = System.Drawing.Color.Transparent;
            this.btnScheduleManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScheduleManage.FlatAppearance.BorderSize = 0;
            this.btnScheduleManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnScheduleManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnScheduleManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnScheduleManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnScheduleManage.Location = new System.Drawing.Point(12, 220);
            this.btnScheduleManage.Name = "btnScheduleManage";
            this.btnScheduleManage.Size = new System.Drawing.Size(216, 44);
            this.btnScheduleManage.TabIndex = 4;
            this.btnScheduleManage.Tag = "ScheduleManage";
            this.btnScheduleManage.Text = "📅   Practice Calendar";
            this.btnScheduleManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnScheduleManage.UseVisualStyleBackColor = false;

            // ─── btnCatalog ──
            this.btnCatalog.BackColor = System.Drawing.Color.Transparent;
            this.btnCatalog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCatalog.FlatAppearance.BorderSize = 0;
            this.btnCatalog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnCatalog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnCatalog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCatalog.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCatalog.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCatalog.Location = new System.Drawing.Point(12, 272);
            this.btnCatalog.Name = "btnCatalog";
            this.btnCatalog.Size = new System.Drawing.Size(216, 44);
            this.btnCatalog.TabIndex = 6;
            this.btnCatalog.Tag = "CatalogManage";
            this.btnCatalog.Text = "📚   Lớp & Môn Học";
            this.btnCatalog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCatalog.UseVisualStyleBackColor = false;

            // ─── btnReports ──
            this.btnReports.BackColor = System.Drawing.Color.Transparent;
            this.btnReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnReports.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReports.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnReports.Location = new System.Drawing.Point(12, 324);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(216, 44);
            this.btnReports.TabIndex = 5;
            this.btnReports.Tag = "Reports";
            this.btnReports.Text = "📊   Reports && Stats";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.UseVisualStyleBackColor = false;

            // ─── pnlLogout ── Nút logout ở đáy sidebar ──
            this.pnlLogout.BackColor = System.Drawing.Color.White;
            this.pnlLogout.Controls.Add(this.btnLogout);
            this.pnlLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLogout.Location = new System.Drawing.Point(0, 662);
            this.pnlLogout.Name = "pnlLogout";
            this.pnlLogout.Padding = new System.Windows.Forms.Padding(10, 8, 10, 10);
            this.pnlLogout.Size = new System.Drawing.Size(240, 58);
            this.pnlLogout.TabIndex = 0;

            // ─── btnLogout ──
            this.btnLogout.BackColor = System.Drawing.Color.Transparent;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(254, 226, 226);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(220, 40);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "↪   Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = false;

            // ─── pnlContent ── Vùng nội dung chính bên phải ──
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(240, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(0);
            this.pnlContent.Size = new System.Drawing.Size(1040, 720);
            this.pnlContent.TabIndex = 1;

            // ─── MainForm ──
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(1100, 650);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PC Room Management System";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebarMenu.ResumeLayout(false);
            this.pnlProfile.ResumeLayout(false);
            this.pnlLogout.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        public System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlSidebarMenu;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Panel pnlProfile;
        private System.Windows.Forms.Panel pnlLogout;
        private System.Windows.Forms.Panel pnlSeparator;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnUserManage;
        private System.Windows.Forms.Button btnScheduleManage;
        private System.Windows.Forms.Button btnComputerManage;
        private System.Windows.Forms.Button btnRoomManage;
        private System.Windows.Forms.Button btnCatalog;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblLogoIcon;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblAvatar;
        private System.Windows.Forms.Label lblRole;
    }
}
