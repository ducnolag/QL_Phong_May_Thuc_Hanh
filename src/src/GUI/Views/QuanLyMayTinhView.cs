using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using src.Helpers;

namespace src.Views
{
    /// <summary>
    /// Quản lý máy tính – Bộ lọc theo phòng/CPU/RAM/Status + CRUD đầy đủ.
    /// </summary>
    public partial class QuanLyMayTinhView : UserControl
    {
        private int currentPage = 1;
        private int pageSize = 15;
        private Guna.UI2.WinForms.Guna2Panel pnlPagination;
        private Button btnPrev;
        private Button btnNext;
        private Label lblPageInfo;

        public QuanLyMayTinhView()
        {
            InitializeComponent();
            SetupPaginationUI();
            SetupView();
        }

        private void SetupPaginationUI()
        {
            pnlPagination = new Guna.UI2.WinForms.Guna2Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            btnPrev = new Button
            {
                Text = "< Trước",
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(245, 247, 250),
                FlatAppearance = { BorderSize = 0 },
                Font = new Font("Segoe UI", 9F)
            };
            btnPrev.Click += (s, e) => { if (currentPage > 1) { currentPage--; ApplyPagination(); } };

            btnNext = new Button
            {
                Text = "Sau >",
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(245, 247, 250),
                FlatAppearance = { BorderSize = 0 },
                Font = new Font("Segoe UI", 9F)
            };
            btnNext.Click += (s, e) => { currentPage++; ApplyPagination(); };

            lblPageInfo = new Label
            {
                AutoSize = true,
                Text = "Trang 1 / 1",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ThemeColors.TextPrimary
            };

            var txtGoToPage = new TextBox
            {
                Size = new Size(40, 30),
                Font = new Font("Segoe UI", 9.5F),
                TextAlign = HorizontalAlignment.Center
            };
            txtGoToPage.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    if (int.TryParse(txtGoToPage.Text, out int page))
                    {
                        if (page < 1) page = 1;
                        currentPage = page;
                        ApplyPagination();
                    }
                }
            };

            var btnGoToPage = new Button
            {
                Text = "Đến",
                Size = new Size(50, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(245, 247, 250),
                FlatAppearance = { BorderSize = 0 },
                Font = new Font("Segoe UI", 9F)
            };
            btnGoToPage.Click += (s, e) => {
                if (int.TryParse(txtGoToPage.Text, out int page))
                {
                    if (page < 1) page = 1;
                    currentPage = page;
                    ApplyPagination();
                }
            };

            pnlPagination.Controls.Add(btnPrev);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(btnNext);
            pnlPagination.Controls.Add(txtGoToPage);
            pnlPagination.Controls.Add(btnGoToPage);

            pnlPagination.Resize += (s, e) =>
            {
                int cx = pnlPagination.Width / 2;
                lblPageInfo.Location = new Point(cx - lblPageInfo.Width / 2, 15);
                btnPrev.Location = new Point(cx - lblPageInfo.Width / 2 - 90, 10);
                btnNext.Location = new Point(cx + lblPageInfo.Width / 2 + 10, 10);
                txtGoToPage.Location = new Point(cx + lblPageInfo.Width / 2 + 100, 12);
                btnGoToPage.Location = new Point(cx + lblPageInfo.Width / 2 + 145, 10);
            };

            this.pnlGrid.Controls.Add(pnlPagination);
            this.dgv.BringToFront();
        }

        private void SetupView()
        {
            // Toolbar và Grid đã được style bằng Guna2Panel trong Designer

            // Load danh sách phòng vào cboRoom
            cboRoom.Items.Clear();
            cboRoom.Items.Add("Tất cả phòng");
            try
            {
                var compService = new src.BLL.MayTinhService();
                var roomNames = compService.GetRoomNames();
                foreach (var rn in roomNames)
                    cboRoom.Items.Add(rn);
            }
            catch { /* giữ mặc định */ }

            cboRoom.SelectedIndex = 0;
            cboCpuFilter.SelectedIndex = 0;
            cboMonitor.SelectedIndex = 0;
            cboStorage.SelectedIndex = 0;
            cboRAM.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;

            // Sự kiện lọc
            txtSearch.TextChanged += (s, e) => FilterRows();
            cboRoom.SelectedIndexChanged += (s, e) => FilterRows();
            cboCpuFilter.SelectedIndexChanged += (s, e) => FilterRows();
            cboMonitor.SelectedIndexChanged += (s, e) => FilterRows();
            cboStorage.SelectedIndexChanged += (s, e) => FilterRows();
            cboRAM.SelectedIndexChanged += (s, e) => FilterRows();
            cboStatus.SelectedIndexChanged += (s, e) => FilterRows();

            // Nút Thêm máy / Sửa / Xóa
            btnAdd.Click += (s, e) => ShowAddDialog();
            btnAdd.Visible = AppSession.IsAdmin;

            // Sửa / Xóa theo hàng được chọn
            dgv.CellClick += Dgv_CellClick;

            SetupGridStyles();
            LoadData();
        }

