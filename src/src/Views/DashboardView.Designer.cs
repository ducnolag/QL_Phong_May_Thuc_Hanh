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
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.pnlActivity = new System.Windows.Forms.Panel();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCards
            // 
            this.pnlCards.BackColor = System.Drawing.Color.Transparent;
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Location = new System.Drawing.Point(5, 5);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Padding = new System.Windows.Forms.Padding(2);
            this.pnlCards.Size = new System.Drawing.Size(790, 130);
            this.pnlCards.TabIndex = 0;
            this.pnlCards.WrapContents = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.pnlActivity);
            this.pnlBottom.Controls.Add(this.pnlChart);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(5, 135);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(2, 8, 2, 2);
            this.pnlBottom.Size = new System.Drawing.Size(790, 310);
            this.pnlBottom.TabIndex = 1;
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.White;
            this.pnlChart.Location = new System.Drawing.Point(5, 5);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(420, 290);
            this.pnlChart.TabIndex = 0;
            // 
            // pnlActivity
            // 
            this.pnlActivity.BackColor = System.Drawing.Color.White;
            this.pnlActivity.Location = new System.Drawing.Point(440, 5);
            this.pnlActivity.Name = "pnlActivity";
            this.pnlActivity.Size = new System.Drawing.Size(340, 290);
            this.pnlActivity.TabIndex = 1;
            // 
            // DashboardView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlCards);
            this.DoubleBuffered = true;
            this.Name = "DashboardView";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(800, 450);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel pnlCards;
        public System.Windows.Forms.Panel pnlBottom;
        public System.Windows.Forms.Panel pnlChart;
        public System.Windows.Forms.Panel pnlActivity;
    }
}
