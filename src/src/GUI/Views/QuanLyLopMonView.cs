using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using src.Helpers;
using src.DTO;

namespace src.Views
{
    public partial class QuanLyLopMonView : UserControl
    {
        private readonly src.BLL.LopMonService _LopMonService;

        public QuanLyLopMonView()
        {
            InitializeComponent();
            _LopMonService = new src.BLL.LopMonService();
            DoubleBuffered = true;

            SetupGrids();
            LoadMonHoc();
            LoadLopHoc();
            WireEvents();
        }

        // ── Cấu hình columns ────────────────────────────────────────────
        private void SetupGrids()
        {
            // Grid Mon hoc
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "PK", Visible = false });
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên Môn Học", ReadOnly = true, FillWeight = 70 });
            AddBtnCol(dgvMonHoc, "Edit",   "✏  Sửa",  Color.FromArgb(239,246,255), ThemeColors.PrimaryBlue,  15);
            AddBtnCol(dgvMonHoc, "Delete", "🗑 Xóa",  Color.FromArgb(254,226,226), ThemeColors.AccentRed,   15);
            ApplyGridStyling(dgvMonHoc);
            WireCursor(dgvMonHoc);

            // Grid Lop hoc phan
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "PK", Visible = false });
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Mã / Tên Lớp Học Phần", ReadOnly = true, FillWeight = 30 });
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Mon", HeaderText = "Thuộc Môn Học", ReadOnly = true, FillWeight = 40 });
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "SiSo", HeaderText = "Sĩ số", ReadOnly = true, FillWeight = 15 });
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaMon", Visible = false });
            AddBtnCol(dgvLopHoc, "Edit",   "✏  Sửa",  Color.FromArgb(239,246,255), ThemeColors.PrimaryBlue, 15);
            AddBtnCol(dgvLopHoc, "Delete", "🗑 Xóa",  Color.FromArgb(254,226,226), ThemeColors.AccentRed,  15);
            ApplyGridStyling(dgvLopHoc);
            WireCursor(dgvLopHoc);
        }

        private void AddBtnCol(DataGridView dgv, string name, string text, Color bg, Color fg, int weight)
        {
            var col = new DataGridViewButtonColumn
            {
                Name = name, HeaderText = "", Text = text,
                UseColumnTextForButtonValue = true, FillWeight = weight, FlatStyle = FlatStyle.Flat
            };
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor  = bg;
            col.DefaultCellStyle.ForeColor  = fg;
            col.DefaultCellStyle.Font       = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            col.DefaultCellStyle.Padding    = new Padding(4, 4, 4, 4);
            dgv.Columns.Add(col);
        }

        private void ApplyGridStyling(DataGridView dgv)
        {
            dgv.Font = new Font("Segoe UI", 9.5F);
            dgv.RowTemplate.Height = 46;
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(249, 250, 251),
                SelectionForeColor = ThemeColors.TextSecondary,
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                SelectionBackColor = Color.FromArgb(241, 245, 249),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
        }

        private void WireCursor(DataGridView dgv)
        {
            dgv.CellMouseEnter += (s, e) =>
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                string col = dgv.Columns[e.ColumnIndex].Name;
                dgv.Cursor = (col == "Edit" || col == "Delete") ? Cursors.Hand : Cursors.Default;
            };
            dgv.CellMouseLeave += (s, e) => dgv.Cursor = Cursors.Default;
        }

        // ── Load dữ liệu ────────────────────────────────────────────────
        private void LoadMonHoc()
        {
            dgvMonHoc.Rows.Clear();
            try
            {
                var list = _LopMonService.GetAllMonHoc().ToList();
                foreach (var r in list) dgvMonHoc.Rows.Add(r.MaMon, r.TenMon);
                lblMonCount.Text = $"{list.Count} môn học";
            }
            catch (Exception ex)
            {
                lblMonCount.Text = "Lỗi tải dữ liệu";
                MessageBox.Show("Lỗi tải môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLopHoc()
        {
            dgvLopHoc.Rows.Clear();
            try
            {
                var list = _LopMonService.GetAllLopHoc().ToList();
                foreach (var r in list) dgvLopHoc.Rows.Add(r.MaLop, r.TenLop, string.IsNullOrEmpty(r.TenMon) ? "(Chưa gắn môn)" : r.TenMon, r.SiSo, r.MaMon);
                lblLopCount.Text = $"{list.Count} lớp học phần";
            }
            catch (Exception ex)
            {
                lblLopCount.Text = "Lỗi tải dữ liệu";
                MessageBox.Show("Lỗi tải lớp học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Wire events ─────────────────────────────────────────────────
        private void WireEvents()
        {
            txtSearchMon.TextChanged += (s, e) => DoSearch(dgvMonHoc, txtSearchMon.Text);
            txtSearchLop.TextChanged += (s, e) => DoSearch(dgvLopHoc, txtSearchLop.Text);

            btnAddMon.Click += (s, e) =>
            {
                string name = ShowMonDialog("Thêm Môn học", "Tên môn:", "");
                if (name == null) return;
                try { _LopMonService.CreateMonHoc(name); LoadMonHoc(); LoadLopHoc(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };

            btnAddLop.Click += (s, e) =>
            {
                var result = ShowLopDialog("Thêm Lớp học phần", "", 30, null);
                if (result == null) return;
                try { _LopMonService.CreateLopHoc(result.Value.name, result.Value.siso, result.Value.maMon); LoadLopHoc(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };

            dgvMonHoc.CellClick += HandleMonClick;
            dgvLopHoc.CellClick += HandleLopClick;
        }

        private void DoSearch(DataGridView dgv, string kw)
        {
            kw = kw.Trim().ToLower();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                row.Visible = string.IsNullOrEmpty(kw)
                    || row.Cells["Ten"].Value?.ToString().ToLower().Contains(kw) == true;
            }
        }

        // ── Xử lý click Grid Mon ────────────────────────────────────────
        private void HandleMonClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvMonHoc.Rows[e.RowIndex];
            string col = dgvMonHoc.Columns[e.ColumnIndex].Name;
            int pk   = Convert.ToInt32(row.Cells["PK"].Value);
            string ten = row.Cells["Ten"].Value?.ToString() ?? "";

            if (col != "Edit" && col != "Delete") return;

            try
            {
                if (col == "Edit")
                {
                    string newName = ShowMonDialog("Sửa Môn học", "Tên môn:", ten);
                    if (newName == null) return;
                    _LopMonService.UpdateMonHoc(pk, newName);
                    LoadMonHoc();
                    LoadLopHoc(); // To update class names
                }
                else if (col == "Delete")
                {
                    if (MessageBox.Show($"Xóa môn '{ten}' và toàn bộ lớp học phần phụ thuộc?\nThao tác này không thể hoàn tác.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        _LopMonService.DeleteMonHoc(pk);
                        LoadMonHoc();
                        LoadLopHoc();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        // ── Xử lý click Grid Lop ────────────────────────────────────────
        private void HandleLopClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvLopHoc.Rows[e.RowIndex];
            string col  = dgvLopHoc.Columns[e.ColumnIndex].Name;
            int pk      = Convert.ToInt32(row.Cells["PK"].Value);
            string ten  = row.Cells["Ten"].Value?.ToString() ?? "";
            int siso    = Convert.ToInt32(row.Cells["SiSo"].Value ?? 30);
            int? maMon  = row.Cells["MaMon"].Value != null ? Convert.ToInt32(row.Cells["MaMon"].Value) : (int?)null;

            if (col != "Edit" && col != "Delete") return;

            try
            {
                if (col == "Edit")
                {
                    var result = ShowLopDialog("Sửa Lớp học phần", ten, siso, maMon);
                    if (result == null) return;
                    _LopMonService.UpdateLopHoc(pk, result.Value.name, result.Value.siso, result.Value.maMon);
                    LoadLopHoc();
                }
                else if (col == "Delete")
                {
                    if (MessageBox.Show($"Xóa lớp '{ten}' khỏi hệ thống?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _LopMonService.DeleteLopHoc(pk);
                        LoadLopHoc();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        // ── Dialogs ─────────────────────────────────────────────────────
        private string ShowMonDialog(string title, string label, string defaultVal)
        {
            var dlg = BuildDialog(title, 380, 165);
            dlg.Controls.Add(new Label { Text = label, Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 10F) });
            var txt = new TextBox { Text = defaultVal, Location = new Point(20, 46), Size = new Size(330, 28), Font = new Font("Segoe UI", 10F) };
            dlg.Controls.Add(txt);
            AddDialogButtons(dlg, 86, () => string.IsNullOrWhiteSpace(txt.Text) ? "Vui lòng nhập thông tin!" : null);
            return dlg.ShowDialog() == DialogResult.OK ? txt.Text.Trim() : null;
        }

        private (string name, int siso, int? maMon)? ShowLopDialog(string title, string defaultVal, int defaultSiso, int? defaultMaMon)
        {
            var dlg = BuildDialog(title, 380, 290);
            
            dlg.Controls.Add(new Label { Text = "Mã / Tên lớp:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 10F) });
            var txt = new TextBox { Text = defaultVal, Location = new Point(20, 46), Size = new Size(330, 28), Font = new Font("Segoe UI", 10F) };
            dlg.Controls.Add(txt);
            
            dlg.Controls.Add(new Label { Text = "Sĩ số:", Location = new Point(20, 84), AutoSize = true, Font = new Font("Segoe UI", 10F) });
            var num = new NumericUpDown { Value = defaultSiso, Minimum = 1, Maximum = 300, Location = new Point(20, 110), Size = new Size(150, 28), Font = new Font("Segoe UI", 10F) };
            dlg.Controls.Add(num);

            dlg.Controls.Add(new Label { Text = "Môn học phụ trách:", Location = new Point(20, 148), AutoSize = true, Font = new Font("Segoe UI", 10F) });
            var cboMon = new ComboBox { Location = new Point(20, 174), Size = new Size(330, 28), Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            
            try 
            {
                var dsMon = _LopMonService.GetAllMonHoc().ToList();
                cboMon.DisplayMember = "TenMon";
                cboMon.ValueMember = "MaMon";
                cboMon.DataSource = dsMon;
                if (defaultMaMon != null && dsMon.Any(m => m.MaMon == defaultMaMon.Value))
                    cboMon.SelectedValue = defaultMaMon.Value;
            } 
            catch { }

            dlg.Controls.Add(cboMon);

            AddDialogButtons(dlg, 214, () => string.IsNullOrWhiteSpace(txt.Text) ? "Vui lòng nhập mã/tên lớp!" : null);
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int? maMonSelected = cboMon.SelectedValue != null ? (int?)cboMon.SelectedValue : null;
                return (txt.Text.Trim(), (int)num.Value, maMonSelected);
            }
            return null;
        }

        private Form BuildDialog(string title, int w, int h)
        {
            return new Form
            {
                Text = title, Size = new Size(w, h),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };
        }

        private void AddDialogButtons(Form dlg, int y, Func<string> validate)
        {
            var btnOk = new Button
            {
                Text = "💾  Lưu", Location = new Point(130, y), Size = new Size(110, 36),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                DialogResult = DialogResult.OK, Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCan = new Button
            {
                Text = "Hủy", Location = new Point(252, y), Size = new Size(90, 36),
                BackColor = Color.FromArgb(241, 245, 249), ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel, Cursor = Cursors.Hand
            };
            btnCan.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) =>
            {
                string err = validate?.Invoke();
                if (!string.IsNullOrEmpty(err)) { MessageBox.Show(err, "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.DialogResult = DialogResult.None; }
            };
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(btnCan);
            dlg.AcceptButton = btnOk;
            dlg.CancelButton = btnCan;
        }
    }
}
