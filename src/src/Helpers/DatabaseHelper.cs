using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace src.Helpers
{
    /// <summary>
    /// Centralized database connection and query helper.
    /// Uses ADO.NET with SQL Server via the user's connection string.
    /// </summary>
    public static class DatabaseHelper
    {
        // Connection string for SQL Server
        public static readonly string ConnectionString =
            @"Data Source=LapLag;Initial Catalog=QuanLyPhongMay;Integrated Security=True;Encrypt=False";

        /// <summary>
        /// Get a new open SqlConnection.
        /// </summary>
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
        /// Hash password using SHA256 for storage.
        /// </summary>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Verify a password against a stored hash.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            string hash = HashPassword(password);
            return string.Equals(hash, storedHash, StringComparison.OrdinalIgnoreCase);
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

                // Check if admin user exists, create if not
                var adminCount = ExecuteScalar(
                    "SELECT COUNT(*) FROM NGUOI_DUNG WHERE TenDangNhap = 'admin'");
                if (Convert.ToInt32(adminCount) == 0)
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

                // Seed room statuses
                var ttPhongCount = ExecuteScalar("SELECT COUNT(*) FROM TRANG_THAI_PHONG");
                if (Convert.ToInt32(ttPhongCount) == 0)
                {
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_PHONG (TenTrangThaiPhong) VALUES (N'Hoạt động')");
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_PHONG (TenTrangThaiPhong) VALUES (N'Bảo trì')");
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_PHONG (TenTrangThaiPhong) VALUES (N'Đóng cửa')");
                }

                // Seed computer statuses
                var ttMayCount = ExecuteScalar("SELECT COUNT(*) FROM TRANG_THAI_MAY");
                if (Convert.ToInt32(ttMayCount) == 0)
                {
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_MAY (TenTrangThaiMay) VALUES (N'Tốt')");
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_MAY (TenTrangThaiMay) VALUES (N'Bảo trì')");
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_MAY (TenTrangThaiMay) VALUES (N'Hỏng')");
                }
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
