using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Login
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
            // Drag support
            pnlLeft.MouseDown += (s, e) => { Tag = e.Location; };
            pnlLeft.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && Tag is Point start)
                {
                    var p = PointToScreen(e.Location);
                    Location = new Point(p.X - start.X, p.Y - start.Y);
                }
            };

            btnClose.Click += (s, e) => Application.Exit();

            // Hover colors
            txtUsername.GotFocus  += (s, e) => { pnlUserWrap.BackColor = Color.FromArgb(232, 238, 255); txtUsername.BackColor = Color.FromArgb(232, 238, 255); };
            txtUsername.LostFocus += (s, e) => { pnlUserWrap.BackColor = Color.FromArgb(245, 247, 252); txtUsername.BackColor = Color.FromArgb(245, 247, 252); };
            txtPassword.GotFocus  += (s, e) => { pnlPassWrap.BackColor = Color.FromArgb(232, 238, 255); txtPassword.BackColor = Color.FromArgb(232, 238, 255); };
            txtPassword.LostFocus += (s, e) => { pnlPassWrap.BackColor = Color.FromArgb(245, 247, 252); txtPassword.BackColor = Color.FromArgb(245, 247, 252); };

            // Icon con mắt – vẽ bằng GDI+
            btnShowPass.Text = "";
            btnShowPass.Paint += BtnShowPass_Paint;
        }

        // Vẽ icon con mắt: mắt bình thường (đang ẩn) hoặc mắt + gạch chéo (đang hiện)
        private void BtnShowPass_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = btnShowPass.Width;
            int h = btnShowPass.Height;
            int cx = w / 2, cy = h / 2;

            bool isHidden = txtPassword.UseSystemPasswordChar; // true = đang ẩn
            Color eyeColor = isHidden
                ? Color.FromArgb(130, 140, 160)   // xám – đang ẩn
                : Color.FromArgb(45, 75, 205);     // xanh – đang hiện

            using var pen = new Pen(eyeColor, 2f);

            // Vẽ hình con mắt (2 cung tròn)
            int ew = 18, eh = 10;
            var eyeRect = new Rectangle(cx - ew / 2, cy - eh / 2, ew, eh);

            // Viền ngoài con mắt – dùng ellipse cắt nửa trên và nửa dưới
            g.DrawArc(pen, eyeRect, 200, 140);  // cung trên
            g.DrawArc(pen, eyeRect, 20, 140);   // cung dưới

            // Đồng tử
            using var brush = new SolidBrush(eyeColor);
            g.FillEllipse(brush, cx - 3, cy - 3, 6, 6);

            // Nếu đang hiện mật khẩu → vẽ đường gạch chéo qua mắt
            if (!isHidden)
            {
                using var penSlash = new Pen(eyeColor, 2.2f);
                penSlash.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                penSlash.EndCap   = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(penSlash, cx - 10, cy + 8, cx + 10, cy - 8);
            }
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
                var NguoiDungService = new src.BLL.NguoiDungService();
                var result = NguoiDungService.Login(user, pass);

                if (!result.IsSuccess)
                {
                    ShowError(result.ErrorMessage);
                    if (result.ErrorMessage.Contains("Mật khẩu"))
                    {
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                    return;
                }

                var loggedInUser = result.User;

                // Đăng nhập thành công – ẩn LoginForm rồi mở SidebarForm
                lblError.Visible = false;
                bool isAdmin = loggedInUser.TenVaiTro.Equals("Admin", StringComparison.OrdinalIgnoreCase);

                // Lưu thông tin user vào session
                AppSession.MaNguoiDung = loggedInUser.MaNguoiDung;
                AppSession.HoTen       = loggedInUser.HoTen;
                AppSession.IsAdmin     = isAdmin;

                this.Hide();
                var SidebarForm = new SidebarForm(loggedInUser.HoTen, isAdmin);
                // Khi SidebarForm đóng: nếu thoát bình thường (không phải logout) thì Exit
                // Logout sẽ gọi Application.Restart() trong SidebarForm nên không cần Exit ở đây
                SidebarForm.FormClosed += (s, args) =>
                {
                    // Chỉ exit nếu không có form nào khác đang mở (tức là không phải logout)
                    if (Application.OpenForms.Count == 0)
                        Application.Exit();
                };
                SidebarForm.Show();
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

        private void BtnShowPass_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnShowPass.Invalidate(); // repaint icon GDI+
        }

        private void lblIcon_Click(object sender, EventArgs e)
        {

        }
    }
}

