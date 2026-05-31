namespace src.Views
{
    partial class QuanLyPhongMayView
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
            txtSearch = new System.Windows.Forms.TextBox();
            cboFilterStatus = new System.Windows.Forms.ComboBox();
            lblSubtitle = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            pnlStats = new System.Windows.Forms.FlowLayoutPanel();
            pnlRoomCards = new System.Windows.Forms.FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.Transparent;
            pnlHeader.Controls.Add(cboFilterStatus);
            pnlHeader.Controls.Add(txtSearch);
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
            btnAdd.Location = new System.Drawing.Point(931, 27);
            btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(160, 51);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "+ Thêm Phòng";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearch.Location = new System.Drawing.Point(661, 37);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo tên hoặc vị trí...";
            txtSearch.Size = new System.Drawing.Size(250, 30);
            txtSearch.TabIndex = 3;
            // 
            // cboFilterStatus
            // 
            cboFilterStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cboFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboFilterStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            cboFilterStatus.Location = new System.Drawing.Point(501, 37);
            cboFilterStatus.Name = "cboFilterStatus";
            cboFilterStatus.Size = new System.Drawing.Size(150, 31);
            cboFilterStatus.TabIndex = 4;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new System.Drawing.Point(11, 73);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(331, 23);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Quản lý thông tin và cơ sở vật chất các phòng thực hành";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(9, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(285, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quản Lý Phòng";
            // 
            // pnlStats
            // 
            pnlStats.BackColor = System.Drawing.Color.Transparent;
            pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            pnlStats.Location = new System.Drawing.Point(23, 120);
            pnlStats.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            pnlStats.Size = new System.Drawing.Size(1097, 120);
            pnlStats.TabIndex = 1;
            pnlStats.WrapContents = false;
            // 
            // pnlRoomCards
            // 
            pnlRoomCards.AutoScroll = true;
            pnlRoomCards.BackColor = System.Drawing.Color.Transparent;
            pnlRoomCards.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRoomCards.Location = new System.Drawing.Point(23, 240);
            pnlRoomCards.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlRoomCards.Name = "pnlRoomCards";
            pnlRoomCards.Padding = new System.Windows.Forms.Padding(2, 8, 2, 8);
            pnlRoomCards.Size = new System.Drawing.Size(1097, 614);
            pnlRoomCards.TabIndex = 2;
            // 
            // QuanLyPhongMayView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(pnlRoomCards);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "QuanLyPhongMayView";
            Padding = new System.Windows.Forms.Padding(23, 13, 23, 13);
            Size = new System.Drawing.Size(1143, 867);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubtitle;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.ComboBox cboFilterStatus;
        public System.Windows.Forms.FlowLayoutPanel pnlStats;
        public System.Windows.Forms.FlowLayoutPanel pnlRoomCards;
    }
}

