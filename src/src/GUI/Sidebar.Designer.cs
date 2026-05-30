namespace src
{
    partial class SidebarForm
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
            pnlSidebar = new System.Windows.Forms.Panel();
            pnlSidebarMenu = new System.Windows.Forms.Panel();
            btnUserManage = new System.Windows.Forms.Button();
            btnRoomManage = new System.Windows.Forms.Button();
            btnComputerManage = new System.Windows.Forms.Button();
            btnCatalog = new System.Windows.Forms.Button();
            btnScheduleManage = new System.Windows.Forms.Button();
            btnReports = new System.Windows.Forms.Button();
            pnlSeparator = new System.Windows.Forms.Panel();
            pnlProfile = new System.Windows.Forms.Panel();
            lblRole = new System.Windows.Forms.Label();
            lblUsername = new System.Windows.Forms.Label();
            lblAvatar = new System.Windows.Forms.Label();
            pnlLogout = new System.Windows.Forms.Panel();
            btnLogout = new System.Windows.Forms.Button();
            pnlLogo = new System.Windows.Forms.Panel();
            lblLogo = new System.Windows.Forms.Label();
            lblLogoIcon = new System.Windows.Forms.Label();
            pnlContent = new System.Windows.Forms.Panel();
            pnlSidebar.SuspendLayout();
            pnlSidebarMenu.SuspendLayout();
            pnlProfile.SuspendLayout();
            pnlLogout.SuspendLayout();
            pnlLogo.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = System.Drawing.Color.White;
            pnlSidebar.Controls.Add(pnlSidebarMenu);
            pnlSidebar.Controls.Add(pnlSeparator);
            pnlSidebar.Controls.Add(pnlProfile);
            pnlSidebar.Controls.Add(pnlLogout);
            pnlSidebar.Controls.Add(pnlLogo);
            pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            pnlSidebar.Location = new System.Drawing.Point(0, 0);
            pnlSidebar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new System.Drawing.Size(274, 960);
            pnlSidebar.TabIndex = 0;
            // 
            // pnlSidebarMenu
            // 
            pnlSidebarMenu.BackColor = System.Drawing.Color.Transparent;
            pnlSidebarMenu.Controls.Add(btnUserManage);
            pnlSidebarMenu.Controls.Add(btnRoomManage);
            pnlSidebarMenu.Controls.Add(btnComputerManage);
            pnlSidebarMenu.Controls.Add(btnCatalog);
            pnlSidebarMenu.Controls.Add(btnScheduleManage);
            pnlSidebarMenu.Controls.Add(btnReports);
            pnlSidebarMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlSidebarMenu.Location = new System.Drawing.Point(0, 184);
            pnlSidebarMenu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlSidebarMenu.Name = "pnlSidebarMenu";
            pnlSidebarMenu.Size = new System.Drawing.Size(274, 699);
            pnlSidebarMenu.TabIndex = 1;
            // 
            // btnUserManage
            // 
            btnUserManage.BackColor = System.Drawing.Color.Transparent;
            btnUserManage.Cursor = System.Windows.Forms.Cursors.Hand;
            btnUserManage.FlatAppearance.BorderSize = 0;
            btnUserManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            btnUserManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            btnUserManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnUserManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnUserManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnUserManage.Location = new System.Drawing.Point(13, 8);
            btnUserManage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnUserManage.Name = "btnUserManage";
            btnUserManage.Size = new System.Drawing.Size(247, 59);
            btnUserManage.TabIndex = 1;
            btnUserManage.Tag = "UserManage";
            btnUserManage.Text = "👤   Quản Lý Nhân Viên";
            btnUserManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnUserManage.UseVisualStyleBackColor = false;
            btnUserManage.Click += btnUserManage_Click;
            // 
            // btnRoomManage
            // 
            btnRoomManage.BackColor = System.Drawing.Color.Transparent;
            btnRoomManage.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRoomManage.FlatAppearance.BorderSize = 0;
            btnRoomManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            btnRoomManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            btnRoomManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRoomManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnRoomManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnRoomManage.Location = new System.Drawing.Point(13, 78);
            btnRoomManage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnRoomManage.Name = "btnRoomManage";
            btnRoomManage.Size = new System.Drawing.Size(247, 59);
            btnRoomManage.TabIndex = 2;
            btnRoomManage.Tag = "RoomManage";
            btnRoomManage.Text = "🏢   Quản Lý Phòng";
            btnRoomManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnRoomManage.UseVisualStyleBackColor = false;
            // 
            // btnComputerManage
            // 
            btnComputerManage.BackColor = System.Drawing.Color.Transparent;
            btnComputerManage.Cursor = System.Windows.Forms.Cursors.Hand;
            btnComputerManage.FlatAppearance.BorderSize = 0;
            btnComputerManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            btnComputerManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            btnComputerManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnComputerManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnComputerManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnComputerManage.Location = new System.Drawing.Point(13, 147);
            btnComputerManage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnComputerManage.Name = "btnComputerManage";
            btnComputerManage.Size = new System.Drawing.Size(247, 59);
            btnComputerManage.TabIndex = 3;
            btnComputerManage.Tag = "ComputerManage";
            btnComputerManage.Text = "💻   Quản Lý Máy ";
            btnComputerManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnComputerManage.UseVisualStyleBackColor = false;
            // 
            // btnCatalog
            // 
            btnCatalog.BackColor = System.Drawing.Color.Transparent;
            btnCatalog.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCatalog.FlatAppearance.BorderSize = 0;
            btnCatalog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            btnCatalog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            btnCatalog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCatalog.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnCatalog.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnCatalog.Location = new System.Drawing.Point(13, 286);
            btnCatalog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnCatalog.Name = "btnCatalog";
            btnCatalog.Size = new System.Drawing.Size(247, 59);
            btnCatalog.TabIndex = 6;
            btnCatalog.Tag = "CatalogManage";
            btnCatalog.Text = "📚   Quản lý Lớp && Môn Học";
            btnCatalog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnCatalog.UseVisualStyleBackColor = false;
            // 
            // btnScheduleManage
            // 
            btnScheduleManage.BackColor = System.Drawing.Color.Transparent;
            btnScheduleManage.Cursor = System.Windows.Forms.Cursors.Hand;
            btnScheduleManage.FlatAppearance.BorderSize = 0;
            btnScheduleManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            btnScheduleManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            btnScheduleManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnScheduleManage.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnScheduleManage.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnScheduleManage.Location = new System.Drawing.Point(13, 216);
            btnScheduleManage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnScheduleManage.Name = "btnScheduleManage";
            btnScheduleManage.Size = new System.Drawing.Size(247, 59);
            btnScheduleManage.TabIndex = 4;
            btnScheduleManage.Tag = "ScheduleManage";
            btnScheduleManage.Text = "📅   Quản Lý Lịch Thực Hành";
            btnScheduleManage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnScheduleManage.UseVisualStyleBackColor = false;
            // 
            // btnReports
            // 
            btnReports.BackColor = System.Drawing.Color.Transparent;
            btnReports.Cursor = System.Windows.Forms.Cursors.Hand;
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            btnReports.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnReports.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnReports.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnReports.Location = new System.Drawing.Point(13, 355);
            btnReports.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnReports.Name = "btnReports";
            btnReports.Size = new System.Drawing.Size(247, 59);
            btnReports.TabIndex = 5;
            btnReports.Tag = "Reports";
            btnReports.Text = "📊   Báo Cáo && Thống Kê";
            btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = false;
            // 
            // pnlSeparator
            // 
            pnlSeparator.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            pnlSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSeparator.Location = new System.Drawing.Point(0, 183);
            pnlSeparator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlSeparator.Name = "pnlSeparator";
            pnlSeparator.Size = new System.Drawing.Size(274, 1);
            pnlSeparator.TabIndex = 4;
            // 
            // pnlProfile
            // 
            pnlProfile.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            pnlProfile.Controls.Add(lblRole);
            pnlProfile.Controls.Add(lblUsername);
            pnlProfile.Controls.Add(lblAvatar);
            pnlProfile.Dock = System.Windows.Forms.DockStyle.Top;
            pnlProfile.Location = new System.Drawing.Point(0, 87);
            pnlProfile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlProfile.Name = "pnlProfile";
            pnlProfile.Size = new System.Drawing.Size(274, 96);
            pnlProfile.TabIndex = 2;
            // 
            // lblRole
            // 
            lblRole.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblRole.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblRole.Location = new System.Drawing.Point(73, 49);
            lblRole.Name = "lblRole";
            lblRole.Size = new System.Drawing.Size(187, 24);
            lblRole.TabIndex = 2;
            lblRole.Text = "Admin";
            // 
            // lblUsername
            // 
            lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblUsername.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblUsername.Location = new System.Drawing.Point(73, 19);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(187, 29);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "admin";
            // 
            // lblAvatar
            // 
            lblAvatar.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
            lblAvatar.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblAvatar.ForeColor = System.Drawing.Color.White;
            lblAvatar.Location = new System.Drawing.Point(18, 21);
            lblAvatar.Name = "lblAvatar";
            lblAvatar.Size = new System.Drawing.Size(46, 53);
            lblAvatar.TabIndex = 0;
            lblAvatar.Text = "A";
            lblAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLogout
            // 
            pnlLogout.BackColor = System.Drawing.Color.White;
            pnlLogout.Controls.Add(btnLogout);
            pnlLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlLogout.Location = new System.Drawing.Point(0, 883);
            pnlLogout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlLogout.Name = "pnlLogout";
            pnlLogout.Padding = new System.Windows.Forms.Padding(11, 11, 11, 13);
            pnlLogout.Size = new System.Drawing.Size(274, 77);
            pnlLogout.TabIndex = 0;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = System.Drawing.Color.Transparent;
            btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            btnLogout.Dock = System.Windows.Forms.DockStyle.Fill;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(254, 226, 226);
            btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnLogout.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            btnLogout.Location = new System.Drawing.Point(11, 11);
            btnLogout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new System.Drawing.Size(252, 53);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "↪   Logout";
            btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = false;
            // 
            // pnlLogo
            // 
            pnlLogo.BackColor = System.Drawing.Color.White;
            pnlLogo.Controls.Add(lblLogo);
            pnlLogo.Controls.Add(lblLogoIcon);
            pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            pnlLogo.Location = new System.Drawing.Point(0, 0);
            pnlLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new System.Drawing.Size(274, 87);
            pnlLogo.TabIndex = 3;
            // 
            // lblLogo
            // 
            lblLogo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblLogo.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblLogo.Location = new System.Drawing.Point(69, 19);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new System.Drawing.Size(194, 59);
            lblLogo.TabIndex = 1;
            lblLogo.Text = "Lab Management System";
            lblLogo.Click += lblLogo_Click;
            // 
            // lblLogoIcon
            // 
            lblLogoIcon.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
            lblLogoIcon.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblLogoIcon.ForeColor = System.Drawing.Color.White;
            lblLogoIcon.Location = new System.Drawing.Point(18, 19);
            lblLogoIcon.Name = "lblLogoIcon";
            lblLogoIcon.Size = new System.Drawing.Size(41, 48);
            lblLogoIcon.TabIndex = 0;
            lblLogoIcon.Text = "🖥";
            lblLogoIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlContent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            pnlContent.Location = new System.Drawing.Point(274, 0);
            pnlContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new System.Drawing.Size(1189, 960);
            pnlContent.TabIndex = 1;
            pnlContent.Paint += pnlContent_Paint;
            // 
            // SidebarForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            ClientSize = new System.Drawing.Size(1463, 960);
            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MinimumSize = new System.Drawing.Size(1255, 851);
            Name = "SidebarForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Lab Management System";
            pnlSidebar.ResumeLayout(false);
            pnlSidebarMenu.ResumeLayout(false);
            pnlProfile.ResumeLayout(false);
            pnlLogout.ResumeLayout(false);
            pnlLogo.ResumeLayout(false);
            ResumeLayout(false);
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

