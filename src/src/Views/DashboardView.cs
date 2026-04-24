using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    public class DashboardView : UserControl
    {
        public DashboardView()
        {
            this.BackColor = ThemeColors.BackgroundMain;
            this.DoubleBuffered = true;
            this.Padding = new Padding(5);
            BuildUI();
        }

        private void BuildUI()
        {
            // ═══ TOP: stat cards ═══
            var pnlCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = Color.Transparent,
                WrapContents = false,
                Padding = new Padding(2)
            };
            this.Controls.Add(pnlCards);

            pnlCards.Controls.Add(MakeCard("Tổng Phòng Máy", "12", "🏢", ThemeColors.PrimaryBlue));
            pnlCards.Controls.Add(MakeCard("Tổng Máy Tính", "248", "💻", ThemeColors.AccentGreen));
            pnlCards.Controls.Add(MakeCard("Lịch Hôm Nay", "8", "📅", ThemeColors.AccentOrange));
            pnlCards.Controls.Add(MakeCard("Bảo Trì", "5", "🔧", ThemeColors.AccentRed));

            // ═══ BOTTOM: chart + activity ═══
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 8, 2, 2)
            };
            this.Controls.Add(pnlBottom);
            pnlBottom.BringToFront();

            var pnlChart = new Panel { BackColor = Color.White, Location = new Point(5, 5) };
            pnlChart.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(pnlChart.ClientRectangle, 12))
                    pnlChart.Region = new Region(p);
                TextRenderer.DrawText(g, "📊  Tình Trạng Sử Dụng Phòng Máy",
                    new Font("Segoe UI", 13F, FontStyle.Bold), new Point(20, 16), ThemeColors.TextPrimary);
                DrawBars(g, new Rectangle(40, 55, pnlChart.Width - 70, pnlChart.Height - 85));
            };
            pnlBottom.Controls.Add(pnlChart);

            var pnlActivity = new Panel { BackColor = Color.White };
            pnlActivity.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(pnlActivity.ClientRectangle, 12))
                    pnlActivity.Region = new Region(p);
                TextRenderer.DrawText(g, "🕐  Hoạt Động Gần Đây",
                    new Font("Segoe UI", 13F, FontStyle.Bold), new Point(20, 16), ThemeColors.TextPrimary);
                DrawActivityItems(g, new Rectangle(20, 52, pnlActivity.Width - 40, pnlActivity.Height - 65));
            };
            pnlBottom.Controls.Add(pnlActivity);

            // resize handler
            pnlBottom.Resize += (s, e) =>
            {
                int w = (pnlBottom.Width - 20) * 55 / 100;
                pnlChart.Size = new Size(w, pnlBottom.Height - 10);
                pnlChart.Location = new Point(5, 5);
                pnlActivity.Size = new Size(pnlBottom.Width - w - 20, pnlBottom.Height - 10);
                pnlActivity.Location = new Point(w + 15, 5);
                pnlChart.Invalidate();
                pnlActivity.Invalidate();
            };
        }

        private Panel MakeCard(string title, string value, string icon, Color accent)
        {
            var card = new Panel
            {
                Size = new Size(235, 112),
                Margin = new Padding(6),
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var p = UIHelper.GetRoundedRectPath(card.ClientRectangle, 12))
                    card.Region = new Region(p);

                // icon bg
                using (var br = new SolidBrush(Color.FromArgb(22, accent)))
                    g.FillEllipse(br, 18, 22, 46, 46);
                TextRenderer.DrawText(g, icon, new Font("Segoe UI", 17F),
                    new Rectangle(18, 22, 46, 46), accent,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // value
                TextRenderer.DrawText(g, value, new Font("Segoe UI", 24F, FontStyle.Bold),
                    new Point(76, 16), ThemeColors.TextPrimary);
                // title
                TextRenderer.DrawText(g, title, new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    new Point(76, 52), ThemeColors.TextSecondary);

                // bottom accent line
                using (var br = new LinearGradientBrush(
                    new Rectangle(0, card.Height - 3, card.Width, 3),
                    accent, Color.FromArgb(60, accent), 0F))
                    g.FillRectangle(br, 0, card.Height - 3, card.Width, 3);
            };
            card.MouseEnter += (s, e) => { card.BackColor = Color.FromArgb(248, 250, 255); card.Invalidate(); };
            card.MouseLeave += (s, e) => { card.BackColor = Color.White; card.Invalidate(); };
            return card;
        }

        private void DrawBars(Graphics g, Rectangle b)
        {
            string[] labels = { "P.A01", "P.A02", "P.A03", "P.B01", "P.B02", "P.C01" };
            int[] vals = { 85, 60, 95, 45, 72, 88 };
            Color[] colors = { ThemeColors.PrimaryBlue, ThemeColors.AccentGreen, ThemeColors.AccentPurple,
                               ThemeColors.AccentOrange, ThemeColors.PrimaryLight, ThemeColors.AccentTeal };

            if (b.Width < 50 || b.Height < 50) return;
            int barW = Math.Max(10, (b.Width - 20) / labels.Length - 12);
            int maxH = b.Height - 40;

            for (int i = 0; i < labels.Length; i++)
            {
                int bh = (int)(maxH * vals[i] / 100.0);
                int x = b.X + 15 + i * (barW + 12);
                int y = b.Y + maxH - bh + 5;

                using (var br = new LinearGradientBrush(new Rectangle(x, y, barW, Math.Max(1, bh)),
                    colors[i], Color.FromArgb(160, colors[i]), 90F))
                using (var p = UIHelper.GetRoundedRectPath(new Rectangle(x, y, barW, Math.Max(1, bh)), 5))
                    g.FillPath(br, p);

                TextRenderer.DrawText(g, vals[i] + "%", new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    new Rectangle(x, y - 18, barW, 16), colors[i], TextFormatFlags.HorizontalCenter);
                TextRenderer.DrawText(g, labels[i], new Font("Segoe UI", 7.5F),
                    new Rectangle(x - 4, b.Bottom - 16, barW + 8, 16), ThemeColors.TextSecondary, TextFormatFlags.HorizontalCenter);
            }
        }

        private void DrawActivityItems(Graphics g, Rectangle b)
        {
            var items = new[]
            {
                ("Phòng A01 đã được xếp lịch", "2 phút trước", ThemeColors.AccentGreen),
                ("Máy B03-15 cập nhật cấu hình", "15 phút trước", ThemeColors.PrimaryBlue),
                ("Phòng C01 bảo trì hoàn tất", "1 giờ trước", ThemeColors.AccentOrange),
                ("Máy A02-08 báo lỗi phần cứng", "2 giờ trước", ThemeColors.AccentRed),
                ("Lịch TH Mạng MT đã tạo", "3 giờ trước", ThemeColors.AccentGreen),
                ("Người dùng mới được thêm", "5 giờ trước", ThemeColors.PrimaryBlue),
                ("Phòng B02 sẵn sàng", "6 giờ trước", ThemeColors.AccentTeal),
            };
            int y = 0;
            foreach (var (text, time, color) in items)
            {
                if (y + 42 > b.Height) break;
                using (var br = new SolidBrush(color))
                    g.FillEllipse(br, b.X + 4, b.Y + y + 14, 8, 8);
                TextRenderer.DrawText(g, text, new Font("Segoe UI", 9F),
                    new Rectangle(b.X + 20, b.Y + y + 3, b.Width - 25, 18), ThemeColors.TextPrimary);
                TextRenderer.DrawText(g, time, new Font("Segoe UI", 7.5F),
                    new Rectangle(b.X + 20, b.Y + y + 22, b.Width - 25, 14), ThemeColors.TextMuted);
                using (var pen = new Pen(Color.FromArgb(18, 0, 0, 0)))
                    g.DrawLine(pen, b.X + 20, b.Y + y + 40, b.Right, b.Y + y + 40);
                y += 42;
            }
        }
    }
}
