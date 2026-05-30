namespace src.Login
{
    partial class LoginForm
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
            pnlMain = new System.Windows.Forms.Panel();
            pnlRight = new System.Windows.Forms.Panel();
            lblCopyright = new System.Windows.Forms.Label();
            btnLogin = new System.Windows.Forms.Button();
            lblError = new System.Windows.Forms.Label();
            pnlPassWrap = new System.Windows.Forms.Panel();
            txtPassword = new System.Windows.Forms.TextBox();
            btnShowPass = new System.Windows.Forms.Button();
            lblPass = new System.Windows.Forms.Label();
            pnlUserWrap = new System.Windows.Forms.Panel();
            txtUsername = new System.Windows.Forms.TextBox();
            lblUser = new System.Windows.Forms.Label();
            lblSub = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            btnClose = new System.Windows.Forms.Label();
            pnlLeft = new System.Windows.Forms.Panel();
            lblDesc = new System.Windows.Forms.Label();
            lblBrand = new System.Windows.Forms.Label();
            lblIcon = new System.Windows.Forms.Label();
            pnlMain.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlPassWrap.SuspendLayout();
            pnlUserWrap.SuspendLayout();
            pnlLeft.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = System.Drawing.Color.White;
            pnlMain.Controls.Add(pnlRight);
            pnlMain.Controls.Add(pnlLeft);
            pnlMain.Location = new System.Drawing.Point(23, 27);
            pnlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new System.Drawing.Size(983, 640);
            pnlMain.TabIndex = 0;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = System.Drawing.Color.White;
            pnlRight.Controls.Add(lblCopyright);
            pnlRight.Controls.Add(btnLogin);
            pnlRight.Controls.Add(lblError);
            pnlRight.Controls.Add(pnlPassWrap);
            pnlRight.Controls.Add(lblPass);
            pnlRight.Controls.Add(pnlUserWrap);
            pnlRight.Controls.Add(lblUser);
            pnlRight.Controls.Add(lblSub);
            pnlRight.Controls.Add(lblTitle);
            pnlRight.Controls.Add(btnClose);
            pnlRight.Location = new System.Drawing.Point(434, 0);
            pnlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new System.Drawing.Size(549, 640);
            pnlRight.TabIndex = 1;
            // 
            // lblCopyright
            // 
            lblCopyright.AutoSize = true;
            lblCopyright.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblCopyright.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            lblCopyright.Location = new System.Drawing.Point(166, 566);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new System.Drawing.Size(217, 19);
            lblCopyright.TabIndex = 10;
            lblCopyright.Text = "© 2026 Lab Management System";
            // 
            // btnLogin
            // 
            btnLogin.BackColor = System.Drawing.Color.FromArgb(45, 75, 205);
            btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.Location = new System.Drawing.Point(57, 467);
            btnLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(434, 64);
            btnLogin.TabIndex = 8;
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;
            // 
            // lblError
            // 
            lblError.BackColor = System.Drawing.Color.Transparent;
            lblError.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblError.ForeColor = System.Drawing.Color.FromArgb(235, 87, 87);
            lblError.Location = new System.Drawing.Point(57, 429);
            lblError.Name = "lblError";
            lblError.Size = new System.Drawing.Size(434, 29);
            lblError.TabIndex = 7;
            lblError.Visible = false;
            // 
            // pnlPassWrap
            // 
            pnlPassWrap.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            pnlPassWrap.Controls.Add(txtPassword);
            pnlPassWrap.Controls.Add(btnShowPass);
            pnlPassWrap.Location = new System.Drawing.Point(57, 353);
            pnlPassWrap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlPassWrap.Name = "pnlPassWrap";
            pnlPassWrap.Size = new System.Drawing.Size(434, 59);
            pnlPassWrap.TabIndex = 6;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtPassword.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            txtPassword.ForeColor = System.Drawing.Color.FromArgb(40, 45, 60);
            txtPassword.Location = new System.Drawing.Point(16, 15);
            txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(352, 24);
            txtPassword.TabIndex = 0;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnShowPass
            // 
            btnShowPass.BackColor = System.Drawing.Color.Transparent;
            btnShowPass.Cursor = System.Windows.Forms.Cursors.Hand;
            btnShowPass.FlatAppearance.BorderSize = 0;
            btnShowPass.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnShowPass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnShowPass.Font = new System.Drawing.Font("Segoe UI", 13F);
            btnShowPass.ForeColor = System.Drawing.Color.FromArgb(140, 150, 170);
            btnShowPass.Location = new System.Drawing.Point(380, 12);
            btnShowPass.Name = "btnShowPass";
            btnShowPass.Size = new System.Drawing.Size(40, 32);
            btnShowPass.TabIndex = 1;
            btnShowPass.TabStop = false;
            btnShowPass.Text = "👁";
            btnShowPass.UseVisualStyleBackColor = false;
            btnShowPass.Click += BtnShowPass_Click;
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblPass.ForeColor = System.Drawing.Color.FromArgb(40, 45, 60);
            lblPass.Location = new System.Drawing.Point(57, 320);
            lblPass.Name = "lblPass";
            lblPass.Size = new System.Drawing.Size(82, 21);
            lblPass.TabIndex = 5;
            lblPass.Text = "Mật khẩu";
            // 
            // pnlUserWrap
            // 
            pnlUserWrap.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            pnlUserWrap.Controls.Add(txtUsername);
            pnlUserWrap.Location = new System.Drawing.Point(57, 240);
            pnlUserWrap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlUserWrap.Name = "pnlUserWrap";
            pnlUserWrap.Size = new System.Drawing.Size(434, 59);
            pnlUserWrap.TabIndex = 4;
            // 
            // txtUsername
            // 
            txtUsername.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUsername.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            txtUsername.ForeColor = System.Drawing.Color.FromArgb(40, 45, 60);
            txtUsername.Location = new System.Drawing.Point(16, 15);
            txtUsername.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(389, 24);
            txtUsername.TabIndex = 0;
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblUser.ForeColor = System.Drawing.Color.FromArgb(40, 45, 60);
            lblUser.Location = new System.Drawing.Point(57, 207);
            lblUser.Name = "lblUser";
            lblUser.Size = new System.Drawing.Size(123, 21);
            lblUser.TabIndex = 3;
            lblUser.Text = "Tên đăng nhập";
            // 
            // lblSub
            // 
            lblSub.AutoSize = true;
            lblSub.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSub.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            lblSub.Location = new System.Drawing.Point(57, 133);
            lblSub.Name = "lblSub";
            lblSub.Size = new System.Drawing.Size(270, 23);
            lblSub.TabIndex = 2;
            lblSub.Text = "Vui lòng nhập thông tin tài khoản";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(40, 45, 60);
            lblTitle.Location = new System.Drawing.Point(57, 73);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(238, 54);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Đăng Nhập";
            // 
            // btnClose
            // 
            btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 13F);
            btnClose.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            btnClose.Location = new System.Drawing.Point(503, 11);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(37, 43);
            btnClose.TabIndex = 0;
            btnClose.Text = "✕";
            btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = System.Drawing.Color.FromArgb(45, 75, 205);
            pnlLeft.Controls.Add(lblDesc);
            pnlLeft.Controls.Add(lblBrand);
            pnlLeft.Controls.Add(lblIcon);
            pnlLeft.Location = new System.Drawing.Point(0, 0);
            pnlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new System.Drawing.Size(434, 640);
            pnlLeft.TabIndex = 0;
            // 
            // lblDesc
            // 
            lblDesc.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            lblDesc.ForeColor = System.Drawing.Color.FromArgb(200, 255, 255, 255);
            lblDesc.Location = new System.Drawing.Point(40, 367);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new System.Drawing.Size(354, 107);
            lblDesc.TabIndex = 2;
            lblDesc.Text = "Quản lý phòng máy thực hành\nhiệu quả cho trường đại học.\nTheo dõi · Xếp lịch · Thống kê";
            // 
            // lblBrand
            // 
            lblBrand.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            lblBrand.ForeColor = System.Drawing.Color.White;
            lblBrand.Location = new System.Drawing.Point(40, 156);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new System.Drawing.Size(354, 185);
            lblBrand.TabIndex = 1;
            lblBrand.Text = "Hệ Thống\nQuản Lý\nPhòng Máy";
            // 
            // lblIcon
            // 
            lblIcon.AutoSize = true;
            lblIcon.Font = new System.Drawing.Font("Segoe UI", 44F);
            lblIcon.ForeColor = System.Drawing.Color.White;
            lblIcon.Location = new System.Drawing.Point(40, 57);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new System.Drawing.Size(144, 99);
            lblIcon.TabIndex = 0;
            lblIcon.Text = "🎓";
            lblIcon.Click += lblIcon_Click;
            // 
            // LoginForm
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(230, 235, 245);
            ClientSize = new System.Drawing.Size(1029, 693);
            Controls.Add(pnlMain);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "LoginForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Đăng Nhập - Quản Lý Phòng Máy";
            pnlMain.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlRight.PerformLayout();
            pnlPassWrap.ResumeLayout(false);
            pnlPassWrap.PerformLayout();
            pnlUserWrap.ResumeLayout(false);
            pnlUserWrap.PerformLayout();
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel pnlUserWrap;
        public System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Panel pnlPassWrap;
        public System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnShowPass;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblCopyright;
    }
}

