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
            @"Data Source=LapLag;Initial Catalog=QuanLyPhongMay;Integrated Security=True;Encrypt=False";


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
                }

                // Seed computer statuses
                var ttMayCount = ExecuteScalar("SELECT COUNT(*) FROM TRANG_THAI_MAY");
                if (Convert.ToInt32(ttMayCount) == 0)
                {
                    ExecuteNonQuery("INSERT INTO TRANG_THAI_MAY (TenTrangThaiMay) VALUES (N'Tốt')");
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
