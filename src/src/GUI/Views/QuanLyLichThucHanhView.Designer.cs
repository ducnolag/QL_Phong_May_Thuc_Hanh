namespace src.Views
{
    partial class QuanLyLichThucHanhView
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
            pnlStats = new System.Windows.Forms.FlowLayoutPanel();
            pnlScheduleList = new System.Windows.Forms.FlowLayoutPanel();
            lblListTitle = new System.Windows.Forms.Label();
            lblListSub = new System.Windows.Forms.Label();
            pnlListHeader = new System.Windows.Forms.Panel();
            pnlHeader.SuspendLayout();
            pnlListHeader.SuspendLayout();
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
            btnAdd.Location = new System.Drawing.Point(903, 27);
            btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(183, 51);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "+ Thêm Lịch";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new System.Drawing.Point(11, 73);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(370, 23);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Quản lý lịch phân công và thời khóa biểu thực hành";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(9, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(441, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quản Lý Lịch Thực Hành";
            lblTitle.Click += lblTitle_Click;
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
            // pnlScheduleList
            // 
            pnlScheduleList.AutoScroll = true;
            pnlScheduleList.BackColor = System.Drawing.Color.Transparent;
            pnlScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlScheduleList.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlScheduleList.Location = new System.Drawing.Point(23, 307);
            pnlScheduleList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlScheduleList.Name = "pnlScheduleList";
            pnlScheduleList.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            pnlScheduleList.Size = new System.Drawing.Size(1097, 547);
            pnlScheduleList.TabIndex = 3;
            pnlScheduleList.WrapContents = true;
            // 
            // lblListTitle
            // 
            lblListTitle.AutoSize = true;
            lblListTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblListTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblListTitle.Location = new System.Drawing.Point(7, 5);
            lblListTitle.Name = "lblListTitle";
            lblListTitle.Size = new System.Drawing.Size(169, 30);
            lblListTitle.TabIndex = 0;
            lblListTitle.Text = "Tất Cả Các Lịch Thực Hành";
            // 
            // lblListSub
            // 
            lblListSub.AutoSize = true;
            lblListSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblListSub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblListSub.Location = new System.Drawing.Point(9, 40);
            lblListSub.Name = "lblListSub";
            lblListSub.Size = new System.Drawing.Size(253, 20);
            lblListSub.TabIndex = 1;
            lblListSub.Text = "Xem và quản lý danh sách lịch thực hành";
            // 
            // pnlListHeader
            // 
            pnlListHeader.BackColor = System.Drawing.Color.Transparent;
            pnlListHeader.Controls.Add(lblListSub);
            pnlListHeader.Controls.Add(lblListTitle);
            pnlListHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlListHeader.Location = new System.Drawing.Point(23, 240);
            pnlListHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlListHeader.Name = "pnlListHeader";
            pnlListHeader.Size = new System.Drawing.Size(1097, 67);
            pnlListHeader.TabIndex = 2;
            // 
            // QuanLyLichThucHanhView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(pnlScheduleList);
            Controls.Add(pnlListHeader);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "QuanLyLichThucHanhView";
            Padding = new System.Windows.Forms.Padding(23, 13, 23, 13);
            Size = new System.Drawing.Size(1143, 867);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlListHeader.ResumeLayout(false);
            pnlListHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubtitle;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.FlowLayoutPanel pnlStats;
        public System.Windows.Forms.Panel pnlListHeader;
        public System.Windows.Forms.Label lblListTitle;
        public System.Windows.Forms.Label lblListSub;
        public System.Windows.Forms.FlowLayoutPanel pnlScheduleList;
    }
}

