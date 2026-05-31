namespace src.Views
{
    partial class BaoCaoThongKeView
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
            btnRefresh = new System.Windows.Forms.Button();
            dtpToDate = new System.Windows.Forms.DateTimePicker();
            lblDenNgay = new System.Windows.Forms.Label();
            dtpFromDate = new System.Windows.Forms.DateTimePicker();
            lblTuNgay = new System.Windows.Forms.Label();
            lblDesc = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            pnlScroll = new System.Windows.Forms.Panel();
            pnlBody = new System.Windows.Forms.Panel();
            pnlHeader.SuspendLayout();
            pnlScroll.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.Transparent;
            pnlHeader.Controls.Add(btnRefresh);
            pnlHeader.Controls.Add(dtpToDate);
            pnlHeader.Controls.Add(lblDenNgay);
            pnlHeader.Controls.Add(dtpFromDate);
            pnlHeader.Controls.Add(lblTuNgay);
            pnlHeader.Controls.Add(lblDesc);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1143, 85);
            pnlHeader.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Location = new System.Drawing.Point(1014, 17);
            btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(113, 40);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dtpToDate
            // 
            dtpToDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dtpToDate.Location = new System.Drawing.Point(891, 23);
            dtpToDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new System.Drawing.Size(102, 29);
            dtpToDate.TabIndex = 1;
            dtpToDate.Value = new System.DateTime(2026, 6, 20, 20, 9, 0, 0);
            // 
            // lblDenNgay
            // 
            lblDenNgay.AutoSize = true;
            lblDenNgay.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDenNgay.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblDenNgay.Location = new System.Drawing.Point(810, 27);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.Size = new System.Drawing.Size(75, 20);
            lblDenNgay.TabIndex = 2;
            lblDenNgay.Text = "Đến ngày:";
            // 
            // dtpFromDate
            // 
            dtpFromDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dtpFromDate.Location = new System.Drawing.Point(686, 23);
            dtpFromDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new System.Drawing.Size(114, 29);
            dtpFromDate.TabIndex = 3;
            // 
            // lblTuNgay
            // 
            lblTuNgay.AutoSize = true;
            lblTuNgay.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblTuNgay.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblTuNgay.Location = new System.Drawing.Point(615, 27);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.Size = new System.Drawing.Size(65, 20);
            lblTuNgay.TabIndex = 4;
            lblTuNgay.Text = "Từ ngày:";
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblDesc.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblDesc.Location = new System.Drawing.Point(25, 53);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new System.Drawing.Size(337, 21);
            lblDesc.TabIndex = 5;
            lblDesc.Text = "Xem số liệu tổng quan về lịch, phòng và thiết bị";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(23, 5);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(383, 46);
            lblTitle.TabIndex = 6;
            lblTitle.Text = "📊 Báo cáo - Thống kê";
            // 
            // pnlScroll
            // 
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = System.Drawing.Color.Transparent;
            pnlScroll.Controls.Add(pnlBody);
            pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlScroll.Location = new System.Drawing.Point(0, 85);
            pnlScroll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Size = new System.Drawing.Size(1143, 699);
            pnlScroll.TabIndex = 0;
            // 
            // pnlBody
            // 
            pnlBody.AutoSize = true;
            pnlBody.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlBody.BackColor = System.Drawing.Color.Transparent;
            pnlBody.Location = new System.Drawing.Point(0, 0);
            pnlBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlBody.Name = "pnlBody";
            pnlBody.Size = new System.Drawing.Size(0, 0);
            pnlBody.TabIndex = 0;
            // 
            // BaoCaoThongKeView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(pnlScroll);
            Controls.Add(pnlHeader);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "BaoCaoThongKeView";
            Size = new System.Drawing.Size(1143, 784);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlScroll.ResumeLayout(false);
            pnlScroll.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblDesc;
        public System.Windows.Forms.Label lblTuNgay;
        public System.Windows.Forms.DateTimePicker dtpFromDate;
        public System.Windows.Forms.Label lblDenNgay;
        public System.Windows.Forms.DateTimePicker dtpToDate;
        public System.Windows.Forms.Button btnRefresh;
        public System.Windows.Forms.Panel pnlScroll;
        public System.Windows.Forms.Panel pnlBody;
    }
}

