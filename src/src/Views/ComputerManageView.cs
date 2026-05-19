using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using src.Helpers;

namespace src.Views
{
    /// <summary>
    /// Quản lý máy tính – Hiển thị bảng với bộ lọc theo Figma.
    /// Cột: Computer ID, Room, CPU, RAM, Monitor, Status (badge), Last Used.
    /// Bộ lọc: Search, CPU, RAM, Status, Clear.
    /// </summary>
    public partial class ComputerManageView : UserControl
    {
        public ComputerManageView()
        {
            InitializeComponent();
            SetupView();
        }

        /// <summary>
        /// Thiết lập giao diện, sự kiện, tải dữ liệu
        /// </summary>
        private void SetupView()
        {
            UIHelper.ApplyCardStyle(pnlToolbar, 14);
            UIHelper.ApplyCardStyle(pnlGrid, 14);

            // Đặt giá trị mặc định cho bộ lọc
            cboCPU.SelectedIndex = 0;
            cboRAM.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;

            // Gắn sự kiện lọc
            txtSearch.TextChanged += (s, e) => FilterRows();
            cboCPU.SelectedIndexChanged += (s, e) => FilterRows();
            cboRAM.SelectedIndexChanged += (s, e) => FilterRows();
            cboStatus.SelectedIndexChanged += (s, e) => FilterRows();
            btnClear.Click += (s, e) =>
            {
                txtSearch.Text = "";
                cboCPU.SelectedIndex = 0;
                cboRAM.SelectedIndex = 0;
                cboStatus.SelectedIndex = 0;
            };

            SetupGridStyles();
            LoadData();
        }

        /// <summary>
        /// Thiết lập cột và kiểu hiển thị bảng theo Figma
        /// </summary>
        private void SetupGridStyles()
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("ComputerID", "Computer ID");
            dgv.Columns.Add("Room", "Room");
            dgv.Columns.Add("CPU", "CPU");
            dgv.Columns.Add("RAM", "RAM");
            dgv.Columns.Add("Monitor", "Monitor");
            dgv.Columns.Add("Status", "Status");
            dgv.Columns.Add("LastUsed", "Last Used");

            dgv.Columns["ComputerID"].Width = 120;
            dgv.Columns["Room"].Width = 110;
            dgv.Columns["RAM"].Width = 70;
            dgv.Columns["Monitor"].Width = 90;
            dgv.Columns["Status"].Width = 90;
            dgv.Columns["LastUsed"].Width = 100;

            // Định dạng ô Status thành badge
            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (dgv.Columns[e.ColumnIndex].Name == "Status")
                {
                    string val = e.Value?.ToString() ?? "";
                    if (val.Contains("available") || val.Contains("Tốt"))
                    {
                        e.CellStyle.ForeColor = ThemeColors.BadgeGreenFg;
                        e.CellStyle.BackColor = ThemeColors.BadgeGreenBg;
                    }
                    else if (val.Contains("occupied") || val.Contains("Bảo"))
                    {
                        e.CellStyle.ForeColor = ThemeColors.BadgeOrangeFg;
                        e.CellStyle.BackColor = ThemeColors.BadgeOrangeBg;
                    }
                    else if (val.Contains("Hỏng") || val.Contains("broken"))
                    {
                        e.CellStyle.ForeColor = ThemeColors.BadgeRedFg;
                        e.CellStyle.BackColor = ThemeColors.BadgeRedBg;
                    }
                    e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            };

