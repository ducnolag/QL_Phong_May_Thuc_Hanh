using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public class UserManageView : UserControl
    {
        private DataGridView dgv;

        public UserManageView()
        {
            this.BackColor = ThemeColors.BackgroundMain;
            this.DoubleBuffered = true;
            BuildUI();
        }

        private void BuildUI()
        {
            // ═══ TOOLBAR ═══
            var toolbar = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.White };
            toolbar.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(toolbar.ClientRectangle, 10))
                    toolbar.Region = new Region(p);
            };
            this.Controls.Add(toolbar);

            var txtSearch = new TextBox
            {
                Size = new Size(240, 28), Location = new Point(14, 14),
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 247, 252),
                PlaceholderText = "🔍 Tìm người dùng..."
            };
            toolbar.Controls.Add(txtSearch);

            var btnAdd = RoomManageView.MakeButton("➕  Thêm User", ThemeColors.PrimaryBlue);
            btnAdd.Size = new Size(150, 34);
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            toolbar.Controls.Add(btnAdd);
            toolbar.Resize += (s, e) => btnAdd.Location = new Point(toolbar.Width - 165, 10);
            btnAdd.Location = new Point(toolbar.Width - 165, 10);

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
            pnlGrid.Controls.Add(dgv);

            LoadData();
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
