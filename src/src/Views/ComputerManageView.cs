using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public class ComputerManageView : UserControl
    {
        private DataGridView dgv;
        private TextBox txtSearch;
        private ComboBox cboRAM;
        private ComboBox cboRoom;

        public ComputerManageView()
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
                Dock = DockStyle.Top, Height = 55, BackColor = Color.White
            };
            toolbar.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(toolbar.ClientRectangle, 10))
                    toolbar.Region = new Region(p);
            };
            this.Controls.Add(toolbar);

            txtSearch = new TextBox
            {
                Size = new Size(190, 28), Location = new Point(14, 14),
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 247, 252),
                PlaceholderText = "🔍 Tìm máy tính..."
            };
            txtSearch.TextChanged += (s, e) => FilterRows();
            toolbar.Controls.Add(txtSearch);

            cboRAM = new ComboBox
            {
                Size = new Size(110, 28), Location = new Point(214, 14),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(245, 247, 252)
            };
            cboRAM.Items.AddRange(new object[] { "Tất cả RAM", "4 GB", "8 GB", "16 GB", "32 GB" });
            cboRAM.SelectedIndex = 0;
            cboRAM.SelectedIndexChanged += (s, e) => FilterRows();
            toolbar.Controls.Add(cboRAM);

            cboRoom = new ComboBox
            {
                Size = new Size(130, 28), Location = new Point(334, 14),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(245, 247, 252)
            };
            cboRoom.Items.Add("Tất cả phòng");
            // Try load rooms from DB
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT TenPhong FROM PHONG_MAY ORDER BY TenPhong");
                foreach (DataRow r in dt.Rows)
                    cboRoom.Items.Add(r["TenPhong"].ToString());
            }
            catch { cboRoom.Items.AddRange(new object[] { "Phòng A01", "Phòng A02", "Phòng B01" }); }
            cboRoom.SelectedIndex = 0;
            cboRoom.SelectedIndexChanged += (s, e) => FilterRows();
            toolbar.Controls.Add(cboRoom);

            var btnAdd = RoomManageView.MakeButton("➕  Thêm Máy", ThemeColors.AccentGreen);
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

            dgv = RoomManageView.CreateStyledGrid();
            dgv.Columns.Add("TenMay", "Tên Máy");
            dgv.Columns.Add("Phong", "Phòng");
            dgv.Columns.Add("CPU", "CPU");
            dgv.Columns.Add("RAM", "RAM");
            dgv.Columns.Add("LuuTru", "Lưu Trữ");
            dgv.Columns.Add("ManHinh", "Màn Hình");
            dgv.Columns.Add("TrangThai", "Trạng Thái");
            dgv.CellFormatting += (s, e) =>
            {
                if (dgv.Columns[e.ColumnIndex].Name == "TrangThai")
                    RoomManageView.ColorStatusCell(e, "TrangThai");
            };
            pnlGrid.Controls.Add(dgv);

            LoadData();
        }

        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT m.TenMay, p.TenPhong, m.CPU, m.RAM, m.DungLuongLuuTru, 
                      m.KichThuocManHinh, t.TenTrangThaiMay
                      FROM MAY_TINH m 
                      JOIN PHONG_MAY p ON m.MaPhong = p.MaPhong
                      JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                      ORDER BY p.TenPhong, m.TenMay");
                foreach (DataRow r in dt.Rows)
                {
                    string status = r["TenTrangThaiMay"].ToString();
                    string icon = status.Contains("Tốt") ? "🟢" : status.Contains("Bảo") ? "🟠" : "🔴";
                    dgv.Rows.Add(r["TenMay"], r["TenPhong"], r["CPU"],
                        r["RAM"] + " GB", r["DungLuongLuuTru"] + " GB",
                        r["KichThuocManHinh"] + "\"", icon + " " + status);
                }
            }
            catch
            {
                dgv.Rows.Add("A01-01", "Phòng A01", "Intel i5-12400", "8 GB", "256 GB SSD", "24\"", "🟢 Tốt");
                dgv.Rows.Add("A01-02", "Phòng A01", "Intel i5-12400", "8 GB", "256 GB SSD", "24\"", "🟢 Tốt");
                dgv.Rows.Add("A01-03", "Phòng A01", "Intel i5-12400", "16 GB", "512 GB SSD", "27\"", "🟠 Bảo trì");
                dgv.Rows.Add("A02-01", "Phòng A02", "Intel i7-12700", "16 GB", "512 GB SSD", "27\"", "🟢 Tốt");
                dgv.Rows.Add("B01-01", "Phòng B01", "AMD Ryzen 5", "8 GB", "256 GB SSD", "24\"", "🔴 Hỏng");
            }
        }

        private void FilterRows()
        {
            string kw = txtSearch.Text.Trim().ToLower();
            string ramF = cboRAM.SelectedItem?.ToString() ?? "";
            string roomF = cboRoom.SelectedItem?.ToString() ?? "";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                bool show = true;

                if (!string.IsNullOrEmpty(kw))
                {
                    bool match = false;
                    foreach (DataGridViewCell c in row.Cells)
                        if (c.Value != null && c.Value.ToString().ToLower().Contains(kw)) { match = true; break; }
                    if (!match) show = false;
                }
                if (show && ramF != "Tất cả RAM" && !string.IsNullOrEmpty(ramF))
                {
                    string v = row.Cells["RAM"].Value?.ToString() ?? "";
                    if (!v.StartsWith(ramF.Replace(" GB", ""))) show = false;
                }
                if (show && roomF != "Tất cả phòng" && !string.IsNullOrEmpty(roomF))
                {
                    string v = row.Cells["Phong"].Value?.ToString() ?? "";
                    if (!v.Contains(roomF)) show = false;
                }
                row.Visible = show;
            }
        }
    }
}
