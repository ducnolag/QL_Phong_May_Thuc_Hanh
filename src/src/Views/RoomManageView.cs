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
                using (var p = UIHelper.GetRoundedRectPath(btnAdd.ClientRectangle, 8))
                    btnAdd.Region = new Region(p);
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
                // Thống kê
                totalRooms = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM PHONG_MAY"));
                available = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong=N'Hoạt động'"));
                occupied = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong!=N'Hoạt động'"));

                // Lấy danh sách phòng
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT p.MaPhong, p.TenPhong, p.ViTri, p.SucChua, t.TenTrangThaiPhong,
                      (SELECT COUNT(*) FROM MAY_TINH m WHERE m.MaPhong=p.MaPhong) AS SoMay
                      FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong
                      ORDER BY p.TenPhong");
                foreach (DataRow r in dt.Rows)
                {
                    string status = r["TenTrangThaiPhong"].ToString();
                    string engStatus = status.Contains("Hoạt") ? "available" : status.Contains("Bảo") ? "maintenance" : "occupied";
                    rooms.Add((Convert.ToInt32(r["MaPhong"]), r["TenPhong"].ToString(),
                        r["ViTri"].ToString(), Convert.ToInt32(r["SucChua"]),
                        Convert.ToInt32(r["SoMay"]), engStatus));
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
        /// Tạo thẻ tổng kết nhỏ (Total Rooms, Available, Occupied)
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
        /// Tạo card phòng theo Figma: icon, tên, vị trí, capacity, computers, status, Edit/Delete
        /// </summary>
        private Panel MakeRoomCard(int id, string name, string location, int capacity, int computers, string status)
        {
            var card = new Panel { Size = new Size(300, 230), Margin = new Padding(6), BackColor = Color.White, Tag = id };
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
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 12))
                    card.Region = new Region(p);
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 12))
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                    g.DrawPath(pen, p);

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
                TextRenderer.DrawText(g, "Capacity", new Font("Segoe UI", 9F),
                    new Point(16, infoY), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, $"{capacity} students", new Font("Segoe UI", 9F, FontStyle.Bold),
                    new Point(card.Width / 2, infoY), ThemeColors.TextPrimary);

                TextRenderer.DrawText(g, "Computers", new Font("Segoe UI", 9F),
                    new Point(16, infoY + 26), ThemeColors.TextSecondary);
                TextRenderer.DrawText(g, $"💻 {computers}", new Font("Segoe UI", 9F, FontStyle.Bold),
                    new Point(card.Width / 2, infoY + 26), ThemeColors.TextPrimary);

                TextRenderer.DrawText(g, "Status", new Font("Segoe UI", 9F),
                    new Point(16, infoY + 52), ThemeColors.TextSecondary);

                // Badge trạng thái
                var sz = TextRenderer.MeasureText(status, new Font("Segoe UI", 8F));
                int bx = card.Width / 2;
                using (var br = new SolidBrush(badgeBg))
                using (var p = UIHelper.GetRoundedRectPath(new Rectangle(bx, infoY + 50, sz.Width + 12, sz.Height + 2), 6))
                    g.FillPath(br, p);
                TextRenderer.DrawText(g, status, new Font("Segoe UI", 8F, FontStyle.Bold),
                    new Point(bx + 6, infoY + 52), badgeFg);

                // Đường ngăn cách
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                    g.DrawLine(pen, 16, infoY + 80, card.Width - 16, infoY + 80);
            };

            // Nút Edit
            var btnEdit = new Button
            {
                Text = "✏  Edit", Size = new Size(120, 32), Location = new Point(16, 190),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = ThemeColors.TextPrimary,
                Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnEdit.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(btnEdit.ClientRectangle, 6))
                    btnEdit.Region = new Region(p);
            };
            btnEdit.Click += (s, e) => ShowEditDialog(id, name);
            card.Controls.Add(btnEdit);

            // Nút Delete
            var btnDel = new Button
            {
                Text = "🗑", Size = new Size(38, 32), Location = new Point(144, 190),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = ThemeColors.AccentRed,
                Font = new Font("Segoe UI", 12F), Cursor = Cursors.Hand
            };
            btnDel.FlatAppearance.BorderColor = Color.FromArgb(254, 226, 226);
            btnDel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(btnDel.ClientRectangle, 6))
                    btnDel.Region = new Region(p);
            };
            btnDel.Click += (s, e) => DeleteRoom(id, name);
            card.Controls.Add(btnDel);

            card.MouseEnter += (s, e) => { card.BackColor = Color.FromArgb(249, 250, 251); card.Invalidate(); };
            card.MouseLeave += (s, e) => { card.BackColor = Color.White; card.Invalidate(); };
            return card;
        }

        /// <summary>
        /// Hiển thị dialog thêm phòng mới
        /// </summary>
        private void ShowAddDialog()
        {
            using (var dlg = CreateRoomDialog("Thêm Phòng Mới", "", "", 30, "Hoạt động"))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string name = FindControl<TextBox>(dlg, "txtName").Text.Trim();
                        string location = FindControl<TextBox>(dlg, "txtLocation").Text.Trim();
                        int capacity = (int)FindControl<NumericUpDown>(dlg, "numCapacity").Value;
                        string status = FindControl<ComboBox>(dlg, "cboStatus").SelectedItem?.ToString() ?? "Hoạt động";

                        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(location))
                        {
                            MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var statusId = DatabaseHelper.ExecuteScalar(
                            "SELECT MaTTPhong FROM TRANG_THAI_PHONG WHERE TenTrangThaiPhong=@s",
                            new SqlParameter("@s", status));

                        DatabaseHelper.ExecuteNonQuery(
                            @"INSERT INTO PHONG_MAY (TenPhong, ViTri, SucChua, MaTTPhong)
                              VALUES (@name, @loc, @cap, @status)",
                            new SqlParameter("@name", name),
                            new SqlParameter("@loc", location),
                            new SqlParameter("@cap", capacity),
                            new SqlParameter("@status", statusId));

                        MessageBox.Show("Đã thêm phòng thành công!", "Thành công",
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
                try
                {
                    // Xóa máy tính trước (FK constraint)
                    DatabaseHelper.ExecuteNonQuery("DELETE FROM MAY_TINH WHERE MaPhong=@id",
                        new SqlParameter("@id", roomId));
                    DatabaseHelper.ExecuteNonQuery("DELETE FROM PHONG_MAY WHERE MaPhong=@id",
                        new SqlParameter("@id", roomId));
                    MessageBox.Show("Đã xóa phòng thành công!", "Thành công",
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
        /// Tạo dialog form cho thêm/sửa phòng
        /// </summary>
        private Form CreateRoomDialog(string title, string name, string location, int capacity, string status)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(420, 350), StartPosition = FormStartPosition.CenterParent,
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

            dlg.Controls.Add(new Label { Text = "Sức chứa:", Location = new Point(20, y + 3), AutoSize = true });
            var numCap = new NumericUpDown { Name = "numCapacity", Value = capacity, Minimum = 1, Maximum = 200, Location = new Point(130, y), Size = new Size(100, 26) };
            dlg.Controls.Add(numCap);
            y += 40;

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
