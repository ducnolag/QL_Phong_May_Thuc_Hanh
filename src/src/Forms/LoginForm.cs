using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Forms
{
    public class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblError;
        private Panel pnlLeft;

        public LoginForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            // ── Form ──
            this.Text = "Đăng Nhập - Quản Lý Phòng Máy";
            this.ClientSize = new Size(900, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(230, 235, 245);
            this.DoubleBuffered = true;

            // ── Main card ──
            var pnlMain = new Panel
            {
                Size = new Size(860, 480),
                Location = new Point(20, 20),
                BackColor = Color.White
            };
            pnlMain.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(pnlMain.ClientRectangle, 18))
                    pnlMain.Region = new Region(p);
            };
            this.Controls.Add(pnlMain);

            // ═══════════════════════════════
            // LEFT – gradient branding
            // ═══════════════════════════════
            pnlLeft = new Panel
            {
                Size = new Size(380, 480),
                Location = new Point(0, 0),
                BackColor = ThemeColors.PrimaryBlue
            };
            pnlLeft.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new LinearGradientBrush(pnlLeft.ClientRectangle,
                    Color.FromArgb(35, 55, 140), ThemeColors.PrimaryBlue, 45F))
                    g.FillRectangle(br, pnlLeft.ClientRectangle);

                // decorative circles
                using (var br = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                {
                    g.FillEllipse(br, 260, -50, 200, 200);
                    g.FillEllipse(br, -40, 370, 140, 140);
                }
            };
            pnlMain.Controls.Add(pnlLeft);

            // Branding text on left
            AddLabel(pnlLeft, "🎓", new Font("Segoe UI", 44F), Color.White, 35, 60);
            AddLabel(pnlLeft, "Hệ Thống\nQuản Lý\nPhòng Máy", new Font("Segoe UI", 26F, FontStyle.Bold), Color.White, 35, 130, 310, 120);
            AddLabel(pnlLeft, "Quản lý phòng máy thực hành\nhiệu quả cho trường đại học.\nTheo dõi · Xếp lịch · Thống kê",
                new Font("Segoe UI", 10.5F), Color.FromArgb(200, 255, 255, 255), 35, 275, 310, 80);

            // drag support
            pnlLeft.MouseDown += (s, e) => { Tag = e.Location; };
            pnlLeft.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && Tag is Point start)
                {
                    var p = PointToScreen(e.Location);
                    Location = new Point(p.X - start.X, p.Y - start.Y);
                }
            };

            // ═══════════════════════════════
            // RIGHT – login form
            // ═══════════════════════════════
            var pnlRight = new Panel
            {
                Size = new Size(480, 480),
                Location = new Point(380, 0),
                BackColor = Color.White
            };
            pnlMain.Controls.Add(pnlRight);

            // close btn
            var btnClose = new Label
            {
                Text = "✕", Font = new Font("Segoe UI", 13F),
                ForeColor = ThemeColors.TextSecondary,
                Size = new Size(32, 32), Location = new Point(440, 8),
                TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand
            };
            btnClose.Click += (s, e) => Application.Exit();
            btnClose.MouseEnter += (s, e) => btnClose.ForeColor = ThemeColors.AccentRed;
            btnClose.MouseLeave += (s, e) => btnClose.ForeColor = ThemeColors.TextSecondary;
            pnlRight.Controls.Add(btnClose);

            AddLabel(pnlRight, "Đăng Nhập", new Font("Segoe UI", 24F, FontStyle.Bold), ThemeColors.TextPrimary, 50, 55);
            AddLabel(pnlRight, "Vui lòng nhập thông tin tài khoản", new Font("Segoe UI", 10F), ThemeColors.TextSecondary, 50, 100);

            // ── Username ──
            AddLabel(pnlRight, "Tên đăng nhập", new Font("Segoe UI", 9.5F, FontStyle.Bold), ThemeColors.TextPrimary, 50, 155);
            txtUsername = MakeTextBox(pnlRight, "Nhập tên đăng nhập...", false, 50, 180);

            // ── Password ──
            AddLabel(pnlRight, "Mật khẩu", new Font("Segoe UI", 9.5F, FontStyle.Bold), ThemeColors.TextPrimary, 50, 240);
            txtPassword = MakeTextBox(pnlRight, "Nhập mật khẩu...", true, 50, 265);

            // ── Error ──
            lblError = new Label
            {
                Text = "", Font = new Font("Segoe UI", 9F),
                ForeColor = ThemeColors.AccentRed, AutoSize = false,
                Size = new Size(380, 22), Location = new Point(50, 322),
                BackColor = Color.Transparent, Visible = false
            };
            pnlRight.Controls.Add(lblError);

            // ── Login button ──
            btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                Size = new Size(380, 48),
                Location = new Point(50, 350),
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeColors.PrimaryBlue,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = UIHelper.GetRoundedRectPath(btnLogin.ClientRectangle, 10))
                {
                    btnLogin.Region = new Region(path);
                    using (var br = new LinearGradientBrush(btnLogin.ClientRectangle,
                        ThemeColors.PrimaryBlue, ThemeColors.PrimaryLight, 0F))
                        g.FillPath(br, path);
                }
                TextRenderer.DrawText(g, btnLogin.Text, btnLogin.Font,
                    btnLogin.ClientRectangle, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            btnLogin.Click += BtnLogin_Click;
            pnlRight.Controls.Add(btnLogin);
            this.AcceptButton = btnLogin;

            // hint
            AddLabel(pnlRight, "Tài khoản mặc định: admin / admin123",
                new Font("Segoe UI", 8.5F, FontStyle.Italic), ThemeColors.TextMuted, 110, 415);

            AddLabel(pnlRight, "© 2026 Lab Management System",
                new Font("Segoe UI", 8F), ThemeColors.TextMuted, 145, 445);
        }

        // ── Login handler with DB ──
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                ShowError("⚠ Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                string sql = @"SELECT nd.MaNguoiDung, nd.HoTen, nd.MatKhauDaMaHoa, 
                               vt.TenVaiTro, nd.TrangThai
                               FROM NGUOI_DUNG nd 
                               JOIN VAI_TRO vt ON nd.MaVaiTro = vt.MaVaiTro
                               WHERE nd.TenDangNhap = @user";

                var dt = DatabaseHelper.ExecuteQuery(sql, new SqlParameter("@user", user));

                if (dt.Rows.Count == 0)
                {
                    ShowError("⚠ Tên đăng nhập không tồn tại!");
                    return;
                }

                var row = dt.Rows[0];
                string storedHash = row["MatKhauDaMaHoa"].ToString();
                bool trangThai = Convert.ToBoolean(row["TrangThai"]);
                string hoTen = row["HoTen"].ToString();
                string vaiTro = row["TenVaiTro"].ToString();

                if (!trangThai)
                {
                    ShowError("⚠ Tài khoản đã bị vô hiệu hóa!");
                    return;
                }

                if (!DatabaseHelper.VerifyPassword(pass, storedHash))
                {
                    ShowError("⚠ Mật khẩu không đúng!");
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Success
                lblError.Visible = false;
                bool isAdmin = vaiTro.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                this.Hide();
                var mainForm = new MainForm(hoTen, isAdmin);
                mainForm.FormClosed += (s, args) => Application.Exit();
                mainForm.Show();
            }
            catch (Exception ex)
            {
                ShowError("⚠ Lỗi kết nối: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            lblError.Text = msg;
            lblError.Visible = true;
        }

        // ── helpers ──
        private Label AddLabel(Control parent, string text, Font font, Color color, int x, int y, int w = 0, int h = 0)
        {
            var lbl = new Label
            {
                Text = text, Font = font, ForeColor = color,
                BackColor = Color.Transparent, Location = new Point(x, y)
            };
            if (w > 0 && h > 0) { lbl.Size = new Size(w, h); lbl.AutoSize = false; }
            else lbl.AutoSize = true;
            parent.Controls.Add(lbl);
            return lbl;
        }

        private TextBox MakeTextBox(Control parent, string placeholder, bool isPassword, int x, int y)
        {
            var wrapper = new Panel
            {
                Size = new Size(380, 44), Location = new Point(x, y),
                BackColor = Color.FromArgb(245, 247, 252)
            };
            wrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(wrapper.ClientRectangle, 10))
                    wrapper.Region = new Region(p);
                using (var pen = new Pen(Color.FromArgb(215, 220, 232), 1))
                using (var p = UIHelper.GetRoundedRectPath(
                    new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1), 10))
                    e.Graphics.DrawPath(pen, p);
            };
            parent.Controls.Add(wrapper);

            var txt = new TextBox
            {
                Font = new Font("Segoe UI", 10.5F),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(245, 247, 252),
                ForeColor = ThemeColors.TextPrimary,
                Size = new Size(340, 24),
                Location = new Point(14, 11),
                PlaceholderText = placeholder,
                UseSystemPasswordChar = isPassword
            };
            txt.GotFocus += (s, e) => { wrapper.BackColor = Color.FromArgb(232, 238, 255); txt.BackColor = Color.FromArgb(232, 238, 255); wrapper.Invalidate(); };
            txt.LostFocus += (s, e) => { wrapper.BackColor = Color.FromArgb(245, 247, 252); txt.BackColor = Color.FromArgb(245, 247, 252); wrapper.Invalidate(); };
            wrapper.Controls.Add(txt);
            return txt;
        }
    }
}
