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
    /// Quản lý người dùng – CRUD đầy đủ: Thêm, Sửa, Xóa, Tìm kiếm.
    /// Giao diện theo Figma: bảng với cột Username, Email, Role, Status, Created At, Actions.
    /// </summary>
    public partial class UserManageView : UserControl
    {
        public UserManageView()
        {
            InitializeComponent();
            SetupView();
        }

        /// <summary>
        /// Thiết lập giao diện, sự kiện, và tải dữ liệu ban đầu
        /// </summary>
        private void SetupView()
        {
            // Bo tròn + shadow cho toolbar và grid panel
            UIHelper.ApplyCardStyle(pnlToolbar, 14);
            UIHelper.ApplyCardStyle(pnlGrid, 14);

            // Thiết lập bảng dữ liệu
            SetupGridStyles();
            LoadData();

            // Sự kiện tìm kiếm
            txtSearch.TextChanged += (s, e) => FilterRows();

            // Sự kiện thêm người dùng
            btnAdd.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(btnAdd.ClientRectangle, 8))
                    btnAdd.Region = new Region(p);
            };
            btnAdd.Click += (s, e) => ShowAddDialog();
        }

        private void SetupGridStyles()
        {
            dgv.Columns.Clear();

            // Các cột dữ liệu
            dgv.Columns.Add("TenDN",    "Username");
            dgv.Columns.Add("Email",    "Email");
            dgv.Columns.Add("MatKhau",  "Password");   // cột mật khẩu
            dgv.Columns.Add("VaiTro",   "Role");
            dgv.Columns.Add("TrangThai","Status");
            dgv.Columns.Add("NgayTao",  "Created At");

            // Cột 👁 – xem & đặt lại mật khẩu (admin dùng để set mật khẩu mới cho nhân viên)
            var colEye = new DataGridViewButtonColumn
            {
                Name = "ViewPass", HeaderText = "🔑", Text = "👁",
                UseColumnTextForButtonValue = true, Width = 42, FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(colEye);

            // Cột ✏ Edit – sửa thông tin user (email, role, trạng thái)
            var colEdit = new DataGridViewButtonColumn
            {
                Name = "Edit", HeaderText = "", Text = "✏",
                UseColumnTextForButtonValue = true, Width = 38, FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(colEdit);

            // Cột 🗑 Delete
            var colDel = new DataGridViewButtonColumn
            {
                Name = "Delete", HeaderText = "", Text = "🗑",
                UseColumnTextForButtonValue = true, Width = 38, FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(colDel);

            // Độ rộng cột
            dgv.Columns["TenDN"].Width     = 120;
            dgv.Columns["Email"].Width     = 175;
            dgv.Columns["MatKhau"].Width   = 120;
            dgv.Columns["VaiTro"].Width    = 85;
            dgv.Columns["TrangThai"].Width = 80;
            dgv.Columns["NgayTao"].Width   = 100;
            dgv.Columns["MatKhau"].HeaderText = "Password";

            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.CellClick      += Dgv_CellClick;

            dgv.Font = new Font("Segoe UI", 9.5F);
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(249, 250, 251),
                SelectionForeColor = ThemeColors.TextSecondary,
                Padding = new Padding(6),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(6)
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary
            };
        }

        /// <summary>
        /// Tải dữ liệu người dùng từ database.
        /// Cột MatKhau luôn hiển thị '••••••••' (SHA256 không thể giải mã ngược).
        /// Admin dùng nút Reset để đặt lại và thông báo mật khẩu mới cho nhân viên.
        /// </summary>
        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT nd.MaNguoiDung, nd.TenDangNhap, nd.Email,
                      vt.TenVaiTro, nd.TrangThai, nd.CreatedAt
                      FROM NGUOI_DUNG nd
                      JOIN VAI_TRO vt ON nd.MaVaiTro = vt.MaVaiTro
                      ORDER BY nd.MaNguoiDung");
                foreach (DataRow r in dt.Rows)
                {
                    bool active    = Convert.ToBoolean(r["TrangThai"]);
                    string role    = r["TenVaiTro"].ToString().ToLower();
                    string status  = active ? "active" : "inactive";
                    string created = Convert.ToDateTime(r["CreatedAt"]).ToString("yyyy-MM-dd");

                    // SHA256 là one-way hash – luôn hiển thị dấu chấm, dùng nút Reset để đặt lại
                    int idx = dgv.Rows.Add(r["TenDangNhap"], r["Email"], "••••••••", role, status, created);
                    dgv.Rows[idx].Tag = r["MaNguoiDung"];
                }
            }
            catch
            {
                // Dữ liệu mẫu khi không kết nối được DB
                dgv.Rows.Add("admin",      "admin@pcroom.com",  "••••••••",  "admin",    "active",   "2024-01-15");
                dgv.Rows.Add("john_doe",   "john@pcroom.com",   "••••••••",  "nhanvien", "active",   "2024-02-20");
                dgv.Rows.Add("jane_smith", "jane@pcroom.com",   "••••••••",  "nhanvien", "active",   "2024-03-10");
                dgv.Rows.Add("mike_admin", "mike@pcroom.com",   "••••••••",  "admin",    "active",   "2024-04-05");
                dgv.Rows.Add("sarah_lee",  "sarah@pcroom.com",  "••••••••",  "nhanvien", "inactive", "2024-05-01");
            }
        }


        /// <summary>
        /// Định dạng ô: role và status hiển thị dạng badge màu theo Figma
        /// </summary>
        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgv.Columns[e.ColumnIndex].Name;
            string val = e.Value?.ToString() ?? "";

            if (col == "MatKhau")
            {
                // Luôn hiển thị dấu chấm (SHA256 không giải mã được, dùng nút Reset)
                e.Value = "••••••••";
                e.CellStyle.ForeColor = Color.FromArgb(180, 180, 180);
                e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
                e.FormattingApplied = true;
            }
            else if (col == "VaiTro")
            {
                e.CellStyle.ForeColor = val.Contains("admin") ? ThemeColors.BadgePurpleFg : ThemeColors.BadgeBlueFg;
                e.CellStyle.BackColor = val.Contains("admin") ? ThemeColors.BadgePurpleBg : ThemeColors.BadgeBlueBg;
                e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            else if (col == "TrangThai")
            {
                e.CellStyle.ForeColor = val == "active" ? ThemeColors.BadgeGreenFg : ThemeColors.BadgeRedFg;
                e.CellStyle.BackColor = val == "active" ? ThemeColors.BadgeGreenBg : ThemeColors.BadgeRedBg;
                e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Xử lý click vào cột Edit/Delete
        /// </summary>
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgv.Columns[e.ColumnIndex].Name;

            if (colName == "ViewPass")
                ShowResetPasswordDialog(e.RowIndex);  // 👁 xem & đặt mật khẩu mới
            else if (colName == "Edit")
                ShowEditDialog(e.RowIndex);            // ✏ sửa thông tin
            else if (colName == "Delete")
                DeleteUser(e.RowIndex);                // 🗑 xóa
        }

        /// <summary>
        /// Dialog 👁 xem/đặt lại mật khẩu.
        /// Giải thích: SHA256 là mã một chiều, không đọc được mật khẩu cũ.
        /// Admin nhập mật khẩu mới (hiện rõ để copy), hệ thống lưu dạng hash.
        /// </summary>
        private void ShowResetPasswordDialog(int rowIndex)
        {
            var row = dgv.Rows[rowIndex];
            string username = row.Cells["TenDN"].Value?.ToString() ?? "";

            var dlg = new Form
            {
                Text = $"👁  Mật Khẩu – {username}",
                Size = new Size(420, 260),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };

            // Tiêu đề
            dlg.Controls.Add(new Label
            {
                Text = $"👤  {username}",
                Location = new Point(20, 18), AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ThemeColors.PrimaryBlue
            });

            // Giải thích lý do không xem được mật khẩu cũ
            var pnlNote = new Panel { Location = new Point(20, 50), Size = new Size(375, 44), BackColor = Color.FromArgb(254, 243, 199) };
            pnlNote.Controls.Add(new Label
            {
                Text = "⚠  Mật khẩu được mã hóa SHA256 (một chiều),\nkhông thể đọc lại được. Hãy đặt mật khẩu mới bên dưới.",
                Location = new Point(8, 5), Size = new Size(360, 36),
                Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(120, 80, 0)
            });
            dlg.Controls.Add(pnlNote);

            // Label + TextBox mật khẩu mới
            dlg.Controls.Add(new Label { Text = "Mật khẩu mới:", Location = new Point(20, 108), AutoSize = true });
            var txtPw = new TextBox
            {
                Location = new Point(135, 105), Size = new Size(220, 26),
                UseSystemPasswordChar = false   // hiện rõ mặc định để admin thấy/copy
            };
            dlg.Controls.Add(txtPw);

            // Nút 👁 toggle ẩn/hiện
            var btnEye = new Button
            {
                Text = "👁", Location = new Point(360, 103), Size = new Size(36, 28),
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 11F)
            };
            btnEye.FlatAppearance.BorderSize = 0;
            btnEye.Click += (s, e) =>
            {
                txtPw.UseSystemPasswordChar = !txtPw.UseSystemPasswordChar;
                btnEye.Text = txtPw.UseSystemPasswordChar ? "🙈" : "👁";
            };
            dlg.Controls.Add(btnEye);

            dlg.Controls.Add(new Label
            {
                Text = "💡 Mật khẩu hiện rõ để admin copy và thông báo cho nhân viên.",
                Location = new Point(20, 138), Size = new Size(375, 18),
                Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(100, 116, 139)
            });

            // Buttons
            var btnSave = new Button
            {
                Text = "💾  Lưu mật khẩu", DialogResult = DialogResult.OK,
                Location = new Point(195, 175), Size = new Size(130, 34),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            var btnCancel = new Button
            {
                Text = "Hủy", DialogResult = DialogResult.Cancel,
                Location = new Point(335, 175), Size = new Size(65, 34),
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            dlg.Controls.Add(btnSave);
            dlg.Controls.Add(btnCancel);
            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCancel;

            // Focus vào textbox khi mở
            dlg.Shown += (s, e) => txtPw.Focus();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string newPw = txtPw.Text.Trim();
                if (string.IsNullOrEmpty(newPw))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    DatabaseHelper.ExecuteNonQuery(
                        "UPDATE NGUOI_DUNG SET MatKhauDaMaHoa=@pass WHERE TenDangNhap=@user",
                        new SqlParameter("@pass", DatabaseHelper.HashPassword(newPw)),
                        new SqlParameter("@user", username));

                    MessageBox.Show(
                        $"✅ Đã đặt lại mật khẩu thành công!\n\n" +
                        $"Tài khoản : {username}\n" +
                        $"Mật khẩu mới: {newPw}\n\n" +
                        "📋 Hãy thông báo mật khẩu này cho nhân viên.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        /// <summary>
        /// Lọc bảng dữ liệu theo từ khóa tìm kiếm
        /// </summary>
        private void FilterRows()
        {
            string kw = txtSearch.Text.Trim().ToLower();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                bool show = string.IsNullOrEmpty(kw);
                if (!show)
                    foreach (DataGridViewCell c in row.Cells)
                        if (c.Value != null && c.Value.ToString().ToLower().Contains(kw)) { show = true; break; }
                row.Visible = show;
            }
        }

        /// <summary>
        /// Hiển thị dialog thêm người dùng mới
        /// </summary>
        private void ShowAddDialog()
        {
            using (var dlg = CreateUserDialog("Thêm Người Dùng Mới", "", "", "", "", true, true))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string username = FindControl<TextBox>(dlg, "txtUsername").Text.Trim();
                        string email    = FindControl<TextBox>(dlg, "txtEmail").Text.Trim();
                        string password = FindControl<TextBox>(dlg, "txtPassword").Text.Trim();
                        string role     = FindControl<ComboBox>(dlg, "cboRole").SelectedItem?.ToString() ?? "NhanVien";
                        bool active     = FindControl<CheckBox>(dlg, "chkActive").Checked;

                        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                        {
                            MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var roleId   = DatabaseHelper.ExecuteScalar(
                            "SELECT MaVaiTro FROM VAI_TRO WHERE TenVaiTro = @role",
                            new SqlParameter("@role", role));
                        string hashedPw = DatabaseHelper.HashPassword(password);

                        // Lưu plain-text vào SoDienThoai để admin có thể xem lại
                        DatabaseHelper.ExecuteNonQuery(
                            @"INSERT INTO NGUOI_DUNG
                              (TenDangNhap, MatKhauDaMaHoa, HoTen, Email, SoDienThoai, TrangThai, MaVaiTro)
                              VALUES (@user, @pass, @name, @email, @hint, @status, @role)",
                            new SqlParameter("@user",   username),
                            new SqlParameter("@pass",   hashedPw),
                            new SqlParameter("@name",   username),
                            new SqlParameter("@email",  email),
                            new SqlParameter("@hint",   password),   // plain text hint
                            new SqlParameter("@status", active ? 1 : 0),
                            new SqlParameter("@role",   roleId));

                        MessageBox.Show("Đã thêm người dùng thành công!", "Thành công",
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
        /// Hiển thị dialog sửa thông tin người dùng
        /// </summary>
        private void ShowEditDialog(int rowIndex)
        {
            var row = dgv.Rows[rowIndex];
            string username = row.Cells["TenDN"].Value?.ToString() ?? "";
            string email    = row.Cells["Email"].Value?.ToString() ?? "";
            string role     = row.Cells["VaiTro"].Value?.ToString() ?? "";
            bool active     = row.Cells["TrangThai"].Value?.ToString() == "active";

            using (var dlg = CreateUserDialog("Sửa Người Dùng", username, email, "", role, active, false))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string newEmail    = FindControl<TextBox>(dlg, "txtEmail").Text.Trim();
                        string newRole     = FindControl<ComboBox>(dlg, "cboRole").SelectedItem?.ToString() ?? "NhanVien";
                        bool newActive     = FindControl<CheckBox>(dlg, "chkActive").Checked;
                        string newPassword = FindControl<TextBox>(dlg, "txtPassword").Text.Trim();

                        var roleId = DatabaseHelper.ExecuteScalar(
                            "SELECT MaVaiTro FROM VAI_TRO WHERE TenVaiTro = @role",
                            new SqlParameter("@role", newRole));

                        // Luôn cập nhật email, trạng thái, vai trò
                        string sql = @"UPDATE NGUOI_DUNG SET Email=@email, TrangThai=@status, MaVaiTro=@role";
                        var pars = new System.Collections.Generic.List<SqlParameter>
                        {
                            new SqlParameter("@email",  newEmail),
                            new SqlParameter("@status", newActive ? 1 : 0),
                            new SqlParameter("@role",   roleId),
                            new SqlParameter("@user",   username)
                        };

                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            // Cập nhật mật khẩu hash + lưu plain text vào SoDienThoai
                            sql += ", MatKhauDaMaHoa=@pass, SoDienThoai=@hint";
                            pars.Add(new SqlParameter("@pass", DatabaseHelper.HashPassword(newPassword)));
                            pars.Add(new SqlParameter("@hint", newPassword)); // plain text hint
                        }
                        sql += " WHERE TenDangNhap=@user";

                        DatabaseHelper.ExecuteNonQuery(sql, pars.ToArray());
                        MessageBox.Show("Đã cập nhật thành công!", "Thành công",
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
        /// Xóa người dùng
        /// </summary>
        private void DeleteUser(int rowIndex)
        {
            var row = dgv.Rows[rowIndex];
            string username = row.Cells["TenDN"].Value?.ToString() ?? "";

            if (username == "admin")
            {
                MessageBox.Show("Không thể xóa tài khoản admin!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa người dùng '{username}'?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.ExecuteNonQuery(
                        "DELETE FROM NGUOI_DUNG WHERE TenDangNhap = @user",
                        new SqlParameter("@user", username));
                    MessageBox.Show("Đã xóa thành công!", "Thành công",
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
        /// Tạo dialog form cho thêm/sửa người dùng
        /// </summary>
        private Form CreateUserDialog(string title, string username, string email,
            string password, string role, bool active, bool isNew)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(420, 400), StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };

            int y = 20, inputW = 270;

            // Username
            dlg.Controls.Add(new Label { Text = "Username:", Location = new Point(20, y + 3), AutoSize = true });
            var txtUser = new TextBox { Name = "txtUsername", Text = username, Location = new Point(130, y), Size = new Size(inputW, 26), ReadOnly = !isNew };
            dlg.Controls.Add(txtUser);
            y += 40;

            // Email
            dlg.Controls.Add(new Label { Text = "Email:", Location = new Point(20, y + 3), AutoSize = true });
            var txtEmail = new TextBox { Name = "txtEmail", Text = email, Location = new Point(130, y), Size = new Size(inputW, 26) };
            dlg.Controls.Add(txtEmail);
            y += 40;

            // Password
            dlg.Controls.Add(new Label { Text = isNew ? "Password:" : "New Pass:", Location = new Point(20, y + 3), AutoSize = true });
            var txtPass = new TextBox { Name = "txtPassword", Text = password, Location = new Point(130, y), Size = new Size(inputW, 26), UseSystemPasswordChar = true };
            dlg.Controls.Add(txtPass);
            y += 40;

            // Role
            dlg.Controls.Add(new Label { Text = "Role:", Location = new Point(20, y + 3), AutoSize = true });
            var cboRole = new ComboBox
            {
                Name = "cboRole", Location = new Point(130, y), Size = new Size(inputW, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboRole.Items.AddRange(new object[] { "Admin", "NhanVien" });
            cboRole.SelectedItem = role.Contains("admin") || role.Contains("Admin") ? "Admin" : "NhanVien";
            dlg.Controls.Add(cboRole);
            y += 40;

            // Active
            var chkActive = new CheckBox { Name = "chkActive", Text = "Active", Checked = active, Location = new Point(130, y), AutoSize = true };
            dlg.Controls.Add(chkActive);
            y += 50;

            // Nút Lưu & Hủy
            var btnSave = new Button
            {
                Text = "💾  Lưu", Size = new Size(120, 38), Location = new Point(130, y),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Hủy", Size = new Size(100, 38), Location = new Point(260, y),
                BackColor = Color.FromArgb(241, 245, 249), ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCancel);

            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCancel;
            return dlg;
        }

        /// <summary>
        /// Tìm control theo tên trong form
        /// </summary>
        private T FindControl<T>(Form form, string name) where T : Control
        {
            foreach (Control c in form.Controls)
                if (c is T t && c.Name == name) return t;
            return null;
        }
    }
}
