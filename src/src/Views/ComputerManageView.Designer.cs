namespace src.Views
{
    partial class ComputerManageView
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
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.cboRAM = new System.Windows.Forms.ComboBox();
            this.cboCPU = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
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
            this.lblTitle.Text = "Computer Management";
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
            this.lblSubtitle.Text = "Manage and monitor all computers across PC rooms";
            // 
            // pnlToolbar - Thanh lọc
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.White;
            this.pnlToolbar.Controls.Add(this.btnClear);
            this.pnlToolbar.Controls.Add(this.cboStatus);
            this.pnlToolbar.Controls.Add(this.cboRAM);
            this.pnlToolbar.Controls.Add(this.cboCPU);
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
            this.txtSearch.PlaceholderText = "🔍  Search computers...";
            this.txtSearch.Size = new System.Drawing.Size(200, 24);
            this.txtSearch.TabIndex = 0;
            // 
            // cboCPU
            // 
            this.cboCPU.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboCPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCPU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCPU.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboCPU.Items.AddRange(new object[] { "All CPUs", "Intel i5", "Intel i7", "Intel i9", "AMD Ryzen 5", "AMD Ryzen 7" });
            this.cboCPU.Location = new System.Drawing.Point(230, 16);
            this.cboCPU.Name = "cboCPU";
            this.cboCPU.Size = new System.Drawing.Size(130, 23);
            this.cboCPU.TabIndex = 1;
            // 
            // cboRAM
            // 
            this.cboRAM.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboRAM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRAM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboRAM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboRAM.Items.AddRange(new object[] { "All RAM", "4 GB", "8 GB", "16 GB", "32 GB" });
            this.cboRAM.Location = new System.Drawing.Point(374, 16);
            this.cboRAM.Name = "cboRAM";
            this.cboRAM.Size = new System.Drawing.Size(110, 23);
            this.cboRAM.TabIndex = 2;
            // 
            // cboStatus
            // 
            this.cboStatus.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboStatus.Items.AddRange(new object[] { "All Status", "Tốt", "Bảo trì", "Hỏng" });
            this.cboStatus.Location = new System.Drawing.Point(498, 16);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(110, 23);
            this.cboStatus.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnClear.Location = new System.Drawing.Point(870, 14);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 28);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "🗑 Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.Controls.Add(this.dgv);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(20, 150);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(12, 4, 12, 12);
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
            // ComputerManageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Name = "ComputerManageView";
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
        public System.Windows.Forms.Panel pnlToolbar;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.ComboBox cboCPU;
        public System.Windows.Forms.ComboBox cboRAM;
        public System.Windows.Forms.ComboBox cboStatus;
        public System.Windows.Forms.Button btnClear;
        public System.Windows.Forms.Panel pnlGrid;
        public System.Windows.Forms.DataGridView dgv;
    }
}
