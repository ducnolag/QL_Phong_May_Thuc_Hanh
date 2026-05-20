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

            this.Load += (s, e) =>
            {
            };

            LoadData();
        }

        /// <summary>
        /// Tải dữ liệu lịch thực hành và hiển thị
        /// </summary>
        private void LoadData()
        {
            pnlStats.Controls.Clear();
            pnlScheduleList.Controls.Clear();

            int totalSchedules = 0, assigned = 0, pending = 0, canceled = 0;
            var schedules = new System.Collections.Generic.List<(int id, string className, string status, string date, string dayName, string time, int students, string room)>();

            try
            {
                totalSchedules = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM LICH_THUC_HANH"));
                assigned = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM LICH_THUC_HANH WHERE TrangThaiLich != N'Đã hủy' AND MaLich IN (SELECT MaLich FROM PHAN_CONG_PHONG)"));
                pending = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM LICH_THUC_HANH WHERE TrangThaiLich != N'Đã hủy' AND MaLich NOT IN (SELECT MaLich FROM PHAN_CONG_PHONG)"));
                canceled = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM LICH_THUC_HANH WHERE TrangThaiLich = N'Đã hủy'"));

                // Chỉ hiển thị lịch chưa bị hủy
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT l.MaLich, mh.TenMon, l.TrangThaiLich, l.NgayThucHanh, 
                      c.TenCa, c.GioBatDau, c.GioKetThuc,
                      l.SoLuongSinhVien, ISNULL(p.TenPhong, '---') AS TenPhong
                      FROM LICH_THUC_HANH l
                      JOIN CA_HOC c ON l.MaCa = c.MaCa
                      JOIN MON_HOC mh ON l.MaMon = mh.MaMon
                      LEFT JOIN PHAN_CONG_PHONG pc ON l.MaLich = pc.MaLich
                      LEFT JOIN PHONG_MAY p ON pc.MaPhong = p.MaPhong
                      WHERE l.TrangThaiLich != N'Đã hủy'
                      ORDER BY l.NgayThucHanh DESC, c.GioBatDau");

                foreach (DataRow r in dt.Rows)
                {
                    DateTime dateVal = Convert.ToDateTime(r["NgayThucHanh"]);
                    // Đã xếp phòng = có trong PHAN_CONG_PHONG (TenPhong != ---)
                    string status = r["TenPhong"].ToString() != "---" ? "Đã xếp" : "Chờ xếp";
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
                totalSchedules = 4; assigned = 3; pending = 1; canceled = 0;
                schedules.Add((1, "CS101", "Đã xếp", "2026-04-16", "Thursday", "08:00-10:00", 25, "Lab A-301"));
                schedules.Add((2, "CS202", "Đã xếp", "2026-04-16", "Thursday", "10:00-12:00", 20, "Lab B-205"));
                schedules.Add((3, "CS303", "Chờ xếp", "2026-04-17", "Friday", "13:00-15:00", 30, "---"));
                schedules.Add((4, "CS404", "Đã xếp", "2026-04-18", "Saturday", "08:00-10:00", 35, "Lab C-102"));
            }

            // === Summary cards: Tổng lịch | Đã xếp | Chờ xếp | Đã hủy ===
            pnlStats.Controls.Add(MakeSummaryCard("Tổng số lịch", totalSchedules.ToString(), ThemeColors.PrimaryBlue));
            pnlStats.Controls.Add(MakeSummaryCard("Đã xếp phòng", assigned.ToString(), ThemeColors.AccentGreen));
            pnlStats.Controls.Add(MakeSummaryCard("Chờ xếp phòng", pending.ToString(), ThemeColors.AccentOrange));
            pnlStats.Controls.Add(MakeSummaryCard("Đã hủy", canceled.ToString(), ThemeColors.AccentRed));

            // === Schedule cards (chỉ hiển thị lịch chưa hủy) ===
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

            Color badgeBg = status == "Đã xếp" ? ThemeColors.BadgeBlueBg : (status == "Đã hủy" ? ThemeColors.BadgeRedBg : ThemeColors.BadgeOrangeBg);
            Color badgeFg = status == "Đã xếp" ? ThemeColors.BadgeBlueFg : (status == "Đã hủy" ? ThemeColors.BadgeRedFg : ThemeColors.BadgeOrangeFg);

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
                TextRenderer.DrawText(g, $"👥  {students} sinh viên", new Font("Segoe UI", 9F),
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
                Text = "Hủy lịch", Size = new Size(80, 30), Location = new Point(card.Width - 145, 16),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnCancel.Click += (s, ev) => CancelSchedule(id);
            card.Controls.Add(btnCancel);

            // Nút Delete đã bị xóa theo yêu cầu

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
                        var cboCa  = FindControl<ComboBox>(dlg, "cboCa");
                        var cboRoom = FindControl<ComboBox>(dlg, "cboRoom");
                        int soSV = (int)FindControl<NumericUpDown>(dlg, "numSV").Value;

                        if (string.IsNullOrWhiteSpace(cboLop.Text) || string.IsNullOrWhiteSpace(cboMon.Text) || cboCa.SelectedValue == null)
                        {
                            MessageBox.Show("Vui lòng nhập đầy đủ thông tin Lớp, Môn và chọn Ca học!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Lấy hoặc tạo Lớp
                        object lopId = cboLop.SelectedValue;
                        if (lopId == null)
                        {
                            lopId = DatabaseHelper.ExecuteScalar("INSERT INTO LOP_HOC (TenLop) OUTPUT INSERTED.MaLop VALUES (@name)", new SqlParameter("@name", cboLop.Text.Trim()));
                        }

                        // Lấy hoặc tạo Môn
                        object monId = cboMon.SelectedValue;
                        if (monId == null)
                        {
                            monId = DatabaseHelper.ExecuteScalar("INSERT INTO MON_HOC (TenMon) OUTPUT INSERTED.MaMon VALUES (@name)", new SqlParameter("@name", cboMon.Text.Trim()));
                        }

                        var creatorId = DatabaseHelper.ExecuteScalar(
                            "SELECT TOP 1 MaNguoiDung FROM NGUOI_DUNG WHERE TenDangNhap='admin'") ?? 1;

                        int reqRam = (int)FindControl<NumericUpDown>(dlg, "numRam").Value;
                        int reqStorage = (int)FindControl<NumericUpDown>(dlg, "numStorage").Value;

                        // Final Check: Kiểm tra xung đột phút chót nếu có chọn phòng
                        int? roomId = null;
                        if (cboRoom != null && cboRoom.SelectedIndex > 0)
                        {
                            roomId = ParseRoomId(cboRoom.SelectedItem?.ToString());
                            if (roomId.HasValue)
                            {
                                int conflictCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                                    @"SELECT COUNT(*) FROM PHAN_CONG_PHONG pc
                                      JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                                      WHERE l.NgayThucHanh = @date AND l.MaCa = @ca AND pc.MaPhong = @phong AND l.TrangThaiLich != N'Đã hủy'",
                                    new SqlParameter("@date", date.Date),
                                    new SqlParameter("@ca", cboCa.SelectedValue),
                                    new SqlParameter("@phong", roomId.Value)));

                                if (conflictCount > 0)
                                {
                                    MessageBox.Show("Rất tiếc, phòng máy này vừa được người khác đặt trước cho ca học và ngày này. Vui lòng chọn phòng khác!", "Xung đột lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        // Insert lịch – lấy ID vừa tạo
                        var newId = DatabaseHelper.ExecuteScalar(
                            @"INSERT INTO LICH_THUC_HANH (NgayThucHanh, SoLuongSinhVien, MaLop, MaMon, MaCa, NguoiTao)
                              OUTPUT INSERTED.MaLich
                              VALUES (@date, @sv, @lop, @mon, @ca, @creator)",
                            new SqlParameter("@date",    date.Date),
                            new SqlParameter("@sv",      soSV),
                            new SqlParameter("@lop",     lopId),
                            new SqlParameter("@mon",     monId),
                            new SqlParameter("@ca",      cboCa.SelectedValue),
                            new SqlParameter("@creator", creatorId));

                        if (newId != null)
                        {
                            int lichId = Convert.ToInt32(newId);

                            // Lưu Yêu cầu cấu hình
                            DatabaseHelper.ExecuteNonQuery(
                                "INSERT INTO YEU_CAU_CAU_HINH (MaLich, RAMToiThieu, LuuTruToiThieu) VALUES (@lich, @ram, @storage)",
                                new SqlParameter("@lich", lichId),
                                new SqlParameter("@ram", reqRam),
                                new SqlParameter("@storage", reqStorage));

                            // Phân công phòng nếu đã chọn
                            if (roomId.HasValue)
                            {
                                DatabaseHelper.ExecuteNonQuery(
                                    "INSERT INTO PHAN_CONG_PHONG (MaLich, MaPhong) VALUES (@lich, @phong)",
                                    new SqlParameter("@lich",  lichId),
                                    new SqlParameter("@phong", roomId.Value));
                            }
                        }

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
                    // Tải dữ liệu hiện tại (join để lấy TênLop, TênMon)
                    var dt = DatabaseHelper.ExecuteQuery(
                        @"SELECT l.NgayThucHanh, l.SoLuongSinhVien, l.MaLop, l.MaMon, l.MaCa,
                          lh.TenLop, mh.TenMon, c.TenCa
                          FROM LICH_THUC_HANH l
                          JOIN LOP_HOC lh ON l.MaLop = lh.MaLop
                          JOIN MON_HOC mh ON l.MaMon = mh.MaMon
                          JOIN CA_HOC c   ON l.MaCa  = c.MaCa
                          WHERE l.MaLich = @id",
                        new SqlParameter("@id", scheduleId));

                    if (dt.Rows.Count > 0)
                    {
                        var r = dt.Rows[0];
                        FindControl<DateTimePicker>(dlg, "dtpDate").Value = Convert.ToDateTime(r["NgayThucHanh"]);
                        FindControl<NumericUpDown>(dlg, "numSV").Value = Convert.ToInt32(r["SoLuongSinhVien"]);

                        // Set cà combo box Lop – nếu có trong danh sách thì chọn, không thì đặt text
                        var cboLopCtrl = FindControl<ComboBox>(dlg, "cboLop");
                        string tenLop = r["TenLop"].ToString();
                        int maLop = Convert.ToInt32(r["MaLop"]);
                        cboLopCtrl.SelectedValue = maLop;
                        if (cboLopCtrl.SelectedValue == null) cboLopCtrl.Text = tenLop;

                        // Set combo Mon
                        var cboMonCtrl = FindControl<ComboBox>(dlg, "cboMon");
                        string tenMon = r["TenMon"].ToString();
                        int maMon = Convert.ToInt32(r["MaMon"]);
                        cboMonCtrl.SelectedValue = maMon;
                        if (cboMonCtrl.SelectedValue == null) cboMonCtrl.Text = tenMon;

                        // Set combo Ca
                        var cboCaCtrl = FindControl<ComboBox>(dlg, "cboCa");
                        cboCaCtrl.SelectedValue = Convert.ToInt32(r["MaCa"]);
                    }

                    // Load cấu hình yêu cầu cũ (nếu có)
                    var dtYeuCau = DatabaseHelper.ExecuteQuery("SELECT * FROM YEU_CAU_CAU_HINH WHERE MaLich=@id", new SqlParameter("@id", scheduleId));
                    if (dtYeuCau.Rows.Count > 0)
                    {
                        var yc = dtYeuCau.Rows[0];
                        if (yc["RAMToiThieu"] != DBNull.Value)
                            FindControl<NumericUpDown>(dlg, "numRam").Value = Convert.ToInt32(yc["RAMToiThieu"]);
                        if (yc["LuuTruToiThieu"] != DBNull.Value)
                            FindControl<NumericUpDown>(dlg, "numStorage").Value = Convert.ToInt32(yc["LuuTruToiThieu"]);
                    }

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        DateTime date = FindControl<DateTimePicker>(dlg, "dtpDate").Value;
                        var cboLop = FindControl<ComboBox>(dlg, "cboLop");
                        var cboMon = FindControl<ComboBox>(dlg, "cboMon");
                        var cboCa = FindControl<ComboBox>(dlg, "cboCa");
                        int soSV = (int)FindControl<NumericUpDown>(dlg, "numSV").Value;

                        if (string.IsNullOrWhiteSpace(cboLop.Text) || string.IsNullOrWhiteSpace(cboMon.Text) || cboCa.SelectedValue == null)
                        {
                            MessageBox.Show("Vui lòng nhập đầy đủ thông tin Lớp, Môn và chọn Ca học!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Lấy hoặc tạo Lớp
                        object lopId = cboLop.SelectedValue;
                        if (lopId == null)
                        {
                            lopId = DatabaseHelper.ExecuteScalar("INSERT INTO LOP_HOC (TenLop) OUTPUT INSERTED.MaLop VALUES (@name)", new SqlParameter("@name", cboLop.Text.Trim()));
                        }

                        // Lấy hoặc tạo Môn
                        object monId = cboMon.SelectedValue;
                        if (monId == null)
                        {
                            monId = DatabaseHelper.ExecuteScalar("INSERT INTO MON_HOC (TenMon) OUTPUT INSERTED.MaMon VALUES (@name)", new SqlParameter("@name", cboMon.Text.Trim()));
                        }

                        var cboRoom = FindControl<ComboBox>(dlg, "cboRoom");
                        int reqRam = (int)FindControl<NumericUpDown>(dlg, "numRam").Value;
                        int reqStorage = (int)FindControl<NumericUpDown>(dlg, "numStorage").Value;

                        // Final Check nếu người dùng chọn phòng mới
                        int? roomId = null;
                        if (cboRoom != null && cboRoom.SelectedIndex > 0)
                        {
                            roomId = ParseRoomId(cboRoom.SelectedItem?.ToString());
                            if (roomId.HasValue)
                            {
                                int conflictCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                                    @"SELECT COUNT(*) FROM PHAN_CONG_PHONG pc
                                      JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                                      WHERE l.NgayThucHanh = @date AND l.MaCa = @ca AND pc.MaPhong = @phong 
                                      AND l.TrangThaiLich != N'Đã hủy' AND l.MaLich != @currentId",
                                    new SqlParameter("@date", date.Date),
                                    new SqlParameter("@ca", cboCa.SelectedValue),
                                    new SqlParameter("@phong", roomId.Value),
                                    new SqlParameter("@currentId", scheduleId)));

                                if (conflictCount > 0)
                                {
                                    MessageBox.Show("Rất tiếc, phòng máy này vừa được người khác đặt trước cho ca học và ngày này. Vui lòng chọn phòng khác!", "Xung đột lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        DatabaseHelper.ExecuteNonQuery(
                            @"UPDATE LICH_THUC_HANH SET NgayThucHanh=@date, SoLuongSinhVien=@sv,
                              MaLop=@lop, MaMon=@mon, MaCa=@ca WHERE MaLich=@id",
                            new SqlParameter("@date", date.Date),
                            new SqlParameter("@sv", soSV),
                            new SqlParameter("@lop", lopId),
                            new SqlParameter("@mon", monId),
                            new SqlParameter("@ca", cboCa.SelectedValue),
                            new SqlParameter("@id", scheduleId));

                        // Cập nhật yêu cầu cấu hình
                        var countYC = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM YEU_CAU_CAU_HINH WHERE MaLich=@id", new SqlParameter("@id", scheduleId)));
                        if (countYC > 0)
                        {
                            DatabaseHelper.ExecuteNonQuery(
                                "UPDATE YEU_CAU_CAU_HINH SET RAMToiThieu=@ram, LuuTruToiThieu=@storage WHERE MaLich=@id",
                                new SqlParameter("@ram", reqRam), new SqlParameter("@storage", reqStorage), new SqlParameter("@id", scheduleId));
                        }
                        else
                        {
                            DatabaseHelper.ExecuteNonQuery(
                                "INSERT INTO YEU_CAU_CAU_HINH (MaLich, RAMToiThieu, LuuTruToiThieu) VALUES (@id, @ram, @storage)",
                                new SqlParameter("@ram", reqRam), new SqlParameter("@storage", reqStorage), new SqlParameter("@id", scheduleId));
                        }

                        // Cập nhật phân công phòng nếu có chọn phòng mới
                        if (roomId.HasValue)
                        {
                            DatabaseHelper.ExecuteNonQuery("DELETE FROM PHAN_CONG_PHONG WHERE MaLich=@id", new SqlParameter("@id", scheduleId));
                            DatabaseHelper.ExecuteNonQuery(
                                "INSERT INTO PHAN_CONG_PHONG (MaLich, MaPhong) VALUES (@id, @phong)",
                                new SqlParameter("@id", scheduleId), new SqlParameter("@phong", roomId.Value));
                        }

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
        /// Tạo dialog form cho tạo/sửa lịch thực hành – có gợi ý phòng tự động
        /// </summary>
        private Form CreateScheduleDialog(string title)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(460, 640), StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };

            int y = 20;

            // Ngày thực hành
            dlg.Controls.Add(new Label { Text = "Ngày TH:", Location = new Point(20, y + 3), AutoSize = true });
            var dtpDate = new DateTimePicker
            {
                Name = "dtpDate", Location = new Point(140, y), Size = new Size(290, 26),
                Format = DateTimePickerFormat.Short
            };
            dlg.Controls.Add(dtpDate);
            y += 40;

            // Lớp học
            dlg.Controls.Add(new Label { Text = "Lớp:", Location = new Point(20, y + 3), AutoSize = true });
            var cboLop = new ComboBox
            {
                Name = "cboLop", Location = new Point(140, y), Size = new Size(290, 26),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            try
            {
                var dtLop = DatabaseHelper.ExecuteQuery("SELECT MaLop, TenLop FROM LOP_HOC ORDER BY TenLop");
                cboLop.DisplayMember = "TenLop"; cboLop.ValueMember = "MaLop"; cboLop.DataSource = dtLop;
            }
            catch { cboLop.Items.AddRange(new object[] { "CNTT01", "CNTT02", "KTPM01" }); }
            dlg.Controls.Add(cboLop);
            y += 40;

            // Môn học
            dlg.Controls.Add(new Label { Text = "Môn học:", Location = new Point(20, y + 3), AutoSize = true });
            var cboMon = new ComboBox
            {
                Name = "cboMon", Location = new Point(140, y), Size = new Size(290, 26),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            try
            {
                var dtMon = DatabaseHelper.ExecuteQuery("SELECT MaMon, TenMon FROM MON_HOC ORDER BY TenMon");
                cboMon.DisplayMember = "TenMon"; cboMon.ValueMember = "MaMon"; cboMon.DataSource = dtMon;
            }
            catch { cboMon.Items.AddRange(new object[] { "Lập trình C#", "Mạng MT" }); }
            dlg.Controls.Add(cboMon);
            y += 40;

            // Ca học
            dlg.Controls.Add(new Label { Text = "Ca học:", Location = new Point(20, y + 3), AutoSize = true });
            var cboCa = new ComboBox
            {
                Name = "cboCa", Location = new Point(140, y), Size = new Size(290, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            try
            {
                var dtCa = DatabaseHelper.ExecuteQuery("SELECT MaCa, TenCa FROM CA_HOC ORDER BY GioBatDau");
                cboCa.DisplayMember = "TenCa"; cboCa.ValueMember = "MaCa"; cboCa.DataSource = dtCa;
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
            y += 40;

            // RAM Tối thiểu
            dlg.Controls.Add(new Label { Text = "RAM tối thiểu (GB):", Location = new Point(20, y + 3), AutoSize = true });
            var numRam = new NumericUpDown
            {
                Name = "numRam", Maximum = 512, Minimum = 0, Value = 8,
                Location = new Point(160, y), Size = new Size(100, 26)
            };
            dlg.Controls.Add(numRam);
            y += 40;

            // Lưu trữ Tối thiểu
            dlg.Controls.Add(new Label { Text = "Lưu trữ tối thiểu (GB):", Location = new Point(20, y + 3), AutoSize = true });
            var numStorage = new NumericUpDown
            {
                Name = "numStorage", Maximum = 4000, Minimum = 1, Value = 128,
                Location = new Point(160, y), Size = new Size(100, 26)
            };
            dlg.Controls.Add(numStorage);
            y += 45;

            // ── Gợi ý phòng tự động ─────────────────────────────────────────
            var pnlRoom = new Panel
            {
                Location = new Point(14, y), Size = new Size(420, 90),
                BackColor = Color.FromArgb(241, 249, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlRoom.Controls.Add(new Label
            {
                Text = "🏢  Gợi ý phòng tự động:", Location = new Point(8, 8), AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = ThemeColors.PrimaryBlue
            });

            var cboRoom = new ComboBox
            {
                Name = "cboRoom", Location = new Point(8, 32), Size = new Size(280, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cboRoom.Items.Add("-- Chưa phân công --");
            cboRoom.SelectedIndex = 0;
            pnlRoom.Controls.Add(cboRoom);

            var btnSuggest = new Button
            {
                Text = "🔍 Gợi ý", Location = new Point(298, 31), Size = new Size(112, 28),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSuggest.FlatAppearance.BorderSize = 0;
            var lblSuggestNote = new Label
            {
                Name = "lblSuggestNote", Location = new Point(8, 62), Size = new Size(400, 18),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.FromArgb(80, 120, 160),
                Text = "Nhấn 'Gợi ý' để tìm phòng trống theo ngày và ca đã chọn."
            };
            pnlRoom.Controls.Add(lblSuggestNote);

            btnSuggest.Click += (s, ev) =>
            {
                cboRoom.Items.Clear();
                cboRoom.Items.Add("-- Chưa phân công --");
                cboRoom.SelectedIndex = 0;
                try
                {
                    object caId    = cboCa.SelectedValue;
                    DateTime selDate = dtpDate.Value.Date;
                    int svCount    = (int)numSV.Value;

                    int reqRam = (int)((NumericUpDown)dlg.Controls["numRam"]).Value;
                    int reqStorage = (int)((NumericUpDown)dlg.Controls["numStorage"]).Value;

                    // Kiểm tra tổng sức chứa tối đa của tất cả phòng hoạt động
                    int maxCapacity = Convert.ToInt32(
                        DatabaseHelper.ExecuteScalar(
                            @"SELECT ISNULL(MAX(SucChua),0) FROM PHONG_MAY p
                              JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong
                              WHERE t.TenTrangThaiPhong=N'Hoạt động'") ?? 0);

                    if (svCount > maxCapacity)
                    {
                        lblSuggestNote.Text = $"❌ Số SV ({svCount}) vượt quá sức chứa lớn nhất ({maxCapacity} máy). Không phòng nào đủ!";
                        lblSuggestNote.ForeColor = Color.FromArgb(180, 30, 30);
                        return;
                    }

                    // Tìm phòng trống (chưa bị chiếm) có đủ sức chứa và đếm số lượng máy đạt cấu hình yêu cầu
                    var dtRooms = DatabaseHelper.ExecuteQuery(
                        @"SELECT p.MaPhong, p.TenPhong, p.SucChua,
                            (SELECT COUNT(*) FROM MAY_TINH m2
                             JOIN TRANG_THAI_MAY tm ON m2.MaTTMay=tm.MaTTMay
                             WHERE m2.MaPhong=p.MaPhong AND tm.TenTrangThaiMay=N'Tốt'
                               AND m2.RAM >= @ram AND m2.DungLuongLuuTru >= @storage) AS SoMayTot
                          FROM PHONG_MAY p
                          JOIN TRANG_THAI_PHONG ttp ON p.MaTTPhong = ttp.MaTTPhong
                          WHERE ttp.TenTrangThaiPhong = N'Hoạt động'
                            AND p.SucChua >= @sv
                            AND p.MaPhong NOT IN (
                                SELECT pc.MaPhong FROM PHAN_CONG_PHONG pc
                                JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                                WHERE l.NgayThucHanh = @date AND l.MaCa = @ca AND l.TrangThaiLich != N'Đã hủy'
                            )
                          ORDER BY p.SucChua",
                        new SqlParameter("@sv",   svCount),
                        new SqlParameter("@date", selDate),
                        new SqlParameter("@ca",   caId ?? DBNull.Value),
                        new SqlParameter("@ram",  reqRam),
                        new SqlParameter("@storage", reqStorage));

                    // Lọc thêm: phòng phải có đủ máy Tốt
                    int found = 0;
                    foreach (DataRow r in dtRooms.Rows)
                    {
                        int soMayTot = Convert.ToInt32(r["SoMayTot"]);
                        int sucChua  = Convert.ToInt32(r["SucChua"]);
                        string label = $"{r["TenPhong"]}  (sức chứa: {sucChua} | máy tốt: {soMayTot})  [ID:{r["MaPhong"]}]";
                        if (soMayTot >= svCount)
                        {
                            cboRoom.Items.Add(label);
                            found++;
                        }
                        else
                        {
                            // Thêm vào nhưng đánh dấu là thiếu máy tốt
                            cboRoom.Items.Add($"⚠ {r["TenPhong"]}  (chỉ có {soMayTot}/{svCount} máy tốt)  [ID:{r["MaPhong"]}]");
                        }
                    }

                    if (found > 0)
                    {
                        // Chọn phòng đầu tiên đủ điều kiện
                        for (int i = 1; i < cboRoom.Items.Count; i++)
                        {
                            if (!cboRoom.Items[i].ToString().StartsWith("⚠"))
                            { cboRoom.SelectedIndex = i; break; }
                        }
                        lblSuggestNote.Text = $"✅ Tìm được {found} phòng phù hợp (≥{svCount} máy tốt).";
                        lblSuggestNote.ForeColor = Color.FromArgb(30, 120, 50);
                    }
                    else if (dtRooms.Rows.Count > 0)
                    {
                        // Có phòng trống nhưng không đủ máy tốt
                        lblSuggestNote.Text = $"⚠ Có {dtRooms.Rows.Count} phòng trống nhưng không đủ máy Tốt cho {svCount} SV.";
                        lblSuggestNote.ForeColor = Color.FromArgb(160, 90, 0);
                    }
                    else
                    {
                        // Không còn phòng trống vào ca/ngày này
                        lblSuggestNote.Text = $"❌ Tất cả phòng ({svCount} máy+) đã bị chiếm vào ngày/ca này!";
                        lblSuggestNote.ForeColor = Color.FromArgb(180, 30, 30);
                    }
                }
                catch (Exception ex)
                {
                    lblSuggestNote.Text = "⚠ Lỗi kết nối DB: " + ex.Message.Substring(0, Math.Min(60, ex.Message.Length));
                    lblSuggestNote.ForeColor = Color.FromArgb(150, 0, 0);
                }
            };
            pnlRoom.Controls.Add(btnSuggest);
            dlg.Controls.Add(pnlRoom);
            y += 100;

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

        /// <summary>
        /// Trích MaPhong từ chuỗi gợi ý dạng "Lab A-301 (30 máy)  [ID:3]"
        /// </summary>
        private int? ParseRoomId(string roomText)
        {
            if (string.IsNullOrEmpty(roomText) || roomText.StartsWith("--")) return null;
            int start = roomText.LastIndexOf("[ID:", StringComparison.Ordinal);
            int end = roomText.LastIndexOf("]");
            if (start < 0 || end <= start) return null;
            string idStr = roomText.Substring(start + 4, end - start - 4);
            return int.TryParse(idStr, out int id) ? id : (int?)null;
        }

        private T FindControl<T>(Form form, string name) where T : Control
        {
            foreach (Control c in form.Controls)
                if (c is T t && c.Name == name) return t;
            // Tìm trong Panel con
            foreach (Control c in form.Controls)
                foreach (Control child in c.Controls)
                    if (child is T t2 && child.Name == name) return t2;
            return null;
        }
    }
}
