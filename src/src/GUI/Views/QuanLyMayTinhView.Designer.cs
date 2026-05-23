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
            this.pnlHeader         = new System.Windows.Forms.Panel();
            this.lblSubtitle       = new System.Windows.Forms.Label();
            this.lblTitle          = new System.Windows.Forms.Label();
            this.pnlToolbar        = new Guna.UI2.WinForms.Guna2Panel();
            this.cboStatus         = new System.Windows.Forms.ComboBox();
            this.cboRAM            = new System.Windows.Forms.ComboBox();
            this.cboMonitor        = new System.Windows.Forms.ComboBox();
            this.cboStorage        = new System.Windows.Forms.ComboBox();
            this.cboRoom           = new System.Windows.Forms.ComboBox();
            this.txtSearch         = new System.Windows.Forms.TextBox();
            this.pnlGrid           = new Guna.UI2.WinForms.Guna2Panel();
            this.dgv               = new System.Windows.Forms.DataGridView();
            this.pnlHeader.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(20, 10);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(960, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(8, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(350, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản Lý Máy Tính";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSubtitle.Location = new System.Drawing.Point(10, 55);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(380, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Quản lý thông tin và tình trạng các máy tính trong phòng thực hành";
            // 
            // pnlToolbar - Thanh lọc
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.Transparent;
            this.pnlToolbar.FillColor = System.Drawing.Color.White;
            this.pnlToolbar.BorderRadius = 14;
            this.pnlToolbar.Controls.Add(this.cboStatus);
            this.pnlToolbar.Controls.Add(this.cboRAM);
            this.pnlToolbar.Controls.Add(this.cboStorage);
            this.pnlToolbar.Controls.Add(this.cboMonitor);
            this.pnlToolbar.Controls.Add(this.cboRoom);
            this.pnlToolbar.Controls.Add(this.txtSearch);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(20, 90);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.pnlToolbar.Size = new System.Drawing.Size(960, 60);
            this.pnlToolbar.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearch.Location = new System.Drawing.Point(16, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "🔍  Tìm máy tính...";
            this.txtSearch.Size = new System.Drawing.Size(200, 24);
            this.txtSearch.TabIndex = 0;
            // 
            // cboRoom – Lọc theo phòng
            this.cboRoom.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboRoom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboRoom.Items.AddRange(new object[] { "Tất cả phòng" });
            this.cboRoom.Location = new System.Drawing.Point(230, 16);
            this.cboRoom.Name = "cboRoom";
            this.cboRoom.Size = new System.Drawing.Size(140, 23);
            this.cboRoom.TabIndex = 1;
            // cboMonitor
            this.cboMonitor.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboMonitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMonitor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboMonitor.IntegralHeight = false;
            this.cboMonitor.MaxDropDownItems = 5;
            this.cboMonitor.Items.AddRange(new object[] { "Tất cả màn hình", "19\"", "21\"", "24\"", "27\"" });
            this.cboMonitor.Location = new System.Drawing.Point(384, 16);
            this.cboMonitor.Name = "cboMonitor";
            this.cboMonitor.Size = new System.Drawing.Size(120, 23);
            this.cboMonitor.TabIndex = 2;
            // cboRAM
            this.cboRAM.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboRAM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRAM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboRAM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboRAM.IntegralHeight = false;
            this.cboRAM.MaxDropDownItems = 5;
            this.cboRAM.Items.AddRange(new object[] { "Tất cả RAM", "4 GB", "8 GB", "16 GB", "32 GB", "64 GB" });
            this.cboRAM.Location = new System.Drawing.Point(518, 16);
            this.cboRAM.Name = "cboRAM";
            this.cboRAM.Size = new System.Drawing.Size(90, 23);
            this.cboRAM.TabIndex = 3;
            // cboStorage
            this.cboStorage.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStorage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStorage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboStorage.IntegralHeight = false;
            this.cboStorage.MaxDropDownItems = 5;
            this.cboStorage.Items.AddRange(new object[] { "Tất cả lưu trữ", "128 GB", "256 GB", "512 GB", "1024 GB" });
            this.cboStorage.Location = new System.Drawing.Point(618, 16);
            this.cboStorage.Name = "cboStorage";
            this.cboStorage.Size = new System.Drawing.Size(100, 23);
            this.cboStorage.TabIndex = 6;
            // cboStatus
            this.cboStatus.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboStatus.IntegralHeight = false;
            this.cboStatus.MaxDropDownItems = 5;
            this.cboStatus.Items.AddRange(new object[] { "Tất cả trạng thái", "Tốt", "Hỏng" });
            this.cboStatus.Location = new System.Drawing.Point(728, 16);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(110, 23);
            this.cboStatus.TabIndex = 4;
            // btnAdd
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(0, 102, 255);
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(845, 16);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 28);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "+ Thêm máy";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.pnlToolbar.Controls.Add(this.btnAdd);
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.Transparent;
            this.pnlGrid.FillColor = System.Drawing.Color.White;
            this.pnlGrid.BorderRadius = 14;
            this.pnlGrid.Controls.Add(this.dgv);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(20, 150);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(12, 14, 12, 12);
            this.pnlGrid.Size = new System.Drawing.Size(960, 490);
            this.pnlGrid.TabIndex = 2;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.GridColor = System.Drawing.Color.FromArgb(238, 240, 246);
            this.dgv.Location = new System.Drawing.Point(12, 4);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 44;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(936, 474);
            this.dgv.TabIndex = 0;
            // 
            // QuanLyMayTinhView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Name = "QuanLyMayTinhView";
            this.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.Size = new System.Drawing.Size(1000, 650);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubtitle;
        public Guna.UI2.WinForms.Guna2Panel pnlToolbar;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.ComboBox cboRoom;
        public System.Windows.Forms.ComboBox cboMonitor;
        public System.Windows.Forms.ComboBox cboStorage;
        public System.Windows.Forms.ComboBox cboRAM;
        public System.Windows.Forms.ComboBox cboStatus;
        public System.Windows.Forms.Button btnAdd;
        public Guna.UI2.WinForms.Guna2Panel pnlGrid;
        public System.Windows.Forms.DataGridView dgv;
    }
}

