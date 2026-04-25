using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            // ── Form ──
            this.BackColor = Color.FromArgb(230, 235, 245);
            this.DoubleBuffered = true;

            // ── Main card ──
            pnlMain.Paint += (s, e) =>
            {
                using (var p = UIHelper.GetRoundedRectPath(pnlMain.ClientRectangle, 18))
                    pnlMain.Region = new Region(p);
            };

            // ═══════════════════════════════
            // LEFT – gradient branding
            // ═══════════════════════════════
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

            // close btn
            btnClose.Click += (s, e) => Application.Exit();
            btnClose.MouseEnter += (s, e) => btnClose.ForeColor = ThemeColors.AccentRed;
            btnClose.MouseLeave += (s, e) => btnClose.ForeColor = ThemeColors.TextSecondary;

            // ── Username Wrapper ──
            pnlUserWrap.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(pnlUserWrap.ClientRectangle, 10))
                    pnlUserWrap.Region = new Region(p);
                using (var pen = new Pen(Color.FromArgb(215, 220, 232), 1))
                using (var p = UIHelper.GetRoundedRectPath(
                    new Rectangle(0, 0, pnlUserWrap.Width - 1, pnlUserWrap.Height - 1), 10))
                    e.Graphics.DrawPath(pen, p);
            };
            txtUsername.GotFocus += (s, e) => { pnlUserWrap.BackColor = Color.FromArgb(232, 238, 255); txtUsername.BackColor = Color.FromArgb(232, 238, 255); pnlUserWrap.Invalidate(); };
            txtUsername.LostFocus += (s, e) => { pnlUserWrap.BackColor = Color.FromArgb(245, 247, 252); txtUsername.BackColor = Color.FromArgb(245, 247, 252); pnlUserWrap.Invalidate(); };

            // ── Password Wrapper ──
            pnlPassWrap.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(pnlPassWrap.ClientRectangle, 10))
                    pnlPassWrap.Region = new Region(p);
                using (var pen = new Pen(Color.FromArgb(215, 220, 232), 1))
                using (var p = UIHelper.GetRoundedRectPath(
                    new Rectangle(0, 0, pnlPassWrap.Width - 1, pnlPassWrap.Height - 1), 10))
                    e.Graphics.DrawPath(pen, p);
            };
            txtPassword.GotFocus += (s, e) => { pnlPassWrap.BackColor = Color.FromArgb(232, 238, 255); txtPassword.BackColor = Color.FromArgb(232, 238, 255); pnlPassWrap.Invalidate(); };
            txtPassword.LostFocus += (s, e) => { pnlPassWrap.BackColor = Color.FromArgb(245, 247, 252); txtPassword.BackColor = Color.FromArgb(245, 247, 252); pnlPassWrap.Invalidate(); };

            // ── Login button ──
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

        private void lblIcon_Click(object sender, EventArgs e)
        {

        }
    }
}
