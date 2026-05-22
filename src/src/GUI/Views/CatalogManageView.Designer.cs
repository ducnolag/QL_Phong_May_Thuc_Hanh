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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabLopHoc = new System.Windows.Forms.TabPage();
            this.pnlGridLop = new System.Windows.Forms.Panel();
            this.dgvLopHoc = new System.Windows.Forms.DataGridView();
            this.pnlToolbarLop = new System.Windows.Forms.Panel();
            this.btnAddLop = new System.Windows.Forms.Button();
            this.txtSearchLop = new System.Windows.Forms.TextBox();
            this.tabMonHoc = new System.Windows.Forms.TabPage();
            this.pnlGridMon = new System.Windows.Forms.Panel();
            this.dgvMonHoc = new System.Windows.Forms.DataGridView();
            this.pnlToolbarMon = new System.Windows.Forms.Panel();
            this.btnAddMon = new System.Windows.Forms.Button();
            this.txtSearchMon = new System.Windows.Forms.TextBox();
            
            this.pnlHeader.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabLopHoc.SuspendLayout();
            this.pnlGridLop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLopHoc)).BeginInit();
            this.pnlToolbarLop.SuspendLayout();
            this.tabMonHoc.SuspendLayout();
            this.pnlGridMon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonHoc)).BeginInit();
            this.pnlToolbarMon.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.lblDesc);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(20, 12);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(960, 64);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblDesc.Location = new System.Drawing.Point(2, 40);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(465, 17);
            this.lblDesc.TabIndex = 1;
            this.lblDesc.Text = "Thêm, sửa, xóa Lớp học phần và Môn học — dùng trong xếp lịch thực hành";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(325, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📚  Quản Lý Lớp & Môn Học";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabLopHoc);
            this.tabControlMain.Controls.Add(this.tabMonHoc);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControlMain.Location = new System.Drawing.Point(20, 76);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.Padding = new System.Drawing.Point(14, 6);
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(960, 500);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabLopHoc
            // 
            this.tabLopHoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.tabLopHoc.Controls.Add(this.pnlGridLop);
            this.tabLopHoc.Controls.Add(this.pnlToolbarLop);
            this.tabLopHoc.Location = new System.Drawing.Point(4, 34);
            this.tabLopHoc.Name = "tabLopHoc";
            this.tabLopHoc.Size = new System.Drawing.Size(952, 462);
            this.tabLopHoc.TabIndex = 0;
            this.tabLopHoc.Text = "Lớp học phần";
            // 
            // pnlGridLop
            // 
            this.pnlGridLop.BackColor = System.Drawing.Color.White;
            this.pnlGridLop.Controls.Add(this.dgvLopHoc);
            this.pnlGridLop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridLop.Location = new System.Drawing.Point(0, 58);
            this.pnlGridLop.Name = "pnlGridLop";
            this.pnlGridLop.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this.pnlGridLop.Size = new System.Drawing.Size(952, 404);
            this.pnlGridLop.TabIndex = 1;
            // 
            // dgvLopHoc
            // 
            this.dgvLopHoc.AllowUserToAddRows = false;
            this.dgvLopHoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLopHoc.BackgroundColor = System.Drawing.Color.White;
            this.dgvLopHoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLopHoc.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLopHoc.ColumnHeadersHeight = 44;
            this.dgvLopHoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLopHoc.EnableHeadersVisualStyles = false;
            this.dgvLopHoc.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvLopHoc.Location = new System.Drawing.Point(12, 8);
            this.dgvLopHoc.Name = "dgvLopHoc";
            this.dgvLopHoc.ReadOnly = true;
            this.dgvLopHoc.RowHeadersVisible = false;
            this.dgvLopHoc.RowTemplate.Height = 42;
            this.dgvLopHoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLopHoc.Size = new System.Drawing.Size(928, 384);
            this.dgvLopHoc.TabIndex = 0;
            // 
            // pnlToolbarLop
            // 
            this.pnlToolbarLop.BackColor = System.Drawing.Color.White;
            this.pnlToolbarLop.Controls.Add(this.btnAddLop);
            this.pnlToolbarLop.Controls.Add(this.txtSearchLop);
            this.pnlToolbarLop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbarLop.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbarLop.Name = "pnlToolbarLop";
            this.pnlToolbarLop.Size = new System.Drawing.Size(952, 58);
            this.pnlToolbarLop.TabIndex = 0;
            // 
            // btnAddLop
            // 
            this.btnAddLop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnAddLop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddLop.FlatAppearance.BorderSize = 0;
            this.btnAddLop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddLop.ForeColor = System.Drawing.Color.White;
            this.btnAddLop.Location = new System.Drawing.Point(284, 13);
            this.btnAddLop.Name = "btnAddLop";
            this.btnAddLop.Size = new System.Drawing.Size(148, 32);
            this.btnAddLop.TabIndex = 1;
            this.btnAddLop.Text = "＋  Thêm lớp học";
            this.btnAddLop.UseVisualStyleBackColor = false;
            // 
            // txtSearchLop
            // 
            this.txtSearchLop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.txtSearchLop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchLop.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearchLop.Location = new System.Drawing.Point(12, 15);
            this.txtSearchLop.Name = "txtSearchLop";
            this.txtSearchLop.PlaceholderText = "🔍  Tìm lớp học...";
            this.txtSearchLop.Size = new System.Drawing.Size(260, 24);
            this.txtSearchLop.TabIndex = 0;
            // 
            // tabMonHoc
            // 
            this.tabMonHoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.tabMonHoc.Controls.Add(this.pnlGridMon);
            this.tabMonHoc.Controls.Add(this.pnlToolbarMon);
            this.tabMonHoc.Location = new System.Drawing.Point(4, 34);
            this.tabMonHoc.Name = "tabMonHoc";
            this.tabMonHoc.Size = new System.Drawing.Size(952, 462);
            this.tabMonHoc.TabIndex = 1;
            this.tabMonHoc.Text = "Môn học";
            // 
            // pnlGridMon
            // 
            this.pnlGridMon.BackColor = System.Drawing.Color.White;
            this.pnlGridMon.Controls.Add(this.dgvMonHoc);
            this.pnlGridMon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridMon.Location = new System.Drawing.Point(0, 58);
            this.pnlGridMon.Name = "pnlGridMon";
            this.pnlGridMon.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this.pnlGridMon.Size = new System.Drawing.Size(952, 404);
            this.pnlGridMon.TabIndex = 1;
            // 
            // dgvMonHoc
            // 
            this.dgvMonHoc.AllowUserToAddRows = false;
            this.dgvMonHoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMonHoc.BackgroundColor = System.Drawing.Color.White;
            this.dgvMonHoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMonHoc.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMonHoc.ColumnHeadersHeight = 44;
            this.dgvMonHoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMonHoc.EnableHeadersVisualStyles = false;
            this.dgvMonHoc.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvMonHoc.Location = new System.Drawing.Point(12, 8);
            this.dgvMonHoc.Name = "dgvMonHoc";
            this.dgvMonHoc.ReadOnly = true;
            this.dgvMonHoc.RowHeadersVisible = false;
            this.dgvMonHoc.RowTemplate.Height = 42;
            this.dgvMonHoc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMonHoc.Size = new System.Drawing.Size(928, 384);
            this.dgvMonHoc.TabIndex = 0;
            // 
            // pnlToolbarMon
            // 
            this.pnlToolbarMon.BackColor = System.Drawing.Color.White;
            this.pnlToolbarMon.Controls.Add(this.btnAddMon);
            this.pnlToolbarMon.Controls.Add(this.txtSearchMon);
            this.pnlToolbarMon.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbarMon.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbarMon.Name = "pnlToolbarMon";
            this.pnlToolbarMon.Size = new System.Drawing.Size(952, 58);
            this.pnlToolbarMon.TabIndex = 0;
            // 
            // btnAddMon
            // 
            this.btnAddMon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnAddMon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddMon.FlatAppearance.BorderSize = 0;
            this.btnAddMon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddMon.ForeColor = System.Drawing.Color.White;
            this.btnAddMon.Location = new System.Drawing.Point(284, 13);
            this.btnAddMon.Name = "btnAddMon";
            this.btnAddMon.Size = new System.Drawing.Size(148, 32);
            this.btnAddMon.TabIndex = 1;
            this.btnAddMon.Text = "＋  Thêm môn học";
            this.btnAddMon.UseVisualStyleBackColor = false;
            // 
            // txtSearchMon
            // 
            this.txtSearchMon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.txtSearchMon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchMon.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearchMon.Location = new System.Drawing.Point(12, 15);
            this.txtSearchMon.Name = "txtSearchMon";
            this.txtSearchMon.PlaceholderText = "🔍  Tìm môn học...";
            this.txtSearchMon.Size = new System.Drawing.Size(260, 24);
            this.txtSearchMon.TabIndex = 0;
            // CatalogManageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Name = "CatalogManageView";
            this.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.Size = new System.Drawing.Size(1000, 588);
            
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabLopHoc.ResumeLayout(false);
            this.pnlGridLop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLopHoc)).EndInit();
            this.pnlToolbarLop.ResumeLayout(false);
            this.pnlToolbarLop.PerformLayout();
            this.tabMonHoc.ResumeLayout(false);
            this.pnlGridMon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonHoc)).EndInit();
            this.pnlToolbarMon.ResumeLayout(false);
            this.pnlToolbarMon.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblDesc;
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
    }
}
