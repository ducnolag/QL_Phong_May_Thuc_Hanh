using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public partial class UserManageView : UserControl
    {
        public UserManageView()
        {
            InitializeComponent();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            // Removed GDI+ Paint events to ensure full Designer compatibility
            SetupGridStyles();
            LoadData();
        }

        private void SetupGridStyles()
        {
            dgv.Columns.Add("MaND", "Mã");
            dgv.Columns.Add("HoTen", "Họ Tên");
            dgv.Columns.Add("TenDN", "Tên Đăng Nhập");
            dgv.Columns.Add("Email", "Email");
            dgv.Columns.Add("SDT", "SĐT");
            dgv.Columns.Add("VaiTro", "Vai Trò");
            dgv.Columns.Add("TrangThai", "Trạng Thái");
            dgv.Columns["MaND"].Width = 55;
            dgv.Columns["SDT"].Width = 110;
            dgv.CellFormatting += (s, e) =>
            {
                string col = dgv.Columns[e.ColumnIndex].Name;
                if (col == "TrangThai" || col == "VaiTro")
                    RoomManageView.ColorStatusCell(e, col);
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

        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT nd.MaNguoiDung, nd.HoTen, nd.TenDangNhap, nd.Email,
                      nd.SoDienThoai, vt.TenVaiTro, nd.TrangThai
                      FROM NGUOI_DUNG nd
                      JOIN VAI_TRO vt ON nd.MaVaiTro = vt.MaVaiTro
                      ORDER BY nd.MaNguoiDung");
                foreach (DataRow r in dt.Rows)
                {
                    bool active = Convert.ToBoolean(r["TrangThai"]);
                    string role = r["TenVaiTro"].ToString();
                    string roleIcon = role == "Admin" ? "👑" : "👤";
                    string statusIcon = active ? "🟢" : "🔴";
                    string statusText = active ? "Hoạt động" : "Vô hiệu";
                    dgv.Rows.Add(
                        r["MaNguoiDung"], r["HoTen"], r["TenDangNhap"],
                        r["Email"], r["SoDienThoai"] ?? "",
                        roleIcon + " " + role, statusIcon + " " + statusText);
                }
            }
            catch
            {
                dgv.Rows.Add(1, "Administrator", "admin", "admin@lab.edu.vn", "0901234567", "👑 Admin", "🟢 Hoạt động");
                dgv.Rows.Add(2, "Trần Thị Bình", "staff01", "binh@lab.edu.vn", "0912345678", "👤 NhanVien", "🟢 Hoạt động");
            }
        }
    }
}
