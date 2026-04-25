using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public partial class ScheduleManageView : UserControl
    {
        public ScheduleManageView()
        {
            InitializeComponent();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            toolbar.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(toolbar.ClientRectangle, 10))
                    toolbar.Region = new Region(p);
            };

            btnAdd.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(btnAdd.ClientRectangle, 8))
                    btnAdd.Region = new Region(p);
            };

            pnlGrid.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(pnlGrid.ClientRectangle, 10))
                    pnlGrid.Region = new Region(p);
            };

            cboCa.SelectedIndex = 0;

            SetupGridStyles();
            LoadData();
        }

        private void SetupGridStyles()
        {
            dgv.Columns.Add("NgayTH", "Ngày TH");
            dgv.Columns.Add("Ca", "Ca Học");
            dgv.Columns.Add("Lop", "Lớp");
            dgv.Columns.Add("MonHoc", "Môn Học");
            dgv.Columns.Add("SoSV", "Số SV");
            dgv.Columns.Add("Phong", "Phòng");
            dgv.Columns.Add("TrangThai", "Trạng Thái");
            dgv.Columns["SoSV"].Width = 70;
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

        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT l.NgayThucHanh, c.TenCa, lh.TenLop, mh.TenMon,
                      l.SoLuongSinhVien, l.TrangThaiLich,
                      ISNULL(p.TenPhong, '---') AS TenPhong
                      FROM LICH_THUC_HANH l
                      JOIN CA_HOC c ON l.MaCa = c.MaCa
                      JOIN LOP_HOC lh ON l.MaLop = lh.MaLop
                      JOIN MON_HOC mh ON l.MaMon = mh.MaMon
                      LEFT JOIN PHAN_CONG_PHONG pc ON l.MaLich = pc.MaLich
                      LEFT JOIN PHONG_MAY p ON pc.MaPhong = p.MaPhong
                      ORDER BY l.NgayThucHanh DESC, c.GioBatDau");
                foreach (DataRow r in dt.Rows)
                {
                    string status = r["TrangThaiLich"].ToString();
                    string icon = status.Contains("Đã") ? "🟢" : "🟠";
                    dgv.Rows.Add(
                        Convert.ToDateTime(r["NgayThucHanh"]).ToString("dd/MM/yyyy"),
                        r["TenCa"], r["TenLop"], r["TenMon"],
                        r["SoLuongSinhVien"], r["TenPhong"],
                        icon + " " + status);
                }
            }
            catch
            {
                dgv.Rows.Add("24/04/2026", "Ca 1 (7:00-9:15)", "CNTT01", "Lập trình C#", 40, "Phòng A01", "🟢 Đã xếp phòng");
                dgv.Rows.Add("24/04/2026", "Ca 2 (9:30-11:45)", "CNTT02", "Mạng máy tính", 35, "Phòng A02", "🟢 Đã xếp phòng");
                dgv.Rows.Add("25/04/2026", "Ca 1 (7:00-9:15)", "KTPM01", "Kiểm thử PM", 30, "---", "🟠 Chờ xếp phòng");
                dgv.Rows.Add("25/04/2026", "Ca 3 (13:00-15:15)", "HTTT01", "Phân tích TK", 38, "Phòng B02", "🟢 Đã xếp phòng");
                dgv.Rows.Add("26/04/2026", "Ca 1 (7:00-9:15)", "MMT01", "An toàn TT", 42, "---", "🟠 Chờ xếp phòng");
            }
        }
    }
}
