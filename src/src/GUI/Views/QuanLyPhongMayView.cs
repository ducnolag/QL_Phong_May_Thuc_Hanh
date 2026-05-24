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
    /// Quản lý phòng máy – Giao diện card-based theo Figma.
    /// CRUD đầy đủ: Thêm, Sửa, Xóa phòng. Hiển thị thống kê tổng quan.
    /// </summary>
    public partial class QuanLyPhongMayView : UserControl
    {
        private int currentPage = 1;
        private int pageSize = 6;
        private System.Collections.Generic.List<Control> allRoomCards = new System.Collections.Generic.List<Control>();
        private Guna.UI2.WinForms.Guna2Panel pnlPagination;
        private Button btnPrev;
        private Button btnNext;
        private Label lblPageInfo;

        public QuanLyPhongMayView()
        {
            InitializeComponent();
            SetupPaginationUI();
            SetupView();
        }

        private void SetupPaginationUI()
        {
            pnlPagination = new Guna.UI2.WinForms.Guna2Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            btnPrev = new Button
            {
                Text = "< Trước",
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                FlatAppearance = { BorderColor = Color.FromArgb(226, 232, 240) },
                Font = new Font("Segoe UI", 9F)
            };
            btnPrev.Click += (s, e) => { if (currentPage > 1) { currentPage--; ApplyPagination(); } };

            btnNext = new Button
            {
                Text = "Sau >",
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                FlatAppearance = { BorderColor = Color.FromArgb(226, 232, 240) },
                Font = new Font("Segoe UI", 9F)
            };
            btnNext.Click += (s, e) => { currentPage++; ApplyPagination(); };

            lblPageInfo = new Label
            {
                AutoSize = true,
                Text = "Trang 1 / 1",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ThemeColors.TextPrimary
            };

            pnlPagination.Controls.Add(btnPrev);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(btnNext);

            pnlPagination.Resize += (s, e) =>
            {
                int cx = pnlPagination.Width / 2;
                lblPageInfo.Location = new Point(cx - lblPageInfo.Width / 2, 15);
                btnPrev.Location = new Point(cx - lblPageInfo.Width / 2 - 90, 10);
                btnNext.Location = new Point(cx + lblPageInfo.Width / 2 + 10, 10);
            };

            this.Controls.Add(pnlPagination);
            pnlRoomCards.BringToFront();
        }

        /// <summary>
        /// Thiết lập giao diện và sự kiện
        /// </summary>
        private void SetupView()
        {
            // Bo tròn nút Add
            btnAdd.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            };
            btnAdd.Click += (s, e) => ShowAddDialog();

            LoadData();
        }

        /// <summary>
        /// Tải dữ liệu phòng và hiển thị dạng card
        /// </summary>
        private void LoadData()
        {
            pnlStats.Controls.Clear();
            allRoomCards.Clear();

            int totalRooms = 0, available = 0, occupied = 0;
            var rooms = new System.Collections.Generic.List<(int id, string name, string location, int capacity, int computerCount, string status)>();

            try
            {
                var PhongMayService = new src.BLL.PhongMayService();
                var stats = PhongMayService.GetRoomStats();
                totalRooms = stats.TotalRooms;
                available = stats.Available;
                occupied = stats.Occupied;

                // Lấy danh sách phòng qua BLL -> DAL (Dapper)
                var dtRooms = PhongMayService.GetAllRooms();
                foreach (var r in dtRooms)
                {
                    rooms.Add((r.MaPhong, r.TenPhong, r.ViTri, r.SucChua, r.SoMay, r.TenTrangThaiPhong)); // Use Vietnamese status
                }
            }
            catch
            {
                // Dữ liệu mẫu
                totalRooms = 6; available = 3; occupied = 2;
                rooms.Add((1, "Lab A-301", "Tòa A, Tầng 3", 30, 30, "Hoạt động"));
                rooms.Add((2, "Lab A-302", "Tòa A, Tầng 3", 25, 25, "Đang sử dụng"));
                rooms.Add((3, "Lab B-205", "Tòa B, Tầng 2", 20, 20, "Hoạt động"));
                rooms.Add((4, "Lab B-206", "Tòa B, Tầng 2", 20, 15, "Đang sử dụng"));
                rooms.Add((6, "Lab C-201", "Tòa C, Tầng 2", 30, 28, "Hoạt động"));
            }

            // === Tạo summary cards theo Figma ===
            pnlStats.Controls.Add(MakeSummaryCard("Tổng số phòng", totalRooms.ToString(), ThemeColors.TextPrimary));
            pnlStats.Controls.Add(MakeSummaryCard("Đang hoạt động", available.ToString(), ThemeColors.AccentGreen));
            pnlStats.Controls.Add(MakeSummaryCard("Đóng cửa", occupied.ToString(), ThemeColors.AccentRed));

            // === Tạo room cards theo Figma ===
            foreach (var room in rooms)
            {
                allRoomCards.Add(MakeRoomCard(room.id, room.name, room.location,
                    room.capacity, room.computerCount, room.status));
            }

            currentPage = 1;
            ApplyPagination();
        }

        private void ApplyPagination()
        {
            int totalRecords = allRoomCards.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalRecords / pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            lblPageInfo.Text = $"Trang {currentPage} / {totalPages}";
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;

            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = startIndex + pageSize - 1;

            pnlRoomCards.SuspendLayout();
            pnlRoomCards.Controls.Clear();
            for (int i = startIndex; i <= endIndex && i < totalRecords; i++)
            {
                pnlRoomCards.Controls.Add(allRoomCards[i]);
            }
            pnlRoomCards.ResumeLayout();
        }

        /// <summary>
        /// Tạo thẻ tổng kết nhỏ (Total Rooms, Available, Occupied) bằng Guna2Panel
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

            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 9F), ForeColor = ThemeColors.TextSecondary, Location = new Point(14, 10), AutoSize = true };
            var lblValue = new Label { Text = value, Font = new Font("Segoe UI", 22F, FontStyle.Bold), ForeColor = valueColor, Location = new Point(12, 30), AutoSize = true };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        /// <summary>
        /// Tạo card phòng theo Figma: icon, tên, vị trí, capacity, computers, status, Edit/Delete bằng Guna2Panel
        /// </summary>
        private Guna.UI2.WinForms.Guna2Panel MakeRoomCard(int id, string name, string location, int capacity, int computers, string status)
        {
            var card = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(300, 230),
                Margin = new Padding(6),
                BackColor = Color.Transparent,
                FillColor = Color.White,
                BorderRadius = 12,
                BorderColor = Color.FromArgb(226, 232, 240),
                BorderThickness = 1,
                Tag = id
            };

            Color statusColor = status == "Hoạt động" ? ThemeColors.AccentGreen :
                                status == "Đóng cửa" ? ThemeColors.AccentOrange : ThemeColors.AccentRed;
            Color badgeBg = status == "Hoạt động" ? ThemeColors.BadgeGreenBg :
                            status == "Đóng cửa" ? ThemeColors.BadgeOrangeBg : ThemeColors.BadgeRedBg;
            Color badgeFg = status == "Hoạt động" ? ThemeColors.BadgeGreenFg :
                            status == "Đóng cửa" ? ThemeColors.BadgeOrangeFg : ThemeColors.BadgeRedFg;

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Chấm trạng thái góc phải trên
                using (var br = new SolidBrush(statusColor))
                    g.FillEllipse(br, card.Width - 22, 14, 10, 10);

                // Icon phòng
                using (var br = new SolidBrush(Color.FromArgb(30, ThemeColors.PrimaryBlue)))
                    g.FillEllipse(br, 16, 14, 38, 38);
                TextRenderer.DrawText(g, "🏢", new Font("Segoe UI", 14F),
                    new Rectangle(16, 14, 38, 38), ThemeColors.PrimaryBlue,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // Tên phòng
                TextRenderer.DrawText(g, name, new Font("Segoe UI", 12F, FontStyle.Bold),
                    new Point(62, 14), ThemeColors.TextPrimary);
                TextRenderer.DrawText(g, location, new Font("Segoe UI", 9F),
                    new Point(62, 38), ThemeColors.TextSecondary);

                // Thông tin chi tiết
                int infoY = 70;
                TextRenderer.DrawText(g, "Số lượng máy", new Font("Segoe UI", 9F),
                    new Point(16, infoY), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, $"{capacity} máy", new Font("Segoe UI", 9F, FontStyle.Bold),
                    new Point(card.Width / 2, infoY), ThemeColors.TextPrimary);

                TextRenderer.DrawText(g, "Số máy sẵn sàng", new Font("Segoe UI", 9F),
                    new Rectangle(16, infoY + 26, 150, 20), ThemeColors.TextSecondary, TextFormatFlags.Left);
                TextRenderer.DrawText(g, $"💻 {computers}", new Font("Segoe UI", 9F, FontStyle.Bold),
                    new Point(card.Width / 2, infoY + 26), ThemeColors.TextPrimary);

                TextRenderer.DrawText(g, "Trạng thái", new Font("Segoe UI", 9F),
                    new Point(16, infoY + 52), ThemeColors.TextSecondary);

                // Badge trạng thái
                var sz = TextRenderer.MeasureText(status, new Font("Segoe UI", 8F));
                int bx = card.Width / 2;
                using (var br = new SolidBrush(badgeBg))
                    g.FillRectangle(br, bx + 4, infoY + 50, sz.Width + 4, 18);
                TextRenderer.DrawText(g, status, new Font("Segoe UI", 8F, FontStyle.Bold),
                    new Point(bx + 6, infoY + 52), badgeFg);

                // Đường ngăn cách
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                    g.DrawLine(pen, 16, infoY + 80, card.Width - 16, infoY + 80);
            };

            // Nút Edit bằng Guna2Button
            var btnEdit = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "✏  Sửa",
                Size = new Size(120, 32),
                Location = new Point(16, 190),
                FillColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                BorderRadius = 6,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240)
            };
            btnEdit.Click += (s, e) => ShowEditDialog(id, name);
            card.Controls.Add(btnEdit);

            // Nút Delete bằng Guna2Button
            var btnDel = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "🗑",
                Size = new Size(38, 32),
                Location = new Point(144, 190),
                FillColor = Color.White,
                ForeColor = ThemeColors.AccentRed,
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand,
                BorderRadius = 6,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(254, 226, 226)
            };
            btnDel.Click += (s, e) => DeleteRoom(id, name);
            card.Controls.Add(btnDel);

            card.MouseEnter += (s, e) => { card.FillColor = Color.FromArgb(249, 250, 251); card.Invalidate(); };
            card.MouseLeave += (s, e) => { card.FillColor = Color.White; card.Invalidate(); };
            return card;
        }

        /// <summary>
        /// Hiển thị dialog thêm phòng mới
        /// </summary>
        private void ShowAddDialog()
        {
            using (var dlg = CreateRoomDialog("Thêm Phòng Mới", "", "", 30, "Hoạt động", true))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string name = FindControl<TextBox>(dlg, "txtName").Text.Trim();
                        string location = FindControl<TextBox>(dlg, "txtLocation").Text.Trim();
                        int capacity = (int)FindControl<NumericUpDown>(dlg, "numCapacity").Value;
                        string status = FindControl<ComboBox>(dlg, "cboStatus").SelectedItem?.ToString() ?? "Hoạt động";

                        string cpu = FindControl<TextBox>(dlg, "txtCPU")?.Text.Trim() ?? "Intel Core i5";
                        int ram = 8;
                        int storage = 256;
                        int monitor = 24;
                        var cboR = FindControl<ComboBox>(dlg, "cboInputRAM");
                        if (cboR != null) ram = Convert.ToInt32(cboR.SelectedItem.ToString().Replace(" GB", ""));
                        var cboS = FindControl<ComboBox>(dlg, "cboInputStorage");
                        if (cboS != null) storage = Convert.ToInt32(cboS.SelectedItem.ToString().Replace(" GB", ""));
                        var cboM = FindControl<ComboBox>(dlg, "cboInputMonitor");
                        if (cboM != null) monitor = Convert.ToInt32(cboM.SelectedItem.ToString().Replace("\"", ""));

                        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(location))
                        {
                            MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var statusId = DatabaseHelper.ExecuteScalar(
                            "SELECT MaTTPhong FROM TRANG_THAI_PHONG WHERE TenTrangThaiPhong=@s",
                            new SqlParameter("@s", status));

                        var newRoomId = DatabaseHelper.ExecuteScalar(
                            @"INSERT INTO PHONG_MAY (TenPhong, ViTri, SucChua, MaTTPhong)
                              VALUES (@name, @loc, @cap, @status);
                              SELECT SCOPE_IDENTITY();",
                            new SqlParameter("@name", name),
                            new SqlParameter("@loc", location),
                            new SqlParameter("@cap", capacity),
                            new SqlParameter("@status", statusId));

                        int maPhong = Convert.ToInt32(newRoomId);
                        var ttMayId = DatabaseHelper.ExecuteScalar("SELECT MaTTMay FROM TRANG_THAI_MAY WHERE TenTrangThaiMay=N'Tốt'");
                        if (ttMayId == null || ttMayId == DBNull.Value) ttMayId = 1;

                        for (int i = 1; i <= capacity; i++)
                        {
                            string tenMay = $"{name}-PC{i:D2}";
                            DatabaseHelper.ExecuteNonQuery(
                                @"INSERT INTO MAY_TINH (TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, MaPhong, MaTTMay)
                                  VALUES (@ten, @cpu, @ram, @sto, @mon, @phong, @tt)",
                                new SqlParameter("@ten", tenMay),
                                new SqlParameter("@cpu", cpu),
                                new SqlParameter("@ram", ram),
                                new SqlParameter("@sto", storage),
                                new SqlParameter("@mon", monitor),
                                new SqlParameter("@phong", maPhong),
                                new SqlParameter("@tt", ttMayId));
                        }

                        MessageBox.Show($"Đã thêm phòng và tự động tạo {capacity} máy tính thành công!", "Thành công",
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
        /// Hiển thị dialog sửa phòng
        /// </summary>
        private void ShowEditDialog(int roomId, string roomName)
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT p.TenPhong, p.ViTri, p.SucChua, t.TenTrangThaiPhong
                      FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong
                      WHERE p.MaPhong=@id",
                    new SqlParameter("@id", roomId));

                if (dt.Rows.Count == 0) return;
                var r = dt.Rows[0];

                using (var dlg = CreateRoomDialog("Sửa Phòng: " + roomName,
                    r["TenPhong"].ToString(), r["ViTri"].ToString(),
                    Convert.ToInt32(r["SucChua"]), r["TenTrangThaiPhong"].ToString()))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string name = FindControl<TextBox>(dlg, "txtName").Text.Trim();
                        string location = FindControl<TextBox>(dlg, "txtLocation").Text.Trim();
                        int capacity = (int)FindControl<NumericUpDown>(dlg, "numCapacity").Value;
                        string status = FindControl<ComboBox>(dlg, "cboStatus").SelectedItem?.ToString() ?? "Hoạt động";

                        var statusId = DatabaseHelper.ExecuteScalar(
                            "SELECT MaTTPhong FROM TRANG_THAI_PHONG WHERE TenTrangThaiPhong=@s",
                            new SqlParameter("@s", status));

                        DatabaseHelper.ExecuteNonQuery(
                            @"UPDATE PHONG_MAY SET TenPhong=@name, ViTri=@loc, SucChua=@cap, MaTTPhong=@status, UpdatedAt=GETDATE()
                              WHERE MaPhong=@id",
                            new SqlParameter("@name", name),
                            new SqlParameter("@loc", location),
                            new SqlParameter("@cap", capacity),
                            new SqlParameter("@status", statusId),
                            new SqlParameter("@id", roomId));

                        MessageBox.Show("Đã cập nhật phòng thành công!", "Thành công",
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
        /// Xóa phòng máy
        /// </summary>
        private void DeleteRoom(int roomId, string roomName)
        {
            if (MessageBox.Show($"Bạn có chắc muốn xóa phòng '{roomName}'?\nTất cả máy tính trong phòng cũng sẽ bị xóa!",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var PhongMayService = new src.BLL.PhongMayService();
                var result = PhongMayService.DeleteRoom(roomId);

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Tạo dialog form cho thêm/sửa phòng
        /// </summary>
        private Form CreateRoomDialog(string title, string name, string location, int capacity, string status, bool isAdd = false)
        {
            var dlg = new Form
            {
                Text = title,
                Size = new Size(420, isAdd ? 530 : 350),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            int y = 20;

            dlg.Controls.Add(new Label { Text = "Tên phòng:", Location = new Point(20, y + 3), AutoSize = true });
            var txtName = new TextBox { Name = "txtName", Text = name, Location = new Point(130, y), Size = new Size(250, 26) };
            dlg.Controls.Add(txtName);
            y += 40;

            dlg.Controls.Add(new Label { Text = "Vị trí:", Location = new Point(20, y + 3), AutoSize = true });
            var txtLoc = new TextBox { Name = "txtLocation", Text = location, Location = new Point(130, y), Size = new Size(250, 26) };
            dlg.Controls.Add(txtLoc);
            y += 40;

            dlg.Controls.Add(new Label { Text = "Số lượng máy:", Location = new Point(20, y + 3), AutoSize = true });
            var numCap = new NumericUpDown { Name = "numCapacity", Minimum = 1, Maximum = 200, Value = capacity, Location = new Point(130, y), Size = new Size(100, 26) };
            dlg.Controls.Add(numCap);
            y += 40;

            if (isAdd)
            {
                dlg.Controls.Add(new Label { Text = "Cấu hình chung:", Location = new Point(20, y + 3), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) });
                y += 40;

                dlg.Controls.Add(new Label { Text = "CPU:", Location = new Point(20, y + 3), AutoSize = true });
                var txtCPU = new TextBox { Name = "txtCPU", Text = "Intel Core i5", Location = new Point(130, y), Size = new Size(250, 26) };
                dlg.Controls.Add(txtCPU);
                y += 40;

                dlg.Controls.Add(new Label { Text = "RAM (GB):", Location = new Point(20, y + 3), AutoSize = true });
                var cboInputRam = new ComboBox { Name = "cboInputRAM", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false, Location = new Point(130, y), Size = new Size(100, 26) };
                cboInputRam.Items.AddRange(new object[] { "4 GB", "8 GB", "16 GB", "32 GB", "64 GB" });
                cboInputRam.SelectedItem = "8 GB";
                dlg.Controls.Add(cboInputRam);
                y += 40;

                dlg.Controls.Add(new Label { Text = "Lưu trữ (GB):", Location = new Point(20, y + 3), AutoSize = true });
                var cboInputStorage = new ComboBox { Name = "cboInputStorage", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false, Location = new Point(130, y), Size = new Size(100, 26) };
                cboInputStorage.Items.AddRange(new object[] { "128 GB", "256 GB", "512 GB", "1024 GB" });
                cboInputStorage.SelectedItem = "256 GB";
                dlg.Controls.Add(cboInputStorage);
                y += 40;

                dlg.Controls.Add(new Label { Text = "Màn hình (in):", Location = new Point(20, y + 3), AutoSize = true });
                var cboInputMonitor = new ComboBox { Name = "cboInputMonitor", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false, Location = new Point(130, y), Size = new Size(100, 26) };
                cboInputMonitor.Items.AddRange(new object[] { "19\"", "21\"", "24\"", "27\"" });
                cboInputMonitor.SelectedItem = "24\"";
                dlg.Controls.Add(cboInputMonitor);
                y += 40;
            }

            dlg.Controls.Add(new Label { Text = "Trạng thái:", Location = new Point(20, y + 3), AutoSize = true });
            var cboStatus = new ComboBox
            {
                Name = "cboStatus",
                Location = new Point(130, y),
                Size = new Size(250, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new object[] { "Hoạt động", "Đóng cửa" });
            cboStatus.SelectedItem = status;
            if (cboStatus.SelectedIndex < 0) cboStatus.SelectedIndex = 0;
            dlg.Controls.Add(cboStatus);
            y += 55;

            var btnSave = new Button
            {
                Text = "💾  Lưu",
                Size = new Size(120, 38),
                Location = new Point(130, y),
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
                Location = new Point(260, y),
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
            return dlg;
        }

        private T FindControl<T>(Form form, string name) where T : Control
        {
            foreach (Control c in form.Controls)
                if (c is T t && c.Name == name) return t;
            return null;
        }

        /// <summary>
        /// Màu trạng thái cho các ô bảng (dùng chung ở nhiều view)
        /// </summary>
        public static void ColorStatusCell(DataGridViewCellFormattingEventArgs e, string colName)
        {
            if (e.Value == null) return;
            string v = e.Value.ToString();
            if (v.Contains("Hoạt") || v.Contains("Tốt") || v.Contains("Đã xếp") || v.Contains("active") || v.Contains("available"))
                e.CellStyle.ForeColor = ThemeColors.AccentGreen;
            else if (v.Contains("Bảo") || v.Contains("Chờ") || v.Contains("maintenance"))
                e.CellStyle.ForeColor = ThemeColors.AccentOrange;
            else if (v.Contains("Đóng") || v.Contains("Hỏng") || v.Contains("Vô hiệu") || v.Contains("inactive") || v.Contains("occupied"))
                e.CellStyle.ForeColor = ThemeColors.AccentRed;
            else if (v.Contains("Admin") || v.Contains("admin"))
                e.CellStyle.ForeColor = ThemeColors.AccentPurple;
        }
    }
}

