using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;
using src.Views;

namespace src
{
    public partial class MainForm : Form
    {
        private Button btnActiveMenu;
        private string _currentUser;
        private bool _isAdmin;
        private UserControl _currentView;

        public MainForm(string username = "admin", bool isAdmin = true)
        {
            InitializeComponent();
            _currentUser = username;
            _isAdmin = isAdmin;
            
            SetupUI();
        }

        private void SetupUI()
        {
            // Setup Profile Info
            lblUsername.Text = _currentUser;
            lblAvatar.Text = _currentUser.Length > 0 ? _currentUser.Substring(0, 1).ToUpper() : "?";
            lblRole.Text = _isAdmin ? "Quản trị viên" : "Nhân viên";

            // Hide admin menus if not admin
            if (!_isAdmin)
            {
                btnUserManage.Visible = false;
                btnReports.Visible = false;
            }

            // Topbar Date
            lblDate.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");

            // Logout events
            btnLogout.MouseEnter += (s, e) => btnLogout.ForeColor = ThemeColors.AccentRed;
            btnLogout.MouseLeave += (s, e) => btnLogout.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new Forms.LoginForm().Show();
                    this.Close();
                }
            };

            // Topbar border line
            pnlTopbar.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(25, 0, 0, 0)))
                    e.Graphics.DrawLine(pen, 0, pnlTopbar.Height - 1, pnlTopbar.Width, pnlTopbar.Height - 1);
            };

            // Attach events to menu buttons
            foreach (Control c in pnlSidebarMenu.Controls)
            {
                if (c is Button btn)
                {
                    btn.Click += MenuButton_Click;
                    btn.Paint += MenuButton_Paint;
                }
            }

            // Navigate to default
            NavigateTo("Dashboard");
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                SetActiveMenu(btn);
                if (btn.Tag != null)
                {
                    NavigateTo(btn.Tag.ToString());
                }
            }
        }

        private void MenuButton_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Button btn && btn == btnActiveMenu)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Draw highlighted rounded rect
                using (var p = UIHelper.GetRoundedRectPath(btn.ClientRectangle, 8))
                using (var br = new SolidBrush(Color.FromArgb(25, 56, 103, 214)))
                    e.Graphics.FillPath(br, p);

                // Draw left blue indicator line
                using (var br = new SolidBrush(ThemeColors.PrimaryBlue))
                    e.Graphics.FillRectangle(br, 0, 6, 3, btn.Height - 12);
            }
        }

        private void SetActiveMenu(Button btn)
        {
            foreach (Control c in pnlSidebarMenu.Controls)
            {
                if (c is Button mb)
                {
                    mb.ForeColor = System.Drawing.Color.FromArgb(150, 160, 175);
                    mb.Font = new Font("Segoe UI", 10F);
                    mb.Invalidate();
                }
            }
            btnActiveMenu = btn;
            btn.ForeColor = Color.White;
            btn.Font = ThemeColors.SidebarActiveFont;
            btn.Invalidate();
        }

        private void NavigateTo(string viewName)
        {
            if (_currentView != null)
            {
                pnlContent.Controls.Remove(_currentView);
                _currentView.Dispose();
            }

            switch (viewName)
            {
                case "Dashboard":
                    _currentView = new DashboardView();
                    lblPageTitle.Text = "📊  Dashboard";
                    break;
                case "RoomManage":
                    _currentView = new RoomManageView();
                    lblPageTitle.Text = "🏢  Quản Lý Phòng Máy";
                    break;
                case "ComputerManage":
                    _currentView = new ComputerManageView();
                    lblPageTitle.Text = "💻  Quản Lý Máy Tính";
                    break;
                case "ScheduleManage":
                    _currentView = new ScheduleManageView();
                    lblPageTitle.Text = "📅  Lịch Thực Hành";
                    break;
                case "UserManage":
                    _currentView = new UserManageView();
                    lblPageTitle.Text = "👥  Quản Lý Người Dùng";
                    break;
                case "Reports":
                    _currentView = new ReportsView();
                    lblPageTitle.Text = "📈  Báo Cáo & Thống Kê";
                    break;
                default:
                    _currentView = new DashboardView();
                    lblPageTitle.Text = "📊  Dashboard";
                    break;
            }

            _currentView.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(_currentView);

            // Sync active menu button
            foreach (Control c in pnlSidebarMenu.Controls)
            {
                if (c is Button mb && mb.Tag?.ToString() == viewName)
                {
                    SetActiveMenu(mb);
                    break;
                }
            }
        }
    }
}
