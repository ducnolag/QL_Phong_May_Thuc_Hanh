using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using src.Helpers;

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

            // Add columns
            SetupGridLop();
            SetupGridMon();

            // Load data
            LoadLopHoc();
            LoadMonHoc();

            // Wire events
            WireEvents();
        }

        private void SetupGridLop()
        {
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "PK", Visible = false });
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên", ReadOnly = true, FillWeight = 60 });
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "SiSo", HeaderText = "Sĩ số", ReadOnly = true, FillWeight = 20 });
            AddActionColumns(dgvLopHoc);
        }

        private void SetupGridMon()
        {
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "PK", Visible = false });
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên", ReadOnly = true, FillWeight = 80 });
            AddActionColumns(dgvMonHoc);
        }

        private void AddActionColumns(DataGridView dgv)
        {
            var colEdit = new DataGridViewButtonColumn { Name = "Edit", HeaderText = "", Text = "✏ Sửa", UseColumnTextForButtonValue = true, FillWeight = 10, FlatStyle = FlatStyle.Flat };
            colEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEdit.DefaultCellStyle.BackColor = Color.FromArgb(239, 246, 255);
            colEdit.DefaultCellStyle.ForeColor = ThemeColors.PrimaryBlue;
            dgv.Columns.Add(colEdit);

            var colDel = new DataGridViewButtonColumn { Name = "Delete", HeaderText = "", Text = "🗑 Xóa", UseColumnTextForButtonValue = true, FillWeight = 10, FlatStyle = FlatStyle.Flat };
            colDel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDel.DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
            colDel.DefaultCellStyle.ForeColor = ThemeColors.AccentRed;
            dgv.Columns.Add(colDel);
            
            WireCursorEvent(dgv);
        }

        private void WireCursorEvent(DataGridView dgv)
        {
            dgv.CellMouseEnter += (s, e) =>
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                string colName = dgv.Columns[e.ColumnIndex].Name;
                if (colName == "Edit" || colName == "Delete") dgv.Cursor = Cursors.Hand;
                else dgv.Cursor = Cursors.Default;
            };
        }

        private void LoadLopHoc()
        {
            dgvLopHoc.Rows.Clear();
            try
            {
                var dt = _LopMonService.GetAllLopHoc();
                foreach (var r in dt) dgvLopHoc.Rows.Add(r.MaLop, r.TenLop, r.SiSo);
            }
            catch { }
        }

        private void LoadMonHoc()
        {
            dgvMonHoc.Rows.Clear();
            try
            {
                var dt = _LopMonService.GetAllMonHoc();
                foreach (var r in dt) dgvMonHoc.Rows.Add(r.MaMon, r.TenMon);
            }
            catch { }
        }

        private void WireEvents()
        {
            // Search
            txtSearchLop.TextChanged += (s, e) => DoSearch(dgvLopHoc, txtSearchLop.Text);
            txtSearchMon.TextChanged += (s, e) => DoSearch(dgvMonHoc, txtSearchMon.Text);

            // Add
            btnAddLop.Click += (s, e) => {
                var result = ShowLopInputDialog("Thêm Lớp học", "", 30);
                if (result == null) return;
                try { _LopMonService.CreateLopHoc(result.Value.name, result.Value.siso); LoadLopHoc(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
            };
            btnAddMon.Click += (s, e) => {
                string name = ShowInputDialog("Thêm Môn học", "Tên môn:", "");
                if (name == null) return;
                try { _LopMonService.CreateMonHoc(name); LoadMonHoc(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
            };

            // Grid clicks
            dgvLopHoc.CellClick += (s, e) => HandleGridLopClick(e);
            dgvMonHoc.CellClick += (s, e) => HandleGridClick(e, dgvMonHoc, "môn học", false, LoadMonHoc);
        }

        private void DoSearch(DataGridView dgv, string kw)
        {
            kw = kw.Trim().ToLower();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                row.Visible = string.IsNullOrEmpty(kw) || row.Cells["Ten"].Value?.ToString().ToLower().Contains(kw) == true;
            }
        }

        private void HandleGridLopClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvLopHoc.Rows[e.RowIndex];
            string col = dgvLopHoc.Columns[e.ColumnIndex].Name;
            int pk = Convert.ToInt32(row.Cells["PK"].Value);
            string ten = row.Cells["Ten"].Value?.ToString() ?? "";
            int siso = Convert.ToInt32(row.Cells["SiSo"].Value ?? 30);

            try
            {
                if (col == "Edit")
                {
                    var newLop = ShowLopInputDialog($"Sửa lớp học", ten, siso);
                    if (newLop == null) return;
                    _LopMonService.UpdateLopHoc(pk, newLop.Value.name, newLop.Value.siso);
                    LoadLopHoc();
                }
                else if (col == "Delete")
                {
                    if (MessageBox.Show($"Xóa '{ten}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _LopMonService.DeleteLopHoc(pk);
                        LoadLopHoc();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HandleGridClick(DataGridViewCellEventArgs e, DataGridView dgv, string label, bool isLop, Action reload)
        {
            if (e.RowIndex < 0) return;
            var row = dgv.Rows[e.RowIndex];
            string col = dgv.Columns[e.ColumnIndex].Name;
            int pk = Convert.ToInt32(row.Cells["PK"].Value);
            string ten = row.Cells["Ten"].Value?.ToString() ?? "";

            try
            {
                if (col == "Edit")
                {
                    string newName = ShowInputDialog($"Sửa {label}", "Tên:", ten);
                    if (newName == null) return;
                    if (isLop) _LopMonService.UpdateLopHoc(pk, newName, 30);
                    else _LopMonService.UpdateMonHoc(pk, newName);
                    reload();
                }
                else if (col == "Delete")
                {
                    if (MessageBox.Show($"Xóa '{ten}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (isLop) _LopMonService.DeleteLopHoc(pk);
                        else _LopMonService.DeleteMonHoc(pk);
                        reload();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string ShowInputDialog(string title, string label, string defaultVal)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(380, 160), StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };
            dlg.Controls.Add(new Label { Text = label, Location = new Point(20, 20), AutoSize = true });
            var txt = new TextBox { Text = defaultVal, Location = new Point(20, 44), Size = new Size(330, 28) };
            dlg.Controls.Add(txt);
            var btnOk = new Button
            {
                Text = "💾  Lưu", Location = new Point(130, 82), Size = new Size(100, 34),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), DialogResult = DialogResult.OK, Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnOk);
            var btnCancel = new Button
            {
                Text = "Hủy", Location = new Point(242, 82), Size = new Size(90, 34),
                BackColor = Color.FromArgb(241, 245, 249), ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel, Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCancel);
            dlg.AcceptButton = btnOk; dlg.CancelButton = btnCancel;
            btnOk.Click += (s, e) => { if (string.IsNullOrWhiteSpace(txt.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.DialogResult = DialogResult.None; } };
            return dlg.ShowDialog() == DialogResult.OK ? txt.Text.Trim() : null;
        }

        private (string name, int siso)? ShowLopInputDialog(string title, string defaultVal, int defaultSiso)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(380, 220), StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };
            dlg.Controls.Add(new Label { Text = "Tên lớp:", Location = new Point(20, 20), AutoSize = true });
            var txt = new TextBox { Text = defaultVal, Location = new Point(20, 44), Size = new Size(330, 28) };
            dlg.Controls.Add(txt);

            dlg.Controls.Add(new Label { Text = "Sĩ số:", Location = new Point(20, 80), AutoSize = true });
            var num = new NumericUpDown { Value = defaultSiso, Minimum = 1, Maximum = 200, Location = new Point(20, 104), Size = new Size(150, 28) };
            dlg.Controls.Add(num);

            var btnOk = new Button
            {
                Text = "💾  Lưu", Location = new Point(130, 142), Size = new Size(100, 34),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), DialogResult = DialogResult.OK, Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnOk);
            var btnCancel = new Button
            {
                Text = "Hủy", Location = new Point(242, 142), Size = new Size(90, 34),
                BackColor = Color.FromArgb(241, 245, 249), ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel, Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCancel);
            dlg.AcceptButton = btnOk; dlg.CancelButton = btnCancel;
            btnOk.Click += (s, e) => { if (string.IsNullOrWhiteSpace(txt.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); dlg.DialogResult = DialogResult.None; } };
            return dlg.ShowDialog() == DialogResult.OK ? (txt.Text.Trim(), (int)num.Value) : null;
        }

    }
}

