using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using src.Helpers;

namespace src.Views
{
    /// <summary>
    /// Quản lý máy tính – Bộ lọc theo phòng/CPU/RAM/Status + CRUD đầy đủ.
    /// </summary>
    public partial class ComputerManageView : UserControl
    {
        public ComputerManageView()
        {
            InitializeComponent();
            SetupView();
        }

        private void SetupView()
        {
            UIHelper.ApplyCardStyle(pnlToolbar, 14);
            UIHelper.ApplyCardStyle(pnlGrid, 14);

            // Load danh sách phòng vào cboRoom
            cboRoom.Items.Clear();
            cboRoom.Items.Add("Tất cả phòng");
            try
            {
                var dtPhong = DatabaseHelper.ExecuteQuery(
                    "SELECT TenPhong FROM PHONG_MAY ORDER BY TenPhong");
                foreach (DataRow r in dtPhong.Rows)
                    cboRoom.Items.Add(r["TenPhong"].ToString());
            }
            catch { /* giữ mặc định */ }

            cboRoom.SelectedIndex   = 0;
            cboCPU.SelectedIndex    = 0;
            cboRAM.SelectedIndex    = 0;
            cboStatus.SelectedIndex = 0;

            // Sự kiện lọc
            txtSearch.TextChanged          += (s, e) => FilterRows();
            cboRoom.SelectedIndexChanged   += (s, e) => FilterRows();
            cboCPU.SelectedIndexChanged    += (s, e) => FilterRows();
            cboRAM.SelectedIndexChanged    += (s, e) => FilterRows();
            cboStatus.SelectedIndexChanged += (s, e) => FilterRows();

            // Nút Thêm máy / Sửa / Xóa
            btnAdd.Click   += (s, e) => ShowAddDialog();
            // Sửa / Xóa theo hàng được chọn
            dgv.CellClick  += Dgv_CellClick;

            SetupGridStyles();
            LoadData();
        }

        // ── Cài cột ─────────────────────────────────────────────────────
        private void SetupGridStyles()
        {
            dgv.Columns.Clear();
            // Cột ẩn chứa PK để Sửa/Xóa
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaMay",   HeaderText = "MaMay",   Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaPhong", HeaderText = "MaPhong", Visible = false });

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ComputerID", HeaderText = "Mã máy",      Width = 110, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Room",       HeaderText = "Phòng",       Width = 120, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "CPU",        HeaderText = "CPU",         FillWeight = 100, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RAM",        HeaderText = "RAM",         Width = 70,  ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Monitor",    HeaderText = "Màn hình",    Width = 90,  ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status",     HeaderText = "Tình trạng",   Width = 90,  ReadOnly = true });

            var colEdit = new DataGridViewButtonColumn
            {
                Name = "Edit", HeaderText = "Sửa", Text = "✏ Sửa",
                UseColumnTextForButtonValue = true, Width = 75, FlatStyle = FlatStyle.Flat
            };
            colEdit.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(colEdit);

            var colDel = new DataGridViewButtonColumn
            {
                Name = "Delete", HeaderText = "Xóa", Text = "🗑",
                UseColumnTextForButtonValue = true, Width = 46, FlatStyle = FlatStyle.Flat
            };
            colDel.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(colDel);

            // Badge màu Status
            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (dgv.Columns[e.ColumnIndex].Name == "Status")
                {
                    string val = e.Value?.ToString() ?? "";
                    if (val.Contains("Tốt"))
                    { e.CellStyle.ForeColor = ThemeColors.BadgeGreenFg; e.CellStyle.BackColor = ThemeColors.BadgeGreenBg; }
                    else if (val.Contains("Bảo"))
                    { e.CellStyle.ForeColor = ThemeColors.BadgeOrangeFg; e.CellStyle.BackColor = ThemeColors.BadgeOrangeBg; }
                    else if (val.Contains("Hỏng"))
                    { e.CellStyle.ForeColor = ThemeColors.BadgeRedFg; e.CellStyle.BackColor = ThemeColors.BadgeRedBg; }
                    e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            };

