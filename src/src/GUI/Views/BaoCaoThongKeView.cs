using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using src.BLL;
using src.DTO;
using src.Helpers;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace src.Views
{
    /// <summary>
    /// Báo Cáo & Thống Kê – vẽ chart thủ công (GDI+) để tránh lỗi LiveCharts.
    /// Layout: Header cố định → Panel cuộn chứa 4 cards + 2 charts + 2 tables.
    /// </summary>
    public partial class BaoCaoThongKeView : UserControl
    {
        // ── Dữ liệu ─────────────────────────────────────────────────────────
        private int _totalRooms, _activeRooms, _closedRooms;
        private int _totalMay,   _mayTot,      _mayHong;
        private int _totalLich,  _lichDaXep,   _lichChoXep, _lichDaHuy;
        private int _totalUsers;

        private Panel _pnlCards, _pnlChartRooms, _pnlChartMay, _pnlMayTable, _pnlLichTable;
        private readonly IBaoCaoThongKeService _service;

        // variables

        public BaoCaoThongKeView()
        {
            InitializeComponent();
            _service = new BaoCaoThongKeService();
            DoubleBuffered = true;
            BackColor      = Color.FromArgb(245, 247, 250);
            Dock           = DockStyle.Fill;

            BuildDynamicLayout();
            InitFilters();

            btnRefresh.Click += (s, e) => Reload();

            pnlScroll.Resize += (s, e) => RelayoutBody(pnlScroll, pnlBody);
            this.Load += (s, e) => RelayoutBody(pnlScroll, pnlBody);

            Reload();
        }

        private void InitFilters()
        {
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Format = DateTimePickerFormat.Short;
            
            // Mặc định chọn từ đầu năm đến cuối tháng 6
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpToDate.Value = new DateTime(DateTime.Now.Year, 6, 30);
            dtpToDate.MinDate = dtpFromDate.Value;

            dtpFromDate.ValueChanged += (s, e) => {
                if (dtpToDate.Value < dtpFromDate.Value) dtpToDate.Value = dtpFromDate.Value;
                dtpToDate.MinDate = dtpFromDate.Value;
                Reload();
            };
            dtpToDate.ValueChanged += (s, e) => Reload();
        }

        // ────────────────────────────────────────────────────────────────────
        // Layout chỉ tạo phần dynamic charts bên trong pnlBody
        // ────────────────────────────────────────────────────────────────────
        private void BuildDynamicLayout()
        {
            int pad = 20; // padding ngang

            // ── 4 Stat Cards ─────────────────────────────────────────────
            _pnlCards = new Panel { Left = pad, Top = 8, Height = 110, BackColor = Color.Transparent };
            pnlBody.Controls.Add(_pnlCards);

            // ── 2 Charts ngang nhau ───────────────────────────────────────
            _pnlChartRooms = MakeCard();
            _pnlChartRooms.Top = 130;
            _pnlChartRooms.Left = pad;
            pnlBody.Controls.Add(_pnlChartRooms);

            _pnlChartMay = MakeCard();
            _pnlChartMay.Top = 130;
            pnlBody.Controls.Add(_pnlChartMay);

            // ── Bảng máy theo phòng ───────────────────────────────────────
            _pnlMayTable = MakeCard();
            _pnlMayTable.Height = 240;
            _pnlMayTable.Top    = 450;
            _pnlMayTable.Left   = pad;
            pnlBody.Controls.Add(_pnlMayTable);

            // ── Bảng lịch ─────────────────────────────────────────────────
            _pnlLichTable = MakeCard();
            _pnlLichTable.Height = 300;
            _pnlLichTable.Top    = 706;
            _pnlLichTable.Left   = pad;
            pnlBody.Controls.Add(_pnlLichTable);
        }

        private void RelayoutBody(Panel scroll, Panel body)
        {
            int sw = scroll.ClientSize.Width - (scroll.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0);
            if (sw < 200) return;
            int pad = 20;
            int w   = sw - pad * 2;
            body.Width = sw;

            _pnlCards.Width = w;
            RelayoutCards(w);

            int chartH = 290;
            int halfW  = (w - 12) / 2;
            _pnlChartRooms.SetBounds(pad, 130, halfW, chartH);
            _pnlChartMay.SetBounds(pad + halfW + 12, 130, halfW, chartH);

            _pnlMayTable.SetBounds(pad, 436, w, 240);
            _pnlLichTable.SetBounds(pad, 692, w, 300);

            body.Height = 692 + 300 + 24;
        }

        private void RelayoutCards(int totalW)
        {
            var cards = _pnlCards.Controls;
            if (cards.Count == 0) return;
            int gap = 12;
            int n   = cards.Count;
            int cardW = (totalW - gap * (n - 1)) / n;
            for (int i = 0; i < n; i++)
                cards[i].SetBounds(i * (cardW + gap), 0, cardW, 100);
        }

        // ────────────────────────────────────────────────────────────────────
        // Data + Render
        // ────────────────────────────────────────────────────────────────────
        private void Reload()
        {
            LoadStats();
            RenderCards();
            RenderChartRooms();
            RenderChartMay();
            RenderMayTable();
            RenderLichTable();
        }

        private void GetDateRange(out DateTime? startDate, out DateTime? endDate)
        {
            startDate = dtpFromDate.Value.Date;
            endDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);
        }

        private void LoadStats()
        {
            DateTime? startDate, endDate;
            GetDateRange(out startDate, out endDate);

            try
            {
                var dto = _service.GetThongKeTongQuan(startDate, endDate);
                
                _totalRooms  = dto.TotalRooms;
                _activeRooms = dto.ActiveRooms;
                _closedRooms = dto.ClosedRooms;

                _totalMay  = dto.TotalMay;
                _mayTot    = dto.MayTot;
                _mayHong   = dto.MayHong;

                _totalLich = dto.TotalLich;
                _lichDaXep = dto.LichDaXep;
                _lichDaHuy = dto.LichDaHuy;
                _lichChoXep = dto.LichChoXep;

                _totalUsers = dto.TotalUsers;
            }
            catch
            {
                _totalRooms=6; _activeRooms=5; _closedRooms=1;
                _totalMay=30;  _mayTot=25;    _mayHong=5;
                _totalLich=8;  _lichDaXep=5;  _lichChoXep=2; _lichDaHuy=1;
                _totalUsers=3;
            }
        }

        // ── 4 Stat Cards ──────────────────────────────────────────────────────
        private void RenderCards()
        {
            _pnlCards.Controls.Clear();
            var defs = new (string title, string val, string sub, string icon, Color accent)[]
            {
                ("Tổng phòng máy",   _totalRooms.ToString(), $"Hoạt động: {_activeRooms}  ·  Đóng cửa: {_closedRooms}",              "🏢", ThemeColors.PrimaryBlue),
                ("Tổng máy tính",    _totalMay.ToString(),   $"Tốt: {_mayTot}  ·  Hỏng: {_mayHong}",    "💻", ThemeColors.AccentGreen),
                ("Lịch thực hành",   _totalLich.ToString(),  $"Đã xếp: {_lichDaXep}  ·  Hủy: {_lichDaHuy}","📅", ThemeColors.AccentOrange),
                ("Người dùng",       _totalUsers.ToString(), "Quản trị viên + Nhân viên",                                          "👤", ThemeColors.AccentPurple),
            };
            foreach (var d in defs) _pnlCards.Controls.Add(MakeStatCard(d.title, d.val, d.sub, d.icon, d.accent));
            // Trigger relayout
            if (_pnlCards.Width > 0) RelayoutCards(_pnlCards.Width);
        }

        // ── Chart: Phòng theo trạng thái (Donut GDI+) ────────────────────────
        private void RenderChartRooms()
        {

            _pnlChartRooms.Controls.Clear();
            _pnlChartRooms.Controls.Add(new Label
            {
                Text = "🏢  Phòng máy theo trạng thái", AutoSize = true,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = ThemeColors.TextPrimary, Location = new Point(16, 12)
            });
            _pnlChartRooms.Controls.Add(new Label
            {
                Text = $"Trạng thái hiện tại", AutoSize = true,
                Font = new Font("Segoe UI", 8.5F), ForeColor = ThemeColors.TextSecondary,
                Location = new Point(16, 32)
            });

            var segments = new List<(string label, int value, Color color)>
            {
                ("Hoạt động", _activeRooms, Color.FromArgb(34, 197, 94)),
                ("Đóng cửa",  _closedRooms, Color.FromArgb(239, 68, 68)),
            };
            var chartPanel = new DonutChartPanel(segments) { Left = 16, Top = 54, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            _pnlChartRooms.Controls.Add(chartPanel);
            _pnlChartRooms.Resize += (s, e) => chartPanel.SetBounds(16, 54, _pnlChartRooms.Width - 32, _pnlChartRooms.Height - 66);
            if (_pnlChartRooms.Width > 0) chartPanel.SetBounds(16, 54, _pnlChartRooms.Width - 32, _pnlChartRooms.Height - 66);
        }

        // ── Chart: Máy theo trạng thái (Bar GDI+) ────────────────────────────
        private void RenderChartMay()
        {

            _pnlChartMay.Controls.Clear();
            _pnlChartMay.Controls.Add(new Label
            {
                Text = "💻  Tình trạng máy tính", AutoSize = true,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = ThemeColors.TextPrimary, Location = new Point(16, 12)
            });
            _pnlChartMay.Controls.Add(new Label
            {
                Text = $"Tổng {_totalMay} máy", AutoSize = true,
                Font = new Font("Segoe UI", 8.5F), ForeColor = ThemeColors.TextSecondary,
                Location = new Point(16, 32)
            });

            var bars = new List<(string label, int value, Color color)>
            {
                ("Tốt",     _mayTot,    Color.FromArgb(34, 197, 94)),
                ("Hỏng",    _mayHong,   Color.FromArgb(239, 68, 68)),
            };
            var barPanel = new BarChartPanel(bars) { Left = 16, Top = 54, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            _pnlChartMay.Controls.Add(barPanel);
            _pnlChartMay.Resize += (s, e) => barPanel.SetBounds(16, 54, _pnlChartMay.Width - 32, _pnlChartMay.Height - 66);
            if (_pnlChartMay.Width > 0) barPanel.SetBounds(16, 54, _pnlChartMay.Width - 32, _pnlChartMay.Height - 66);
        }

        // ── Bảng máy theo phòng ──────────────────────────────────────────────
        private void RenderMayTable()
        {

            _pnlMayTable.Controls.Clear();
            _pnlMayTable.Controls.Add(new Label
            {
                Text = "💻  Tình Trạng Máy Tính Theo Phòng",
                Location = new Point(16, 12), AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = ThemeColors.TextPrimary
            });

            var dgv = MakeDgv();
            dgv.SetBounds(16, 46, _pnlMayTable.Width - 32, _pnlMayTable.Height - 58);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgv.Columns.Add("Phong",  "Phòng máy");
            dgv.Columns.Add("Tong",   "Tổng máy");
            dgv.Columns.Add("Tot",    "Tốt");
            dgv.Columns.Add("Hong",   "Hỏng");
            dgv.Columns.Add("TiLe",   "% Tốt");

            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != "TiLe" || e.Value == null) return;
                if (int.TryParse(e.Value.ToString().Replace("%", ""), out int v))
                    e.CellStyle.ForeColor = v >= 80 ? ThemeColors.AccentGreen : v >= 50 ? ThemeColors.AccentOrange : ThemeColors.AccentRed;
                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                e.FormattingApplied = true;
            };

            try
            {
                var list = _service.GetThongKeMayTheoPhong();
                foreach (var item in list)
                {
                    dgv.Rows.Add(item.TenPhong, item.Tong, item.Tot, item.Hong,
                        item.Tong > 0 ? $"{item.Tot * 100 / item.Tong}%" : "—");
                }
            }
            catch
            {
                dgv.Rows.Add("Lab A-301", 30, 28, 2, "93%");
            }
            _pnlMayTable.Controls.Add(dgv);
        }

        private void RenderLichTable()
        {

            _pnlLichTable.Controls.Clear();

            string period = $" ({dtpFromDate.Value:dd/MM/yyyy} - {dtpToDate.Value:dd/MM/yyyy})";

            _pnlLichTable.Controls.Add(new Label
            {
                Text = "📅  Thống Kê Lịch Thực Hành" + period,
                Location = new Point(16, 12), AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = ThemeColors.TextPrimary
            });

            var dgv = MakeDgv();
            dgv.SetBounds(16, 46, _pnlLichTable.Width - 32, _pnlLichTable.Height - 58);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgv.Columns.Add("Ngay",     "Ngày TH");
            dgv.Columns.Add("Mon",      "Môn học");
            dgv.Columns.Add("Ca",       "Ca học");
            dgv.Columns.Add("SV",       "Số SV");
            dgv.Columns.Add("Phong",    "Phòng xếp");
            dgv.Columns.Add("TrangThai","Trạng thái");

            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != "TrangThai" || e.Value == null) return;
                string v = e.Value.ToString();
                e.CellStyle.ForeColor = v.Contains("hủy") ? ThemeColors.AccentRed
                    : v.Contains("Đã xếp") ? ThemeColors.AccentGreen
                    : ThemeColors.AccentOrange;
                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                e.FormattingApplied = true;
            };

            try
            {
                DateTime? startDate, endDate;
                GetDateRange(out startDate, out endDate);

                var list = _service.GetThongKeLich(startDate, endDate);

                foreach (var item in list)
                {
                    string trangThai = item.TrangThaiLich == "Đã hủy" ? "Đã hủy"
                        : item.TenPhong == "Chưa xếp" ? "Chờ xếp phòng"
                        : "Đã xếp phòng";
                    
                    dgv.Rows.Add(
                        item.NgayThucHanh.ToString("dd/MM/yyyy"),
                        item.TenMon, item.TenCa,
                        item.SoLuongSinhVien + " SV",
                        item.TenPhong, trangThai);
                }
                
                if (list.Count == 0)
                    dgv.Rows.Add("—", "Chưa có lịch trong kỳ này", "—", "—", "—", "—");
            }
            catch
            {
                dgv.Rows.Add("01/06/2025", "Lập trình C#", "Ca 1", "35 SV", "Lab A-301", "Đã xếp phòng");
            }
            _pnlLichTable.Controls.Add(dgv);
        }

        // ── Helpers ──────────────────────────────────────────────────────────
        private static Panel MakeCard()
            => new Panel { BackColor = Color.White, Location = new Point(0, 0) };

        private static ComboBox MakeCbo(string[] items, int selected)
        {
            var c = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 70, Font = new Font("Segoe UI", 9F) };
            c.Items.AddRange(items);
            c.SelectedIndex = selected;
            return c;
        }

        private Panel MakeStatCard(string title, string value, string sub, string icon, Color accent)
        {
            var card = new Panel { BackColor = Color.White };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Accent bar
                using (var br = new SolidBrush(accent))
                    g.FillRectangle(br, 0, 10, 4, 80);
                // Icon circle
                int ix = card.Width - 52;
                using (var br = new SolidBrush(Color.FromArgb(25, accent)))
                    g.FillEllipse(br, ix, 10, 40, 40);
                TextRenderer.DrawText(g, icon, new Font("Segoe UI", 15F), new Rectangle(ix, 10, 40, 40), accent,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                // Texts
                TextRenderer.DrawText(g, title, new Font("Segoe UI", 8.5F),
                    new Rectangle(14, 10, card.Width - 70, 22), ThemeColors.TextSecondary,
                    TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
                TextRenderer.DrawText(g, value, new Font("Segoe UI", 26F, FontStyle.Bold),
                    new Rectangle(14, 28, card.Width - 70, 44), ThemeColors.TextPrimary,
                    TextFormatFlags.Left);
                TextRenderer.DrawText(g, sub, new Font("Segoe UI", 7.5F),
                    new Rectangle(14, 76, card.Width - 20, 20), ThemeColors.TextSecondary,
                    TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            };
            return card;
        }

        private DataGridView MakeDgv()
        {
            var dgv = new DataGridView
            {
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(226, 232, 240),
                AllowUserToAddRows = false, ReadOnly = true, RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9.5F),
                RowTemplate = { Height = 38 },
                ColumnHeadersHeight = 40, EnableHeadersVisualStyles = false
            };
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(249, 250, 251),
                Padding = new Padding(8, 0, 0, 0),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(249, 250, 252) };
            return dgv;
        }
    }

    // ── LiveCharts2: Donut Chart ─────────────────────────────────────────────
    internal class DonutChartPanel : Panel
    {
        public DonutChartPanel(List<(string label, int value, Color color)> segments)
        {
            BackColor = Color.White;
            var chart = new PieChart
            {
                Dock = DockStyle.Fill,
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
                Series = segments.Select(s => new PieSeries<int>
                {
                    Values = new[] { s.value },
                    Name = s.label,
                    InnerRadius = 40,
                    Fill = new SolidColorPaint(new SKColor(s.color.R, s.color.G, s.color.B))
                }).ToArray()
            };
            Controls.Add(chart);
        }
    }

    // ── LiveCharts2: Bar Chart ───────────────────────────────────────────────
    internal class BarChartPanel : Panel
    {
        public BarChartPanel(List<(string label, int value, Color color)> bars)
        {
            BackColor = Color.White;
            var chart = new CartesianChart
            {
                Dock = DockStyle.Fill,
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
                Series = bars.Select((b, i) =>
                {
                    var vals = new int?[bars.Count];
                    vals[i] = b.value;
                    return new ColumnSeries<int?>
                    {
                        Name = b.label,
                        Values = vals,
                        Fill = new SolidColorPaint(new SKColor(b.color.R, b.color.G, b.color.B)),
                        MaxBarWidth = 40,
                        IgnoresBarPosition = true // Center the bar on the tick
                    };
                }).ToArray(),
                XAxes = new[]
                {
                    new Axis
                    {
                        Labels = bars.Select(b => b.label).ToArray(),
                        LabelsPaint = new SolidColorPaint(new SKColor(100, 116, 139))
                    }
                },
                YAxes = new[]
                {
                    new Axis
                    {
                        MinLimit = 0,
                        LabelsPaint = new SolidColorPaint(new SKColor(100, 116, 139))
                    }
                }
            };
            Controls.Add(chart);
        }
    }
}

