namespace src.Views
{
    partial class UserManageView
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
            btnAdd = new System.Windows.Forms.Button();
            lblSubtitle = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            pnlToolbar = new System.Windows.Forms.Panel();
            txtSearch = new System.Windows.Forms.TextBox();
            lblSearchSub = new System.Windows.Forms.Label();
            lblSearchTitle = new System.Windows.Forms.Label();
            pnlGrid = new System.Windows.Forms.Panel();
            dgv = new System.Windows.Forms.DataGridView();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.Transparent;
            pnlHeader.Controls.Add(btnAdd);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(23, 13);
            pnlHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1097, 107);
            pnlHeader.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAdd.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Location = new System.Drawing.Point(881, 27);
            btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(205, 51);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "+ Thêm Người Dùng";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new System.Drawing.Point(11, 73);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(359, 23);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Quản lý tài khoản, vai trò và phân quyền người dùng";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(9, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(387, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quản Lý Người Dùng";
            lblTitle.Click += lblTitle_Click;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = System.Drawing.Color.White;
            pnlToolbar.Controls.Add(txtSearch);
            pnlToolbar.Controls.Add(lblSearchSub);
            pnlToolbar.Controls.Add(lblSearchTitle);
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbar.Location = new System.Drawing.Point(23, 120);
            pnlToolbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new System.Windows.Forms.Padding(18, 16, 18, 11);
            pnlToolbar.Size = new System.Drawing.Size(1097, 133);
            pnlToolbar.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearch.Location = new System.Drawing.Point(18, 80);
            txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Search users...";
            txtSearch.Size = new System.Drawing.Size(1060, 30);
            txtSearch.TabIndex = 2;
            // 
            // lblSearchSub
            // 
            lblSearchSub.AutoSize = true;
            lblSearchSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblSearchSub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblSearchSub.Location = new System.Drawing.Point(18, 44);
            lblSearchSub.Name = "lblSearchSub";
            lblSearchSub.Size = new System.Drawing.Size(234, 20);
            lblSearchSub.TabIndex = 1;
            lblSearchSub.Text = "Xem và quản lý tất cả người dùng trong hệ thống";
            // 
            // lblSearchTitle
            // 
            lblSearchTitle.AutoSize = true;
            lblSearchTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblSearchTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblSearchTitle.Location = new System.Drawing.Point(18, 13);
            lblSearchTitle.Name = "lblSearchTitle";
            lblSearchTitle.Size = new System.Drawing.Size(179, 28);
            lblSearchTitle.TabIndex = 0;
            lblSearchTitle.Text = "Người Dùng && Tài Khoản";
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = System.Drawing.Color.White;
            pnlGrid.Controls.Add(dgv);
            pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGrid.Location = new System.Drawing.Point(23, 253);
            pnlGrid.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new System.Windows.Forms.Padding(14, 0, 14, 16);
            pnlGrid.Size = new System.Drawing.Size(1097, 601);
            pnlGrid.TabIndex = 2;
            // 
            // dgv
            // 
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = System.Drawing.Color.White;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersHeight = 29;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = System.Drawing.Color.FromArgb(238, 240, 246);
            dgv.Location = new System.Drawing.Point(14, 0);
            dgv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.RowHeadersWidth = 51;
            dgv.RowTemplate.Height = 46;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgv.Size = new System.Drawing.Size(1069, 585);
            dgv.TabIndex = 0;
            // 
            // UserManageView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(pnlGrid);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "UserManageView";
            Padding = new System.Windows.Forms.Padding(23, 13, 23, 13);
            Size = new System.Drawing.Size(1143, 867);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubtitle;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.Panel pnlToolbar;
        public System.Windows.Forms.Label lblSearchTitle;
        public System.Windows.Forms.Label lblSearchSub;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.Panel pnlGrid;
        public System.Windows.Forms.DataGridView dgv;
    }
}
