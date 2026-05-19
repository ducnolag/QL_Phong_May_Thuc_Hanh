namespace src.Views
{
    partial class DashboardView
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
            lblSubtitle = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            pnlBottom = new System.Windows.Forms.Panel();
            pnlRoomStatus = new System.Windows.Forms.Panel();
            pnlChart = new System.Windows.Forms.Panel();
            pnlHeader.SuspendLayout();
            pnlBottom.SuspendLayout();
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
            pnlHeader.Padding = new System.Windows.Forms.Padding(11, 21, 11, 0);
            pnlHeader.Size = new System.Drawing.Size(1097, 107);
            pnlHeader.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new System.Drawing.Point(11, 71);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            lblSubtitle.Size = new System.Drawing.Size(383, 26);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Welcome back! Here's an overview of the system.";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lblTitle.Location = new System.Drawing.Point(11, 21);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(211, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Dashboard";
            // 
            // pnlCards
            // 
            pnlCards.BackColor = System.Drawing.Color.Transparent;
            pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCards.Location = new System.Drawing.Point(23, 120);
            pnlCards.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlCards.Name = "pnlCards";
            pnlCards.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            pnlCards.Size = new System.Drawing.Size(1097, 187);
            pnlCards.TabIndex = 1;
            pnlCards.WrapContents = false;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.Color.Transparent;
            pnlBottom.Controls.Add(pnlRoomStatus);
            pnlBottom.Controls.Add(pnlChart);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBottom.Location = new System.Drawing.Point(23, 307);
            pnlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Padding = new System.Windows.Forms.Padding(7, 11, 7, 8);
            pnlBottom.Size = new System.Drawing.Size(1097, 547);
            pnlBottom.TabIndex = 2;
            // 
            // pnlRoomStatus
            // 
            pnlRoomStatus.BackColor = System.Drawing.Color.White;
            pnlRoomStatus.Dock = System.Windows.Forms.DockStyle.Right;
            pnlRoomStatus.Location = new System.Drawing.Point(724, 11);
            pnlRoomStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pnlRoomStatus.Name = "pnlRoomStatus";
            pnlRoomStatus.Padding = new System.Windows.Forms.Padding(18, 21, 18, 21);
            pnlRoomStatus.Size = new System.Drawing.Size(366, 528);
            pnlRoomStatus.TabIndex = 1;
            // 
            // pnlChart
            // 
            pnlChart.BackColor = System.Drawing.Color.White;
            pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlChart.Location = new System.Drawing.Point(7, 11);
            pnlChart.Margin = new System.Windows.Forms.Padding(7, 8, 9, 8);
            pnlChart.Name = "pnlChart";
            pnlChart.Padding = new System.Windows.Forms.Padding(18, 21, 18, 21);
            pnlChart.Size = new System.Drawing.Size(1083, 528);
            pnlChart.TabIndex = 0;
            // 
            // DashboardView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            Controls.Add(pnlBottom);
            Controls.Add(pnlCards);
            Controls.Add(pnlHeader);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "DashboardView";
            Padding = new System.Windows.Forms.Padding(23, 13, 23, 13);
            Size = new System.Drawing.Size(1143, 867);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubtitle;
        public System.Windows.Forms.FlowLayoutPanel pnlCards;
        public System.Windows.Forms.Panel pnlBottom;
        public System.Windows.Forms.Panel pnlChart;
        public System.Windows.Forms.Panel pnlRoomStatus;
    }
}
