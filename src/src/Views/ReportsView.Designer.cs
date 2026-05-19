namespace src.Views
{
    partial class ReportsView
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
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCharts = new System.Windows.Forms.Panel();
            this.pnlChartLeft = new System.Windows.Forms.Panel();
            this.pnlChartRight = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.pnlCharts.SuspendLayout();
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
            this.lblTitle.Size = new System.Drawing.Size(330, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Reports && Statistics";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSubtitle.Location = new System.Drawing.Point(10, 55);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(310, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Comprehensive analytics and usage insights";
            // 
            // pnlCards
            // 
            this.pnlCards.BackColor = System.Drawing.Color.Transparent;
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Location = new System.Drawing.Point(20, 90);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCards.Size = new System.Drawing.Size(960, 120);
            this.pnlCards.TabIndex = 1;
            this.pnlCards.WrapContents = false;
            // 
            // pnlCharts
            // 
            this.pnlCharts.BackColor = System.Drawing.Color.Transparent;
            this.pnlCharts.Controls.Add(this.pnlChartRight);
            this.pnlCharts.Controls.Add(this.pnlChartLeft);
            this.pnlCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCharts.Location = new System.Drawing.Point(20, 210);
            this.pnlCharts.Name = "pnlCharts";
            this.pnlCharts.Padding = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this.pnlCharts.Size = new System.Drawing.Size(960, 430);
            this.pnlCharts.TabIndex = 2;
            // 
            // pnlChartLeft - Biểu đồ PC Usage Overview
            // 
            this.pnlChartLeft.BackColor = System.Drawing.Color.White;
            this.pnlChartLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartLeft.Location = new System.Drawing.Point(4, 8);
            this.pnlChartLeft.Name = "pnlChartLeft";
            this.pnlChartLeft.Padding = new System.Windows.Forms.Padding(16);
            this.pnlChartLeft.Size = new System.Drawing.Size(600, 418);
            this.pnlChartLeft.TabIndex = 0;
            // 
            // pnlChartRight - Biểu đồ Room Status Distribution
            // 
            this.pnlChartRight.BackColor = System.Drawing.Color.White;
            this.pnlChartRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlChartRight.Location = new System.Drawing.Point(620, 8);
            this.pnlChartRight.Name = "pnlChartRight";
            this.pnlChartRight.Padding = new System.Windows.Forms.Padding(16);
            this.pnlChartRight.Size = new System.Drawing.Size(336, 418);
            this.pnlChartRight.TabIndex = 1;
            // 
            // ReportsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.pnlCharts);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Name = "ReportsView";
            this.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.Size = new System.Drawing.Size(1000, 650);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCharts.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubtitle;
        public System.Windows.Forms.FlowLayoutPanel pnlCards;
        public System.Windows.Forms.Panel pnlCharts;
        public System.Windows.Forms.Panel pnlChartLeft;
        public System.Windows.Forms.Panel pnlChartRight;
    }
}
