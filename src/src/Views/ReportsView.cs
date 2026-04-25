using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            pnlCards.Controls.Add(MakeReportCard("Phòng Hoạt Động", "10/12", ThemeColors.AccentGreen));
            pnlCards.Controls.Add(MakeReportCard("Máy Sẵn Sàng", "230/248", ThemeColors.PrimaryBlue));
            pnlCards.Controls.Add(MakeReportCard("Máy Bảo Trì", "13", ThemeColors.AccentOrange));
            pnlCards.Controls.Add(MakeReportCard("Máy Hỏng", "5", ThemeColors.AccentRed));

            pnlPie.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(pnlPie.ClientRectangle, 12))
                    pnlPie.Region = new Region(p);
                TextRenderer.DrawText(g, "📊  Trạng Thái Máy Tính",
                    new Font("Segoe UI", 13F, FontStyle.Bold), new Point(20, 16), ThemeColors.TextPrimary);
                DrawDonut(g, new Rectangle(30, 55, pnlPie.Width - 60, pnlPie.Height - 80));
            };

            pnlBar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(pnlBar.ClientRectangle, 12))
                    pnlBar.Region = new Region(p);
                TextRenderer.DrawText(g, "📈  Tần Suất Sử Dụng Phòng (Tháng)",
                    new Font("Segoe UI", 13F, FontStyle.Bold), new Point(20, 16), ThemeColors.TextPrimary);
                DrawMonthlyBars(g, new Rectangle(30, 55, pnlBar.Width - 60, pnlBar.Height - 80));
            };

            pnlCharts.Resize += (s, e) =>
            {
                int w = (pnlCharts.Width - 20) / 2;
                pnlPie.Size = new Size(w, pnlCharts.Height - 10);
                pnlPie.Location = new Point(5, 5);
                pnlBar.Size = new Size(pnlCharts.Width - w - 20, pnlCharts.Height - 10);
                pnlBar.Location = new Point(w + 15, 5);
                pnlPie.Invalidate();
                pnlBar.Invalidate();
            };
        }

        private Panel MakeReportCard(string title, string value, Color accent)
        {
            var card = new Panel { Size = new Size(230, 98), Margin = new Padding(6), BackColor = Color.White };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 12))
                    card.Region = new Region(p);
                TextRenderer.DrawText(g, value, new Font("Segoe UI", 26F, FontStyle.Bold),
                    new Point(20, 12), accent);
                TextRenderer.DrawText(g, title, new Font("Segoe UI", 9.5F),
                    new Point(20, 55), ThemeColors.TextSecondary);
                using (var br = new LinearGradientBrush(
                    new Rectangle(0, card.Height - 3, card.Width, 3), accent, Color.FromArgb(60, accent), 0F))
                    g.FillRectangle(br, 0, card.Height - 3, card.Width, 3);
            };
            return card;
        }

        private void DrawDonut(Graphics g, Rectangle b)
        {
            if (b.Width < 80 || b.Height < 80) return;
            var data = new[] {
                ("Tốt", 230, ThemeColors.AccentGreen),
                ("Bảo trì", 13, ThemeColors.AccentOrange),
                ("Hỏng", 5, ThemeColors.AccentRed),
            };
            int total = 0;
            foreach (var d in data) total += d.Item2;

            int size = Math.Min(b.Width, b.Height) - 50;
            if (size < 40) return;
            int cx = b.X + (b.Width - size) / 2, cy = b.Y + 5;
            float start = -90;

            foreach (var (label, val, color) in data)
            {
                float sweep = 360f * val / total;
                using (var br = new SolidBrush(color))
                    g.FillPie(br, cx, cy, size, size, start, sweep);
                start += sweep;
            }

            // inner circle for donut
            int inner = size * 50 / 100;
            int ix = cx + (size - inner) / 2, iy = cy + (size - inner) / 2;
            using (var br = new SolidBrush(Color.White))
                g.FillEllipse(br, ix, iy, inner, inner);
            TextRenderer.DrawText(g, total.ToString(), new Font("Segoe UI", 18F, FontStyle.Bold),
                new Rectangle(ix, iy, inner, inner), ThemeColors.TextPrimary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            // legend
            int ly = cy + size + 12, lx = cx;
            foreach (var (label, val, color) in data)
            {
                using (var br = new SolidBrush(color))
                    g.FillRectangle(br, lx, ly, 10, 10);
                TextRenderer.DrawText(g, $"{label}: {val}", new Font("Segoe UI", 8.5F),
                    new Point(lx + 14, ly - 2), ThemeColors.TextPrimary);
                lx += 100;
            }
        }

        private void DrawMonthlyBars(Graphics g, Rectangle b)
        {
            if (b.Width < 80 || b.Height < 50) return;
            string[] months = { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };
            int[] vals = { 65, 72, 80, 85, 90, 78, 45, 30, 82, 88, 75, 70 };
            int maxH = b.Height - 35;
            int barW = Math.Max(6, (b.Width - 15) / months.Length - 5);

            for (int i = 0; i < months.Length; i++)
            {
                int bh = Math.Max(1, (int)(maxH * vals[i] / 100.0));
                int x = b.X + 8 + i * (barW + 5), y = b.Y + maxH - bh;
                Color c = vals[i] > 80 ? ThemeColors.PrimaryBlue : vals[i] > 50 ? ThemeColors.AccentTeal : ThemeColors.AccentOrange;
                using (var br = new LinearGradientBrush(new Rectangle(x, y, barW, bh), c, Color.FromArgb(140, c), 90F))
                using (var p = UIHelper.GetRoundedRectPath(new Rectangle(x, y, barW, bh), 3))
                    g.FillPath(br, p);
                TextRenderer.DrawText(g, months[i], new Font("Segoe UI", 7F),
                    new Rectangle(x - 3, b.Bottom - 16, barW + 6, 16), ThemeColors.TextSecondary, TextFormatFlags.HorizontalCenter);
            }
        }
    }
}
