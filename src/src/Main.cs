using System;
using System.Collections.Generic;
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

        private readonly List<(string Icon, string Text, string ViewName)> _menuItems
            = new List<(string, string, string)>();

        public MainForm(string username = "admin", bool isAdmin = true)
        {
            InitializeComponent();
            _currentUser = username;
            _isAdmin = isAdmin;
            SetupUI();
        }

        private void SetupUI()
        {
            // Menu items
            _menuItems.Add(("📊", "Dashboard", "Dashboard"));
            _menuItems.Add(("🏢", "Phòng Máy", "RoomManage"));
            _menuItems.Add(("💻", "Máy Tính", "ComputerManage"));
            _menuItems.Add(("📅", "Lịch Thực Hành", "ScheduleManage"));
            if (_isAdmin)
            {
                _menuItems.Add(("👥", "Người Dùng", "UserManage"));
                _menuItems.Add(("📈", "Báo Cáo", "Reports"));
            }

            ApplyCustomStyles();

            // Build dynamic menu buttons
            int y = 38;
            foreach (var item in _menuItems)
            {
                var btn = MakeMenuButton(item.Icon, item.Text, item.ViewName);
                btn.Location = new Point(6, y);
                pnlSidebarMenu.Controls.Add(btn);
                y += 46;
            }

            NavigateTo("Dashboard");
        }

        private void ApplyCustomStyles()
        {
            // ── Logo ──
            pnlLogo.Paint += (s, e) =>
            {
                TextRenderer.DrawText(e.Graphics, "🎓  LabManager",
                    new Font("Segoe UI", 15F, FontStyle.Bold),
                    new Rectangle(18, 0, 220, 70), Color.White,
                    TextFormatFlags.VerticalCenter);
                using (var pen = new Pen(Color.FromArgb(40, 255, 255, 255)))
                    e.Graphics.DrawLine(pen, 18, 69, 232, 69);
            };

            // ── User profile at bottom ──
            pnlProfile.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // avatar
                using (var br = new LinearGradientBrush(new Rectangle(12, 13, 38, 38),
                    ThemeColors.AccentTeal, ThemeColors.PrimaryBlue, 45F))
                    g.FillEllipse(br, 12, 13, 38, 38);

                string initial = _currentUser.Length > 0 ? _currentUser.Substring(0, 1).ToUpper() : "?";
                TextRenderer.DrawText(g, initial, new Font("Segoe UI", 13F, FontStyle.Bold),
                    new Rectangle(12, 13, 38, 38), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // name & role
                TextRenderer.DrawText(g, _currentUser, new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    new Point(58, 12), Color.White);
                TextRenderer.DrawText(g, _isAdmin ? "Quản trị viên" : "Nhân viên",
                    new Font("Segoe UI", 8.5F), new Point(58, 32), ThemeColors.SidebarText);
            };

            // Logout events
            btnLogout.MouseEnter += (s, e) => btnLogout.ForeColor = ThemeColors.AccentRed;
            btnLogout.MouseLeave += (s, e) => btnLogout.ForeColor = ThemeColors.SidebarText;
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    new Forms.LoginForm().Show();
                    // Application.Exit is hooked in Program or login, here we just close
                    this.Close();
                }
            };

            // ── Topbar ──
            pnlTopbar.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(25, 0, 0, 0)))
                    e.Graphics.DrawLine(pen, 0, pnlTopbar.Height - 1, pnlTopbar.Width, pnlTopbar.Height - 1);
            };

            lblDate.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
        }

        private Button MakeMenuButton(string icon, string text, string viewName)
        {
            var btn = new Button
            {
                Text = $"  {icon}   {text}",
                Font = ThemeColors.SidebarFont,
                ForeColor = ThemeColors.SidebarText,
                Size = new Size(224, 40),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Cursor = Cursors.Hand,
                Tag = viewName,
                BackColor = Color.Transparent
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ThemeColors.SidebarHoverItem;
            btn.FlatAppearance.MouseDownBackColor = ThemeColors.SidebarActiveItem;

            btn.Paint += (s, e) =>
            {
                if (btn == btnActiveMenu)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var p = UIHelper.GetRoundedRectPath(btn.ClientRectangle, 8))
                    using (var br = new SolidBrush(Color.FromArgb(25, 56, 103, 214)))
                        e.Graphics.FillPath(br, p);

                    using (var br = new SolidBrush(ThemeColors.PrimaryBlue))
                        e.Graphics.FillRectangle(br, 0, 6, 3, btn.Height - 12);
                }
            };

            btn.Click += (s, e) =>
            {
                SetActiveMenu(btn);
                NavigateTo(viewName);
            };

            return btn;
        }

        private void SetActiveMenu(Button btn)
        {
            foreach (Control c in pnlSidebarMenu.Controls)
            {
                if (c is Button mb)
                {
                    mb.ForeColor = ThemeColors.SidebarText;
                    mb.Font = ThemeColors.SidebarFont;
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
