using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public partial class ComputerManageView : UserControl
    {
        public ComputerManageView()
        {
            InitializeComponent();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            // Removed GDI+ Paint events to ensure full Designer compatibility
            SetupGridStyles();
            LoadRooms();
            LoadData();

            cboRAM.SelectedIndex = 0;
            cboRoom.SelectedIndex = 0;
        }

        private void SetupGridStyles()
        {
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

            dgv.Font = new Font("Segoe UI", 9.5F);
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 247, 252),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(245, 247, 252),
                SelectionForeColor = ThemeColors.TextSecondary,
                Padding = new Padding(6),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                SelectionBackColor = Color.FromArgb(228, 237, 255),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(6)
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 251, 254),
                SelectionBackColor = Color.FromArgb(228, 237, 255),
                SelectionForeColor = ThemeColors.TextPrimary
            };
        }

        private void LoadRooms()
        {
            cboRoom.Items.Clear();
            cboRoom.Items.Add("Tất cả phòng");
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT TenPhong FROM PHONG_MAY ORDER BY TenPhong");
                foreach (DataRow r in dt.Rows)
                    cboRoom.Items.Add(r["TenPhong"].ToString());
            }
            catch { cboRoom.Items.AddRange(new object[] { "Phòng A01", "Phòng A02", "Phòng B01" }); }
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

        private void Filter_Changed(object sender, EventArgs e)
        {
            FilterRows();
        }

        private void FilterRows()
        {
            string kw = txtSearch.Text?.Trim().ToLower() ?? "";
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
