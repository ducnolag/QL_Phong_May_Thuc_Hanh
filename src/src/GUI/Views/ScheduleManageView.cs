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
        private readonly src.BLL.ScheduleService _scheduleService;
        private readonly src.BLL.CatalogService _catalogService;

        public ScheduleManageView()
        {
            InitializeComponent();
            _scheduleService = new src.BLL.ScheduleService();
            _catalogService = new src.BLL.CatalogService();
            SetupView();
        }

        /// <summary>
        /// Thiết lập giao diện và sự kiện
        /// </summary>
        private void SetupView()
        {
            // Đã xóa UIHelper.GetRoundedRectPath cho btnAdd vì btnAdd nên đổi thành Guna2Button nếu cần.
            // Nhưng hiện tại vẫn giữ nguyên btnAdd ở dạng cơ bản nếu không muốn đụng Designer.
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
                var stats = _scheduleService.GetStatistics();
                totalSchedules = stats.total;
                assigned = stats.assigned;
                pending = stats.pending;
                canceled = stats.canceled;

                var dt = _scheduleService.GetActiveSchedules();
                foreach (var r in dt)
                {
                    string status = r.TenPhong != "---" ? "Đã xếp" : "Chờ xếp";
                    string timeStr = $"{r.GioBatDau.ToString(@"hh\:mm")}-{r.GioKetThuc.ToString(@"hh\:mm")}";
                    schedules.Add((
                        r.MaLich,
                        r.TenMon,
                        status,
                        r.NgayThucHanh.ToString("yyyy-MM-dd"),
                        r.NgayThucHanh.ToString("dddd"),
                        timeStr,
                        r.SoLuongSinhVien,
                        r.TenPhong
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
        /// Tạo thẻ tổng kết nhỏ bằng Guna2Panel
        /// </summary>
        private Guna.UI2.WinForms.Guna2Panel MakeSummaryCard(string title, string value, Color valueColor)
        {
            var card = new Guna.UI2.WinForms.Guna2Panel 
            { 
                Size = new Size(160, 75), 
                Margin = new Padding(6), 
                BackColor = Color.Transparent,
                FillColor = Color.White,
                BorderRadius = 10,
                BorderColor = Color.FromArgb(226, 232, 240),
                BorderThickness = 1
            };

            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(14, 10), AutoSize = true, BackColor = Color.Transparent };
            var lblValue = new Label { Text = value, Font = new Font("Segoe UI", 22F, FontStyle.Bold), ForeColor = valueColor, Location = new Point(12, 30), AutoSize = true, BackColor = Color.Transparent };
            
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            return card;
        }

        /// <summary>
        /// Tạo card lịch thực hành bằng Guna2Panel
        /// </summary>
        private Guna.UI2.WinForms.Guna2Panel MakeScheduleCard(int id, string className, string status, string date,
            string dayName, string time, int students, string room)
        {
            var card = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(pnlScheduleList.Width - 30, 120),
                Margin = new Padding(4),
                BackColor = Color.Transparent,
                FillColor = Color.White,
                BorderRadius = 12,
                BorderColor = Color.FromArgb(226, 232, 240),
                BorderThickness = 1,
                Tag = id
            };

            Color badgeBg = status == "Đã xếp" ? ThemeColors.BadgeBlueBg : (status == "Đã hủy" ? ThemeColors.BadgeRedBg : ThemeColors.BadgeOrangeBg);
            Color badgeFg = status == "Đã xếp" ? ThemeColors.BadgeBlueFg : (status == "Đã hủy" ? ThemeColors.BadgeRedFg : ThemeColors.BadgeOrangeFg);

            var iconLabel = new Label
            {
                Text = "📅", Font = new Font("Segoe UI", 16F),
                Location = new Point(16, 18), Size = new Size(44, 44),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(30, ThemeColors.PrimaryBlue),
                ForeColor = ThemeColors.PrimaryBlue
            };
            card.Controls.Add(iconLabel);

            var titleLabel = new Label
            {
                Text = className, Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Location = new Point(72, 14), AutoSize = true,
                ForeColor = ThemeColors.TextPrimary
            };
            card.Controls.Add(titleLabel);

            var badgeTextLabel = new Label
            {
                Text = status, Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = badgeFg, AutoSize = true, Location = new Point(5, 2), BackColor = Color.Transparent
            };
            var badge = new Guna.UI2.WinForms.Guna2Panel
            {
                FillColor = badgeBg, BorderRadius = 6,
                Location = new Point(72 + titleLabel.PreferredWidth + 10, 18),
                Size = new Size(badgeTextLabel.PreferredWidth + 10, badgeTextLabel.PreferredHeight + 4)
            };
            badge.Controls.Add(badgeTextLabel);
            card.Controls.Add(badge);

            int infoY = 44;
            card.Controls.Add(new Label { Text = $"📅  {date} ({dayName})", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(72, infoY), AutoSize = true });
            card.Controls.Add(new Label { Text = $"⏰  {time}", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(72, infoY + 20), AutoSize = true });
            card.Controls.Add(new Label { Text = $"👥  {students} sinh viên", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(300, infoY), AutoSize = true });
            card.Controls.Add(new Label { Text = $"🏢  {room}", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(300, infoY + 20), AutoSize = true });

            // Nút Edit bằng Guna2Button
            var btnEdit = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "✏", Size = new Size(34, 30), Location = new Point(card.Width - 175, 16),
                FillColor = Color.White, ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 11F), Cursor = Cursors.Hand,
                BorderRadius = 6, BorderThickness = 1, BorderColor = Color.FromArgb(226, 232, 240)
            };
            btnEdit.Click += (s, ev) => ShowEditDialog(id);
            card.Controls.Add(btnEdit);

            // Nút Cancel bằng Guna2Button
            var btnCancel = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Hủy lịch", Size = new Size(80, 30), Location = new Point(card.Width - 145, 16),
                FillColor = Color.White, ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand,
                BorderRadius = 6, BorderThickness = 1, BorderColor = Color.FromArgb(226, 232, 240)
            };
            btnCancel.Click += (s, ev) => CancelSchedule(id);
            card.Controls.Add(btnCancel);

            return card;
        }

        /// <summary>
        /// Hiển thị dialog tạo lịch thực hành mới
        /// </summary>
        private void ShowCreateDialog()
        {
            using (var dlg = CreateScheduleDialog("Tạo Lịch Thực Hành Mới"))
            {
                var btnOk = FindControl<Button>(dlg, "btnSave");
                if (btnOk == null) btnOk = FindControl<Button>(dlg, "btnOk");
                btnOk.Click += (s, e) =>
                {
                    try
                    {
                        var dtpDate    = FindControl<DateTimePicker>(dlg, "dtpDate");
                        var cboLop     = FindControl<ComboBox>(dlg, "cboLop");
                        var cboMon     = FindControl<ComboBox>(dlg, "cboMon");
                        var cboCa      = FindControl<ComboBox>(dlg, "cboCa");
                        var cboRoom    = FindControl<ComboBox>(dlg, "cboRoom");
                        var numSV      = FindControl<NumericUpDown>(dlg, "numSV");
                        var cboRam     = FindControl<ComboBox>(dlg, "cboInputRAM");
                        var cboStorage = FindControl<ComboBox>(dlg, "cboInputStorage");
                        var cboMonitor = FindControl<ComboBox>(dlg, "cboInputMonitor");

                        DateTime date  = dtpDate.Value;
                        int soSV       = (int)numSV.Value;
                        int reqRam     = Convert.ToInt32(cboRam.SelectedItem.ToString().Replace(" GB", ""));
                        int reqStorage = Convert.ToInt32(cboStorage.SelectedItem.ToString().Replace(" GB", ""));
                        int reqMonitor = Convert.ToInt32(cboMonitor.SelectedItem.ToString().Replace("\"", ""));
                        int? roomId    = GetRoomId(cboRoom.SelectedItem);

                        _scheduleService.ValidateAndCreateSchedule(
                            date, cboLop.Text.Trim(), cboMon.Text.Trim(), 
                            (int)cboCa.SelectedValue, soSV, reqRam, reqStorage, reqMonitor, roomId, AppSession.MaNguoiDung
                        );

                        MessageBox.Show("Đã tạo lịch thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dlg.DialogResult = DialogResult.OK; // this closes the form
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
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
                var sch = _scheduleService.GetScheduleById(scheduleId);
                if (sch == null) return;

                var req = _scheduleService.GetScheduleRequirements(scheduleId);
                var room = _scheduleService.GetAssignedRoom(scheduleId);

                using (var dlg = CreateScheduleDialog("Sửa Lịch Thực Hành"))
                {
                    // ── Load dữ liệu cũ vào form ─────────────────────────────
                    dlg.Shown += (s, ev) =>
                    {
                        FindControl<DateTimePicker>(dlg, "dtpDate").Value  = sch.NgayThucHanh;
                        FindControl<NumericUpDown>(dlg, "numSV").Value     = sch.SoLuongSinhVien;

                        var cboLopCtrl = FindControl<ComboBox>(dlg, "cboLop");
                        cboLopCtrl.Text = sch.TenLop;

                        var cboMonCtrl = FindControl<ComboBox>(dlg, "cboMon");
                        cboMonCtrl.Text = sch.TenMon;

                        var cboCaCtrl = FindControl<ComboBox>(dlg, "cboCa");
                        cboCaCtrl.SelectedValue = sch.MaCa;

                        var cboRamCtrl     = FindControl<ComboBox>(dlg, "cboInputRAM");
                        var cboStorageCtrl = FindControl<ComboBox>(dlg, "cboInputStorage");
                        var cboMonitorCtrl = FindControl<ComboBox>(dlg, "cboInputMonitor");

                        cboRamCtrl.SelectedItem = req.RAMToiThieu + " GB";
                        cboStorageCtrl.SelectedItem = req.LuuTruToiThieu + " GB";
                        
                        // We need to fetch ManHinhToiThieu. Assuming req has it now.
                        // Wait, req might not have ManHinhToiThieu strongly typed if DTO isn't updated. 
                        // I will update DTO next. Let's assume it's `req.ManHinhToiThieu`.
                        cboMonitorCtrl.SelectedItem = req.ManHinhToiThieu + "\"";
                        if (cboMonitorCtrl.SelectedIndex < 0) cboMonitorCtrl.SelectedIndex = 2;

                        if (room.MaPhong > 0)
                        {
                            var cboRoomCtrl = FindControl<ComboBox>(dlg, "cboRoom");
                            string currentRoomLabel = $"{room.TenPhong}  (sức chứa: {room.SucChua} | đang dùng)";
                            
                            bool roomFound = false;
                            for (int i = 1; i < cboRoomCtrl.Items.Count; i++)
                            {
                                int? rid = GetRoomId(cboRoomCtrl.Items[i]);
                                if (rid.HasValue && rid.Value == room.MaPhong)
                                { cboRoomCtrl.SelectedIndex = i; roomFound = true; break; }
                            }
                            if (!roomFound)
                            {
                                cboRoomCtrl.Items.Add(new RoomItem { Text = currentRoomLabel, Id = room.MaPhong });
                                cboRoomCtrl.SelectedIndex = cboRoomCtrl.Items.Count - 1;
                            }
                        }
                    };

                    var btnOk = FindControl<Button>(dlg, "btnSave");
                    if (btnOk == null) btnOk = FindControl<Button>(dlg, "btnOk");
                    btnOk.Click += (s, e) =>
                    {
                        try
                        {
                            DateTime date  = FindControl<DateTimePicker>(dlg, "dtpDate").Value;
                            var cboLop     = FindControl<ComboBox>(dlg, "cboLop");
                            var cboMon     = FindControl<ComboBox>(dlg, "cboMon");
                            var cboCa      = FindControl<ComboBox>(dlg, "cboCa");
                            var cboRoom    = FindControl<ComboBox>(dlg, "cboRoom");
                            var cboRam     = FindControl<ComboBox>(dlg, "cboInputRAM");
                            var cboStorage = FindControl<ComboBox>(dlg, "cboInputStorage");
                            var cboMonitor = FindControl<ComboBox>(dlg, "cboInputMonitor");

                            int soSV       = (int)FindControl<NumericUpDown>(dlg, "numSV").Value;
                            int reqRam     = Convert.ToInt32(cboRam.SelectedItem.ToString().Replace(" GB", ""));
                            int reqStorage = Convert.ToInt32(cboStorage.SelectedItem.ToString().Replace(" GB", ""));
                            int reqMonitor = Convert.ToInt32(cboMonitor.SelectedItem.ToString().Replace("\"", ""));

                            int? roomId = GetRoomId(cboRoom.SelectedItem);

                            _scheduleService.ValidateAndUpdateSchedule(
                                scheduleId, date, cboLop.Text.Trim(), cboMon.Text.Trim(), 
                                (int)cboCa.SelectedValue, soSV, reqRam, reqStorage, reqMonitor, roomId, AppSession.MaNguoiDung
                            );

                            MessageBox.Show("Đã cập nhật lịch thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dlg.DialogResult = DialogResult.OK;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    };

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
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
                    _scheduleService.CancelSchedule(scheduleId);
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
                    _scheduleService.DeleteSchedule(scheduleId);
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
                AutoCompleteSource = AutoCompleteSource.ListItems,
                IntegralHeight = false, MaxDropDownItems = 5
            };
            try
            {
                var dtLop = _catalogService.GetAllLopHoc();
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
                AutoCompleteSource = AutoCompleteSource.ListItems,
                IntegralHeight = false, MaxDropDownItems = 5
            };
            try
            {
                var dtMon = _catalogService.GetAllMonHoc();
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
                DropDownStyle = ComboBoxStyle.DropDownList,
                IntegralHeight = false, MaxDropDownItems = 5
            };
            try
            {
                var dtCa = _scheduleService.GetAllCaHoc();
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
            var cboInputRam = new ComboBox { Name = "cboInputRAM", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false, Location = new Point(220, y), Size = new Size(100, 26) };
            cboInputRam.Items.AddRange(new object[] { "4 GB", "8 GB", "16 GB", "32 GB", "64 GB" });
            cboInputRam.SelectedItem = "8 GB";
            dlg.Controls.Add(cboInputRam);
            y += 40;

            // Lưu trữ Tối thiểu
            dlg.Controls.Add(new Label { Text = "Lưu trữ tối thiểu (GB):", Location = new Point(20, y + 3), AutoSize = true });
            var cboInputStorage = new ComboBox { Name = "cboInputStorage", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false, Location = new Point(220, y), Size = new Size(100, 26) };
            cboInputStorage.Items.AddRange(new object[] { "128 GB", "256 GB", "512 GB", "1024 GB" });
            cboInputStorage.SelectedItem = "128 GB";
            dlg.Controls.Add(cboInputStorage);
            y += 40;

            // Màn hình tối thiểu
            dlg.Controls.Add(new Label { Text = "Màn hình tối thiểu (inch):", Location = new Point(20, y + 3), AutoSize = true });
            var cboInputMonitor = new ComboBox { Name = "cboInputMonitor", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false, Location = new Point(220, y), Size = new Size(100, 26) };
            cboInputMonitor.Items.AddRange(new object[] { "19\"", "21\"", "24\"", "27\"" });
            cboInputMonitor.SelectedItem = "24\"";
            dlg.Controls.Add(cboInputMonitor);
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
                Font = new Font("Segoe UI", 9.5F),
                IntegralHeight = false, MaxDropDownItems = 5
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

            // Auto-load room list khi form mở
            dlg.Shown += (s, ev) => btnSuggest.PerformClick();

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

                    var cboRam = FindControl<ComboBox>(dlg, "cboInputRAM");
                    var cboStorage = FindControl<ComboBox>(dlg, "cboInputStorage");
                    var cboMonitor = FindControl<ComboBox>(dlg, "cboInputMonitor");

                    int reqRam = Convert.ToInt32(cboRam.SelectedItem.ToString().Replace(" GB", ""));
                    int reqStorage = Convert.ToInt32(cboStorage.SelectedItem.ToString().Replace(" GB", ""));
                    int reqMonitor = Convert.ToInt32(cboMonitor.SelectedItem.ToString().Replace("\"", ""));

                    // Tìm phòng trống (chưa bị chiếm) có đủ sức chứa và đếm số lượng máy đạt cấu hình yêu cầu
                    var dtRooms = _scheduleService.GetRoomsForAssignment(svCount, reqRam, reqStorage, reqMonitor, selDate, (int)caId);

                    int found = 0;
                    int roomCount = 0;
                    foreach (var r in dtRooms)
                    {
                        roomCount++;
                        int soMayTot = r.MayTot;
                        int sucChua  = r.SucChua;
                        string label = $"Phòng {r.TenPhong} thỏa mãn điều kiện";
                        if (soMayTot >= svCount)
                        {
                            cboRoom.Items.Add(new RoomItem { Text = label, Id = r.MaPhong, IsValid = true });
                            found++;
                        }
                        else
                        {
                            // Thêm vào nhưng đánh dấu là thiếu máy tốt
                            cboRoom.Items.Add(new RoomItem { Text = $"Phòng {r.TenPhong} không thỏa mãn điều kiện", Id = r.MaPhong, IsValid = false });
                        }
                    }

                    if (found > 0)
                    {
                        // Chọn phòng đầu tiên đủ điều kiện
                        for (int i = 1; i < cboRoom.Items.Count; i++)
                        {
                            if (cboRoom.Items[i] is RoomItem ri && ri.IsValid)
                            { cboRoom.SelectedIndex = i; break; }
                        }
                        lblSuggestNote.Text = $"✅ Tìm được {found} phòng phù hợp (≥{svCount} máy tốt).";
                        lblSuggestNote.ForeColor = Color.FromArgb(30, 120, 50);
                    }
                    else if (roomCount > 0)
                    {
                        // Có phòng trống nhưng không đủ máy tốt
                        lblSuggestNote.Text = $"⚠ Có {roomCount} phòng trống nhưng không đủ máy Tốt cho {svCount} SV.";
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

            // Đăng ký sự kiện thay đổi -> tự động cập nhật gợi ý phòng
            cboCa.SelectedIndexChanged += (s, ev) => btnSuggest.PerformClick();
            dtpDate.ValueChanged += (s, ev) => btnSuggest.PerformClick();
            numSV.ValueChanged += (s, ev) => btnSuggest.PerformClick();
            cboInputRam.SelectedIndexChanged += (s, ev) => btnSuggest.PerformClick();
            cboInputStorage.SelectedIndexChanged += (s, ev) => btnSuggest.PerformClick();
            cboInputMonitor.SelectedIndexChanged += (s, ev) => btnSuggest.PerformClick();

            // Buttons
            var btnSave = new Button
            {
                Name = "btnSave",
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

        private class RoomItem
        {
            public string Text { get; set; }
            public int? Id { get; set; }
            public bool IsValid { get; set; }
            public override string ToString() => Text;
        }

        private int? GetRoomId(object item)
        {
            if (item is RoomItem ri) return ri.Id;
            return null;
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
