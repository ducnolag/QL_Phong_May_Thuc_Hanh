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
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCharts = new System.Windows.Forms.Panel();
            this.pnlPie = new System.Windows.Forms.Panel();
            this.pnlBar = new System.Windows.Forms.Panel();
            this.pnlCharts.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCards
            // 
            this.pnlCards.BackColor = System.Drawing.Color.Transparent;
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Location = new System.Drawing.Point(0, 0);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Padding = new System.Windows.Forms.Padding(2);
            this.pnlCards.Size = new System.Drawing.Size(800, 115);
            this.pnlCards.TabIndex = 0;
            this.pnlCards.WrapContents = false;
            // 
            // pnlCharts
            // 
            this.pnlCharts.BackColor = System.Drawing.Color.Transparent;
            this.pnlCharts.Controls.Add(this.pnlBar);
            this.pnlCharts.Controls.Add(this.pnlPie);
            this.pnlCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCharts.Location = new System.Drawing.Point(0, 115);
            this.pnlCharts.Name = "pnlCharts";
            this.pnlCharts.Padding = new System.Windows.Forms.Padding(2, 6, 2, 2);
            this.pnlCharts.Size = new System.Drawing.Size(800, 335);
            this.pnlCharts.TabIndex = 1;
            // 
            // pnlPie
            // 
            this.pnlPie.BackColor = System.Drawing.Color.White;
            this.pnlPie.Location = new System.Drawing.Point(5, 5);
            this.pnlPie.Name = "pnlPie";
            this.pnlPie.Size = new System.Drawing.Size(380, 320);
            this.pnlPie.TabIndex = 0;
            // 
            // pnlBar
            // 
            this.pnlBar.BackColor = System.Drawing.Color.White;
            this.pnlBar.Location = new System.Drawing.Point(400, 5);
            this.pnlBar.Name = "pnlBar";
            this.pnlBar.Size = new System.Drawing.Size(380, 320);
            this.pnlBar.TabIndex = 1;
            // 
            // ReportsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.pnlCharts);
            this.Controls.Add(this.pnlCards);
            this.DoubleBuffered = true;
            this.Name = "ReportsView";
            this.Size = new System.Drawing.Size(800, 450);
            this.pnlCharts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel pnlCards;
        public System.Windows.Forms.Panel pnlCharts;
        public System.Windows.Forms.Panel pnlPie;
        public System.Windows.Forms.Panel pnlBar;
    }
}
