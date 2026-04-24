using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public class ScheduleManageView : UserControl
    {
        private DataGridView dgv;

        public ScheduleManageView()
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

            var dtpDate = new DateTimePicker
            {
                Location = new Point(14, 14), Size = new Size(180, 28),
                Font = new Font("Segoe UI", 9.5F), Format = DateTimePickerFormat.Short
            };
            toolbar.Controls.Add(dtpDate);

            var cboCa = new ComboBox
            {
                Location = new Point(204, 14), Size = new Size(140, 28),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(245, 247, 252)
            };
            cboCa.Items.AddRange(new object[] { "Tất cả ca", "Ca 1 (7:00)", "Ca 2 (9:30)", "Ca 3 (13:00)", "Ca 4 (15:30)" });
            cboCa.SelectedIndex = 0;
            toolbar.Controls.Add(cboCa);

            var btnAdd = RoomManageView.MakeButton("➕  Tạo Lịch", ThemeColors.AccentPurple);
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
            pnlGrid.Controls.Add(dgv);

            LoadData();
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
