namespace src.Views
{
    partial class RoomManageView
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
            this.pnlRoomCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader - Tiêu đề
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
            this.lblTitle.Size = new System.Drawing.Size(354, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "PC Room Management";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSubtitle.Location = new System.Drawing.Point(10, 55);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(305, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Manage computer lab rooms and facilities";
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
            this.btnAdd.Location = new System.Drawing.Point(815, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(140, 38);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "+ Add Room";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // pnlStats - Summary cards
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
            // pnlRoomCards - Grid phòng dạng card
            // 
            this.pnlRoomCards.AutoScroll = true;
            this.pnlRoomCards.BackColor = System.Drawing.Color.Transparent;
            this.pnlRoomCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRoomCards.Location = new System.Drawing.Point(20, 180);
            this.pnlRoomCards.Name = "pnlRoomCards";
            this.pnlRoomCards.Padding = new System.Windows.Forms.Padding(2, 6, 2, 6);
            this.pnlRoomCards.Size = new System.Drawing.Size(960, 460);
            this.pnlRoomCards.TabIndex = 2;
            // 
            // RoomManageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Controls.Add(this.pnlRoomCards);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Name = "RoomManageView";
            this.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.Size = new System.Drawing.Size(1000, 650);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblSubtitle;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.FlowLayoutPanel pnlStats;
        public System.Windows.Forms.FlowLayoutPanel pnlRoomCards;
    }
}
