using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace src.Helpers
{
    /// <summary>
    /// Utility methods for UI styling, shadow painting, and card creation.
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Create a styled stat card panel with icon, value, and label.
        /// </summary>
        public static Panel CreateStatCard(string title, string value, string iconText, Color accentColor, int width = 220, int height = 120)
        {
            var card = new Panel
            {
                Size = new Size(width, height),
                BackColor = ThemeColors.BackgroundCard,
                Margin = new Padding(10),
                Padding = new Padding(15),
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Draw rounded rectangle
                using (var path = GetRoundedRectPath(card.ClientRectangle, 12))
                {
                    card.Region = new Region(path);
                    using (var brush = new SolidBrush(ThemeColors.BackgroundCard))
                        g.FillPath(brush, path);
                }
                // Draw accent bar on left
                using (var brush = new SolidBrush(accentColor))
                    g.FillRectangle(brush, 0, 15, 4, height - 30);
            };

            // Icon circle
            var iconPanel = new Panel
            {
                Size = new Size(42, 42),
                Location = new Point(18, 18),
                BackColor = Color.FromArgb(30, accentColor)
            };
            iconPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = GetRoundedRectPath(iconPanel.ClientRectangle, 10))
                    iconPanel.Region = new Region(path);
                using (var brush = new SolidBrush(Color.FromArgb(30, accentColor)))
                    e.Graphics.FillRectangle(brush, iconPanel.ClientRectangle);
                TextRenderer.DrawText(e.Graphics, iconText, new Font("Segoe UI", 16F), iconPanel.ClientRectangle, accentColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            card.Controls.Add(iconPanel);

            // Value label
            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = ThemeColors.TextPrimary,
                AutoSize = true,
                Location = new Point(70, 15),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblValue);

            // Title label
            var lblTitle = new Label
            {
                Text = title,
                Font = ThemeColors.BodyFont,
                ForeColor = ThemeColors.TextSecondary,
                AutoSize = true,
                Location = new Point(70, 50),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblTitle);

            return card;
        }

        /// <summary>
        /// Create a rounded rectangle GraphicsPath.
        /// </summary>
        public static GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Apply rounded corners to a control.
        /// </summary>
        public static void ApplyRoundedCorners(Control control, int radius)
        {
            control.Paint += (s, e) =>
            {
                using (var path = GetRoundedRectPath(control.ClientRectangle, radius))
                    control.Region = new Region(path);
            };
        }

        /// <summary>
        /// Draw a soft shadow around a rectangle.
        /// </summary>
        public static void DrawShadow(Graphics g, Rectangle bounds, int radius, int shadowDepth)
        {
            for (int i = 1; i <= shadowDepth; i++)
            {
                int alpha = 10 - i;
                if (alpha < 1) alpha = 1;
                using (var pen = new Pen(Color.FromArgb(alpha, 0, 0, 0), 1.5f))
                {
                    var shadowRect = new Rectangle(bounds.X - i, bounds.Y - i, bounds.Width + 2 * i, bounds.Height + 2 * i);
                    using (var path = GetRoundedRectPath(shadowRect, radius + i))
                        g.DrawPath(pen, path);
                }
            }
        }
    }
}
