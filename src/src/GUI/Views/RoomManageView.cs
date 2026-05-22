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
    public partial class RoomManageView : UserControl
    {
        public RoomManageView()
        {
            InitializeComponent();
            SetupView();
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
            pnlRoomCards.Controls.Clear();

            int totalRooms = 0, available = 0, occupied = 0;
            var rooms = new System.Collections.Generic.List<(int id, string name, string location, int capacity, int computerCount, string status)>();

            try
            {
                var roomService = new src.BLL.RoomService();
                var stats = roomService.GetRoomStats();
                totalRooms = stats.TotalRooms;
                available = stats.Available;
                occupied = stats.Occupied;

                // Lấy danh sách phòng qua BLL -> DAL (Dapper)
                var dtRooms = roomService.GetAllRooms();
                foreach (var r in dtRooms)
                {
                    rooms.Add((r.MaPhong, r.TenPhong, r.ViTri, r.SucChua, r.SoMay, r.StatusEng));
                }
            }
            catch
            {
                // Dữ liệu mẫu
                totalRooms = 6; available = 3; occupied = 2;
                rooms.Add((1, "Lab A-301", "Building A, Floor 3", 30, 30, "available"));
                rooms.Add((2, "Lab A-302", "Building A, Floor 3", 25, 25, "occupied"));
                rooms.Add((3, "Lab B-205", "Building B, Floor 2", 20, 20, "available"));
                rooms.Add((4, "Lab B-206", "Building B, Floor 2", 20, 15, "occupied"));
                rooms.Add((5, "Lab C-102", "Building C, Floor 1", 35, 30, "maintenance"));
                rooms.Add((6, "Lab C-201", "Building C, Floor 2", 30, 28, "available"));
            }

            // === Tạo summary cards theo Figma ===
            pnlStats.Controls.Add(MakeSummaryCard("Total Rooms", totalRooms.ToString(), ThemeColors.TextPrimary));
            pnlStats.Controls.Add(MakeSummaryCard("Available", available.ToString(), ThemeColors.AccentGreen));
            pnlStats.Controls.Add(MakeSummaryCard("Occupied", occupied.ToString(), ThemeColors.AccentRed));

            // === Tạo room cards theo Figma ===
            foreach (var room in rooms)
            {
                pnlRoomCards.Controls.Add(MakeRoomCard(room.id, room.name, room.location,
                    room.capacity, room.computerCount, room.status));
            }
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
            
            Color statusColor = status == "available" ? ThemeColors.AccentGreen :
                                status == "maintenance" ? ThemeColors.AccentOrange : ThemeColors.AccentRed;
            Color badgeBg = status == "available" ? ThemeColors.BadgeGreenBg :
                            status == "maintenance" ? ThemeColors.BadgeOrangeBg : ThemeColors.BadgeRedBg;
            Color badgeFg = status == "available" ? ThemeColors.BadgeGreenFg :
                            status == "maintenance" ? ThemeColors.BadgeOrangeFg : ThemeColors.BadgeRedFg;

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

                TextRenderer.DrawText(g, "Đã cài đặt", new Font("Segoe UI", 9F),
                    new Point(16, infoY + 26), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, $"💻 {computers}", new Font("Segoe UI", 9F, FontStyle.Bold),
                    new Point(card.Width / 2, infoY + 26), ThemeColors.TextPrimary);

                TextRenderer.DrawText(g, "Status", new Font("Segoe UI", 9F),
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
                Text = "✏  Edit", Size = new Size(120, 32), Location = new Point(16, 190),
                FillColor = Color.White, ForeColor = ThemeColors.TextPrimary,
                Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand,
                BorderRadius = 6, BorderThickness = 1, BorderColor = Color.FromArgb(226, 232, 240)
            };
            btnEdit.Click += (s, e) => ShowEditDialog(id, name);
            card.Controls.Add(btnEdit);

            // Nút Delete bằng Guna2Button
            var btnDel = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "🗑", Size = new Size(38, 32), Location = new Point(144, 190),
                FillColor = Color.White, ForeColor = ThemeColors.AccentRed,
                Font = new Font("Segoe UI", 12F), Cursor = Cursors.Hand,
                BorderRadius = 6, BorderThickness = 1, BorderColor = Color.FromArgb(254, 226, 226)
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
                        
                        string cpu     = FindControl<TextBox>(dlg, "txtCPU")?.Text.Trim() ?? "Intel Core i5";
                        int    ram     = (int)(FindControl<NumericUpDown>(dlg, "numRAM")?.Value ?? 8);
                        int    storage = (int)(FindControl<NumericUpDown>(dlg, "numStorage")?.Value ?? 256);
                        int    monitor = (int)(FindControl<NumericUpDown>(dlg, "numMonitor")?.Value ?? 24);

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
                                new SqlParameter("@ten",   tenMay),
                                new SqlParameter("@cpu",   cpu),
                                new SqlParameter("@ram",   ram),
                                new SqlParameter("@sto",   storage),
                                new SqlParameter("@mon",   monitor),
                                new SqlParameter("@phong", maPhong),
                                new SqlParameter("@tt",    ttMayId));
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
                            @"UPDATE PHONG_MAY SET TenPhong=@name, ViTri=@loc, SucChua=@cap, MaTTPhong=@status
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
                var roomService = new src.BLL.RoomService();
                var result = roomService.DeleteRoom(roomId);
                
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
                Text = title, Size = new Size(420, isAdd ? 530 : 350), StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
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
                var numRAM = new NumericUpDown { Name = "numRAM", Minimum = 1, Maximum = 128, Value = 8, Location = new Point(130, y), Size = new Size(100, 26) };
                dlg.Controls.Add(numRAM);
                y += 40;

                dlg.Controls.Add(new Label { Text = "Lưu trữ (GB):", Location = new Point(20, y + 3), AutoSize = true });
                var numStorage = new NumericUpDown { Name = "numStorage", Minimum = 32, Maximum = 4096, Value = 256, Location = new Point(130, y), Size = new Size(100, 26) };
                dlg.Controls.Add(numStorage);
                y += 40;

                dlg.Controls.Add(new Label { Text = "Màn hình (in):", Location = new Point(20, y + 3), AutoSize = true });
                var numMonitor = new NumericUpDown { Name = "numMonitor", Minimum = 10, Maximum = 50, Value = 24, Location = new Point(130, y), Size = new Size(100, 26) };
                dlg.Controls.Add(numMonitor);
                y += 40;
            }

            dlg.Controls.Add(new Label { Text = "Trạng thái:", Location = new Point(20, y + 3), AutoSize = true });
            var cboStatus = new ComboBox
            {
                Name = "cboStatus", Location = new Point(130, y), Size = new Size(250, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new object[] { "Hoạt động", "Bảo trì" });
            cboStatus.SelectedItem = status;
            if (cboStatus.SelectedIndex < 0) cboStatus.SelectedIndex = 0;
            dlg.Controls.Add(cboStatus);
            y += 55;

            var btnSave = new Button
            {
                Text = "💾  Lưu", Size = new Size(120, 38), Location = new Point(130, y),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand, DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Hủy", Size = new Size(100, 38), Location = new Point(260, y),
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
