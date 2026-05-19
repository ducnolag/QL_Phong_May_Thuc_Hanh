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
    /// Quản lý lịch thực hành – Giao diện card-based theo Figma.
    /// Hiển thị: summary cards, danh sách lịch dạng card với các nút Edit/Cancel/Delete.
    /// CRUD đầy đủ: Tạo lịch, Sửa lịch, Hủy lịch, Xóa lịch.
    /// </summary>
    public partial class ScheduleManageView : UserControl
    {
        public ScheduleManageView()
        {
            InitializeComponent();
            SetupView();
        }

        /// <summary>
        /// Thiết lập giao diện và sự kiện
        /// </summary>
        private void SetupView()
        {
            btnAdd.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(btnAdd.ClientRectangle, 8))
                    btnAdd.Region = new Region(p);
            };
            btnAdd.Click += (s, e) => ShowCreateDialog();

            LoadData();
        }

        /// <summary>
        /// Tải dữ liệu lịch thực hành và hiển thị
        /// </summary>
        private void LoadData()
        {
            pnlStats.Controls.Clear();
            pnlScheduleList.Controls.Clear();

            int totalSchedules = 0, upcoming = 0;
            var schedules = new System.Collections.Generic.List<(int id, string className, string status, string date, string dayName, string time, int students, string room)>();

            try
            {
                totalSchedules = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM LICH_THUC_HANH"));
                upcoming = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM LICH_THUC_HANH WHERE NgayThucHanh >= CAST(GETDATE() AS DATE)"));

                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT l.MaLich, mh.TenMon, l.TrangThaiLich, l.NgayThucHanh, 
                      c.TenCa, c.GioBatDau, c.GioKetThuc,
                      l.SoLuongSinhVien, ISNULL(p.TenPhong, '---') AS TenPhong
                      FROM LICH_THUC_HANH l
                      JOIN CA_HOC c ON l.MaCa = c.MaCa
                      JOIN MON_HOC mh ON l.MaMon = mh.MaMon
                      LEFT JOIN PHAN_CONG_PHONG pc ON l.MaLich = pc.MaLich
                      LEFT JOIN PHONG_MAY p ON pc.MaPhong = p.MaPhong
                      ORDER BY l.NgayThucHanh DESC, c.GioBatDau");

                foreach (DataRow r in dt.Rows)
                {
                    DateTime dateVal = Convert.ToDateTime(r["NgayThucHanh"]);
                    string status = r["TrangThaiLich"].ToString().Contains("Đã") ? "scheduled" : "pending";
                    string timeStr = $"{r["GioBatDau"].ToString().Substring(0, 5)}-{r["GioKetThuc"].ToString().Substring(0, 5)}";
                    schedules.Add((
                        Convert.ToInt32(r["MaLich"]),
                        r["TenMon"].ToString(),
                        status,
                        dateVal.ToString("yyyy-MM-dd"),
                        dateVal.ToString("dddd"),
                        timeStr,
                        Convert.ToInt32(r["SoLuongSinhVien"]),
                        r["TenPhong"].ToString()
                    ));
                }
            }
            catch
            {
                totalSchedules = 4; upcoming = 3;
                schedules.Add((1, "CS101", "scheduled", "2026-04-16", "Thursday", "08:00-10:00", 25, "Lab A-301"));
                schedules.Add((2, "CS202", "scheduled", "2026-04-16", "Thursday", "10:00-12:00", 20, "Lab B-205"));
                schedules.Add((3, "CS303", "pending", "2026-04-17", "Friday", "13:00-15:00", 30, "---"));
                schedules.Add((4, "CS404", "scheduled", "2026-04-18", "Saturday", "08:00-10:00", 35, "Lab C-102"));
            }

            // === Summary cards ===
            pnlStats.Controls.Add(MakeSummaryCard("Total Schedules", totalSchedules.ToString(), ThemeColors.TextPrimary));
            pnlStats.Controls.Add(MakeSummaryCard("Upcoming", upcoming.ToString(), ThemeColors.AccentGreen));

            // === Schedule cards ===
            foreach (var sch in schedules)
            {
                pnlScheduleList.Controls.Add(MakeScheduleCard(
                    sch.id, sch.className, sch.status, sch.date, sch.dayName,
                    sch.time, sch.students, sch.room));
            }
        }

        /// <summary>
        /// Tạo thẻ tổng kết nhỏ
        /// </summary>
        private Panel MakeSummaryCard(string title, string value, Color valueColor)
        {
            var card = new Panel { Size = new Size(160, 75), Margin = new Padding(6), BackColor = Color.White };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 10))
                    card.Region = new Region(p);
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 10))
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                    g.DrawPath(pen, p);

                TextRenderer.DrawText(g, title, new Font("Segoe UI", 9F),
                    new Point(14, 10), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, value, new Font("Segoe UI", 22F, FontStyle.Bold),
                    new Point(12, 30), valueColor);
            };
            return card;
        }

        /// <summary>
        /// Tạo card lịch thực hành theo Figma: icon lịch, tên môn + badge, thông tin chi tiết, nút hành động
        /// </summary>
        private Panel MakeScheduleCard(int id, string className, string status, string date,
            string dayName, string time, int students, string room)
        {
            var card = new Panel
            {
                Size = new Size(pnlScheduleList.Width - 30, 120),
                Margin = new Padding(4),
                BackColor = Color.White,
                Tag = id
            };

            Color badgeBg = status == "scheduled" ? ThemeColors.BadgeBlueBg : ThemeColors.BadgeOrangeBg;
            Color badgeFg = status == "scheduled" ? ThemeColors.BadgeBlueFg : ThemeColors.BadgeOrangeFg;

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 12))
                    card.Region = new Region(p);
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 12))
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                    g.DrawPath(pen, p);

                // Icon lịch
                using (var br = new SolidBrush(Color.FromArgb(30, ThemeColors.PrimaryBlue)))
                    g.FillEllipse(br, 16, 18, 44, 44);
                TextRenderer.DrawText(g, "📅", new Font("Segoe UI", 16F),
                    new Rectangle(16, 18, 44, 44), ThemeColors.PrimaryBlue,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // Tên môn học
                TextRenderer.DrawText(g, className, new Font("Segoe UI", 13F, FontStyle.Bold),
                    new Point(72, 14), ThemeColors.TextPrimary);

                // Badge trạng thái
                var badgeText = status;
                var sz = TextRenderer.MeasureText(badgeText, new Font("Segoe UI", 8F));
                int bx = 72 + (int)g.MeasureString(className, new Font("Segoe UI", 13F, FontStyle.Bold)).Width + 10;
                using (var br = new SolidBrush(badgeBg))
                using (var p = UIHelper.GetRoundedRectPath(new Rectangle(bx, 18, sz.Width + 10, sz.Height), 6))
                    g.FillPath(br, p);
                TextRenderer.DrawText(g, badgeText, new Font("Segoe UI", 8F, FontStyle.Bold),
                    new Point(bx + 5, 19), badgeFg);

                // Thông tin chi tiết
                int infoY = 44;
                TextRenderer.DrawText(g, $"📅  {date} ({dayName})", new Font("Segoe UI", 9F),
                    new Point(72, infoY), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, $"⏰  {time}", new Font("Segoe UI", 9F),
                    new Point(72, infoY + 20), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, $"👥  {students} students", new Font("Segoe UI", 9F),
                    new Point(300, infoY), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, $"🏢  {room}", new Font("Segoe UI", 9F),
                    new Point(300, infoY + 20), ThemeColors.TextSecondary);
            };

            // Nút Edit
            var btnEdit = new Button
            {
                Text = "✏", Size = new Size(34, 30), Location = new Point(card.Width - 175, 16),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 11F), Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnEdit.Click += (s, ev) => ShowEditDialog(id);
            card.Controls.Add(btnEdit);

            // Nút Cancel
            var btnCancel = new Button
            {
                Text = "Cancel", Size = new Size(70, 30), Location = new Point(card.Width - 135, 16),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnCancel.Click += (s, ev) => CancelSchedule(id);
            card.Controls.Add(btnCancel);

            // Nút Delete
            var btnDel = new Button
            {
                Text = "🗑", Size = new Size(34, 30), Location = new Point(card.Width - 55, 16),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = ThemeColors.AccentRed,
                Font = new Font("Segoe UI", 11F), Cursor = Cursors.Hand
            };
            btnDel.FlatAppearance.BorderColor = Color.FromArgb(254, 226, 226);
            btnDel.Click += (s, ev) => DeleteSchedule(id);
            card.Controls.Add(btnDel);

            return card;
        }

        /// <summary>
        /// Hiển thị dialog tạo lịch thực hành mới
        /// </summary>
        private void ShowCreateDialog()
        {
            using (var dlg = CreateScheduleDialog("Tạo Lịch Thực Hành Mới"))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DateTime date = FindControl<DateTimePicker>(dlg, "dtpDate").Value;
                        var cboLop = FindControl<ComboBox>(dlg, "cboLop");
                        var cboMon = FindControl<ComboBox>(dlg, "cboMon");
                        var cboCa = FindControl<ComboBox>(dlg, "cboCa");
                        int soSV = (int)FindControl<NumericUpDown>(dlg, "numSV").Value;

                        if (cboLop.SelectedValue == null || cboMon.SelectedValue == null || cboCa.SelectedValue == null)
                        {
                            MessageBox.Show("Vui lòng chọn đầy đủ thông tin!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Lấy ID admin (NguoiTao)
                        var creatorId = DatabaseHelper.ExecuteScalar(
                            "SELECT TOP 1 MaNguoiDung FROM NGUOI_DUNG WHERE TenDangNhap='admin'") ?? 1;

                        DatabaseHelper.ExecuteNonQuery(
                            @"INSERT INTO LICH_THUC_HANH (NgayThucHanh, SoLuongSinhVien, MaLop, MaMon, MaCa, NguoiTao)
                              VALUES (@date, @sv, @lop, @mon, @ca, @creator)",
                            new SqlParameter("@date", date.Date),
                            new SqlParameter("@sv", soSV),
                            new SqlParameter("@lop", cboLop.SelectedValue),
                            new SqlParameter("@mon", cboMon.SelectedValue),
                            new SqlParameter("@ca", cboCa.SelectedValue),
                            new SqlParameter("@creator", creatorId));

                        MessageBox.Show("Đã tạo lịch thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Hiển thị dialog sửa lịch thực hành
        /// </summary>
        private void ShowEditDialog(int scheduleId)
        {
            try
            {
                using (var dlg = CreateScheduleDialog("Sửa Lịch Thực Hành"))
                {
                    // Tải dữ liệu hiện tại
                    var dt = DatabaseHelper.ExecuteQuery(
                        "SELECT NgayThucHanh, SoLuongSinhVien, MaLop, MaMon, MaCa FROM LICH_THUC_HANH WHERE MaLich=@id",
                        new SqlParameter("@id", scheduleId));
                    if (dt.Rows.Count > 0)
                    {
                        var r = dt.Rows[0];
                        FindControl<DateTimePicker>(dlg, "dtpDate").Value = Convert.ToDateTime(r["NgayThucHanh"]);
                        FindControl<NumericUpDown>(dlg, "numSV").Value = Convert.ToInt32(r["SoLuongSinhVien"]);
                    }

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        DateTime date = FindControl<DateTimePicker>(dlg, "dtpDate").Value;
                        var cboLop = FindControl<ComboBox>(dlg, "cboLop");
                        var cboMon = FindControl<ComboBox>(dlg, "cboMon");
                        var cboCa = FindControl<ComboBox>(dlg, "cboCa");
                        int soSV = (int)FindControl<NumericUpDown>(dlg, "numSV").Value;

                        DatabaseHelper.ExecuteNonQuery(
                            @"UPDATE LICH_THUC_HANH SET NgayThucHanh=@date, SoLuongSinhVien=@sv,
                              MaLop=@lop, MaMon=@mon, MaCa=@ca WHERE MaLich=@id",
                            new SqlParameter("@date", date.Date),
                            new SqlParameter("@sv", soSV),
                            new SqlParameter("@lop", cboLop.SelectedValue ?? 1),
                            new SqlParameter("@mon", cboMon.SelectedValue ?? 1),
                            new SqlParameter("@ca", cboCa.SelectedValue ?? 1),
                            new SqlParameter("@id", scheduleId));

                        MessageBox.Show("Đã cập nhật lịch thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hủy lịch (đổi trạng thái thành "Đã hủy")
        /// </summary>
        private void CancelSchedule(int scheduleId)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy lịch này?", "Xác nhận hủy",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.ExecuteNonQuery(
                        "UPDATE LICH_THUC_HANH SET TrangThaiLich=N'Đã hủy' WHERE MaLich=@id",
                        new SqlParameter("@id", scheduleId));
                    MessageBox.Show("Đã hủy lịch!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Xóa lịch vĩnh viễn
        /// </summary>
        private void DeleteSchedule(int scheduleId)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa lịch này vĩnh viễn?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Xóa yêu cầu cấu hình liên quan
                    DatabaseHelper.ExecuteNonQuery(
                        "DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich=@id",
                        new SqlParameter("@id", scheduleId));
                    // Xóa phân công phòng liên quan
                    DatabaseHelper.ExecuteNonQuery(
                        "DELETE FROM PHAN_CONG_PHONG WHERE MaLich=@id",
                        new SqlParameter("@id", scheduleId));
                    // Xóa lịch
                    DatabaseHelper.ExecuteNonQuery(
                        "DELETE FROM LICH_THUC_HANH WHERE MaLich=@id",
                        new SqlParameter("@id", scheduleId));

                    MessageBox.Show("Đã xóa lịch!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Tạo dialog form cho tạo/sửa lịch thực hành
        /// </summary>
        private Form CreateScheduleDialog(string title)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(440, 400), StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };

            int y = 20;

            // Ngày thực hành
            dlg.Controls.Add(new Label { Text = "Ngày TH:", Location = new Point(20, y + 3), AutoSize = true });
            var dtpDate = new DateTimePicker
            {
                Name = "dtpDate", Location = new Point(140, y), Size = new Size(260, 26),
                Format = DateTimePickerFormat.Short
            };
            dlg.Controls.Add(dtpDate);
            y += 40;

            // Lớp học
            dlg.Controls.Add(new Label { Text = "Lớp:", Location = new Point(20, y + 3), AutoSize = true });
            var cboLop = new ComboBox
            {
                Name = "cboLop", Location = new Point(140, y), Size = new Size(260, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            try
            {
                var dtLop = DatabaseHelper.ExecuteQuery("SELECT MaLop, TenLop FROM LOP_HOC ORDER BY TenLop");
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
                cboLop.DataSource = dtLop;
            }
            catch { cboLop.Items.AddRange(new object[] { "CNTT01", "CNTT02", "KTPM01" }); }
            dlg.Controls.Add(cboLop);
            y += 40;

            // Môn học
            dlg.Controls.Add(new Label { Text = "Môn học:", Location = new Point(20, y + 3), AutoSize = true });
            var cboMon = new ComboBox
            {
                Name = "cboMon", Location = new Point(140, y), Size = new Size(260, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            try
            {
                var dtMon = DatabaseHelper.ExecuteQuery("SELECT MaMon, TenMon FROM MON_HOC ORDER BY TenMon");
                cboMon.DisplayMember = "TenMon";
                cboMon.ValueMember = "MaMon";
                cboMon.DataSource = dtMon;
            }
            catch { cboMon.Items.AddRange(new object[] { "Lập trình C#", "Mạng MT" }); }
            dlg.Controls.Add(cboMon);
            y += 40;

            // Ca học
            dlg.Controls.Add(new Label { Text = "Ca học:", Location = new Point(20, y + 3), AutoSize = true });
            var cboCa = new ComboBox
            {
                Name = "cboCa", Location = new Point(140, y), Size = new Size(260, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            try
            {
                var dtCa = DatabaseHelper.ExecuteQuery("SELECT MaCa, TenCa FROM CA_HOC ORDER BY GioBatDau");
                cboCa.DisplayMember = "TenCa";
                cboCa.ValueMember = "MaCa";
                cboCa.DataSource = dtCa;
            }
            catch { cboCa.Items.AddRange(new object[] { "Ca 1 (7:00)", "Ca 2 (9:30)", "Ca 3 (13:00)" }); }
            dlg.Controls.Add(cboCa);
            y += 40;

            // Số SV
            dlg.Controls.Add(new Label { Text = "Số SV:", Location = new Point(20, y + 3), AutoSize = true });
            var numSV = new NumericUpDown
            {
                Name = "numSV", Value = 30, Minimum = 1, Maximum = 200,
                Location = new Point(140, y), Size = new Size(120, 26)
            };
            dlg.Controls.Add(numSV);
            y += 55;

            // Buttons
            var btnSave = new Button
            {
                Text = "💾  Lưu", Size = new Size(120, 38), Location = new Point(140, y),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand, DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Hủy", Size = new Size(100, 38), Location = new Point(270, y),
                BackColor = Color.FromArgb(241, 245, 249), ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand, DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCancel);

            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCancel;
            return dlg;
        }

        private T FindControl<T>(Form form, string name) where T : Control
        {
            foreach (Control c in form.Controls)
                if (c is T t && c.Name == name) return t;
            return null;
        }
    }
}
