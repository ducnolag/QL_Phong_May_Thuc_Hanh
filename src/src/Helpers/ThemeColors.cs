using System.Drawing;

namespace src.Helpers
{
    /// <summary>
    /// Centralized theme colors for the Lab Management System.
    /// Bright, vibrant palette suitable for a university environment.
    /// </summary>
    public static class ThemeColors
    {
        // ── Primary Colors ──────────────────────────────────────────
        public static Color PrimaryBlue = Color.FromArgb(56, 103, 214);      // #3867D6 – Vibrant Blue
        public static Color PrimaryDark = Color.FromArgb(30, 39, 73);        // #1E2749 – Deep Navy
        public static Color PrimaryLight = Color.FromArgb(69, 170, 242);     // #45AAF2 – Sky Blue

        // ── Accent Colors ───────────────────────────────────────────
        public static Color AccentGreen = Color.FromArgb(38, 222, 129);      // #26DE81 – Fresh Green
        public static Color AccentOrange = Color.FromArgb(253, 150, 68);     // #FD9644 – Warm Orange
        public static Color AccentRed = Color.FromArgb(252, 92, 101);        // #FC5C65 – Coral Red
        public static Color AccentPurple = Color.FromArgb(165, 94, 234);     // #A55EEA – Vibrant Purple
        public static Color AccentYellow = Color.FromArgb(254, 211, 48);     // #FED330 – Bright Yellow
        public static Color AccentTeal = Color.FromArgb(43, 203, 186);       // #2BCBBA – Teal

        // ── Background Colors ───────────────────────────────────────
        public static Color BackgroundMain = Color.FromArgb(241, 243, 249);  // #F1F3F9 – Light Gray-Blue
        public static Color BackgroundCard = Color.FromArgb(255, 255, 255);  // #FFFFFF – White
        public static Color BackgroundSidebar = Color.FromArgb(30, 39, 73);  // #1E2749 – Deep Navy
        public static Color BackgroundTopbar = Color.FromArgb(255, 255, 255);// #FFFFFF – White

        // ── Text Colors ─────────────────────────────────────────────
        public static Color TextPrimary = Color.FromArgb(45, 52, 70);       // #2D3446 – Dark Gray
        public static Color TextSecondary = Color.FromArgb(130, 140, 165);   // #828CA5 – Medium Gray
        public static Color TextOnDark = Color.FromArgb(255, 255, 255);      // #FFFFFF – White
        public static Color TextMuted = Color.FromArgb(175, 185, 210);       // #AFB9D2 – Light Gray

        // ── Sidebar Colors ──────────────────────────────────────────
        public static Color SidebarActiveItem = Color.FromArgb(56, 103, 214);// #3867D6
        public static Color SidebarHoverItem = Color.FromArgb(40, 50, 90);   // Slightly lighter navy
        public static Color SidebarText = Color.FromArgb(175, 185, 210);     // #AFB9D2

        // ── Status Colors ───────────────────────────────────────────
        public static Color StatusAvailable = Color.FromArgb(38, 222, 129);  // Green
        public static Color StatusMaintenance = Color.FromArgb(253, 150, 68);// Orange
        public static Color StatusBroken = Color.FromArgb(252, 92, 101);     // Red
        public static Color StatusInUse = Color.FromArgb(69, 170, 242);      // Blue

        // ── Gradients (start, end) ──────────────────────────────────
        public static Color GradientStart = Color.FromArgb(56, 103, 214);
        public static Color GradientEnd = Color.FromArgb(69, 170, 242);

        // ── Fonts ───────────────────────────────────────────────────
        public static Font HeaderFont = new Font("Segoe UI", 20F, FontStyle.Bold);
        public static Font SubHeaderFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static Font BodyFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font SmallFont = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static Font ButtonFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static Font SidebarFont = new Font("Segoe UI", 11F, FontStyle.Regular);
        public static Font SidebarActiveFont = new Font("Segoe UI", 11F, FontStyle.Bold);
    }
}
