namespace src.Views
{
    partial class QuanLyLopMonView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblDesc = new System.Windows.Forms.Label();
            pnlBody = new System.Windows.Forms.Panel();
            tabControl = new Guna.UI2.WinForms.Guna2TabControl();
            tabMonHoc = new System.Windows.Forms.TabPage();
            pnlMonToolbar = new System.Windows.Forms.Panel();
            lblMonCount = new System.Windows.Forms.Label();
            txtSearchMon = new System.Windows.Forms.TextBox();
            btnAddMon = new System.Windows.Forms.Button();
            dgvMonHoc = new System.Windows.Forms.DataGridView();
            tabLopHoc = new System.Windows.Forms.TabPage();
            pnlLopToolbar = new System.Windows.Forms.Panel();
            lblLopCount = new System.Windows.Forms.Label();
            txtSearchLop = new System.Windows.Forms.TextBox();
            btnAddLop = new System.Windows.Forms.Button();
            dgvLopHoc = new System.Windows.Forms.DataGridView();
            
            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            tabControl.SuspendLayout();
            tabMonHoc.SuspendLayout();
            pnlMonToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMonHoc).BeginInit();
            tabLopHoc.SuspendLayout();
            pnlLopToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.Transparent;
            pnlHeader.Controls.Add(lblDesc);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(24, 16);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1095, 85);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(0, 5);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📚  Quản Lý Lớp & Môn Học";
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblDesc.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblDesc.Location = new System.Drawing.Point(2, 53);
            lblDesc.Name = "lblDesc";
            lblDesc.TabIndex = 1;
            lblDesc.Text = "Quản lý danh sách Môn học và Lớp học phần";
            // 
            // pnlBody
            // 
            pnlBody.BackColor = System.Drawing.Color.Transparent;
            pnlBody.Controls.Add(tabControl);
            pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBody.Location = new System.Drawing.Point(24, 101);
            pnlBody.Name = "pnlBody";
            pnlBody.Size = new System.Drawing.Size(1095, 667);
            pnlBody.TabIndex = 1;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabMonHoc);
            tabControl.Controls.Add(tabLopHoc);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.ItemSize = new System.Drawing.Size(180, 40);
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1095, 667);
            tabControl.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            tabControl.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(241, 245, 249);
            tabControl.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            tabControl.TabButtonHoverState.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            tabControl.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(241, 245, 249);
            tabControl.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            tabControl.TabButtonIdleState.FillColor = System.Drawing.Color.White;
            tabControl.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            tabControl.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            tabControl.TabButtonIdleState.InnerColor = System.Drawing.Color.White;
            tabControl.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            tabControl.TabButtonSelectedState.FillColor = System.Drawing.Color.White;
            tabControl.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            tabControl.TabButtonSelectedState.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            tabControl.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(37, 99, 235);
            tabControl.TabButtonSize = new System.Drawing.Size(180, 40);
            tabControl.TabIndex = 0;
            tabControl.TabMenuBackColor = System.Drawing.Color.Transparent;
            tabControl.TabMenuOrientation = Guna.UI2.WinForms.TabMenuOrientation.HorizontalTop;
            // 
            // tabMonHoc
            // 
            tabMonHoc.BackColor = System.Drawing.Color.White;
            tabMonHoc.Controls.Add(dgvMonHoc);
            tabMonHoc.Controls.Add(pnlMonToolbar);
            tabMonHoc.Location = new System.Drawing.Point(4, 44);
            tabMonHoc.Name = "tabMonHoc";
            tabMonHoc.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            tabMonHoc.Size = new System.Drawing.Size(1087, 619);
            tabMonHoc.TabIndex = 0;
            tabMonHoc.Text = "Môn học";
            // 
            // pnlMonToolbar
            // 
            pnlMonToolbar.Controls.Add(lblMonCount);
            pnlMonToolbar.Controls.Add(txtSearchMon);
            pnlMonToolbar.Controls.Add(btnAddMon);
            pnlMonToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlMonToolbar.Location = new System.Drawing.Point(12, 12);
            pnlMonToolbar.Name = "pnlMonToolbar";
            pnlMonToolbar.Size = new System.Drawing.Size(1063, 60);
            pnlMonToolbar.TabIndex = 0;
            // 
            // lblMonCount
            // 
            lblMonCount.AutoSize = true;
            lblMonCount.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            lblMonCount.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblMonCount.Location = new System.Drawing.Point(0, 16);
            lblMonCount.Name = "lblMonCount";
            lblMonCount.TabIndex = 3;
            lblMonCount.Text = "0 môn học";
            // 
            // txtSearchMon
            // 
            txtSearchMon.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtSearchMon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearchMon.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearchMon.Location = new System.Drawing.Point(140, 15);
            txtSearchMon.Name = "txtSearchMon";
            txtSearchMon.PlaceholderText = "🔍  Tìm môn học...";
            txtSearchMon.Size = new System.Drawing.Size(280, 30);
            txtSearchMon.TabIndex = 0;
            // 
            // btnAddMon
            // 
            btnAddMon.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAddMon.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnAddMon.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAddMon.FlatAppearance.BorderSize = 0;
            btnAddMon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddMon.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAddMon.ForeColor = System.Drawing.Color.White;
            btnAddMon.Location = new System.Drawing.Point(923, 14);
            btnAddMon.Name = "btnAddMon";
            btnAddMon.Size = new System.Drawing.Size(140, 32);
            btnAddMon.TabIndex = 1;
            btnAddMon.Text = "＋ Thêm môn";
            btnAddMon.UseVisualStyleBackColor = false;
            // 
            // dgvMonHoc
            // 
            dgvMonHoc.AllowUserToAddRows = false;
            dgvMonHoc.AllowUserToDeleteRows = false;
            dgvMonHoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvMonHoc.BackgroundColor = System.Drawing.Color.White;
            dgvMonHoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvMonHoc.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMonHoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvMonHoc.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvMonHoc.EnableHeadersVisualStyles = false;
            dgvMonHoc.GridColor = System.Drawing.Color.FromArgb(238, 242, 246);
            dgvMonHoc.Location = new System.Drawing.Point(12, 72);
            dgvMonHoc.Name = "dgvMonHoc";
            dgvMonHoc.ReadOnly = true;
            dgvMonHoc.RowHeadersVisible = false;
            dgvMonHoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvMonHoc.Size = new System.Drawing.Size(1063, 535);
            dgvMonHoc.TabIndex = 1;
            // 
            // tabLopHoc
            // 
            tabLopHoc.BackColor = System.Drawing.Color.White;
            tabLopHoc.Controls.Add(dgvLopHoc);
            tabLopHoc.Controls.Add(pnlLopToolbar);
            tabLopHoc.Location = new System.Drawing.Point(4, 44);
            tabLopHoc.Name = "tabLopHoc";
            tabLopHoc.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            tabLopHoc.Size = new System.Drawing.Size(1087, 619);
            tabLopHoc.TabIndex = 1;
            tabLopHoc.Text = "Lớp học phần";
            // 
            // pnlLopToolbar
            // 
            pnlLopToolbar.Controls.Add(lblLopCount);
            pnlLopToolbar.Controls.Add(txtSearchLop);
            pnlLopToolbar.Controls.Add(btnAddLop);
            pnlLopToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlLopToolbar.Location = new System.Drawing.Point(12, 12);
            pnlLopToolbar.Name = "pnlLopToolbar";
            pnlLopToolbar.Size = new System.Drawing.Size(1063, 60);
            pnlLopToolbar.TabIndex = 0;
            // 
            // lblLopCount
            // 
            lblLopCount.AutoSize = true;
            lblLopCount.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            lblLopCount.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblLopCount.Location = new System.Drawing.Point(0, 16);
            lblLopCount.Name = "lblLopCount";
            lblLopCount.TabIndex = 3;
            lblLopCount.Text = "0 lớp học";
            // 
            // txtSearchLop
            // 
            txtSearchLop.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtSearchLop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearchLop.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearchLop.Location = new System.Drawing.Point(140, 15);
            txtSearchLop.Name = "txtSearchLop";
            txtSearchLop.PlaceholderText = "🔍  Tìm lớp học...";
            txtSearchLop.Size = new System.Drawing.Size(280, 30);
            txtSearchLop.TabIndex = 0;
            // 
            // btnAddLop
            // 
            btnAddLop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAddLop.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnAddLop.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAddLop.FlatAppearance.BorderSize = 0;
            btnAddLop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddLop.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAddLop.ForeColor = System.Drawing.Color.White;
            btnAddLop.Location = new System.Drawing.Point(923, 14);
            btnAddLop.Name = "btnAddLop";
            btnAddLop.Size = new System.Drawing.Size(140, 32);
            btnAddLop.TabIndex = 1;
            btnAddLop.Text = "＋ Thêm lớp";
            btnAddLop.UseVisualStyleBackColor = false;
            // 
            // dgvLopHoc
            // 
            dgvLopHoc.AllowUserToAddRows = false;
            dgvLopHoc.AllowUserToDeleteRows = false;
            dgvLopHoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvLopHoc.BackgroundColor = System.Drawing.Color.White;
            dgvLopHoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvLopHoc.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLopHoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLopHoc.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvLopHoc.EnableHeadersVisualStyles = false;
            dgvLopHoc.GridColor = System.Drawing.Color.FromArgb(238, 242, 246);
            dgvLopHoc.Location = new System.Drawing.Point(12, 72);
            dgvLopHoc.Name = "dgvLopHoc";
            dgvLopHoc.ReadOnly = true;
            dgvLopHoc.RowHeadersVisible = false;
            dgvLopHoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLopHoc.Size = new System.Drawing.Size(1063, 535);
            dgvLopHoc.TabIndex = 1;
            // 
            // QuanLyLopMonView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(pnlBody);
            Controls.Add(pnlHeader);
            DoubleBuffered = true;
            Name = "QuanLyLopMonView";
            Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            Size = new System.Drawing.Size(1143, 784);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBody.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabMonHoc.ResumeLayout(false);
            pnlMonToolbar.ResumeLayout(false);
            pnlMonToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMonHoc).EndInit();
            tabLopHoc.ResumeLayout(false);
            pnlLopToolbar.ResumeLayout(false);
            pnlLopToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblDesc;
        public System.Windows.Forms.Panel pnlBody;

        public Guna.UI2.WinForms.Guna2TabControl tabControl;
        
        public System.Windows.Forms.TabPage tabMonHoc;
        public System.Windows.Forms.Panel pnlMonToolbar;
        public System.Windows.Forms.Label lblMonCount;
        public System.Windows.Forms.TextBox txtSearchMon;
        public System.Windows.Forms.Button btnAddMon;
        public System.Windows.Forms.DataGridView dgvMonHoc;

        public System.Windows.Forms.TabPage tabLopHoc;
        public System.Windows.Forms.Panel pnlLopToolbar;
        public System.Windows.Forms.Label lblLopCount;
        public System.Windows.Forms.TextBox txtSearchLop;
        public System.Windows.Forms.Button btnAddLop;
        public System.Windows.Forms.DataGridView dgvLopHoc;
    }
}
