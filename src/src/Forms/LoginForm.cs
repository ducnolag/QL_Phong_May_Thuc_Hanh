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
            // Removed GDI+ Paint events to ensure full Designer compatibility
            
            // Keep drag support since form might be borderless
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
            
            // Hover colors for username/password textboxes
            txtUsername.GotFocus += (s, e) => { pnlUserWrap.BackColor = Color.FromArgb(232, 238, 255); txtUsername.BackColor = Color.FromArgb(232, 238, 255); pnlUserWrap.Invalidate(); };
            txtUsername.LostFocus += (s, e) => { pnlUserWrap.BackColor = Color.FromArgb(245, 247, 252); txtUsername.BackColor = Color.FromArgb(245, 247, 252); pnlUserWrap.Invalidate(); };
            
            txtPassword.GotFocus += (s, e) => { pnlPassWrap.BackColor = Color.FromArgb(232, 238, 255); txtPassword.BackColor = Color.FromArgb(232, 238, 255); pnlPassWrap.Invalidate(); };
            txtPassword.LostFocus += (s, e) => { pnlPassWrap.BackColor = Color.FromArgb(245, 247, 252); txtPassword.BackColor = Color.FromArgb(245, 247, 252); pnlPassWrap.Invalidate(); };
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
