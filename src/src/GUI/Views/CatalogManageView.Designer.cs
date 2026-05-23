namespace src.Views
{
    partial class CatalogManageView
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            tabControlMain = new System.Windows.Forms.TabControl();
            tabLopHoc = new System.Windows.Forms.TabPage();
            pnlGridLop = new System.Windows.Forms.Panel();
            dgvLopHoc = new System.Windows.Forms.DataGridView();
            pnlToolbarLop = new System.Windows.Forms.Panel();
            btnAddLop = new System.Windows.Forms.Button();
            txtSearchLop = new System.Windows.Forms.TextBox();
            tabMonHoc = new System.Windows.Forms.TabPage();
            pnlGridMon = new System.Windows.Forms.Panel();
            dgvMonHoc = new System.Windows.Forms.DataGridView();
            pnlToolbarMon = new System.Windows.Forms.Panel();
            btnAddMon = new System.Windows.Forms.Button();
            txtSearchMon = new System.Windows.Forms.TextBox();
            lblDesc = new System.Windows.Forms.Label();
            pnlHeader.SuspendLayout();
            tabControlMain.SuspendLayout();
            tabLopHoc.SuspendLayout();
            pnlGridLop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).BeginInit();
            pnlToolbarLop.SuspendLayout();
            tabMonHoc.SuspendLayout();
            pnlGridMon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMonHoc).BeginInit();
            pnlToolbarMon.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.Transparent;
            pnlHeader.Controls.Add(lblDesc);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(23, 16);
            pnlHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1097, 85);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(0, 5);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(448, 46);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📚  Quản Lý Lớp & Môn Học";
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabLopHoc);
            tabControlMain.Controls.Add(tabMonHoc);
            tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlMain.Font = new System.Drawing.Font("Segoe UI", 10F);
            tabControlMain.Location = new System.Drawing.Point(23, 101);
            tabControlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.Padding = new System.Drawing.Point(14, 6);
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new System.Drawing.Size(1097, 667);
            tabControlMain.TabIndex = 1;
            // 
            // tabLopHoc
            // 
            tabLopHoc.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            tabLopHoc.Controls.Add(pnlGridLop);
            tabLopHoc.Controls.Add(pnlToolbarLop);
            tabLopHoc.Location = new System.Drawing.Point(4, 38);
            tabLopHoc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabLopHoc.Name = "tabLopHoc";
            tabLopHoc.Size = new System.Drawing.Size(1089, 625);
            tabLopHoc.TabIndex = 0;
            tabLopHoc.Text = "Lớp học phần";
            // 
            // pnlGridLop
            // 
            pnlGridLop.BackColor = System.Drawing.Color.White;
            pnlGridLop.Controls.Add(dgvLopHoc);
            pnlGridLop.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGridLop.Location = new System.Drawing.Point(0, 77);
            pnlGridLop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlGridLop.Name = "pnlGridLop";
            pnlGridLop.Padding = new System.Windows.Forms.Padding(14, 11, 14, 16);
            pnlGridLop.Size = new System.Drawing.Size(1089, 548);
            pnlGridLop.TabIndex = 1;
            // 
            // dgvLopHoc
            // 
            dgvLopHoc.AllowUserToAddRows = false;
            dgvLopHoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvLopHoc.BackgroundColor = System.Drawing.Color.White;
            dgvLopHoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvLopHoc.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLopHoc.ColumnHeadersHeight = 44;
            dgvLopHoc.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvLopHoc.EnableHeadersVisualStyles = false;
            dgvLopHoc.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            dgvLopHoc.Location = new System.Drawing.Point(14, 11);
            dgvLopHoc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            dgvLopHoc.Name = "dgvLopHoc";
            dgvLopHoc.ReadOnly = true;
            dgvLopHoc.RowHeadersVisible = false;
            dgvLopHoc.RowHeadersWidth = 51;
            dgvLopHoc.RowTemplate.Height = 42;
            dgvLopHoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLopHoc.Size = new System.Drawing.Size(1061, 521);
            dgvLopHoc.TabIndex = 0;
            // 
            // pnlToolbarLop
            // 
            pnlToolbarLop.BackColor = System.Drawing.Color.White;
            pnlToolbarLop.Controls.Add(btnAddLop);
            pnlToolbarLop.Controls.Add(txtSearchLop);
            pnlToolbarLop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbarLop.Location = new System.Drawing.Point(0, 0);
            pnlToolbarLop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlToolbarLop.Name = "pnlToolbarLop";
            pnlToolbarLop.Size = new System.Drawing.Size(1089, 77);
            pnlToolbarLop.TabIndex = 0;
            // 
            // btnAddLop
            // 
            btnAddLop.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnAddLop.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAddLop.FlatAppearance.BorderSize = 0;
            btnAddLop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddLop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnAddLop.ForeColor = System.Drawing.Color.White;
            btnAddLop.Location = new System.Drawing.Point(325, 17);
            btnAddLop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnAddLop.Name = "btnAddLop";
            btnAddLop.Size = new System.Drawing.Size(169, 43);
            btnAddLop.TabIndex = 1;
            btnAddLop.Text = "＋  Thêm lớp học";
            btnAddLop.UseVisualStyleBackColor = false;
            // 
            // txtSearchLop
            // 
            txtSearchLop.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            txtSearchLop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearchLop.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtSearchLop.Location = new System.Drawing.Point(14, 20);
            txtSearchLop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtSearchLop.Name = "txtSearchLop";
            txtSearchLop.PlaceholderText = "🔍  Tìm lớp học...";
            txtSearchLop.Size = new System.Drawing.Size(297, 29);
            txtSearchLop.TabIndex = 0;
            // 
            // tabMonHoc
            // 
            tabMonHoc.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            tabMonHoc.Controls.Add(pnlGridMon);
            tabMonHoc.Controls.Add(pnlToolbarMon);
            tabMonHoc.Location = new System.Drawing.Point(4, 38);
            tabMonHoc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabMonHoc.Name = "tabMonHoc";
            tabMonHoc.Size = new System.Drawing.Size(1089, 625);
            tabMonHoc.TabIndex = 1;
            tabMonHoc.Text = "Môn học";
            // 
            // pnlGridMon
            // 
            pnlGridMon.BackColor = System.Drawing.Color.White;
            pnlGridMon.Controls.Add(dgvMonHoc);
            pnlGridMon.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGridMon.Location = new System.Drawing.Point(0, 77);
            pnlGridMon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlGridMon.Name = "pnlGridMon";
            pnlGridMon.Padding = new System.Windows.Forms.Padding(14, 11, 14, 16);
            pnlGridMon.Size = new System.Drawing.Size(1089, 548);
            pnlGridMon.TabIndex = 1;
            // 
            // dgvMonHoc
            // 
            dgvMonHoc.AllowUserToAddRows = false;
            dgvMonHoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvMonHoc.BackgroundColor = System.Drawing.Color.White;
            dgvMonHoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvMonHoc.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMonHoc.ColumnHeadersHeight = 44;
            dgvMonHoc.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvMonHoc.EnableHeadersVisualStyles = false;
            dgvMonHoc.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            dgvMonHoc.Location = new System.Drawing.Point(14, 11);
            dgvMonHoc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            dgvMonHoc.Name = "dgvMonHoc";
            dgvMonHoc.ReadOnly = true;
            dgvMonHoc.RowHeadersVisible = false;
            dgvMonHoc.RowHeadersWidth = 51;
            dgvMonHoc.RowTemplate.Height = 42;
            dgvMonHoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvMonHoc.Size = new System.Drawing.Size(1061, 521);
            dgvMonHoc.TabIndex = 0;
            // 
            // pnlToolbarMon
            // 
            pnlToolbarMon.BackColor = System.Drawing.Color.White;
            pnlToolbarMon.Controls.Add(btnAddMon);
            pnlToolbarMon.Controls.Add(txtSearchMon);
            pnlToolbarMon.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbarMon.Location = new System.Drawing.Point(0, 0);
            pnlToolbarMon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlToolbarMon.Name = "pnlToolbarMon";
            pnlToolbarMon.Size = new System.Drawing.Size(1089, 77);
            pnlToolbarMon.TabIndex = 0;
            // 
            // btnAddMon
            // 
            btnAddMon.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnAddMon.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAddMon.FlatAppearance.BorderSize = 0;
            btnAddMon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddMon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnAddMon.ForeColor = System.Drawing.Color.White;
            btnAddMon.Location = new System.Drawing.Point(325, 17);
            btnAddMon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnAddMon.Name = "btnAddMon";
            btnAddMon.Size = new System.Drawing.Size(169, 43);
            btnAddMon.TabIndex = 1;
            btnAddMon.Text = "＋  Thêm môn học";
            btnAddMon.UseVisualStyleBackColor = false;
            // 
            // txtSearchMon
            // 
            txtSearchMon.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            txtSearchMon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearchMon.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtSearchMon.Location = new System.Drawing.Point(14, 20);
            txtSearchMon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtSearchMon.Name = "txtSearchMon";
            txtSearchMon.PlaceholderText = "🔍  Tìm môn học...";
            txtSearchMon.Size = new System.Drawing.Size(297, 29);
            txtSearchMon.TabIndex = 0;
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblDesc.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblDesc.Location = new System.Drawing.Point(2, 53);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new System.Drawing.Size(526, 21);
            lblDesc.TabIndex = 1;
            lblDesc.Text = "Quản lý thông tin và thiết lập Lớp học phần cùng Môn học";
            // 
            // CatalogManageView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(tabControlMain);
            Controls.Add(pnlHeader);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "CatalogManageView";
            Padding = new System.Windows.Forms.Padding(23, 16, 23, 16);
            Size = new System.Drawing.Size(1143, 784);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            tabControlMain.ResumeLayout(false);
            tabLopHoc.ResumeLayout(false);
            pnlGridLop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).EndInit();
            pnlToolbarLop.ResumeLayout(false);
            pnlToolbarLop.PerformLayout();
            tabMonHoc.ResumeLayout(false);
            pnlGridMon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMonHoc).EndInit();
            pnlToolbarMon.ResumeLayout(false);
            pnlToolbarMon.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.TabControl tabControlMain;
        public System.Windows.Forms.TabPage tabLopHoc;
        public System.Windows.Forms.Panel pnlToolbarLop;
        public System.Windows.Forms.TextBox txtSearchLop;
        public System.Windows.Forms.Button btnAddLop;
        public System.Windows.Forms.Panel pnlGridLop;
        public System.Windows.Forms.DataGridView dgvLopHoc;
        public System.Windows.Forms.TabPage tabMonHoc;
        public System.Windows.Forms.Panel pnlToolbarMon;
        public System.Windows.Forms.TextBox txtSearchMon;
        public System.Windows.Forms.Button btnAddMon;
        public System.Windows.Forms.Panel pnlGridMon;
        public System.Windows.Forms.DataGridView dgvMonHoc;
        public System.Windows.Forms.Label lblDesc;
    }
}
