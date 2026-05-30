using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
    public partial class QuanLyLichThucHanhView : UserControl
    {
        private readonly src.BLL.LichThucHanhService _LichThucHanhService;
        private readonly src.BLL.LopMonService _LopMonService;

        public QuanLyLichThucHanhView()
        {
            InitializeComponent();
            _LichThucHanhService = new src.BLL.LichThucHanhService();
            _LopMonService = new src.BLL.LopMonService();
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
                pnlScheduleList.BringToFront();
            };

            pnlScheduleList.Resize += (s, e) =>
            {
                int w = pnlScheduleList.ClientSize.Width - 10;
                if (w < 100) return;
                pnlScheduleList.SuspendLayout();
                foreach (Control c in pnlScheduleList.Controls)
                {
                    c.Width = w;
                }
                pnlScheduleList.ResumeLayout();
            };

            InitFilters();
            LoadData();
        }

        private void InitFilters()
        {
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Format = DateTimePickerFormat.Short;

            // Mặc định chọn đầu tháng đến cuối tháng
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpToDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            dtpToDate.MinDate = dtpFromDate.Value;

            dtpFromDate.ValueChanged += (s, e) => {
                if (dtpToDate.Value < dtpFromDate.Value) dtpToDate.Value = dtpFromDate.Value;
                dtpToDate.MinDate = dtpFromDate.Value;
                LoadData();
            };
            dtpToDate.ValueChanged += (s, e) => LoadData();

            cboRoomFilter.Items.Add("Tất cả phòng");
            try {
                var rooms = new src.BLL.PhongMayService().GetAllRooms();
                foreach (var r in rooms) cboRoomFilter.Items.Add(r.TenPhong);
            } catch {}
            cboRoomFilter.SelectedIndex = 0;

            txtSearch.TextChanged += (s, e) => LoadData();
            cboRoomFilter.SelectedIndexChanged += (s, e) => LoadData();
        }

        private void chkXemLichCu_CheckedChanged(object sender, EventArgs e)
        {
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
            var schedules = new System.Collections.Generic.List<(int id, string className, string status, string date, string dayName, string time, int students, string room, string creator)>();

            try
            {
                DateTime startDate = dtpFromDate.Value.Date;
                DateTime endDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

                var stats = _LichThucHanhService.GetStatistics(startDate, endDate);
                totalSchedules = stats.total;
                assigned = stats.assigned;
                pending = stats.pending;
                canceled = stats.canceled;

                bool includePast = chkXemLichCu.Checked;
                var dt = _LichThucHanhService.GetActiveSchedules(startDate, endDate, includePast);

                foreach (var r in dt)
                {
                    // Lay trang thai truc tiep tu DB thay vi tu suy ra
                    string status;
                    if (r.TrangThaiLich == "Đã hủy") status = "Đã hủy";
                    else if (r.TrangThaiLich == "Không được xếp") status = "Không được xếp";
                    else status = "Đã xếp";

                    string timeStr = $"{r.GioBatDau.ToString(@"hh\:mm")}-{r.GioKetThuc.ToString(@"hh\:mm")}";
                    schedules.Add((
                        r.MaLich,
                        r.MaLopHocPhan, // Show Class instead of Subject
                        status,
                        r.NgayThucHanh.ToString("yyyy-MM-dd"),
                        r.NgayThucHanh.ToString("dddd", new System.Globalization.CultureInfo("vi-VN")),
                        timeStr,
                        r.SoLuongSinhVien,
                        r.TenPhong,
                        r.TenNguoiTao // Include Creator
                    ));
                }
            }
            catch
            {
                totalSchedules = 4; assigned = 3; pending = 1; canceled = 0;
                schedules.Add((1, "KTPM01-01", "Đã xếp", "2026-04-16", "Thứ Năm", "08:00-10:00", 25, "Lab A-301", "Admin"));
                schedules.Add((2, "CNTT01-01", "Đã xếp", "2026-04-16", "Thứ Năm", "10:00-12:00", 20, "Lab B-205", "NhanVien"));
                schedules.Add((3, "KTPM02-01", "Chờ xếp", "2026-04-17", "Thứ Sáu", "13:00-15:00", 30, "---", "NhanVien"));
                schedules.Add((4, "CNTT02-01", "Đã xếp", "2026-04-18", "Thứ Bảy", "08:00-10:00", 35, "Lab C-102", "Admin"));
            }

            // === Summary cards: Tổng lịch | Đã xếp | Đã hủy ===
            pnlStats.Controls.Add(MakeSummaryCard("Tổng lịch hiện tại", totalSchedules.ToString(), ThemeColors.PrimaryBlue));
            pnlStats.Controls.Add(MakeSummaryCard("Đã xếp phòng", assigned.ToString(), ThemeColors.AccentGreen));
            pnlStats.Controls.Add(MakeSummaryCard("Đã hủy", canceled.ToString(), ThemeColors.AccentRed));

            // === Schedule cards ===
            string searchTxt = txtSearch?.Text.ToLower() ?? "";
            string roomFilter = cboRoomFilter?.SelectedItem?.ToString() ?? "Tất cả phòng";

            foreach (var sch in schedules)
            {
                if (!string.IsNullOrEmpty(searchTxt) && !sch.className.ToLower().Contains(searchTxt) && !sch.room.ToLower().Contains(searchTxt)) continue;
                if (roomFilter != "Tất cả phòng" && sch.room != roomFilter) continue;

                // Mode xem lich cu: SQL da loc lich qua khu, isOld=true cho tat ca
                bool isOld = chkXemLichCu.Checked;

                pnlScheduleList.Controls.Add(MakeScheduleCard(
                    sch.id, sch.className, sch.status, sch.date, sch.dayName,
                    sch.time, sch.students, sch.room, sch.creator, isOld));
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
            string dayName, string time, int students, string room, string creator, bool isOld = false)
        {
            Color cardFill = isOld ? Color.FromArgb(249, 250, 251) : Color.White;
            var card = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(pnlScheduleList.Width - 30, 135),
                Margin = new Padding(4),
                BackColor = Color.Transparent,
                FillColor = cardFill,
                BorderRadius = 12,
                BorderColor = Color.FromArgb(226, 232, 240),
                BorderThickness = 1,
                Tag = id
            };

            Color badgeBg, badgeFg;
            if (status == "Đã xếp")        { badgeBg = ThemeColors.BadgeBlueBg;   badgeFg = ThemeColors.BadgeBlueFg; }
            else if (status == "Đã hủy")   { badgeBg = ThemeColors.BadgeRedBg;    badgeFg = ThemeColors.BadgeRedFg; }
            else if (status == "Không được xếp") { badgeBg = Color.FromArgb(241, 245, 249); badgeFg = Color.FromArgb(100, 116, 139); }
            else                           { badgeBg = ThemeColors.BadgeOrangeBg; badgeFg = ThemeColors.BadgeOrangeFg; }

            var iconLabel = new Label
            {
                Text = "📅",
                Font = new Font("Segoe UI", 16F),
                Location = new Point(16, 18),
                Size = new Size(44, 44),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(30, ThemeColors.PrimaryBlue),
                ForeColor = ThemeColors.PrimaryBlue
            };
            card.Controls.Add(iconLabel);

            var titleLabel = new Label
            {
                Text = className,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Location = new Point(72, 14),
                AutoSize = true,
                ForeColor = ThemeColors.TextPrimary
            };
            card.Controls.Add(titleLabel);

            var badgeTextLabel = new Label
            {
                Text = status,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = badgeFg,
                AutoSize = true,
                Location = new Point(5, 2),
                BackColor = Color.Transparent
            };
            var badge = new Guna.UI2.WinForms.Guna2Panel
            {
                FillColor = badgeBg,
                BorderRadius = 6,
                Location = new Point(72 + titleLabel.PreferredWidth + 10, 18),
                Size = new Size(badgeTextLabel.PreferredWidth + 10, badgeTextLabel.PreferredHeight + 4)
            };
            badge.Controls.Add(badgeTextLabel);
            card.Controls.Add(badge);

            int infoY = 44;
            card.Controls.Add(new Label { Text = $"Ngày thực hành: {date} ({dayName})", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(72, infoY), AutoSize = true });
            card.Controls.Add(new Label { Text = $"Ca học: {time}", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(72, infoY + 20), AutoSize = true });
            card.Controls.Add(new Label { Text = $"Người lập: {creator}", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(72, infoY + 40), AutoSize = true });

            card.Controls.Add(new Label { Text = $"Số sinh viên: {students}", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(300, infoY), AutoSize = true });
            card.Controls.Add(new Label { Text = $"Phòng: {room}", Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(300, infoY + 20), AutoSize = true });

            // Nút Edit bằng Guna2Button
            var btnEdit = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "✏",
                Size = new Size(34, 30),
                Location = new Point(card.Width - 130, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FillColor = Color.White,
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 11F),
                Cursor = Cursors.Hand,
                BorderRadius = 6,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                Enabled = !isOld
            };
            if (!isOld) btnEdit.Click += (s, ev) => ShowEditDialog(id);
            card.Controls.Add(btnEdit);

            // Nút Cancel bằng Guna2Button
            var btnCancel = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Hủy lịch",
                Size = new Size(80, 30),
                Location = new Point(card.Width - 90, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FillColor = Color.White,
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                BorderRadius = 6,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                Enabled = !isOld
            };
            if (!isOld) btnCancel.Click += (s, ev) => CancelSchedule(id);
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
                        var dtpDate = FindControl<DateTimePicker>(dlg, "dtpDate");
                        var cboLop = FindControl<ComboBox>(dlg, "cboLop");
                        var txtMonHidden = FindControl<TextBox>(dlg, "txtMonHidden");
                        var cboCa = FindControl<ComboBox>(dlg, "cboCa");
                        var cboRoom = FindControl<ComboBox>(dlg, "cboRoom");
                        var numSV = FindControl<NumericUpDown>(dlg, "numSV");
                        var cboRam = FindControl<ComboBox>(dlg, "cboInputRAM");
                        var cboStorage = FindControl<ComboBox>(dlg, "cboInputStorage");
                        var cboMonitor = FindControl<ComboBox>(dlg, "cboInputMonitor");
                        var cboCpu = FindControl<ComboBox>(dlg, "cboReqCpu");

                        string tenMon = txtMonHidden?.Text.Trim() ?? "";
                        if (string.IsNullOrEmpty(tenMon))
                        {
                            MessageBox.Show("Lớp học phần chưa được gắn môn học. Hãy chọn lớp khác!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        DateTime date = dtpDate.Value;
                        int soSV = (int)numSV.Value;
                        int reqRam = Convert.ToInt32(cboRam.SelectedItem.ToString().Replace(" GB", ""));
                        int reqStorage = Convert.ToInt32(cboStorage.SelectedItem.ToString().Replace(" GB", ""));
                        int reqMonitor = Convert.ToInt32(cboMonitor.SelectedItem.ToString().Replace("\"", ""));
                        string reqCpu = cboCpu.SelectedItem?.ToString() ?? "Intel Core i5";
                        int? roomId = GetRoomId(cboRoom.SelectedItem);

                        _LichThucHanhService.ValidateAndCreateSchedule(
                            date, cboLop.Text.Trim(), tenMon,
                            (int)cboCa.SelectedValue, soSV, reqRam, reqStorage, reqMonitor, reqCpu, roomId, AppSession.MaNguoiDung
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
                var sch = _LichThucHanhService.GetScheduleById(scheduleId);
                if (sch == null) return;

                var req = _LichThucHanhService.GetScheduleRequirements(scheduleId);
                var room = _LichThucHanhService.GetAssignedRoom(scheduleId);

                using (var dlg = CreateScheduleDialog("Sửa Lịch Thực Hành", scheduleId))
                {
                    // ── Load dữ liệu cũ vào form ─────────────────────────────
                    dlg.Shown += (s, ev) =>
                    {
                        FindControl<DateTimePicker>(dlg, "dtpDate").Value = sch.NgayThucHanh;

                        var cboLopCtrl = FindControl<ComboBox>(dlg, "cboLop");
                        int idxLop = cboLopCtrl.FindStringExact(sch.TenLop);
                        
                        var numSV = FindControl<NumericUpDown>(dlg, "numSV");
                        
                        if (idxLop >= 0) 
                        {
                            if (cboLopCtrl.SelectedIndex != idxLop) cboLopCtrl.SelectedIndex = idxLop;
                            
                            // Explicitly trigger the SiSo update
                            if (cboLopCtrl.DataSource is System.Collections.IList ds && idxLop < ds.Count && ds[idxLop] is src.DTO.LopHocDTO lop)
                            {
                                if (lop.SiSo > 0 && lop.SiSo <= numSV.Maximum && lop.SiSo >= numSV.Minimum)
                                {
                                    numSV.Value = lop.SiSo;
                                }
                            }
                        }
                        else 
                        {
                            cboLopCtrl.Text = sch.TenLop;
                        }
                        
                        // If it's an old schedule with default 30, but class has a specific SiSo, prioritize class SiSo
                        if (sch.SoLuongSinhVien != 30) 
                        {
                            numSV.Value = sch.SoLuongSinhVien;
                        }
                        else if (idxLop < 0) 
                        {
                            numSV.Value = sch.SoLuongSinhVien;
                        }

                        // Mon hoc: tu dong lay tu Lop hoc phan, chi override khi lop khong co mon
                        var txtMonHiddenCtrl = FindControl<TextBox>(dlg, "txtMonHidden");
                        var lblMonAutoCtrl   = FindControl<Label>(dlg, "lblMonAuto");
                        if (txtMonHiddenCtrl != null && !string.IsNullOrEmpty(sch.TenMon))
                        {
                            txtMonHiddenCtrl.Text = sch.TenMon;
                            if (lblMonAutoCtrl != null) lblMonAutoCtrl.Text = sch.TenMon;
                        }

                        var cboCaCtrl = FindControl<ComboBox>(dlg, "cboCa");
                        cboCaCtrl.SelectedValue = sch.MaCa;

                        var cboRamCtrl = FindControl<ComboBox>(dlg, "cboInputRAM");
                        var cboStorageCtrl = FindControl<ComboBox>(dlg, "cboInputStorage");
                        var cboMonitorCtrl = FindControl<ComboBox>(dlg, "cboInputMonitor");

                        cboRamCtrl.SelectedItem = req.RAMToiThieu + " GB";
                        cboStorageCtrl.SelectedItem = req.LuuTruToiThieu + " GB";

                        cboMonitorCtrl.SelectedItem = req.ManHinhToiThieu + "\"";
                        if (cboMonitorCtrl.SelectedIndex < 0) cboMonitorCtrl.SelectedIndex = 2;

                        var cboCpuCtrl = FindControl<ComboBox>(dlg, "cboReqCpu");
                        cboCpuCtrl.SelectedItem = req.CPUToiThieu;
                        if (cboCpuCtrl.SelectedIndex < 0) cboCpuCtrl.SelectedIndex = 1;

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
                            DateTime date = FindControl<DateTimePicker>(dlg, "dtpDate").Value;
                            var cboLop = FindControl<ComboBox>(dlg, "cboLop");
                            var txtMonHidden = FindControl<TextBox>(dlg, "txtMonHidden");
                            var cboCa = FindControl<ComboBox>(dlg, "cboCa");
                            var cboRoom = FindControl<ComboBox>(dlg, "cboRoom");
                            var cboRam = FindControl<ComboBox>(dlg, "cboInputRAM");
                            var cboStorage = FindControl<ComboBox>(dlg, "cboInputStorage");
                            var cboMonitor = FindControl<ComboBox>(dlg, "cboInputMonitor");
                            var cboCpu = FindControl<ComboBox>(dlg, "cboReqCpu");

                            string tenMon = txtMonHidden?.Text.Trim() ?? "";
                            if (string.IsNullOrEmpty(tenMon))
                            {
                                MessageBox.Show("Lớp học phần chưa được gắn môn học!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            int soSV = (int)FindControl<NumericUpDown>(dlg, "numSV").Value;
                            int reqRam = Convert.ToInt32(cboRam.SelectedItem.ToString().Replace(" GB", ""));
                            int reqStorage = Convert.ToInt32(cboStorage.SelectedItem.ToString().Replace(" GB", ""));
                            int reqMonitor = Convert.ToInt32(cboMonitor.SelectedItem.ToString().Replace("\"", ""));
                            string reqCpu = cboCpu.SelectedItem?.ToString() ?? "Intel Core i5";

                            int? roomId = GetRoomId(cboRoom.SelectedItem);

                            _LichThucHanhService.ValidateAndUpdateSchedule(
                                scheduleId, date, cboLop.Text.Trim(), tenMon,
                                (int)cboCa.SelectedValue, soSV, reqRam, reqStorage, reqMonitor, reqCpu, roomId, AppSession.MaNguoiDung
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
                    _LichThucHanhService.CancelSchedule(scheduleId);
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
                    _LichThucHanhService.DeleteSchedule(scheduleId);
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
        private Form CreateScheduleDialog(string title, int currentScheduleId = 0)
        {
            var dlg = new Form
            {
                Text = title,
                Size = new Size(460, 680),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            int y = 20;

            // Ngày thực hành
            dlg.Controls.Add(new Label { Text = "Ngày thực hành:", Location = new Point(20, y + 3), AutoSize = true });
            var dtpDate = new DateTimePicker
            {
                Name = "dtpDate",
                Location = new Point(140, y),
                Size = new Size(290, 26),
                Format = DateTimePickerFormat.Short
            };
            dlg.Controls.Add(dtpDate);
            y += 40;

            // Lớp học
            dlg.Controls.Add(new Label { Text = "Lớp:", Location = new Point(20, y + 3), AutoSize = true });
            var cboLop = new ComboBox
            {
                Name = "cboLop",
                Location = new Point(140, y),
                Size = new Size(290, 26),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                IntegralHeight = false,
                MaxDropDownItems = 5
            };
            try
            {
                var dtLop = _LopMonService.GetAllLopHoc().ToList();
                cboLop.DisplayMember = "TenLop"; cboLop.ValueMember = "MaLopHocPhan"; cboLop.DataSource = dtLop;
            }
            catch { cboLop.Items.AddRange(new object[] { "CNTT01", "CNTT02", "KTPM01" }); }
            dlg.Controls.Add(cboLop);
            y += 40;

            // Mon hoc: tu dong hien thi tu Lop hoc phan (khong cho chon rieng)
            dlg.Controls.Add(new Label { Text = "Môn học:", Location = new Point(20, y + 3), AutoSize = true });
            var lblMonAuto = new Label
            {
                Name = "lblMonAuto",
                Text = "--- Chọn lớp để xem ---",
                Location = new Point(140, y + 3),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ThemeColors.PrimaryBlue
            };
            // Hidden textbox de luu ten mon
            var txtMonHidden = new TextBox { Name = "txtMonHidden", Visible = false, Text = "" };
            dlg.Controls.Add(lblMonAuto);
            dlg.Controls.Add(txtMonHidden);
            y += 40;

            // Ca học
            dlg.Controls.Add(new Label { Text = "Ca học:", Location = new Point(20, y + 3), AutoSize = true });
            var cboCa = new ComboBox
            {
                Name = "cboCa",
                Location = new Point(140, y),
                Size = new Size(290, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                IntegralHeight = false,
                MaxDropDownItems = 5
            };
            try
            {
                var dtCa = _LichThucHanhService.GetAllCaHoc();
                cboCa.DisplayMember = "TenCa"; cboCa.ValueMember = "MaCa"; cboCa.DataSource = dtCa;
            }
            catch { cboCa.Items.AddRange(new object[] { "Ca 1 (7:00)", "Ca 2 (9:30)", "Ca 3 (13:00)" }); }
            dlg.Controls.Add(cboCa);
            y += 40;

            // Số sinh viên
            dlg.Controls.Add(new Label { Text = "Số sinh viên:", Location = new Point(20, y + 3), AutoSize = true });
            var numSV = new NumericUpDown
            {
                Name = "numSV",
                Value = 30,
                Minimum = 1,
                Maximum = 200,
                Location = new Point(140, y),
                Size = new Size(120, 26)
            };
            dlg.Controls.Add(numSV);

            Action updateSiSo = () =>
            {
                if (cboLop.SelectedItem is src.DTO.LopHocDTO lop)
                {
                    // Cap nhat si so
                    if (lop.SiSo > 0 && lop.SiSo <= numSV.Maximum && lop.SiSo >= numSV.Minimum)
                        numSV.Value = lop.SiSo;
                    // Cap nhat ten mon tu lop hoc phan
                    if (!string.IsNullOrEmpty(lop.TenMon))
                    {
                        lblMonAuto.Text = lop.TenMon;
                        lblMonAuto.ForeColor = ThemeColors.PrimaryBlue;
                        txtMonHidden.Text = lop.TenMon;
                    }
                    else
                    {
                        lblMonAuto.Text = "(Lớp chưa gắn môn)";
                        lblMonAuto.ForeColor = ThemeColors.AccentRed;
                        txtMonHidden.Text = "";
                    }
                }
            };

            // Auto-fill numSV and Mon based on cboLop
            cboLop.SelectedIndexChanged += (s, ev) => updateSiSo();

            // Trigger initially
            if (cboLop.DataSource is System.Collections.Generic.IEnumerable<src.DTO.LopHocDTO> dList)
            {
                var firstLop = System.Linq.Enumerable.FirstOrDefault(dList);
                if (firstLop != null)
                {
                    if (firstLop.SiSo > 0 && firstLop.SiSo <= numSV.Maximum && firstLop.SiSo >= numSV.Minimum)
                        numSV.Value = firstLop.SiSo;
                    if (!string.IsNullOrEmpty(firstLop.TenMon))
                    {
                        lblMonAuto.Text = firstLop.TenMon;
                        txtMonHidden.Text = firstLop.TenMon;
                    }
                }
            }
            else if (cboLop.Items.Count > 0)
            {
                cboLop.SelectedIndex = 0;
                updateSiSo();
            }

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
            y += 40;

            // CPU Tối thiểu
            dlg.Controls.Add(new Label { Text = "CPU tối thiểu:", Location = new Point(20, y + 3), AutoSize = true });
            var cboReqCpu = new ComboBox { Name = "cboReqCpu", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false, Location = new Point(140, y), Size = new Size(180, 26) };
            cboReqCpu.Items.AddRange(new object[] { "Intel Core i3", "Intel Core i5", "Intel Core i7", "Intel Core i9", "AMD Ryzen 3", "AMD Ryzen 5", "AMD Ryzen 7" });
            cboReqCpu.SelectedItem = "Intel Core i5";
            dlg.Controls.Add(cboReqCpu);
            y += 45;

            // ── Gợi ý phòng tự động ─────────────────────────────────────────
            var pnlRoom = new Panel
            {
                Location = new Point(14, y),
                Size = new Size(420, 90),
                BackColor = Color.FromArgb(241, 249, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlRoom.Controls.Add(new Label
            {
                Text = "🏢  Gợi ý phòng tự động:",
                Location = new Point(8, 8),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ThemeColors.PrimaryBlue
            });

            var cboRoom = new ComboBox
            {
                Name = "cboRoom",
                Location = new Point(8, 32),
                Size = new Size(280, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                IntegralHeight = false,
                MaxDropDownItems = 5
            };
            cboRoom.Items.Add("-- Chưa phân công --");
            cboRoom.SelectedIndex = 0;
            pnlRoom.Controls.Add(cboRoom);

            var btnSuggest = new Button
            {
                Text = "🔍 Gợi ý",
                Location = new Point(298, 31),
                Size = new Size(112, 28),
                BackColor = ThemeColors.PrimaryBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSuggest.FlatAppearance.BorderSize = 0;
            var lblSuggestNote = new Label
            {
                Name = "lblSuggestNote",
                Location = new Point(8, 62),
                Size = new Size(400, 18),
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
                    object caId = cboCa.SelectedValue;
                    DateTime selDate = dtpDate.Value.Date;
                    int svCount = (int)numSV.Value;

                    var cboRam = FindControl<ComboBox>(dlg, "cboInputRAM");
                    var cboStorage = FindControl<ComboBox>(dlg, "cboInputStorage");
                    var cboMonitor = FindControl<ComboBox>(dlg, "cboInputMonitor");
                    var cboCpu = FindControl<ComboBox>(dlg, "cboReqCpu");

                    int reqRam = Convert.ToInt32(cboRam.SelectedItem.ToString().Replace(" GB", ""));
                    int reqStorage = Convert.ToInt32(cboStorage.SelectedItem.ToString().Replace(" GB", ""));
                    int reqMonitor = Convert.ToInt32(cboMonitor.SelectedItem.ToString().Replace("\"", ""));
                    string reqCpu = cboCpu.SelectedItem?.ToString() ?? "Intel Core i5";
                    int found = 0;
                    // Tìm phòng trống (chưa bị chiếm) có đủ sức chứa và đếm số lượng máy đạt cấu hình yêu cầu
                    var dtRooms = _LichThucHanhService.GetRoomsForAssignment(svCount, reqRam, reqStorage, reqMonitor, reqCpu, selDate, (int)caId, currentScheduleId);
                    int roomCount = 0;
                    foreach (var r in dtRooms)
                    {
                        roomCount++;
                        int soMayTot = r.MayTot;
                        int sucChua = r.SucChua;
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
            cboReqCpu.SelectedIndexChanged += (s, ev) => btnSuggest.PerformClick();

            // Buttons
            var btnSave = new Button
            {
                Name = "btnSave",
                Text = "💾  Lưu",
                Size = new Size(120, 38),
                Location = new Point(140, y),
                BackColor = ThemeColors.PrimaryBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 38),
                Location = new Point(270, y),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCancel);

            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCancel;

            dlg.Shown += (s, e) => btnSuggest.PerformClick();

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

        private void lblTitle_Click(object sender, EventArgs e)
        {

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

