using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;
using src.Views;

namespace src
{
    public partial class SidebarForm : Form
    {
        private Button _activeBtn;
        internal string _currentUser;
        internal bool _isAdmin;
        private UserControl _currentView;

        public SidebarForm(string username = "admin", bool isAdmin = true)
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
            lblRole.Text = _isAdmin ? "Quản trị viên" : "Nhân viên";

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
                // path region removed
            };

            // Logo icon bo tròn
            lblLogoIcon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(ThemeColors.PrimaryBlue))
                    e.Graphics.FillEllipse(br, 0, 0, lblLogoIcon.Width - 1, lblLogoIcon.Height - 1);
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
                btnRoomManage.Visible = false;
                btnCatalog.Visible = false;
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
            btnUserManage.Click += MenuBtn_Click;
            btnRoomManage.Click += MenuBtn_Click;
            btnComputerManage.Click += MenuBtn_Click;
            btnCatalog.Click += MenuBtn_Click;
            btnScheduleManage.Click += MenuBtn_Click;
            btnReports.Click += MenuBtn_Click;

            // Dồn layout sau khi form đã load xong
            this.Load += (s, e) =>
            {
                RelayoutMenuButtons();
                // Admin mở báo cáo, NhanViên mở lịch thực hành
                NavigateTo(_isAdmin ? "Reports" : "ScheduleManage");
            };
        }

        // ── Dồn các nút menu visible lên trên (sau khi ẩn theo role) ──
        private void RelayoutMenuButtons()
        {
            var menuBtns = pnlSidebarMenu.Controls.OfType<Button>().OrderBy(b => b.Top).ToList();
            if (menuBtns.Count == 0) return;

            int y = menuBtns[0].Top; // Bắt đầu bằng toạ độ của nút đầu tiên
            foreach (var b in menuBtns)
            {
                if (b.Visible)
                {
                    b.Top = y;
                    y += b.Height + 11; // 11 là khoảng cách giữa các nút (vd: 70 - 59 = 11)
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
            Button[] menuBtns = { btnUserManage, btnRoomManage,
                                   btnComputerManage, btnScheduleManage, btnCatalog, btnReports };

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
                "RoomManage" => new QuanLyPhongMayView(),
                "ComputerManage" => new QuanLyMayTinhView(),
                "CatalogManage" => new QuanLyLopMonView(),
                "ScheduleManage" => new QuanLyLichThucHanhView(),
                "UserManage" => new QuanLyNguoiDungView(),
                "Reports" => new BaoCaoThongKeView(),
                _ => new QuanLyPhongMayView()
            };

            _currentView.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(_currentView);
            _currentView.BringToFront();

            // Đồng bộ active menu
            Button[] menuBtns = { btnUserManage, btnRoomManage,
                                   btnComputerManage, btnScheduleManage, btnCatalog, btnReports };
            foreach (var b in menuBtns)
            {
                if (b.Tag?.ToString() == viewName)
                {
                    SetActiveMenu(b);
                    break;
                }
            }
        }
        /// <summary>Cập nhật tên hiển thị trên sidebar khi admin sửa hồ sơ của mình.</summary>
        public void UpdateSidebarName(string newHoTen)
        {
            lblUsername.Text = newHoTen;
            lblAvatar.Text = newHoTen.Length > 0 ? newHoTen[0].ToString().ToUpper() : "?";
            lblAvatar.Invalidate();
        }

        private void lblLogo_Click(object sender, EventArgs e)
        {

        }

        private void btnUserManage_Click(object sender, EventArgs e)
        {

        }

        private void SidebarForm_Load(object sender, EventArgs e)
        {

        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

