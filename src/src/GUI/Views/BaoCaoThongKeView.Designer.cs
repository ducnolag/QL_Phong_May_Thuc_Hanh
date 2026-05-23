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
            cboNam = new System.Windows.Forms.ComboBox();
            lblNam = new System.Windows.Forms.Label();
            cboThang = new System.Windows.Forms.ComboBox();
            lblThang = new System.Windows.Forms.Label();
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
            pnlHeader.Controls.Add(cboNam);
            pnlHeader.Controls.Add(lblNam);
            pnlHeader.Controls.Add(cboThang);
            pnlHeader.Controls.Add(lblThang);
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
            // cboNam
            // 
            cboNam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboNam.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cboNam.Location = new System.Drawing.Point(891, 23);
            cboNam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cboNam.Name = "cboNam";
            cboNam.Size = new System.Drawing.Size(102, 29);
            cboNam.TabIndex = 1;
            // 
            // lblNam
            // 
            lblNam.AutoSize = true;
            lblNam.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblNam.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblNam.Location = new System.Drawing.Point(846, 28);
            lblNam.Name = "lblNam";
            lblNam.Size = new System.Drawing.Size(44, 20);
            lblNam.TabIndex = 2;
            lblNam.Text = "Năm:";
            // 
            // cboThang
            // 
            cboThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboThang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cboThang.Location = new System.Drawing.Point(714, 23);
            cboThang.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cboThang.Name = "cboThang";
            cboThang.Size = new System.Drawing.Size(114, 29);
            cboThang.TabIndex = 3;
            // 
            // lblThang
            // 
            lblThang.AutoSize = true;
            lblThang.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblThang.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblThang.Location = new System.Drawing.Point(663, 28);
            lblThang.Name = "lblThang";
            lblThang.Size = new System.Drawing.Size(53, 20);
            lblThang.TabIndex = 4;
            lblThang.Text = "Tháng:";
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
        public System.Windows.Forms.Label lblThang;
        public System.Windows.Forms.ComboBox cboThang;
        public System.Windows.Forms.Label lblNam;
        public System.Windows.Forms.ComboBox cboNam;
        public System.Windows.Forms.Button btnRefresh;
        public System.Windows.Forms.Panel pnlScroll;
        public System.Windows.Forms.Panel pnlBody;
    }
}

