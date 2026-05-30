using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using src.Helpers;

namespace src.Views
{
    /// <summary>
    /// Quản lý người dùng – CRUD: Thêm, Sửa, Xóa, Tìm kiếm.
    /// Chỉ có 2 nút hành động: ✏ Sửa và 🗑 Xóa.
    /// Dialog Sửa cho phép xem/đổi mật khẩu có icon mắt toggle.
    /// </summary>
    public partial class QuanLyNguoiDungView : UserControl
    {
        private readonly src.BLL.NguoiDungService _NguoiDungService;

        public QuanLyNguoiDungView()
        {
            InitializeComponent();
            _NguoiDungService = new src.BLL.NguoiDungService();
            SetupView();
        }

        private void SetupView()
        {


            SetupGridStyles();
            LoadData();
            txtSearch.TextChanged += (s, e) => FilterRows();
            btnAdd.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            };
            btnAdd.Click += (s, e) => ShowAddDialog();
        }

        // ── Cấu hình bảng ────────────────────────────────────────────────
        private void SetupGridStyles()
        {
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "TenDN", HeaderText = "Tên đăng nhập", Width = 140, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "HoTen", HeaderText = "Họ tên", Width = 150, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Email", HeaderText = "Email", Width = 185, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "SoDienThoai", HeaderText = "Số điện thoại", Width = 110, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "TrangThai", HeaderText = "Trạng thái", Width = 90, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "NgayTao", HeaderText = "Ngày tạo", Width = 100, ReadOnly = true });

            // Nút sửa
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "Sửa",
                Text = "✏  Sửa",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            });

            // Nút xóa
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Xóa",
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Width = 50,
                FlatStyle = FlatStyle.Flat
            });

            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.CellClick += Dgv_CellClick;

            dgv.Columns["Edit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Edit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Delete"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Delete"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Font = new Font("Segoe UI", 9.5F);
            dgv.RowTemplate.Height = 46;
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(249, 250, 251),
                SelectionForeColor = ThemeColors.TextSecondary,
                Padding = new Padding(8, 0, 0, 0),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary
            };
        }

        // ── Tải dữ liệu ──────────────────────────────────────────────────
        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var users = _NguoiDungService.GetAllUsers();
                foreach (var r in users)
                {
                    if (r.TenDangNhap.Equals("admin", StringComparison.OrdinalIgnoreCase) || 
                        r.TenVaiTro.Equals("Admin", StringComparison.OrdinalIgnoreCase)) continue;

                    bool active = r.TrangThai;
                    string status = active ? "active" : "inactive";
                    string ngay = r.CreatedAt.ToString("yyyy-MM-dd");

                    int idx = dgv.Rows.Add(
                        r.TenDangNhap, r.HoTen, r.Email,
                        r.SoDienThoai, status, ngay);
                    // Tag lưu MaNguoiDung và trạng thái active
                    dgv.Rows[idx].Tag = (id: r.MaNguoiDung, active: active);
                }
            }
            catch
            {
                dgv.Rows.Add("admin", "Administrator", "admin@lab.vn", "0123456789", "active", "2024-01-15");
                dgv.Rows.Add("nhanvien1", "Trần Thị Bình", "binh@lab.vn", "0987654321", "active", "2024-02-20");
                dgv.Rows.Add("nhanvien2", "Lê Hoàng Nam", "nam@lab.vn", "0912345678", "inactive", "2024-03-10");
            }
        }

        // ── Định dạng badge ───────────────────────────────────────────────
        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgv.Columns[e.ColumnIndex].Name;
            string val = e.Value?.ToString() ?? "";

            if (col == "TrangThai")
            {
                bool active = val == "active";
                e.CellStyle.ForeColor = active ? ThemeColors.BadgeGreenFg : ThemeColors.BadgeRedFg;
                e.CellStyle.BackColor = active ? ThemeColors.BadgeGreenBg : ThemeColors.BadgeRedBg;
                e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                e.Value = active ? "✔ Hoạt động" : "✘ Vô hiệu hóa";
                e.FormattingApplied = true;
            }
        }

        // ── Xử lý click nút ──────────────────────────────────────────────
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgv.Columns[e.ColumnIndex].Name;
            if (col == "Edit") ShowEditDialog(e.RowIndex);
            else if (col == "Delete") DeleteUser(e.RowIndex);
        }

        // ── Tìm kiếm ─────────────────────────────────────────────────────
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

        // ── Dialog Thêm ───────────────────────────────────────────────────
        private void ShowAddDialog()
        {
            using var dlg = BuildUserDialog("Thêm Nhân Viên Mới", "", "", "", "", "", true, true);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                string username = Find<TextBox>(dlg, "txtUsername").Text.Trim();
                string hoTen = Find<TextBox>(dlg, "txtHoTen").Text.Trim();
                string email = Find<TextBox>(dlg, "txtEmail").Text.Trim();
                string password = Find<TextBox>(dlg, "txtPassword").Text.Trim();
                string phone = Find<TextBox>(dlg, "txtPhone").Text.Trim();
                bool active = Find<CheckBox>(dlg, "chkActive").Checked;

                _NguoiDungService.CreateUser(username, password, hoTen, email, phone, active);

                MessageBox.Show("Đã thêm nhân viên thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Dialog Sửa ───────────────────────────────────────────────────
        private void ShowEditDialog(int rowIndex)
        {
            var row = dgv.Rows[rowIndex];
            string username = row.Cells["TenDN"].Value?.ToString() ?? "";
            string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
            string email = row.Cells["Email"].Value?.ToString() ?? "";
            string phone = row.Cells["SoDienThoai"].Value?.ToString() ?? "";
            // raw value lưu là "active"/"inactive" (lowercase)
            bool active = row.Cells["TrangThai"].Value?.ToString() == "active";

            // Mật khẩu đã hash – để trống, admin nhập mới nếu muốn đổi
            using var dlg = BuildUserDialog("Chỉnh Sửa Nhân Viên", username, hoTen, email, "", phone, active, false);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                string newHoTen = Find<TextBox>(dlg, "txtHoTen").Text.Trim();
                string newEmail = Find<TextBox>(dlg, "txtEmail").Text.Trim();
                string newPw = Find<TextBox>(dlg, "txtPassword").Text.Trim();
                string newPhone = Find<TextBox>(dlg, "txtPhone").Text.Trim();
                bool newActive = Find<CheckBox>(dlg, "chkActive").Checked;

                _NguoiDungService.UpdateUser(username, newPw, newHoTen, newEmail, newPhone, newActive);

                // Nếu đang sửa chính tài khoản đang đăng nhập → cập nhật sidebar
                if (this.TopLevelControl is SidebarForm mf && string.Equals(mf._currentUser, username, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(newHoTen))
                    mf.UpdateSidebarName(newHoTen);

                MessageBox.Show("Cập nhật thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Xóa người dùng ────────────────────────────────────────────────
        private void DeleteUser(int rowIndex)
        {
            var row = dgv.Rows[rowIndex];
            string username = row.Cells["TenDN"].Value?.ToString() ?? "";

            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không thể xóa tài khoản admin!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(row.Tag is ValueTuple<int, bool> tagInfo)) return;
            int userId = tagInfo.Item1;
            bool active = tagInfo.Item2;

            try
            {
                if (MessageBox.Show($"Xóa nhân viên '{username}'?", "Xác nhận xóa",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                _NguoiDungService.DeleteUser(userId, username, active);

                MessageBox.Show("Đã xóa thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Tạo dialog Thêm/Sửa ──────────────────────────────────────────
        /// <summary>
        /// Tạo form dialog dùng chung cho Thêm và Sửa.
        /// password = mật khẩu plain-text đã lưu (hint), hiển thị sẵn khi Sửa.
        /// </summary>
        private Form BuildUserDialog(string title, string username, string hoTen,
            string email, string password, string phone, bool active, bool isNew)
        {
            var dlg = new Form
            {
                Text = title,
                Size = new Size(460, isNew ? 430 : 420),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            int y = 20, lx = 20, tx = 145, inputW = 268;

            // ── Hàm tiện ích thêm label ──
            void AddLabel(string text, int posY) =>
                dlg.Controls.Add(new Label
                {
                    Text = text,
                    Location = new Point(lx, posY + 3),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9.5F),
                    ForeColor = ThemeColors.TextSecondary
                });

            // ── Username (readonly khi sửa) ──
            AddLabel("Tên đăng nhập *", y);
            var txtUser = new TextBox
            {
                Name = "txtUsername",
                Text = username,
                Location = new Point(tx, y),
                Size = new Size(inputW, 26),
                ReadOnly = !isNew,
                BackColor = isNew ? Color.White : Color.FromArgb(245, 247, 250),
                Font = new Font("Segoe UI", 10F)
            };
            dlg.Controls.Add(txtUser);
            y += 42;

            // ── Họ tên ──
            AddLabel("Họ và tên *", y);
            var txtHoTen = new TextBox
            {
                Name = "txtHoTen",
                Text = hoTen,
                Location = new Point(tx, y),
                Size = new Size(inputW, 26),
                Font = new Font("Segoe UI", 10F)
            };
            dlg.Controls.Add(txtHoTen);
            y += 42;

            // ── Email ──
            AddLabel("Email *", y);
            var txtEmail = new TextBox
            {
                Name = "txtEmail",
                Text = email,
                Location = new Point(tx, y),
                Size = new Size(inputW, 26),
                Font = new Font("Segoe UI", 10F)
            };
            dlg.Controls.Add(txtEmail);
            y += 42;

            // ── Mật khẩu + nút mắt ──
            AddLabel(isNew ? "Mật khẩu *" : "Mật khẩu", y);

            // TextBox mật khẩu (hiện sẵn hint khi Sửa)
            var txtPass = new TextBox
            {
                Name = "txtPassword",
                Text = password,
                Location = new Point(tx, y),
                Size = new Size(inputW - 36, 26),
                UseSystemPasswordChar = false,   // hiện rõ để admin biết/copy
                Font = new Font("Segoe UI", 10F),
                BackColor = string.IsNullOrEmpty(password)
                    ? Color.White : Color.FromArgb(240, 255, 245)
            };
            dlg.Controls.Add(txtPass);

            // Nút mắt GDI+
            bool passVisible = true;
            var btnEye = new Button
            {
                Location = new Point(tx + inputW - 34, y - 1),
                Size = new Size(34, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                TabStop = false,
                Text = ""
            };
            btnEye.FlatAppearance.BorderSize = 0;
            btnEye.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnEye.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int cx = btnEye.Width / 2, cy = btnEye.Height / 2;
                Color col = passVisible
                    ? Color.FromArgb(45, 75, 205)
                    : Color.FromArgb(160, 165, 175);
                using var pen = new Pen(col, 1.8f);
                using var brush = new SolidBrush(col);
                // Hình con mắt
                var eye = new Rectangle(cx - 9, cy - 5, 18, 10);
                g.DrawArc(pen, eye, 200, 140);
                g.DrawArc(pen, eye, 20, 140);
                g.FillEllipse(brush, cx - 3, cy - 3, 6, 6);
                // Đường gạch chéo khi đang ẩn
                if (!passVisible)
                {
                    using var slash = new Pen(col, 2f)
                    {
                        StartCap = LineCap.Round,
                        EndCap = LineCap.Round
                    };
                    g.DrawLine(slash, cx - 8, cy + 7, cx + 8, cy - 7);
                }
            };
            btnEye.Click += (s, ev) =>
            {
                passVisible = !passVisible;
                txtPass.UseSystemPasswordChar = !passVisible;
                txtPass.BackColor = passVisible && !string.IsNullOrEmpty(txtPass.Text)
                    ? Color.FromArgb(240, 255, 245) : Color.White;
                btnEye.Invalidate();
            };
            dlg.Controls.Add(btnEye);

            // Ghi chú nhỏ
            if (!isNew && !string.IsNullOrEmpty(password))
            {
                dlg.Controls.Add(new Label
                {
                    Text = "💡 Đây là mật khẩu hiện tại. Sửa nếu muốn đổi, để trống nếu giữ nguyên.",
                    Location = new Point(tx, y + 29),
                    Size = new Size(inputW, 18),
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(100, 130, 100)
                });
            }
            y += isNew ? 42 : 52;

            // ── Số điện thoại ──
            AddLabel("Số điện thoại *", y);
            var txtPhone = new TextBox
            {
                Name = "txtPhone",
                Text = phone,
                Location = new Point(tx, y),
                Size = new Size(inputW, 26),
                Font = new Font("Segoe UI", 10F),
                MaxLength = 11
            };
            // Chỉ cho nhập ký tự số
            txtPhone.KeyPress += (s, kpe) =>
            {
                if (!char.IsDigit(kpe.KeyChar) && !char.IsControl(kpe.KeyChar))
                    kpe.Handled = true;
            };
            dlg.Controls.Add(txtPhone);
            y += 42;

            // ── Trạng thái ──
            var chkActive = new CheckBox
            {
                Name = "chkActive",
                Text = "Kích hoạt tài khoản",
                Checked = active,
                Location = new Point(tx, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = ThemeColors.TextPrimary
            };
            dlg.Controls.Add(chkActive);
            y += 46;

            // ── Phân cách ──
            dlg.Controls.Add(new Label
            {
                Location = new Point(20, y),
                Size = new Size(410, 1),
                BackColor = Color.FromArgb(226, 232, 240)
            });
            y += 12;

            // ── Nút Lưu & Hủy ──
            var btnSave = new Button
            {
                Text = "💾  Lưu",
                Size = new Size(130, 38),
                Location = new Point(tx, y),
                BackColor = ThemeColors.PrimaryBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(90, 38),
                Location = new Point(tx + 140, y),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCancel);

            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCancel;

            dlg.FormClosing += (s, e) =>
            {
                if (dlg.DialogResult == DialogResult.OK)
                {
                    string mUser = txtUser.Text.Trim();
                    string mHoTen = txtHoTen.Text.Trim();
                    string mEmail = txtEmail.Text.Trim();
                    string mPhone = txtPhone.Text.Trim();
                    string mPass = txtPass.Text;

                    // Kiểm tra tên đăng nhập (chỉ khi thêm mới)
                    if (isNew)
                    {
                        if (string.IsNullOrWhiteSpace(mUser))
                        {
                            MessageBox.Show("Tên đăng nhập không được để trống!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cancel = true;
                            txtUser.Focus();
                            return;
                        }

                        var svc = new src.BLL.NguoiDungService();
                        if (svc.CheckUserExists(mUser))
                        {
                            MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng chọn tên khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cancel = true;
                            txtUser.Focus();
                            return;
                        }
                    }

                    // Kiểm tra họ tên
                    if (string.IsNullOrWhiteSpace(mHoTen))
                    {
                        MessageBox.Show("Họ và tên không được để trống!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        txtHoTen.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(mEmail))
                    {
                        MessageBox.Show("Email không được để trống!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        txtEmail.Focus();
                        return;
                    }

                    if (!Regex.IsMatch(mEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        MessageBox.Show("Định dạng email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        txtEmail.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(mPhone))
                    {
                        MessageBox.Show("Số điện thoại không được để trống!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        txtPhone.Focus();
                        return;
                    }

                    if (!Regex.IsMatch(mPhone, @"^\d{10,11}$"))
                    {
                        MessageBox.Show("Số điện thoại phải chứa từ 10 đến 11 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        txtPhone.Focus();
                        return;
                    }

                    if (isNew || (!isNew && !string.IsNullOrEmpty(mPass)))
                    {
                        if (mPass.Length < 8)
                        {
                            MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cancel = true;
                            txtPass.Focus();
                            return;
                        }
                    }
                }
            };

            return dlg;
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        // ── Helper tìm control theo tên ──────────────────────────────────
        private static T Find<T>(Form form, string name) where T : Control
        {
            foreach (Control c in form.Controls)
                if (c is T t && c.Name == name) return t;
            return null;
        }
    }
}

