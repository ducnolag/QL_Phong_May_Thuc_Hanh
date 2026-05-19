namespace src.Views
{
    partial class ScheduleManageView
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlStats = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlScheduleList = new System.Windows.Forms.FlowLayoutPanel();
            this.lblListTitle = new System.Windows.Forms.Label();
            this.lblListSub = new System.Windows.Forms.Label();
            this.pnlListHeader = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.pnlListHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.btnAdd);
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
            this.lblTitle.Size = new System.Drawing.Size(278, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Practice Calendar";
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
            this.lblSubtitle.Text = "Manage class schedules and room assignments";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(790, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(160, 38);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "+ Create Schedule";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // pnlStats
            // 
            this.pnlStats.BackColor = System.Drawing.Color.Transparent;
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Location = new System.Drawing.Point(20, 90);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlStats.Size = new System.Drawing.Size(960, 90);
            this.pnlStats.TabIndex = 1;
            this.pnlStats.WrapContents = false;
            // 
            // pnlListHeader
            // 
            this.pnlListHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlListHeader.Controls.Add(this.lblListSub);
            this.pnlListHeader.Controls.Add(this.lblListTitle);
            this.pnlListHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlListHeader.Location = new System.Drawing.Point(20, 180);
            this.pnlListHeader.Name = "pnlListHeader";
            this.pnlListHeader.Size = new System.Drawing.Size(960, 50);
            this.pnlListHeader.TabIndex = 2;
            // 
            // lblListTitle
            // 
            this.lblListTitle.AutoSize = true;
            this.lblListTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblListTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblListTitle.Location = new System.Drawing.Point(6, 4);
            this.lblListTitle.Name = "lblListTitle";
            this.lblListTitle.Size = new System.Drawing.Size(124, 25);
            this.lblListTitle.TabIndex = 0;
            this.lblListTitle.Text = "All Schedules";
            // 
            // lblListSub
            // 
            this.lblListSub.AutoSize = true;
            this.lblListSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblListSub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblListSub.Location = new System.Drawing.Point(8, 30);
            this.lblListSub.Name = "lblListSub";
            this.lblListSub.Size = new System.Drawing.Size(208, 15);
            this.lblListSub.TabIndex = 1;
            this.lblListSub.Text = "View and manage practice schedules";
            // 
            // pnlScheduleList
            // 
            this.pnlScheduleList.AutoScroll = true;
            this.pnlScheduleList.BackColor = System.Drawing.Color.Transparent;
            this.pnlScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScheduleList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlScheduleList.Location = new System.Drawing.Point(20, 230);
            this.pnlScheduleList.Name = "pnlScheduleList";
            this.pnlScheduleList.Padding = new System.Windows.Forms.Padding(2);
            this.pnlScheduleList.Size = new System.Drawing.Size(960, 410);
            this.pnlScheduleList.TabIndex = 3;
            this.pnlScheduleList.WrapContents = false;
            // 
            // ScheduleManageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.pnlScheduleList);
            this.Controls.Add(this.pnlListHeader);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Name = "ScheduleManageView";
            this.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.Size = new System.Drawing.Size(1000, 650);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlListHeader.ResumeLayout(false);
            this.pnlListHeader.PerformLayout();
            this.ResumeLayout(false);
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
