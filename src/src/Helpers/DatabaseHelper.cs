using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace src.Helpers
{

    public static class DatabaseHelper
    {
        // Connection string for SQL Server
        public static readonly string ConnectionString =
            @"Data Source=DESKTOP-BIIH3IM\MSSQLSERVER02;Initial Catalog=QuanLyPhongMay;Integrated Security=True;Trust Server Certificate=True";


        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Execute a non-query SQL command (INSERT, UPDATE, DELETE).
        /// </summary>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Execute a scalar SQL command (returns first column of first row).
        /// </summary>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Execute a query and return a DataTable.
        /// </summary>
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Hash password using BCrypt with automatic salt generation.
        /// </summary>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verify a password against a stored BCrypt hash.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
            catch
            {
                // In case the stored hash is old SHA256 and not BCrypt, it will throw an exception
                return false;
            }
        }

        /// <summary>
        /// Seed initial data: roles and admin user if they don't exist.
        /// Call this at application startup.
        /// </summary>
        public static void SeedInitialData()
        {
            try
            {
                // Check if roles exist, insert if not
                var roleCount = ExecuteScalar("SELECT COUNT(*) FROM VAI_TRO");
                if (Convert.ToInt32(roleCount) == 0)
                {
                    ExecuteNonQuery("INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES (N'Admin', N'Quản trị viên hệ thống')");
                    ExecuteNonQuery("INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES (N'NhanVien', N'Nhân viên phòng máy')");
                }

                // Check if admin user exists, create or update if needed
                var dtAdmin = ExecuteQuery("SELECT MatKhauDaMaHoa FROM NGUOI_DUNG WHERE TenDangNhap = 'admin'");
                if (dtAdmin.Rows.Count == 0)
                {
                    // Get Admin role ID
                    var adminRoleId = ExecuteScalar("SELECT MaVaiTro FROM VAI_TRO WHERE TenVaiTro = N'Admin'");
                    string hashedPassword = HashPassword("admin123");

                    ExecuteNonQuery(
                        @"INSERT INTO NGUOI_DUNG (TenDangNhap, MatKhauDaMaHoa, HoTen, Email, SoDienThoai, TrangThai, MaVaiTro)
                          VALUES (@user, @pass, @name, @email, @phone, 1, @role)",
                        new SqlParameter("@user", "admin"),
                        new SqlParameter("@pass", hashedPassword),
                        new SqlParameter("@name", "Administrator"),
                        new SqlParameter("@email", "admin@lab.edu.vn"),
                        new SqlParameter("@phone", "0901234567"),
                        new SqlParameter("@role", adminRoleId));
                }
                else
                {
                    // Force update password to BCrypt if it's still using the old SHA256 format
                    string currentHash = dtAdmin.Rows[0]["MatKhauDaMaHoa"].ToString();
                    if (!currentHash.StartsWith("$2a$") && !currentHash.StartsWith("$2b$") && !currentHash.StartsWith("$2y$"))
                    {
                        string newBcryptHash = HashPassword("admin123");
                        ExecuteNonQuery("UPDATE NGUOI_DUNG SET MatKhauDaMaHoa = @pass WHERE TenDangNhap = 'admin'",
                            new SqlParameter("@pass", newBcryptHash));
                    }
                }

                // Seed room statuses
                var ttPhongCount = ExecuteScalar("SELECT COUNT(*) FROM TRANG_THAI_PHONG");
                if (Convert.ToInt32(ttPhongCount) == 0)
                {
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_PHONG (TenTrangThaiPhong) VALUES (N'Hoạt động')");
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_PHONG (TenTrangThaiPhong) VALUES (N'Đóng cửa')");
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_PHONG (TenTrangThaiPhong) VALUES (N'Cần bảo trì')");
                }
                else
                {
                    var existMaintenance = ExecuteScalar("SELECT COUNT(*) FROM TRANG_THAI_PHONG WHERE TenTrangThaiPhong = N'Cần bảo trì'");
                    if (Convert.ToInt32(existMaintenance) == 0)
                    {
                        ExecuteNonQuery("INSERT INTO TRANG_THAI_PHONG (TenTrangThaiPhong) VALUES (N'Cần bảo trì')");
                    }
                }

                // Seed computer statuses
                var ttMayCount = ExecuteScalar("SELECT COUNT(*) FROM TRANG_THAI_MAY");
                if (Convert.ToInt32(ttMayCount) == 0)
                {
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_MAY (TenTrangThaiMay) VALUES (N'Tốt')");
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_MAY (TenTrangThaiMay) VALUES (N'Hỏng')");
                }

                // Add CreatedAt and UpdatedAt to PHONG_MAY if not exists
                ExecuteNonQuery(@"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'PHONG_MAY') AND name = 'CreatedAt')
                    BEGIN
                        ALTER TABLE PHONG_MAY ADD CreatedAt DATETIME DEFAULT GETDATE();
                        ALTER TABLE PHONG_MAY ADD UpdatedAt DATETIME DEFAULT GETDATE();
                        EXEC('UPDATE PHONG_MAY SET CreatedAt = GETDATE(), UpdatedAt = GETDATE() WHERE CreatedAt IS NULL');
                    END
                ");

                // Add CreatedAt and UpdatedAt to MAY_TINH if not exists
                ExecuteNonQuery(@"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MAY_TINH') AND name = 'CreatedAt')
                    BEGIN
                        ALTER TABLE MAY_TINH ADD CreatedAt DATETIME DEFAULT GETDATE();
                        ALTER TABLE MAY_TINH ADD UpdatedAt DATETIME DEFAULT GETDATE();
                        EXEC('UPDATE MAY_TINH SET CreatedAt = GETDATE(), UpdatedAt = GETDATE() WHERE CreatedAt IS NULL');
                    END
                ");

                // Add CreatedAt and UpdatedAt to NGUOI_DUNG if not exists
                ExecuteNonQuery(@"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'NGUOI_DUNG') AND name = 'CreatedAt')
                    BEGIN
                        ALTER TABLE NGUOI_DUNG ADD CreatedAt DATETIME DEFAULT GETDATE();
                        ALTER TABLE NGUOI_DUNG ADD UpdatedAt DATETIME DEFAULT GETDATE();
                        EXEC('UPDATE NGUOI_DUNG SET CreatedAt = GETDATE(), UpdatedAt = GETDATE() WHERE CreatedAt IS NULL');
                    END
                ");

                // Thêm trường MaMon vào bảng LOP_HOC để biến thành Lớp Học Phần
                ExecuteNonQuery(@"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'LOP_HOC') AND name = 'MaMon')
                    BEGIN
                        ALTER TABLE LOP_HOC ADD MaMon INT NULL;
                        ALTER TABLE LOP_HOC ADD CONSTRAINT FK_LOP_HOC_MON_HOC FOREIGN KEY (MaMon) REFERENCES MON_HOC(MaMon);
                    END
                ");

                // Create CHOT_SO_LIEU table if it doesn't exist
                ExecuteNonQuery(@"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'CHOT_SO_LIEU') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE CHOT_SO_LIEU (
                            NgayChot DATE PRIMARY KEY,
                            TotalRooms INT,
                            ActiveRooms INT,
                            TotalMay INT,
                            MayTot INT,
                            MayHong INT,
                            TotalUsers INT
                        )
                    END
                ");

                // Snapshot today's data and backfill historical data
                ExecuteNonQuery(@"
                    DECLARE @Today DATE = CAST(GETDATE() AS DATE);
                    
                    -- Delete today's snapshot if exists to recalculate
                    DELETE FROM CHOT_SO_LIEU WHERE NgayChot = @Today;
                    
                    INSERT INTO CHOT_SO_LIEU (NgayChot, TotalRooms, ActiveRooms, TotalMay, MayTot, MayHong, TotalUsers)
                    SELECT 
                        @Today,
                        (SELECT COUNT(*) FROM PHONG_MAY),
                        (SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong=N'Hoạt động'),
                        (SELECT COUNT(*) FROM MAY_TINH),
                        (SELECT COUNT(*) FROM MAY_TINH m JOIN TRANG_THAI_MAY t ON m.MaTTMay=t.MaTTMay WHERE t.TenTrangThaiMay=N'Tốt'),
                        (SELECT COUNT(*) FROM MAY_TINH m JOIN TRANG_THAI_MAY t ON m.MaTTMay=t.MaTTMay WHERE t.TenTrangThaiMay=N'Hỏng'),
                        (SELECT COUNT(*) FROM NGUOI_DUNG);
                        
                    -- Backfill missing historical dates
                    INSERT INTO CHOT_SO_LIEU (NgayChot, TotalRooms, ActiveRooms, TotalMay, MayTot, MayHong, TotalUsers)
                    SELECT DISTINCT d, 0, 0, 0, 0, 0, 0
                    FROM (
                        SELECT CAST(CreatedAt AS DATE) as d FROM NGUOI_DUNG
                        UNION SELECT CAST(CreatedAt AS DATE) FROM PHONG_MAY
                        UNION SELECT CAST(CreatedAt AS DATE) FROM MAY_TINH
                    ) dates
                    WHERE d < @Today AND d NOT IN (SELECT NgayChot FROM CHOT_SO_LIEU);
                    
                    -- Update running totals for the backfilled historical dates (only newly inserted ones which have 0s)
                    UPDATE C
                    SET 
                        TotalRooms = (SELECT COUNT(*) FROM PHONG_MAY WHERE CAST(CreatedAt AS DATE) <= C.NgayChot),
                        ActiveRooms = (SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong=N'Hoạt động' AND CAST(p.CreatedAt AS DATE) <= C.NgayChot),
                        TotalMay = (SELECT COUNT(*) FROM MAY_TINH WHERE CAST(CreatedAt AS DATE) <= C.NgayChot),
                        MayTot = (SELECT COUNT(*) FROM MAY_TINH m JOIN TRANG_THAI_MAY t ON m.MaTTMay=t.MaTTMay WHERE t.TenTrangThaiMay=N'Tốt' AND CAST(m.CreatedAt AS DATE) <= C.NgayChot),
                        MayHong = (SELECT COUNT(*) FROM MAY_TINH m JOIN TRANG_THAI_MAY t ON m.MaTTMay=t.MaTTMay WHERE t.TenTrangThaiMay=N'Hỏng' AND CAST(m.CreatedAt AS DATE) <= C.NgayChot),
                        TotalUsers = (SELECT COUNT(*) FROM NGUOI_DUNG WHERE CAST(CreatedAt AS DATE) <= C.NgayChot)
                    FROM CHOT_SO_LIEU C
                    WHERE NgayChot < @Today AND TotalUsers = 0 AND TotalRooms = 0;
                ");

                // Fix: Backdate initial seed data to 2026-05-01 so historical reports don't show 0
                ExecuteNonQuery(@"
                    UPDATE PHONG_MAY SET CreatedAt = '2026-05-01' WHERE CAST(CreatedAt AS DATE) = '2024-01-01';
                    UPDATE MAY_TINH SET CreatedAt = '2026-05-01' WHERE CAST(CreatedAt AS DATE) = '2024-01-01';
                    UPDATE NGUOI_DUNG SET CreatedAt = '2026-05-01' WHERE CAST(CreatedAt AS DATE) = '2024-01-01';
                    
                    -- Always keep Admin created in 2020 so there is always at least 1 user in the past
                    UPDATE NGUOI_DUNG SET CreatedAt = '2020-01-01' WHERE TenDangNhap = 'admin';
                    
                    -- Remove accidentally stored unencrypted passwords
                    UPDATE NGUOI_DUNG SET SoDienThoai = '' WHERE SoDienThoai NOT LIKE '0%';
                ");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Lỗi khởi tạo dữ liệu: " + ex.Message,
                    "Database Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Test the database connection.
        /// </summary>
        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = null;
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
