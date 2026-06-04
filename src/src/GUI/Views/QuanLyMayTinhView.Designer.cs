namespace src.Views
{
    partial class QuanLyMayTinhView
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlHeader = new System.Windows.Forms.Panel();
            lblSubtitle = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            pnlToolbar = new Guna.UI2.WinForms.Guna2Panel();
            cboStatus = new System.Windows.Forms.ComboBox();
            cboRAM = new System.Windows.Forms.ComboBox();
            cboCpuFilter = new System.Windows.Forms.ComboBox();
            cboStorage = new System.Windows.Forms.ComboBox();
            cboMonitor = new System.Windows.Forms.ComboBox();
            cboRoom = new System.Windows.Forms.ComboBox();
            txtSearch = new System.Windows.Forms.TextBox();
            btnAdd = new System.Windows.Forms.Button();
            pnlGrid = new Guna.UI2.WinForms.Guna2Panel();
            dgv = new System.Windows.Forms.DataGridView();
            pnlFilterRow = new System.Windows.Forms.TableLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlFilterRow.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.Transparent;
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(23, 13);
            pnlHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1097, 107);
            pnlHeader.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new System.Drawing.Point(11, 73);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(535, 23);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Quản lý thông tin và tình trạng các máy tính trong phòng thực hành";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(9, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(334, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quản Lý Máy Tính";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = System.Drawing.Color.Transparent;
            pnlToolbar.BorderRadius = 14;
            pnlToolbar.Controls.Add(txtSearch);
            pnlToolbar.Controls.Add(cboRoom);
            pnlToolbar.Controls.Add(btnAdd);
            pnlToolbar.Controls.Add(pnlFilterRow);
            pnlToolbar.CustomizableEdges = customizableEdges1;
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbar.FillColor = System.Drawing.Color.White;
            pnlToolbar.Location = new System.Drawing.Point(23, 120);
            pnlToolbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            pnlToolbar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pnlToolbar.Size = new System.Drawing.Size(1097, 110);
            pnlToolbar.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            txtSearch.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtSearch.Location = new System.Drawing.Point(10, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Tìm máy tính...";
            txtSearch.Size = new System.Drawing.Size(180, 29);
            txtSearch.TabIndex = 0;
            // 
            // cboRoom
            // 
            cboRoom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            cboRoom.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            cboRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cboRoom.Font = new System.Drawing.Font("Segoe UI", 9F);
            cboRoom.Items.AddRange(new object[] { "Tất cả phòng" });
            cboRoom.Location = new System.Drawing.Point(198, 12);
            cboRoom.MaxDropDownItems = 5;
            cboRoom.Name = "cboRoom";
            cboRoom.Size = new System.Drawing.Size(170, 28);
            cboRoom.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAdd.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
            btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Location = new System.Drawing.Point(957, 10);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(130, 36);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "+ Thêm máy";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // pnlFilterRow
            // 
            pnlFilterRow.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlFilterRow.BackColor = System.Drawing.Color.Transparent;
            pnlFilterRow.ColumnCount = 5;
            pnlFilterRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            pnlFilterRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            pnlFilterRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            pnlFilterRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            pnlFilterRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            pnlFilterRow.Controls.Add(cboCpuFilter, 0, 0);
            pnlFilterRow.Controls.Add(cboMonitor, 1, 0);
            pnlFilterRow.Controls.Add(cboStorage, 2, 0);
            pnlFilterRow.Controls.Add(cboRAM, 3, 0);
            pnlFilterRow.Controls.Add(cboStatus, 4, 0);
            pnlFilterRow.Location = new System.Drawing.Point(10, 52);
            pnlFilterRow.Name = "pnlFilterRow";
            pnlFilterRow.RowCount = 1;
            pnlFilterRow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlFilterRow.Size = new System.Drawing.Size(1077, 36);
            pnlFilterRow.TabIndex = 10;
            // 
            // cboCpuFilter
            // 
            cboCpuFilter.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            cboCpuFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            cboCpuFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboCpuFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cboCpuFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            cboCpuFilter.Items.AddRange(new object[] { "Tất cả CPU", "Intel Core i3", "Intel Core i5", "Intel Core i7", "Intel Core i9", "AMD Ryzen 3", "AMD Ryzen 5", "AMD Ryzen 7" });
            cboCpuFilter.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            cboCpuFilter.MaxDropDownItems = 5;
            cboCpuFilter.Name = "cboCpuFilter";
            cboCpuFilter.TabIndex = 6;
            // 
            // cboMonitor
            // 
            cboMonitor.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            cboMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            cboMonitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cboMonitor.Font = new System.Drawing.Font("Segoe UI", 9F);
            cboMonitor.Items.AddRange(new object[] { "Tất cả màn hình", "19\"", "21\"", "24\"", "27\"" });
            cboMonitor.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            cboMonitor.MaxDropDownItems = 5;
            cboMonitor.Name = "cboMonitor";
            cboMonitor.TabIndex = 2;
            // 
            // cboStorage
            // 
            cboStorage.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            cboStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            cboStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboStorage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cboStorage.Font = new System.Drawing.Font("Segoe UI", 9F);
            cboStorage.Items.AddRange(new object[] { "Tất cả lưu trữ", "128 GB", "256 GB", "512 GB", "1024 GB" });
            cboStorage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            cboStorage.MaxDropDownItems = 5;
            cboStorage.Name = "cboStorage";
            cboStorage.TabIndex = 6;
            // 
            // cboRAM
            // 
            cboRAM.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            cboRAM.Dock = System.Windows.Forms.DockStyle.Fill;
            cboRAM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboRAM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cboRAM.Font = new System.Drawing.Font("Segoe UI", 9F);
            cboRAM.Items.AddRange(new object[] { "Tất cả RAM", "4 GB", "8 GB", "16 GB", "32 GB", "64 GB" });
            cboRAM.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            cboRAM.MaxDropDownItems = 5;
            cboRAM.Name = "cboRAM";
            cboRAM.TabIndex = 3;
            // 
            // cboStatus
            // 
            cboStatus.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            cboStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cboStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            cboStatus.Items.AddRange(new object[] { "Tất cả trạng thái", "Tốt", "Hỏng" });
            cboStatus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            cboStatus.MaxDropDownItems = 5;
            cboStatus.Name = "cboStatus";
            cboStatus.TabIndex = 4;
            cboStatus.SelectedIndexChanged += cboStatus_SelectedIndexChanged;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = System.Drawing.Color.Transparent;
            pnlGrid.BorderRadius = 14;
            pnlGrid.Controls.Add(dgv);
            pnlGrid.CustomizableEdges = customizableEdges3;
            pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGrid.FillColor = System.Drawing.Color.White;
            pnlGrid.Location = new System.Drawing.Point(23, 230);
            pnlGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new System.Windows.Forms.Padding(14, 19, 14, 16);
            pnlGrid.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlGrid.Size = new System.Drawing.Size(1097, 624);
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
            dgv.Location = new System.Drawing.Point(14, 19);
            dgv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.RowHeadersWidth = 51;
            dgv.RowTemplate.Height = 44;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgv.Size = new System.Drawing.Size(1069, 589);
            dgv.TabIndex = 0;
            // 
            // QuanLyMayTinhView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(pnlGrid);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "QuanLyMayTinhView";
            Padding = new System.Windows.Forms.Padding(23, 13, 23, 13);
            Size = new System.Drawing.Size(1143, 867);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilterRow.ResumeLayout(false);
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
        public Guna.UI2.WinForms.Guna2Panel pnlToolbar;
        public System.Windows.Forms.TableLayoutPanel pnlFilterRow;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.ComboBox cboRoom;
        public System.Windows.Forms.ComboBox cboCpuFilter;
        public System.Windows.Forms.ComboBox cboMonitor;
        public System.Windows.Forms.ComboBox cboStorage;
        public System.Windows.Forms.ComboBox cboRAM;
        public System.Windows.Forms.ComboBox cboStatus;
        public System.Windows.Forms.Button btnAdd;
        public Guna.UI2.WinForms.Guna2Panel pnlGrid;
        public System.Windows.Forms.DataGridView dgv;
    }
}

