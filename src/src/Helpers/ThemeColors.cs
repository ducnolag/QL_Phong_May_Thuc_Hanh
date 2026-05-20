using System.Drawing;

namespace src.Helpers
{
    /// <summary>
    /// Bảng màu tập trung cho Hệ Thống Quản Lý Phòng Máy.
    /// Cập nhật theo thiết kế Figma: sidebar trắng, accent xanh dương.
    /// </summary>
    public static class ThemeColors
    {
        // ── Màu chính (Primary) ──────────────────────────────────────
        public static Color PrimaryBlue = Color.FromArgb(0, 102, 255);       // #0066FF – Xanh dương chính
        public static Color PrimaryDark = Color.FromArgb(30, 39, 73);        // #1E2749 – Navy đậm
        public static Color PrimaryLight = Color.FromArgb(69, 170, 242);     // #45AAF2 – Xanh nhạt

        // ── Màu nhấn (Accent) ────────────────────────────────────────
        public static Color AccentGreen = Color.FromArgb(34, 197, 94);       // #22C55E – Xanh lá
        public static Color AccentOrange = Color.FromArgb(249, 115, 22);     // #F97316 – Cam
        public static Color AccentRed = Color.FromArgb(239, 68, 68);         // #EF4444 – Đỏ
        public static Color AccentPurple = Color.FromArgb(139, 92, 246);     // #8B5CF6 – Tím
        public static Color AccentYellow = Color.FromArgb(234, 179, 8);      // #EAB308 – Vàng
        public static Color AccentTeal = Color.FromArgb(20, 184, 166);       // #14B8A6 – Teal

        // ── Màu nền ─────────────────────────────────────────────────
        public static Color BackgroundMain = Color.FromArgb(245, 247, 250);  // #F5F7FA – Nền chính
        public static Color BackgroundCard = Color.FromArgb(255, 255, 255);  // #FFFFFF – Card trắng
        public static Color BackgroundSidebar = Color.FromArgb(255, 255, 255);// #FFFFFF – Sidebar trắng
        public static Color BackgroundTopbar = Color.FromArgb(255, 255, 255);// #FFFFFF – Topbar trắng

        // ── Màu chữ ─────────────────────────────────────────────────
        public static Color TextPrimary = Color.FromArgb(30, 41, 59);       // #1E293B – Đen xám
        public static Color TextSecondary = Color.FromArgb(100, 116, 139);   // #64748B – Xám trung
        public static Color TextOnDark = Color.FromArgb(255, 255, 255);      // #FFFFFF – Trắng
        public static Color TextMuted = Color.FromArgb(148, 163, 184);       // #94A3B8 – Xám nhạt

        // ── Màu sidebar ─────────────────────────────────────────────
        public static Color SidebarActiveItem = Color.FromArgb(0, 102, 255); // #0066FF – Xanh active
        public static Color SidebarActiveBg = Color.FromArgb(239, 246, 255); // #EFF6FF – Nền active nhạt
        public static Color SidebarHoverItem = Color.FromArgb(248, 250, 252);// #F8FAFC – Hover nhạt
        public static Color SidebarText = Color.FromArgb(71, 85, 105);       // #475569 – Chữ sidebar
        public static Color SidebarBorder = Color.FromArgb(226, 232, 240);   // #E2E8F0 – Viền sidebar

        // ── Màu trạng thái ──────────────────────────────────────────
        public static Color StatusAvailable = Color.FromArgb(34, 197, 94);   // Xanh lá
        public static Color StatusMaintenance = Color.FromArgb(249, 115, 22);// Cam
        public static Color StatusBroken = Color.FromArgb(239, 68, 68);      // Đỏ
        public static Color StatusInUse = Color.FromArgb(59, 130, 246);      // Xanh dương

        // ── Màu badge ───────────────────────────────────────────────
        public static Color BadgeGreenBg = Color.FromArgb(220, 252, 231);    // #DCFCE7
        public static Color BadgeGreenFg = Color.FromArgb(22, 163, 74);      // #16A34A
        public static Color BadgeRedBg = Color.FromArgb(254, 226, 226);      // #FEE2E2
        public static Color BadgeRedFg = Color.FromArgb(220, 38, 38);        // #DC2626
        public static Color BadgeOrangeBg = Color.FromArgb(255, 237, 213);   // #FFEDD5
        public static Color BadgeOrangeFg = Color.FromArgb(234, 88, 12);     // #EA580C
        public static Color BadgeBlueBg = Color.FromArgb(219, 234, 254);     // #DBEAFE
        public static Color BadgeBlueFg = Color.FromArgb(37, 99, 235);       // #2563EB
        public static Color BadgePurpleBg = Color.FromArgb(237, 233, 254);   // #EDE9FE
        public static Color BadgePurpleFg = Color.FromArgb(124, 58, 237);    // #7C3AED

        // ── Gradient ─────────────────────────────────────────────────
        public static Color GradientStart = Color.FromArgb(0, 102, 255);
        public static Color GradientEnd = Color.FromArgb(69, 170, 242);

        // ── Font chữ ─────────────────────────────────────────────────
        public static Font HeaderFont = new Font("Segoe UI", 22F, FontStyle.Bold);
        public static Font SubHeaderFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static Font BodyFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font SmallFont = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static Font ButtonFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static Font SidebarFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font SidebarActiveFont = new Font("Segoe UI", 10F, FontStyle.Bold);
    }
}