            dgv.Font = new Font("Segoe UI", 9.5F);
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = ThemeColors.TextSecondary,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(249, 250, 251),
                Padding = new Padding(6),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ThemeColors.TextPrimary,
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary,
                Padding = new Padding(6)
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = ThemeColors.TextPrimary
            };
        }

        // ── Tải dữ liệu ────────────────────────────────────────────────
        private void LoadData()
        {
            dgv.Rows.Clear();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT m.MaMay, m.MaPhong, m.TenMay, p.TenPhong, m.CPU, m.RAM,
                      m.KichThuocManHinh, t.TenTrangThaiMay
                      FROM MAY_TINH m
                      JOIN PHONG_MAY p   ON m.MaPhong  = p.MaPhong
                      JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                      ORDER BY p.TenPhong, m.TenMay");
                foreach (DataRow r in dt.Rows)
                {
                    string status = r["TenTrangThaiMay"].ToString();
                    int idx = dgv.Rows.Add(
                        r["MaMay"], r["MaPhong"],
                        r["TenMay"], r["TenPhong"], r["CPU"],
                        r["RAM"] + " GB", r["KichThuocManHinh"] + "\"",
                        status);
                }
            }
            catch
            {
                // Dữ liệu mẫu
                dgv.Rows.Add(0, 0, "PC-A301-01", "Lab A-301", "Intel i7-12700", "16 GB", "24\"", "Tốt");
                dgv.Rows.Add(0, 0, "PC-A301-02", "Lab A-301", "Intel i7-12700", "16 GB", "24\"", "Bảo trì");
            }

            // Reload cboRoom nếu cần
            RefreshRoomFilter();
        }

        private void RefreshRoomFilter()
        {
            string selected = cboRoom.SelectedItem?.ToString() ?? "Tất cả phòng";
            cboRoom.SelectedIndexChanged -= (s, e) => FilterRows(); // tạm tách để tránh double-call
            cboRoom.Items.Clear();
            cboRoom.Items.Add("Tất cả phòng");
            try
            {
                var dtP = DatabaseHelper.ExecuteQuery("SELECT TenPhong FROM PHONG_MAY ORDER BY TenPhong");
                foreach (DataRow r in dtP.Rows) cboRoom.Items.Add(r["TenPhong"].ToString());
            }
            catch { }
            int idx = cboRoom.Items.IndexOf(selected);
            cboRoom.SelectedIndex = idx >= 0 ? idx : 0;
            cboRoom.SelectedIndexChanged += (s, e) => FilterRows();
        }

        // ── Lọc bảng ────────────────────────────────────────────────────
        private void FilterRows()
        {
            string kw      = txtSearch.Text?.Trim().ToLower() ?? "";
            string roomF   = cboRoom.SelectedItem?.ToString()   ?? "";
            string cpuF    = cboCPU.SelectedItem?.ToString()    ?? "";
            string ramF    = cboRAM.SelectedItem?.ToString()    ?? "";
            string statusF = cboStatus.SelectedItem?.ToString() ?? "";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                bool show = true;

                if (!string.IsNullOrEmpty(kw))
                {
                    bool m = false;
                    foreach (DataGridViewCell c in row.Cells)
                        if (c.Value?.ToString().ToLower().Contains(kw) == true) { m = true; break; }
                    if (!m) show = false;
                }
                if (show && roomF != "Tất cả phòng" && !string.IsNullOrEmpty(roomF))
                    if (row.Cells["Room"].Value?.ToString() != roomF) show = false;

                if (show && cpuF != "Tất cả CPU" && !string.IsNullOrEmpty(cpuF))
                    if (row.Cells["CPU"].Value?.ToString().ToLower().Contains(cpuF.ToLower()) != true) show = false;

                if (show && ramF != "Tất cả RAM" && !string.IsNullOrEmpty(ramF))
                    if (row.Cells["RAM"].Value?.ToString().StartsWith(ramF.Replace(" GB", "")) != true) show = false;

                if (show && statusF != "Tất cả trạng thái" && !string.IsNullOrEmpty(statusF))
                    if (row.Cells["Status"].Value?.ToString() != statusF) show = false;

                row.Visible = show;
            }
        }

        // ── Click Sửa/Xóa trong bảng ───────────────────────────────────
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgv.Columns[e.ColumnIndex].Name;
            if      (col == "Edit")   ShowEditDialog(e.RowIndex);
            else if (col == "Delete") DeleteComputer(e.RowIndex);
        }

        // ── Dialog Thêm máy ────────────────────────────────────────────
        private void ShowAddDialog()
        {
            using var dlg = BuildComputerDialog("Thêm Máy Tính Mới", "", "", "", 8, 24, 0, "Tốt");
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                string tenMay  = Find<TextBox>(dlg, "txtTenMay").Text.Trim();
                string cpu     = Find<TextBox>(dlg, "txtCPU").Text.Trim();
                int    ram     = (int)Find<NumericUpDown>(dlg, "numRAM").Value;
                int    monitor = (int)Find<NumericUpDown>(dlg, "numMonitor").Value;
                int    maPhong = (int)Find<ComboBox>(dlg, "cboPhong").SelectedValue;
                string ttMay   = Find<ComboBox>(dlg, "cboTT").SelectedItem?.ToString() ?? "Tốt";

                if (string.IsNullOrEmpty(tenMay) || string.IsNullOrEmpty(cpu))
                { MessageBox.Show("Vui lòng nhập Tên máy và CPU!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                var ttId = DatabaseHelper.ExecuteScalar(
                    "SELECT MaTTMay FROM TRANG_THAI_MAY WHERE TenTrangThaiMay=@t",
                    new SqlParameter("@t", ttMay));

                DatabaseHelper.ExecuteNonQuery(
                    @"INSERT INTO MAY_TINH (TenMay, CPU, RAM, KichThuocManHinh, MaPhong, MaTTMay)
                      VALUES (@ten, @cpu, @ram, @mon, @phong, @tt)",
                    new SqlParameter("@ten",   tenMay),
                    new SqlParameter("@cpu",   cpu),
                    new SqlParameter("@ram",   ram),
                    new SqlParameter("@mon",   monitor),
                    new SqlParameter("@phong", maPhong),
                    new SqlParameter("@tt",    ttId));

                MessageBox.Show("Đã thêm máy tính!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Dialog Sửa máy ─────────────────────────────────────────────
        private void ShowEditDialog(int rowIndex)
        {
            var row   = dgv.Rows[rowIndex];
            int maMay = Convert.ToInt32(row.Cells["MaMay"].Value);
            if (maMay == 0) { MessageBox.Show("Không thể sửa dữ liệu mẫu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    "SELECT * FROM MAY_TINH m JOIN TRANG_THAI_MAY t ON m.MaTTMay=t.MaTTMay WHERE m.MaMay=@id",
                    new SqlParameter("@id", maMay));
                if (dt.Rows.Count == 0) return;
                var r = dt.Rows[0];

                using var dlg = BuildComputerDialog("Sửa Máy: " + r["TenMay"],
                    r["TenMay"].ToString(), r["CPU"].ToString(),
                    r["TenTrangThaiMay"].ToString(),
                    Convert.ToInt32(r["RAM"]),
                    r["KichThuocManHinh"] == DBNull.Value ? 24 : Convert.ToInt32(r["KichThuocManHinh"]),
                    Convert.ToInt32(r["MaPhong"]),
                    r["TenTrangThaiMay"].ToString());

                if (dlg.ShowDialog() != DialogResult.OK) return;

                string tenMay  = Find<TextBox>(dlg, "txtTenMay").Text.Trim();
                string cpu     = Find<TextBox>(dlg, "txtCPU").Text.Trim();
                int    ram     = (int)Find<NumericUpDown>(dlg, "numRAM").Value;
                int    monitor = (int)Find<NumericUpDown>(dlg, "numMonitor").Value;
                int    maPhong = (int)Find<ComboBox>(dlg, "cboPhong").SelectedValue;
                string ttMay   = Find<ComboBox>(dlg, "cboTT").SelectedItem?.ToString() ?? "Tốt";

                var ttId = DatabaseHelper.ExecuteScalar(
                    "SELECT MaTTMay FROM TRANG_THAI_MAY WHERE TenTrangThaiMay=@t",
                    new SqlParameter("@t", ttMay));

                DatabaseHelper.ExecuteNonQuery(
                    @"UPDATE MAY_TINH SET TenMay=@ten, CPU=@cpu, RAM=@ram,
                      KichThuocManHinh=@mon, MaPhong=@phong, MaTTMay=@tt
                      WHERE MaMay=@id",
                    new SqlParameter("@ten",   tenMay),
                    new SqlParameter("@cpu",   cpu),
                    new SqlParameter("@ram",   ram),
                    new SqlParameter("@mon",   monitor),
                    new SqlParameter("@phong", maPhong),
                    new SqlParameter("@tt",    ttId),
                    new SqlParameter("@id",    maMay));

                MessageBox.Show("Đã cập nhật máy tính!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Xóa máy ───────────────────────────────────────────────────
        private void DeleteComputer(int rowIndex)
        {
            var row   = dgv.Rows[rowIndex];
            int maMay = Convert.ToInt32(row.Cells["MaMay"].Value);
            string ten = row.Cells["ComputerID"].Value?.ToString() ?? "";
            if (maMay == 0) { MessageBox.Show("Không thể xóa dữ liệu mẫu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (MessageBox.Show($"Xóa máy '{ten}'?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM MAY_TINH WHERE MaMay=@id",
                    new SqlParameter("@id", maMay));
                MessageBox.Show("Đã xóa máy!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Xây dựng dialog thêm/sửa máy ─────────────────────────────
        private Form BuildComputerDialog(string title, string tenMay, string cpu,
            string tt, int ram, int monitor, int maPhong, string status)
        {
            var dlg = new Form
            {
                Text = title, Size = new Size(420, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White, Font = new Font("Segoe UI", 10F)
            };

            int y = 20;
            void AddRow(string label, Control ctrl)
            {
                ctrl.Location = new Point(140, y);
                ctrl.Size     = new Size(240, 26);
                dlg.Controls.Add(new Label { Text = label, Location = new Point(20, y + 3), AutoSize = true });
                dlg.Controls.Add(ctrl);
                y += 40;
            }

            var txtTen = new TextBox { Name = "txtTenMay", Text = tenMay };
            AddRow("Tên máy *:", txtTen);

            var txtCpu = new TextBox { Name = "txtCPU", Text = cpu };
            AddRow("CPU *:", txtCpu);

            var numRam = new NumericUpDown { Name = "numRAM", Value = ram, Minimum = 1, Maximum = 256 };
            AddRow("RAM (GB):", numRam);

            var numMon = new NumericUpDown { Name = "numMonitor", Value = monitor, Minimum = 10, Maximum = 50 };
            AddRow("Màn hình (inch):", numMon);

            // Phòng
            var cboPh = new ComboBox { Name = "cboPhong", DropDownStyle = ComboBoxStyle.DropDownList };
            try
            {
                var dtP = DatabaseHelper.ExecuteQuery("SELECT MaPhong, TenPhong FROM PHONG_MAY ORDER BY TenPhong");
                cboPh.DisplayMember = "TenPhong"; cboPh.ValueMember = "MaPhong";
                cboPh.DataSource = dtP;
                // Chọn phòng hiện tại
                if (maPhong > 0)
                    for (int i = 0; i < cboPh.Items.Count; i++)
                        if (Convert.ToInt32(((DataRowView)cboPh.Items[i]).Row["MaPhong"]) == maPhong)
                        { cboPh.SelectedIndex = i; break; }
            }
            catch { cboPh.Items.Add("-- Không tải được --"); cboPh.SelectedIndex = 0; }
            AddRow("Phòng máy:", cboPh);

            // Trạng thái
            var cboTT = new ComboBox { Name = "cboTT", DropDownStyle = ComboBoxStyle.DropDownList };
            cboTT.Items.AddRange(new object[] { "Tốt", "Bảo trì", "Hỏng" });
            cboTT.SelectedItem = status;
            if (cboTT.SelectedIndex < 0) cboTT.SelectedIndex = 0;
            AddRow("Tình trạng:", cboTT);

            y += 5;
            var btnSave = new Button
            {
                Text = "💾  Lưu", Location = new Point(140, y), Size = new Size(120, 38),
                BackColor = ThemeColors.PrimaryBlue, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand, DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnSave);

            var btnCan = new Button
            {
                Text = "Hủy", Location = new Point(270, y), Size = new Size(100, 38),
                BackColor = Color.FromArgb(241, 245, 249), ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, DialogResult = DialogResult.Cancel
            };
            btnCan.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCan);

            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCan;
            return dlg;
        }

        private T Find<T>(Form f, string name) where T : Control
        {
            foreach (Control c in f.Controls)
                if (c is T t && c.Name == name) return t;
            return null;
        }
    }
}
