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
        private Button _activeBtn;
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

        // ── Thiết lập giao diện ban đầu ──────────────────────────────
        private void SetupUI()
        {
            // Thông tin user
            lblUsername.Text = _currentUser;
            lblAvatar.Text = _currentUser.Length > 0 ? _currentUser[0].ToString().ToUpper() : "?";
            lblRole.Text = _isAdmin ? "Administrator" : "Employee";

            // Avatar tròn – dùng Paint để bo tròn nền
            lblAvatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Vẽ nền tròn xanh
                using (var br = new SolidBrush(ThemeColors.PrimaryBlue))
                    e.Graphics.FillEllipse(br, 1, 1, lblAvatar.Width - 3, lblAvatar.Height - 3);
                // Vẽ chữ cái
                TextRenderer.DrawText(e.Graphics, lblAvatar.Text,
                    new Font("Segoe UI", 13F, FontStyle.Bold),
                    lblAvatar.ClientRectangle, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                // Cắt region hình tròn
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, lblAvatar.Width - 1, lblAvatar.Height - 1);
                    lblAvatar.Region = new Region(path);
                }
            };

            // Logo icon bo tròn
            lblLogoIcon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = UIHelper.GetRoundedRectPath(lblLogoIcon.ClientRectangle, 8))
                {
                    lblLogoIcon.Region = new Region(path);
                    using (var br = new SolidBrush(ThemeColors.PrimaryBlue))
                        e.Graphics.FillPath(br, path);
                }
                TextRenderer.DrawText(e.Graphics, "🖥", new Font("Segoe UI", 14F),
                    lblLogoIcon.ClientRectangle, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            lblLogoIcon.Text = ""; // Tránh render đôi

            // Sidebar – vẽ đường viền phải
            pnlSidebar.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawLine(pen, pnlSidebar.Width - 1, 0,
                        pnlSidebar.Width - 1, pnlSidebar.Height);
            };

            // Ẩn menu nếu không phải admin
            if (!_isAdmin)
            {
                btnUserManage.Visible = false;
                btnReports.Visible = false;
            }

            // Sự kiện nút logout
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Restart();
                }
            };

            // Gắn Click cho tất cả nút menu
            btnDashboard.Click     += MenuBtn_Click;
            btnUserManage.Click    += MenuBtn_Click;
            btnRoomManage.Click    += MenuBtn_Click;
            btnComputerManage.Click+= MenuBtn_Click;
            btnScheduleManage.Click+= MenuBtn_Click;
            btnReports.Click       += MenuBtn_Click;

            // Dồn layout sau khi form đã load xong (tránh WinForms reset vị trí)
            this.Load += (s, e) =>
            {
                RelayoutMenuButtons();
                NavigateTo("Dashboard");
            };
        }

        // ── Dồn các nút menu visible lên trên (sau khi ẩn theo role) ──
        /// <summary>
        /// Tái bố cục các nút sidebar: nút nào visible thì xếp từ trên xuống,
        /// cách nhau 52px. Đảm bảo role Nhân viên không có khoảng trống.
        /// </summary>
        private void RelayoutMenuButtons()
        {
            Button[] menuBtns = { btnDashboard, btnUserManage, btnRoomManage,
                                   btnComputerManage, btnScheduleManage, btnReports };
            int y = 12; // vị trí Y bắt đầu, cách top 12px
            foreach (var b in menuBtns)
            {
                if (b.Visible)
                {
                    b.Location = new Point(12, y);
                    y += 52; // khoảng cách giữa các nút
                }
            }
        }

        // ── Click menu sidebar ────────────────────────────────────────
        private void MenuBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
                NavigateTo(btn.Tag.ToString());
        }

        // ── Đặt trạng thái active cho nút menu ───────────────────────
        /// <summary>
        /// Highlight nút menu đang active: nền xanh nhạt + chữ xanh đậm.
        /// Dùng BackColor thay vì override Paint để text vẫn hiển thị đúng.
        /// </summary>
        private void SetActiveMenu(Button btn)
        {
            Button[] menuBtns = { btnDashboard, btnUserManage, btnRoomManage,
                                   btnComputerManage, btnScheduleManage, btnReports };

            // Bước 1: Reset tất cả nút và gỡ handler cũ (tránh chồng chuyện event)
            foreach (var b in menuBtns)
            {
                b.Paint -= ActiveBtn_Paint;   // gỡ indicator khỏi mọi nút trước
                b.BackColor = Color.Transparent;
                b.ForeColor = ThemeColors.SidebarText;
                b.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 250, 252);
                b.Invalidate();
            }

            // Bước 2: Gắn active style và handler cho nút mới
            _activeBtn = btn;
            btn.BackColor = ThemeColors.SidebarActiveBg;   // nền xanh nhạt
            btn.ForeColor = ThemeColors.PrimaryBlue;        // chữ xanh đậm
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.FlatAppearance.MouseOverBackColor = ThemeColors.SidebarActiveBg;
            btn.Paint += ActiveBtn_Paint;  // gắn handler – giữ mãi đến khi đổi view
            btn.Invalidate();
        }

        // ── Vẽ thanh indicator trái cho active button ─────────────────
        private void ActiveBtn_Paint(object sender, PaintEventArgs e)
        {
            // Chỉ vẽ khi đây là nút đang active
            if (sender is Button btn && btn == _activeBtn)
            {
                using (var br = new SolidBrush(ThemeColors.PrimaryBlue))
                    e.Graphics.FillRectangle(br, 0, 6, 3, btn.Height - 12);
            }
            // QUAN TRỌNG: không tự hủy event – indicator phải còn sau khi hover/leave
        }

        // ── Điều hướng đến view ────────────────────────────────────────
        /// <summary>
        /// Load UserControl tương ứng vào vùng nội dung chính (pnlContent).
        /// </summary>
        private void NavigateTo(string viewName)
        {
            // Gỡ view cũ
            if (_currentView != null)
            {
                pnlContent.Controls.Remove(_currentView);
                _currentView.Dispose();
                _currentView = null;
            }

            // Tạo view mới
            _currentView = viewName switch
            {
                "Dashboard"       => (UserControl)new DashboardView(),
                "RoomManage"      => new RoomManageView(),
                "ComputerManage"  => new ComputerManageView(),
                "ScheduleManage"  => new ScheduleManageView(),
                "UserManage"      => new UserManageView(),
                "Reports"         => new ReportsView(),
                _                 => new DashboardView()
            };

            _currentView.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(_currentView);
            _currentView.BringToFront();

            // Đồng bộ active menu
            Button[] menuBtns = { btnDashboard, btnUserManage, btnRoomManage,
                                   btnComputerManage, btnScheduleManage, btnReports };
            foreach (var b in menuBtns)
            {
                if (b.Tag?.ToString() == viewName)
                {
                    SetActiveMenu(b);
                    break;
                }
            }
        }
    }
}