        // ── Cài cột ─────────────────────────────────────────────────────
        private void SetupGridStyles()
        {
            dgv.Columns.Clear();
            // Cột ẩn chứa PK để Sửa/Xóa
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaMay", HeaderText = "MaMay", Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaPhong", HeaderText = "MaPhong", Visible = false });

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ComputerID", HeaderText = "Mã máy", Width = 110, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Room", HeaderText = "Phòng", Width = 120, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "CPU", HeaderText = "CPU", FillWeight = 100, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RAM", HeaderText = "RAM", Width = 70, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Monitor", HeaderText = "Màn hình", Width = 90, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Tình trạng", Width = 90, ReadOnly = true });

            var colEdit = new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "Sửa",
                Text = "✏ Sửa",
                UseColumnTextForButtonValue = true,
                Width = 75,
                FlatStyle = FlatStyle.Flat
            };
            colEdit.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(colEdit);

            var colDel = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Xóa",
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Width = 46,
                FlatStyle = FlatStyle.Flat
            };
            colDel.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDel.Visible = AppSession.IsAdmin;
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
            dgv.SuspendLayout();
            dgv.Rows.Clear();
            try
            {
                var compService = new src.BLL.MayTinhService();
                var dt = compService.GetAllComputers();
                foreach (var m in dt)
                {
                    int idx = dgv.Rows.Add(
                        m.MaMay, m.MaPhong,
                        m.TenMay, m.TenPhong, m.CPU,
                        m.RAM + " GB", m.KichThuocManHinh + "\"",
                        m.TenTrangThaiMay);
                }
            }
            catch
            {
                // Dữ liệu mẫu
                dgv.Rows.Add(0, 0, "PC-A301-01", "Lab A-301", "Intel i7-12700", "16 GB", "24\"", "Tốt");
                dgv.Rows.Add(0, 0, "PC-A301-02", "Lab A-301", "Intel i7-12700", "16 GB", "24\"", "Hỏng");
            }

            // Reload cboRoom nếu cần
            RefreshRoomFilter();
            dgv.ResumeLayout();
            FilterRows(); // Trigger filter and pagination
        }

        private void RefreshRoomFilter()
        {
            string selected = cboRoom.SelectedItem?.ToString() ?? "Tất cả phòng";
            cboRoom.SelectedIndexChanged -= (s, e) => FilterRows(); // tạm tách để tránh double-call
            cboRoom.Items.Clear();
            cboRoom.Items.Add("Tất cả phòng");
            try
            {
                var compService = new src.BLL.MayTinhService();
                var roomNames = compService.GetRoomNames();
                foreach (var rn in roomNames) cboRoom.Items.Add(rn);
            }
            catch { }
            int idx = cboRoom.Items.IndexOf(selected);
            cboRoom.SelectedIndex = idx >= 0 ? idx : 0;
            cboRoom.SelectedIndexChanged += (s, e) => FilterRows();
        }

        // ── Lọc bảng ────────────────────────────────────────────────────
        private void FilterRows()
        {
            string kw = txtSearch.Text?.Trim().ToLower() ?? "";
            string roomF = cboRoom.SelectedItem?.ToString() ?? "";
            string cpuF = cboCpuFilter.SelectedItem?.ToString() ?? "";
            string monF = cboMonitor.SelectedItem?.ToString() ?? "";
            string storF = cboStorage.SelectedItem?.ToString() ?? "";
            string ramF = cboRAM.SelectedItem?.ToString() ?? "";
            string statusF = cboStatus.SelectedItem?.ToString() ?? "";

            var filteredRows = new System.Collections.Generic.List<DataGridViewRow>();

            // Lấy currency manager để suspend binding / avoid layout issues during bulk hide/show
            dgv.CurrentCell = null;

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
                    if (row.Cells["CPU"].Value?.ToString() != cpuF) show = false;

                if (show && monF != "Tất cả màn hình" && !string.IsNullOrEmpty(monF))
                {
                    string monVal = monF.Replace("\"", "");
                    if (row.Cells["Monitor"].Value?.ToString().Replace("\"", "") != monVal) show = false;
                }

                if (show && ramF != "Tất cả RAM" && !string.IsNullOrEmpty(ramF))
                    if (row.Cells["RAM"].Value?.ToString().StartsWith(ramF.Replace(" GB", "")) != true) show = false;

                if (show && statusF != "Tất cả trạng thái" && !string.IsNullOrEmpty(statusF))
                    if (row.Cells["Status"].Value?.ToString() != statusF) show = false;

                if (show)
                {
                    filteredRows.Add(row);
                }
                else
                {
                    row.Visible = false;
                }
            }

            // Reset về trang 1 mỗi khi lọc
            currentPage = 1;
            ApplyPagination(filteredRows);
        }

        private void ApplyPagination(System.Collections.Generic.List<DataGridViewRow> filteredRows = null)
        {
            if (filteredRows == null)
            {
                filteredRows = new System.Collections.Generic.List<DataGridViewRow>();
                string kw = txtSearch.Text?.Trim().ToLower() ?? "";
                string roomF = cboRoom.SelectedItem?.ToString() ?? "";
                string cpuF = cboCpuFilter.SelectedItem?.ToString() ?? "";
                string monF = cboMonitor.SelectedItem?.ToString() ?? "";
                string ramF = cboRAM.SelectedItem?.ToString() ?? "";
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
                        if (row.Cells["CPU"].Value?.ToString() != cpuF) show = false;
                    if (show && monF != "Tất cả màn hình" && !string.IsNullOrEmpty(monF))
                    {
                        string monVal = monF.Replace("\"", "");
                        if (row.Cells["Monitor"].Value?.ToString().Replace("\"", "") != monVal) show = false;
                    }
                    if (show && ramF != "Tất cả RAM" && !string.IsNullOrEmpty(ramF))
                        if (row.Cells["RAM"].Value?.ToString().StartsWith(ramF.Replace(" GB", "")) != true) show = false;
                    if (show && statusF != "Tất cả trạng thái" && !string.IsNullOrEmpty(statusF))
                        if (row.Cells["Status"].Value?.ToString() != statusF) show = false;

                    if (show) filteredRows.Add(row);
                }
            }

            int totalRecords = filteredRows.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalRecords / pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            lblPageInfo.Text = $"Trang {currentPage} / {totalPages}";
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;

            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = startIndex + pageSize - 1;

            dgv.CurrentCell = null;
            dgv.SuspendLayout();
            for (int i = 0; i < filteredRows.Count; i++)
            {
                filteredRows[i].Visible = (i >= startIndex && i <= endIndex);
            }
            dgv.ResumeLayout();
        }

        // ── Click Sửa/Xóa trong bảng ───────────────────────────────────
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgv.Columns[e.ColumnIndex].Name;
            if (col == "Edit") ShowEditDialog(e.RowIndex);
            else if (col == "Delete") DeleteComputer(e.RowIndex);
        }

        // ── Dialog Thêm máy ────────────────────────────────────────────
        private void ShowAddDialog()
        {
            // Lay phong dang duoc chon tren filter de tu dong dien tien to ma may
            int initialMaPhong = 0;
            string selectedRoom = cboRoom.SelectedItem?.ToString() ?? "";
            if (!string.IsNullOrEmpty(selectedRoom) && selectedRoom != "Tất cả phòng")
            {
                try
                {
                    var compService = new src.BLL.MayTinhService();
                    int? maPhongResult = compService.GetRoomIdByName(selectedRoom);
                    if (maPhongResult.HasValue)
                        initialMaPhong = maPhongResult.Value;
                }
                catch { }
            }

            using var dlg = BuildComputerDialog("Thêm Máy Tính Mới", "", "", "", 8, 256, 24, initialMaPhong, "Tốt");
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                string tenMay = Find<TextBox>(dlg, "txtTenMay").Text.Trim();
                string cpu = Find<ComboBox>(dlg, "cboCPU").SelectedItem?.ToString() ?? "Intel Core i5";
                int ram = Convert.ToInt32(Find<ComboBox>(dlg, "cboInputRAM").SelectedItem.ToString().Replace(" GB", ""));
                int storage = Convert.ToInt32(Find<ComboBox>(dlg, "cboInputStorage").SelectedItem.ToString().Replace(" GB", ""));
                int monitor = Convert.ToInt32(Find<ComboBox>(dlg, "cboInputMonitor").SelectedItem.ToString().Replace("\"", ""));
                string ttMay = Find<ComboBox>(dlg, "cboTT").SelectedItem?.ToString() ?? "Tốt";
                int maPhong = (int)Find<ComboBox>(dlg, "cboPhong").SelectedValue;

                var compService = new src.BLL.MayTinhService();
                var result = compService.AddComputer(new src.DTO.MayTinhDTO
                {
                    TenMay = tenMay,
                    CPU = cpu,
                    RAM = ram,
                    DungLuongLuuTru = storage,
                    KichThuocManHinh = monitor,
                    MaPhong = maPhong,
                    TenTrangThaiMay = ttMay
                });

                if (result.IsSuccess)
                {
                    MessageBox.Show("Đã thêm máy tính!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Dialog Sửa máy ─────────────────────────────────────────────
        private void ShowEditDialog(int rowIndex)
        {
            var row = dgv.Rows[rowIndex];
            int maMay = Convert.ToInt32(row.Cells["MaMay"].Value);
            if (maMay == 0) { MessageBox.Show("Không thể sửa dữ liệu mẫu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                var compService = new src.BLL.MayTinhService();
                var comp = compService.GetComputerById(maMay);
                if (comp == null) return;

                using var dlg = BuildComputerDialog("Sửa Máy: " + comp.TenMay,
                    comp.TenMay, comp.CPU,
                    comp.TenTrangThaiMay,
                    comp.RAM,
                    comp.DungLuongLuuTru == 0 ? 256 : comp.DungLuongLuuTru,
                    comp.KichThuocManHinh == 0 ? 24 : comp.KichThuocManHinh,
                    comp.MaPhong,
                    comp.TenTrangThaiMay,
                    maMay);

                if (dlg.ShowDialog() != DialogResult.OK) return;

                string tenMay = Find<TextBox>(dlg, "txtTenMay").Text.Trim();
                string cpu = Find<ComboBox>(dlg, "cboCPU").SelectedItem?.ToString() ?? "Intel Core i5";
                int ram = Convert.ToInt32(Find<ComboBox>(dlg, "cboInputRAM").SelectedItem.ToString().Replace(" GB", ""));
                int storage = Convert.ToInt32(Find<ComboBox>(dlg, "cboInputStorage").SelectedItem.ToString().Replace(" GB", ""));
                int monitor = Convert.ToInt32(Find<ComboBox>(dlg, "cboInputMonitor").SelectedItem.ToString().Replace("\"", ""));
                int maPhong = (int)Find<ComboBox>(dlg, "cboPhong").SelectedValue;
                string ttMay = Find<ComboBox>(dlg, "cboTT").SelectedItem?.ToString() ?? "Tốt";

                var result = compService.UpdateComputer(new src.DTO.MayTinhDTO
                {
                    MaMay = maMay,
                    TenMay = tenMay,
                    CPU = cpu,
                    RAM = ram,
                    DungLuongLuuTru = storage,
                    KichThuocManHinh = monitor,
                    MaPhong = maPhong,
                    TenTrangThaiMay = ttMay
                });

                if (result.IsSuccess)
                {
                    MessageBox.Show("Đã cập nhật máy tính!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Xóa máy ───────────────────────────────────────────────────
        private void DeleteComputer(int rowIndex)
        {
            var row = dgv.Rows[rowIndex];
            int maMay = Convert.ToInt32(row.Cells["MaMay"].Value);
            string ten = row.Cells["ComputerID"].Value?.ToString() ?? "";
            if (maMay == 0) { MessageBox.Show("Không thể xóa dữ liệu mẫu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (MessageBox.Show($"Xóa máy '{ten}'?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                var compService = new src.BLL.MayTinhService();
                var result = compService.DeleteComputer(maMay);
                if (result.IsSuccess)
                {
                    MessageBox.Show("Đã xóa máy!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Xây dựng dialog thêm/sửa máy ─────────────────────────────
        private Form BuildComputerDialog(string title, string tenMay, string cpu,
            string tt, int ram, int storage, int monitor, int maPhong, string status, int originalMaMay = 0)
        {
            var dlg = new Form
            {
                Text = title,
                Size = new Size(420, 440),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            int y = 20;
            void AddRow(string label, Control ctrl)
            {
                ctrl.Location = new Point(140, y);
                ctrl.Size = new Size(240, 26);
                dlg.Controls.Add(new Label { Text = label, Location = new Point(20, y + 3), AutoSize = true });
                dlg.Controls.Add(ctrl);
                y += 40;
            }

            // Panel chứa prefix và suffix
            var pnlMaMay = new FlowLayoutPanel { Size = new Size(240, 26), Margin = new Padding(0), BorderStyle = BorderStyle.Fixed3D, BackColor = Color.White, WrapContents = false };
            var lblPrefix = new Label { Name = "lblPrefix", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 2, 0, 0), Margin = new Padding(0), ForeColor = SystemColors.ControlText, Font = new Font("Segoe UI", 9.5F) };
            var txtSuffix = new TextBox { Name = "txtSuffix", BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 9.5F), Margin = new Padding(0, 2, 0, 0), Width = 120 };
            pnlMaMay.Controls.Add(lblPrefix);
            pnlMaMay.Controls.Add(txtSuffix);

            var txtTen = new TextBox { Name = "txtTenMay", Visible = false };
            dlg.Controls.Add(txtTen);

            Action updateTxtTenMay = () => { txtTen.Text = lblPrefix.Text + txtSuffix.Text; };
            txtSuffix.TextChanged += (s, e) => updateTxtTenMay();
            lblPrefix.TextChanged += (s, e) => updateTxtTenMay();

            if (!string.IsNullOrEmpty(tenMay))
            {
                int pcIndex = tenMay.LastIndexOf("-PC");
                if (pcIndex >= 0)
                {
                    lblPrefix.Text = tenMay.Substring(0, pcIndex + 3);
                    txtSuffix.Text = tenMay.Substring(pcIndex + 3);
                }
                else
                {
                    int lastDash = tenMay.LastIndexOf('-');
                    if (lastDash >= 0)
                    {
                        lblPrefix.Text = tenMay.Substring(0, lastDash + 1);
                        txtSuffix.Text = tenMay.Substring(lastDash + 1);
                    }
                    else
                    {
                        lblPrefix.Text = "";
                        txtSuffix.Text = tenMay;
                    }
                }
            }
            else
            {
                lblPrefix.Text = "";
                txtSuffix.Text = "";
            }

            AddRow("Mã máy *:", pnlMaMay);

            var cboCpu = new ComboBox { Name = "cboCPU", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false };
            cboCpu.Items.AddRange(new object[] { "Intel Core i3", "Intel Core i5", "Intel Core i7", "Intel Core i9", "AMD Ryzen 3", "AMD Ryzen 5", "AMD Ryzen 7" });
            cboCpu.SelectedItem = cpu;
            if (cboCpu.SelectedIndex < 0) cboCpu.SelectedIndex = 1; // i5 default
            AddRow("CPU *:", cboCpu);

            var cboInputRam = new ComboBox { Name = "cboInputRAM", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false };
            cboInputRam.Items.AddRange(new object[] { "4 GB", "8 GB", "16 GB", "32 GB", "64 GB" });
            cboInputRam.SelectedItem = ram + " GB";
            if (cboInputRam.SelectedIndex < 0) cboInputRam.SelectedIndex = 1; // 8GB default
            AddRow("RAM (GB):", cboInputRam);

            var cboInputStorage = new ComboBox { Name = "cboInputStorage", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false };
            cboInputStorage.Items.AddRange(new object[] { "128 GB", "256 GB", "512 GB", "1024 GB" });
            cboInputStorage.SelectedItem = storage + " GB";
            if (cboInputStorage.SelectedIndex < 0) cboInputStorage.SelectedIndex = 1; // 256GB default
            AddRow("Lưu trữ (GB):", cboInputStorage);

            var cboInputMonitor = new ComboBox { Name = "cboInputMonitor", DropDownStyle = ComboBoxStyle.DropDownList, MaxDropDownItems = 5, IntegralHeight = false };
            cboInputMonitor.Items.AddRange(new object[] { "19\"", "21\"", "24\"", "27\"" });
            cboInputMonitor.SelectedItem = monitor + "\"";
            if (cboInputMonitor.SelectedIndex < 0) cboInputMonitor.SelectedIndex = 2; // 24" default
            AddRow("Màn hình (inch):", cboInputMonitor);

            // Phòng
            var cboPh = new ComboBox { Name = "cboPhong", DropDownStyle = ComboBoxStyle.DropDownList };
            try
            {
                cboPh.SelectedIndexChanged += (s, e) =>
                {
                    if (originalMaMay == 0 && cboPh.SelectedItem is DataRowView drv)
                    {
                        string pName = drv["TenPhong"].ToString();
                        lblPrefix.Text = $"{pName}-PC";
                    }
                };

                var compService = new src.BLL.MayTinhService();
                var dtP = compService.GetRoomListForComboBox();
                cboPh.DisplayMember = "TenPhong"; cboPh.ValueMember = "MaPhong";
                cboPh.DataSource = dtP;

                dlg.Load += (s, e) =>
                {
                    if (maPhong > 0)
                    {
                        cboPh.SelectedValue = maPhong;
                    }

                    if (originalMaMay == 0 && cboPh.SelectedItem is DataRowView drvLoad)
                    {
                        string pName = drvLoad["TenPhong"].ToString();
                        lblPrefix.Text = $"{pName}-PC";
                    }
                    
                    if (originalMaMay == 0) txtSuffix.Focus();
                };
            }
            catch { cboPh.DataSource = null; cboPh.Items.Clear(); cboPh.Items.Add("-- Không tải được --"); cboPh.SelectedIndex = 0; }
            AddRow("Phòng máy:", cboPh);

            // Trạng thái
            var cboTT = new ComboBox { Name = "cboTT", DropDownStyle = ComboBoxStyle.DropDownList };
            cboTT.Items.AddRange(new object[] { "Tốt", "Hỏng" });
            cboTT.SelectedItem = status;
            if (cboTT.SelectedIndex < 0) cboTT.SelectedIndex = 0;
            AddRow("Tình trạng:", cboTT);

            if (!AppSession.IsAdmin)
            {
                txtSuffix.Enabled = false;
                cboCpu.Enabled = false;
                cboInputRam.Enabled = false;
                cboInputStorage.Enabled = false;
                cboInputMonitor.Enabled = false;
                cboPh.Enabled = false;
                // cboTT vẫn được bật để mọi người dùng có thể chuyển máy sang Hỏng
            }

            y += 5;
            var btnSave = new Button
            {
                Text = "💾  Lưu",
                Location = new Point(140, y),
                Size = new Size(120, 38),
                BackColor = ThemeColors.PrimaryBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnSave);

            var btnCan = new Button
            {
                Text = "Hủy",
                Location = new Point(270, y),
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = ThemeColors.TextSecondary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCan.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnCan);

            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCan;

            dlg.FormClosing += (s, e) =>
            {
                if (dlg.DialogResult == DialogResult.OK)
                {
                    // Kiểm tra phần suffix (phần người dùng nhập) không được trống
                    if (string.IsNullOrWhiteSpace(txtSuffix.Text))
                    {
                        MessageBox.Show("Vui lòng nhập mã máy (phần số)!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        txtSuffix.Focus();
                        return;
                    }

                    string mMay = txtTen.Text.Trim();
                    string mCpu = cboCpu.SelectedItem?.ToString() ?? "";

                    if (string.IsNullOrEmpty(mMay) || string.IsNullOrEmpty(mCpu))
                    {
                        MessageBox.Show("Vui lòng nhập Mã máy và CPU!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }

                    var compService = new src.BLL.MayTinhService();
                    bool nameExists;
                    if (originalMaMay == 0)
                    {
                        nameExists = compService.IsComputerNameExists(mMay);
                    }
                    else if (!mMay.Equals(tenMay, StringComparison.OrdinalIgnoreCase))
                    {
                        nameExists = compService.IsComputerNameExists(mMay, originalMaMay);
                    }
                    else
                    {
                        nameExists = false;
                    }

                    if (nameExists)
                    {
                        MessageBox.Show("Mã máy đã tồn tại! Vui lòng nhập lại.", "Trùng mã máy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        txtSuffix.SelectAll();
                        txtSuffix.Focus();
                    }
                }
            };

            return dlg;
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private T Find<T>(Form f, string name) where T : Control
        {
            foreach (Control c in f.Controls)
                if (c is T t && c.Name == name) return t;
            return null;
        }
    }
}

