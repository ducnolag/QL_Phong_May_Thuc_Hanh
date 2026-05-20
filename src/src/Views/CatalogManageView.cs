using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using src.Helpers;

namespace src.Views
{
    public partial class CatalogManageView : UserControl
    {
        public CatalogManageView()
        {
            InitializeComponent();
            DoubleBuffered = true;

            // Apply card styles
            UIHelper.ApplyCardStyle(pnlToolbarLop, 8);
            UIHelper.ApplyCardStyle(pnlGridLop, 8);
            UIHelper.ApplyCardStyle(pnlToolbarMon, 8);
            UIHelper.ApplyCardStyle(pnlGridMon, 8);
            UIHelper.ApplyCardStyle(pnlToolbarCa, 8);
            UIHelper.ApplyCardStyle(pnlGridCa, 8);

            // Add columns
            SetupGridLop();
            SetupGridMon();
            SetupGridCa();

            // Load data
            LoadLopHoc();
            LoadMonHoc();
            LoadCaHoc();

            // Wire events
            WireEvents();
        }

        private void SetupGridLop()
        {
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "PK", Visible = false });
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên", ReadOnly = true, FillWeight = 80 });
            AddActionColumns(dgvLopHoc);
        }

        private void SetupGridMon()
        {
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "PK", Visible = false });
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên", ReadOnly = true, FillWeight = 80 });
            AddActionColumns(dgvMonHoc);
        }

        private void SetupGridCa()
        {
            dgvCaHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "PK", Visible = false });
            dgvCaHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên ca", ReadOnly = true, FillWeight = 50 });
            dgvCaHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "GioBD", HeaderText = "Giờ bắt đầu", ReadOnly = true, FillWeight = 20 });
            dgvCaHoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "GioKT", HeaderText = "Giờ kết thúc", ReadOnly = true, FillWeight = 20 });
            var colEdit = new DataGridViewButtonColumn { Name = "Edit", HeaderText = "", Text = "✏ Sửa", UseColumnTextForButtonValue = true, FillWeight = 10, FlatStyle = FlatStyle.Flat };
            colEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEdit.DefaultCellStyle.BackColor = Color.FromArgb(239, 246, 255);
            colEdit.DefaultCellStyle.ForeColor = ThemeColors.PrimaryBlue;
            dgvCaHoc.Columns.Add(colEdit);
            var colDel = new DataGridViewButtonColumn { Name = "Delete", HeaderText = "", Text = "🗑 Xóa", UseColumnTextForButtonValue = true, FillWeight = 10, FlatStyle = FlatStyle.Flat };
            colDel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDel.DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
            colDel.DefaultCellStyle.ForeColor = ThemeColors.AccentRed;
            dgvCaHoc.Columns.Add(colDel);
            
            WireCursorEvent(dgvCaHoc);
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
                var dt = DatabaseHelper.ExecuteQuery("SELECT MaLop, TenLop FROM LOP_HOC ORDER BY TenLop");
                foreach (DataRow r in dt.Rows) dgvLopHoc.Rows.Add(r["MaLop"], r["TenLop"]);
            }
            catch { }
        }

        private void LoadMonHoc()
        {
            dgvMonHoc.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT MaMon, TenMon FROM MON_HOC ORDER BY TenMon");
                foreach (DataRow r in dt.Rows) dgvMonHoc.Rows.Add(r["MaMon"], r["TenMon"]);
            }
            catch { }
        }

        private void LoadCaHoc()
        {
            dgvCaHoc.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT MaCa, TenCa, GioBatDau, GioKetThuc FROM CA_HOC ORDER BY GioBatDau");
                foreach (DataRow r in dt.Rows)
                {
                    string gb = r["GioBatDau"] == DBNull.Value ? "--" : Convert.ToDateTime(r["GioBatDau"]).ToString("HH:mm");
                    string gk = r["GioKetThuc"] == DBNull.Value ? "--" : Convert.ToDateTime(r["GioKetThuc"]).ToString("HH:mm");
                    dgvCaHoc.Rows.Add(r["MaCa"], r["TenCa"], gb, gk);
                }
            }
            catch { }
        }

        private void WireEvents()
        {
            // Search
            txtSearchLop.TextChanged += (s, e) => DoSearch(dgvLopHoc, txtSearchLop.Text);
            txtSearchMon.TextChanged += (s, e) => DoSearch(dgvMonHoc, txtSearchMon.Text);
            txtSearchCa.TextChanged += (s, e) => DoSearch(dgvCaHoc, txtSearchCa.Text);

            // Add
            btnAddLop.Click += (s, e) => {
                string name = ShowInputDialog("Thêm Lớp học", "Tên lớp:", "");
                if (name == null) return;
                ExecuteDb("INSERT INTO LOP_HOC (TenLop) VALUES (@ten)", new SqlParameter("@ten", name));
                LoadLopHoc();
            };
            btnAddMon.Click += (s, e) => {
                string name = ShowInputDialog("Thêm Môn học", "Tên môn:", "");
                if (name == null) return;
                ExecuteDb("INSERT INTO MON_HOC (TenMon) VALUES (@ten)", new SqlParameter("@ten", name));
                LoadMonHoc();
            };
            btnAddCa.Click += (s, e) => {
                using var dlg = BuildCaDialog("Thêm Ca Học", "", "", "");
                if (dlg.ShowDialog() != DialogResult.OK) return;
                string ten = ((TextBox)dlg.Controls["txtTen"]).Text.Trim();
                string gb = ((TextBox)dlg.Controls["txtGioBD"]).Text.Trim();
                string gk = ((TextBox)dlg.Controls["txtGioKT"]).Text.Trim();
                if (string.IsNullOrEmpty(ten)) return;
                ExecuteDb("INSERT INTO CA_HOC (TenCa, GioBatDau, GioKetThuc) VALUES (@ten, @bd, @kt)",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@bd", string.IsNullOrEmpty(gb) ? DBNull.Value : (object)TimeSpan.Parse(gb)),
                    new SqlParameter("@kt", string.IsNullOrEmpty(gk) ? DBNull.Value : (object)TimeSpan.Parse(gk)));
                LoadCaHoc();
            };

            // Grid clicks
            dgvLopHoc.CellClick += (s, e) => HandleGridClick(e, dgvLopHoc, "lớp học", "LOP_HOC", "TenLop", "MaLop", LoadLopHoc);
            dgvMonHoc.CellClick += (s, e) => HandleGridClick(e, dgvMonHoc, "môn học", "MON_HOC", "TenMon", "MaMon", LoadMonHoc);
            dgvCaHoc.CellClick += (s, e) => HandleCaGridClick(e);
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

        private void HandleGridClick(DataGridViewCellEventArgs e, DataGridView dgv, string label, string table, string colName, string colPk, Action reload)
        {
            if (e.RowIndex < 0) return;
            var row = dgv.Rows[e.RowIndex];
            string col = dgv.Columns[e.ColumnIndex].Name;
            int pk = Convert.ToInt32(row.Cells["PK"].Value);
            string ten = row.Cells["Ten"].Value?.ToString() ?? "";

            if (col == "Edit")
            {
                string newName = ShowInputDialog($"Sửa {label}", "Tên:", ten);
                if (newName == null) return;
                ExecuteDb($"UPDATE {table} SET {colName}=@ten WHERE {colPk}=@pk",
                    new SqlParameter("@ten", newName), new SqlParameter("@pk", pk));
                reload();
            }
            else if (col == "Delete")
            {
                if (MessageBox.Show($"Xóa '{ten}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ExecuteDb($"DELETE FROM {table} WHERE {colPk}=@pk", new SqlParameter("@pk", pk));
                    reload();
                }
            }
        }

        private void HandleCaGridClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvCaHoc.Rows[e.RowIndex];
            string col = dgvCaHoc.Columns[e.ColumnIndex].Name;
            int pk = Convert.ToInt32(row.Cells["PK"].Value);
            string ten = row.Cells["Ten"].Value?.ToString() ?? "";
            string gb = row.Cells["GioBD"].Value?.ToString() ?? "";
            string gk = row.Cells["GioKT"].Value?.ToString() ?? "";

            if (col == "Edit")
            {
                using var dlg = BuildCaDialog($"Sửa Ca: {ten}", ten, gb, gk);
                if (dlg.ShowDialog() != DialogResult.OK) return;
                string newTen = ((TextBox)dlg.Controls["txtTen"]).Text.Trim();
                string newGb = ((TextBox)dlg.Controls["txtGioBD"]).Text.Trim();
                string newGk = ((TextBox)dlg.Controls["txtGioKT"]).Text.Trim();
                if (string.IsNullOrEmpty(newTen)) return;
                ExecuteDb("UPDATE CA_HOC SET TenCa=@ten, GioBatDau=@bd, GioKetThuc=@kt WHERE MaCa=@pk",
                    new SqlParameter("@ten", newTen),
                    new SqlParameter("@bd", string.IsNullOrEmpty(newGb) ? DBNull.Value : (object)TimeSpan.Parse(newGb)),
                    new SqlParameter("@kt", string.IsNullOrEmpty(newGk) ? DBNull.Value : (object)TimeSpan.Parse(newGk)),
                    new SqlParameter("@pk", pk));
                LoadCaHoc();
            }
            else if (col == "Delete")
            {
                if (MessageBox.Show($"Xóa '{ten}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ExecuteDb("DELETE FROM CA_HOC WHERE MaCa=@pk", new SqlParameter("@pk", pk));
                    LoadCaHoc();
                }
            }
        }

        private void ExecuteDb(string sql, params SqlParameter[] parameters)
        {
            try
            {
                DatabaseHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                string msg = (ex.Message.Contains("REFERENCE") || ex.Message.Contains("FK_"))
                    ? "Không thể xóa vì đang được sử dụng trong lịch thực hành."
                    : "Lỗi: " + ex.Message;
                MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private Form BuildCaDialog(string title, string ten, string gioBD, string gioKT)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(360, 240), StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };
            int y = 20;
            dlg.Controls.Add(new Label { Text = "Tên ca:", Location = new Point(20, y + 3), AutoSize = true });
            var txtTen = new TextBox { Name = "txtTen", Text = ten, Location = new Point(130, y), Size = new Size(200, 26) };
            dlg.Controls.Add(txtTen);
            y += 40;
            dlg.Controls.Add(new Label { Text = "Giờ bắt đầu:", Location = new Point(20, y + 3), AutoSize = true });
            var txtBD = new TextBox { Name = "txtGioBD", Text = gioBD, Location = new Point(130, y), Size = new Size(100, 26), PlaceholderText = "07:00" };
            dlg.Controls.Add(txtBD);
            y += 40;
            dlg.Controls.Add(new Label { Text = "Giờ kết thúc:", Location = new Point(20, y + 3), AutoSize = true });
            var txtKT = new TextBox { Name = "txtGioKT", Text = gioKT, Location = new Point(130, y), Size = new Size(100, 26), PlaceholderText = "09:30" };
            dlg.Controls.Add(txtKT);
            y += 48;
            var btnSave = new Button { Text = "💾  Lưu", Location = new Point(100, y), Size = new Size(100, 34), BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Cursor = Cursors.Hand, DialogResult = DialogResult.OK };
            btnSave.FlatAppearance.BorderSize = 0; dlg.Controls.Add(btnSave);
            var btnCan = new Button { Text = "Hủy", Location = new Point(212, y), Size = new Size(90, 34), BackColor = Color.FromArgb(241, 245, 249), ForeColor = ThemeColors.TextSecondary, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, DialogResult = DialogResult.Cancel };
            btnCan.FlatAppearance.BorderSize = 0; dlg.Controls.Add(btnCan);
            dlg.AcceptButton = btnSave; dlg.CancelButton = btnCan;
            return dlg;
        }
    }
}
