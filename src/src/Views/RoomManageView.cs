using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public class RoomManageView : UserControl
    {
        private DataGridView dgv;
        private TextBox txtSearch;

        public RoomManageView()
        {
            this.BackColor = ThemeColors.BackgroundMain;
            this.DoubleBuffered = true;
            BuildUI();
        }

        private void BuildUI()
        {
            // ═══ TOOLBAR ═══
            var toolbar = new Panel
            {
                Dock = DockStyle.Top, Height = 55,
                BackColor = Color.White
            };
            toolbar.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(toolbar.ClientRectangle, 10))
                    toolbar.Region = new Region(p);
            };
            this.Controls.Add(toolbar);

            txtSearch = new TextBox
            {
                Size = new Size(260, 28), Location = new Point(14, 14),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 247, 252),
                PlaceholderText = "🔍 Tìm kiếm phòng máy..."
            };
            txtSearch.TextChanged += (s, e) => FilterRows();
            toolbar.Controls.Add(txtSearch);

            var btnAdd = MakeButton("➕  Thêm Phòng", ThemeColors.PrimaryBlue);
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            toolbar.Controls.Add(btnAdd);
            toolbar.Resize += (s, e) => btnAdd.Location = new Point(toolbar.Width - 155, 10);
            btnAdd.Location = new Point(toolbar.Width - 155, 10);

            // ═══ GRID ═══
            var pnlGrid = new Panel
            {
                Dock = DockStyle.Fill, BackColor = Color.White,
                Padding = new Padding(12), Margin = new Padding(0, 8, 0, 0)
            };
            pnlGrid.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(pnlGrid.ClientRectangle, 10))
                    pnlGrid.Region = new Region(p);
            };
            this.Controls.Add(pnlGrid);
            pnlGrid.BringToFront();

            dgv = CreateStyledGrid();
            dgv.Columns.Add("MaPhong", "Mã Phòng");
            dgv.Columns.Add("TenPhong", "Tên Phòng");
            dgv.Columns.Add("ViTri", "Vị Trí");
            dgv.Columns.Add("SucChua", "Sức Chứa");
            dgv.Columns.Add("TrangThai", "Trạng Thái");
            dgv.Columns["MaPhong"].Width = 80;
            dgv.Columns["SucChua"].Width = 90;
            dgv.Columns["TrangThai"].Width = 130;
            dgv.CellFormatting += (s, e) => ColorStatusCell(e, "TrangThai");
            pnlGrid.Controls.Add(dgv);

            LoadData();
        }

        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT p.MaPhong, p.TenPhong, p.ViTri, p.SucChua, t.TenTrangThaiPhong
                      FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong = t.MaTTPhong
                      ORDER BY p.TenPhong");
                foreach (DataRow r in dt.Rows)
                {
                    string status = r["TenTrangThaiPhong"].ToString();
                    string icon = status.Contains("Hoạt") ? "🟢" : status.Contains("Bảo") ? "🟠" : "🔴";
                    dgv.Rows.Add(r["MaPhong"], r["TenPhong"], r["ViTri"], r["SucChua"], icon + " " + status);
                }
            }
            catch
            {
                // Demo data fallback
                dgv.Rows.Add("P01", "Phòng A01", "Tầng 1 - Tòa A", 40, "🟢 Hoạt động");
                dgv.Rows.Add("P02", "Phòng A02", "Tầng 1 - Tòa A", 35, "🟢 Hoạt động");
                dgv.Rows.Add("P03", "Phòng A03", "Tầng 2 - Tòa A", 40, "🟠 Bảo trì");
                dgv.Rows.Add("P04", "Phòng B01", "Tầng 1 - Tòa B", 50, "🟢 Hoạt động");
                dgv.Rows.Add("P05", "Phòng B02", "Tầng 2 - Tòa B", 45, "🟢 Hoạt động");
                dgv.Rows.Add("P06", "Phòng C01", "Tầng 1 - Tòa C", 30, "🔴 Đóng cửa");
            }
        }

        private void FilterRows()
        {
            string kw = txtSearch.Text.Trim().ToLower();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                bool show = string.IsNullOrEmpty(kw);
                if (!show)
                    foreach (DataGridViewCell c in row.Cells)
                        if (c.Value != null && c.Value.ToString().ToLower().Contains(kw)) { show = true; break; }
                row.Visible = show;
            }
        }

        // ═══ shared helpers ═══

        public static DataGridView CreateStyledGrid()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(238, 240, 246),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5F),
                EnableHeadersVisualStyles = false
            };
            g.RowTemplate.Height = 42;
            g.ColumnHeadersHeight = 44;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 247, 252),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(245, 247, 252),
                SelectionForeColor = ThemeColors.TextSecondary,
                Padding = new Padding(6),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            g.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                SelectionBackColor = Color.FromArgb(228, 237, 255),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(6)
            };
            g.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 251, 254),
                SelectionBackColor = Color.FromArgb(228, 237, 255),
                SelectionForeColor = ThemeColors.TextPrimary
            };
            return g;
        }

        public static void ColorStatusCell(DataGridViewCellFormattingEventArgs e, string colName)
        {
            var dgv = e.CellStyle; // just for accessing
            // We check column index dynamically
            if (e.Value == null) return;
            string v = e.Value.ToString();
            if (v.Contains("Hoạt") || v.Contains("Tốt") || v.Contains("Đã xếp"))
                e.CellStyle.ForeColor = ThemeColors.AccentGreen;
            else if (v.Contains("Bảo") || v.Contains("Chờ"))
                e.CellStyle.ForeColor = ThemeColors.AccentOrange;
            else if (v.Contains("Đóng") || v.Contains("Hỏng") || v.Contains("Vô hiệu"))
                e.CellStyle.ForeColor = ThemeColors.AccentRed;
            else if (v.Contains("Admin"))
                e.CellStyle.ForeColor = ThemeColors.AccentPurple;
        }

        public static Button MakeButton(string text, Color bg)
        {
            var btn = new Button
            {
                Text = text, Size = new Size(140, 34),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = bg, ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(btn.ClientRectangle, 8))
                    btn.Region = new Region(p);
            };
            return btn;
        }
    }
}