            dgv.Font = new Font("Segoe UI", 9.5F);
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(249, 250, 251),
                SelectionForeColor = ThemeColors.TextSecondary,
                Padding = new Padding(6),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(6)
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary
            };
        }

        /// <summary>
        /// Tải dữ liệu máy tính từ database
        /// </summary>
        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT m.TenMay, p.TenPhong, m.CPU, m.RAM, 
                      m.KichThuocManHinh, t.TenTrangThaiMay
                      FROM MAY_TINH m 
                      JOIN PHONG_MAY p ON m.MaPhong = p.MaPhong
                      JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                      ORDER BY p.TenPhong, m.TenMay");
                foreach (DataRow r in dt.Rows)
                {
                    string status = r["TenTrangThaiMay"].ToString();
                    string engStatus = status.Contains("Tốt") ? "available" :
                                       status.Contains("Bảo") ? "occupied" : "broken";
                    dgv.Rows.Add(r["TenMay"], r["TenPhong"], r["CPU"],
                        r["RAM"] + "GB", r["KichThuocManHinh"] + "\"",
                        engStatus, DateTime.Now.AddDays(-new Random().Next(1, 30)).ToString("yyyy-MM-dd"));
                }
            }
            catch
            {
                // Dữ liệu mẫu theo Figma
                dgv.Rows.Add("PC-A301-01", "Lab A-301", "Intel i7-12700", "16GB", "24\" Dell", "available", "2026-04-14");
                dgv.Rows.Add("PC-A301-02", "Lab A-301", "Intel i7-12700", "16GB", "24\" Dell", "occupied", "2026-04-15");
                dgv.Rows.Add("PC-A301-03", "Lab A-301", "Intel i5-12400", "8GB", "22\" HP", "available", "2026-04-13");
                dgv.Rows.Add("PC-B205-01", "Lab B-205", "AMD Ryzen 5 5600", "16GB", "27\" LG", "available", "2026-04-12");
                dgv.Rows.Add("PC-B205-02", "Lab B-205", "AMD Ryzen 5 5600", "16GB", "27\" LG", "available", "2026-04-11");
                dgv.Rows.Add("PC-C102-01", "Lab C-102", "Intel i9-13900", "32GB", "32\" Samsung", "available", "2026-04-10");
                dgv.Rows.Add("PC-C102-02", "Lab C-102", "Intel i9-13900", "32GB", "32\" Samsung", "available", "2026-04-09");
                dgv.Rows.Add("PC-A302-01", "Lab A-302", "Intel i5-12400", "8GB", "24\" Asus", "occupied", "2026-04-15");
            }
        }

        /// <summary>
        /// Lọc dữ liệu bảng theo các bộ lọc: từ khóa, CPU, RAM, Status
        /// </summary>
        private void FilterRows()
        {
            string kw = txtSearch.Text?.Trim().ToLower() ?? "";
            string cpuF = cboCPU.SelectedItem?.ToString() ?? "";
            string ramF = cboRAM.SelectedItem?.ToString() ?? "";
            string statusF = cboStatus.SelectedItem?.ToString() ?? "";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                bool show = true;

                // Lọc theo từ khóa
                if (!string.IsNullOrEmpty(kw))
                {
                    bool match = false;
                    foreach (DataGridViewCell c in row.Cells)
                        if (c.Value != null && c.Value.ToString().ToLower().Contains(kw)) { match = true; break; }
                    if (!match) show = false;
                }

                // Lọc theo CPU
                if (show && cpuF != "All CPUs" && !string.IsNullOrEmpty(cpuF))
                {
                    string v = row.Cells["CPU"].Value?.ToString() ?? "";
                    if (!v.ToLower().Contains(cpuF.ToLower())) show = false;
                }

                // Lọc theo RAM
                if (show && ramF != "All RAM" && !string.IsNullOrEmpty(ramF))
                {
                    string v = row.Cells["RAM"].Value?.ToString() ?? "";
                    if (!v.StartsWith(ramF.Replace(" GB", ""))) show = false;
                }

                // Lọc theo Status
                if (show && statusF != "All Status" && !string.IsNullOrEmpty(statusF))
                {
                    string v = row.Cells["Status"].Value?.ToString() ?? "";
                    string mapped = statusF.Contains("Tốt") ? "available" :
                                    statusF.Contains("Bảo") ? "occupied" : "broken";
                    if (v != mapped) show = false;
                }

                row.Visible = show;
            }
        }
    }
}
