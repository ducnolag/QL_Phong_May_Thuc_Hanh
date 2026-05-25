using System.Collections.Generic;
using System.Data;
using Dapper;
using src.DTO;
using src.Helpers;

namespace src.DAL
{
    public interface INguoiDungRepository
    {
        TaiKhoanDTO GetUserByUsername(string username);
        IEnumerable<TaiKhoanDTO> GetAllUsers();
        int GetRoleIdByName(string roleName);
        void CreateUser(TaiKhoanDTO user);
        void UpdateUser(TaiKhoanDTO user, bool updatePassword);
        bool CheckUserHasData(int userId);
        void DeleteUserAndRelatedData(int userId);
    }

    public class NguoiDungRepository : INguoiDungRepository
    {
        public TaiKhoanDTO GetUserByUsername(string username)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT nd.MaNguoiDung, nd.HoTen, nd.MatKhauDaMaHoa, nd.TrangThai,
                                      vt.TenVaiTro, nd.MaVaiTro, nd.TenDangNhap, nd.Email, nd.SoDienThoai
                               FROM NGUOI_DUNG nd 
                               JOIN VAI_TRO vt ON nd.MaVaiTro = vt.MaVaiTro
                               WHERE nd.TenDangNhap = @username";
                
                return db.QueryFirstOrDefault<TaiKhoanDTO>(sql, new { username });
            }
        }

        public IEnumerable<TaiKhoanDTO> GetAllUsers()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT nd.MaNguoiDung, nd.TenDangNhap, nd.HoTen, nd.Email,
                                      vt.TenVaiTro, nd.TrangThai, nd.CreatedAt, nd.SoDienThoai
                               FROM NGUOI_DUNG nd
                               JOIN VAI_TRO vt ON nd.MaVaiTro = vt.MaVaiTro
                               ORDER BY nd.MaNguoiDung";
                return db.Query<TaiKhoanDTO>(sql);
            }
        }

        public int GetRoleIdByName(string roleName)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>("SELECT MaVaiTro FROM VAI_TRO WHERE TenVaiTro=@roleName", new { roleName });
            }
        }

        public void CreateUser(TaiKhoanDTO user)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"INSERT INTO NGUOI_DUNG (TenDangNhap, MatKhauDaMaHoa, HoTen, Email, SoDienThoai, TrangThai, MaVaiTro)
                               VALUES (@TenDangNhap, @MatKhauDaMaHoa, @HoTen, @Email, @SoDienThoai, @TrangThai, @MaVaiTro)";
                db.Execute(sql, user);
            }
        }

        public void UpdateUser(TaiKhoanDTO user, bool updatePassword)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE NGUOI_DUNG SET HoTen=@HoTen, Email=@Email, SoDienThoai=@SoDienThoai, TrangThai=@TrangThai";
                if (updatePassword)
                {
                    sql += ", MatKhauDaMaHoa=@MatKhauDaMaHoa";
                }
                sql += " WHERE TenDangNhap=@TenDangNhap";
                db.Execute(sql, user);
            }
        }

        public bool CheckUserHasData(int userId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT COUNT(*) FROM (
                                SELECT 1 AS X FROM LICH_THUC_HANH WHERE NguoiTao = @userId
                                UNION ALL
                                SELECT 1 FROM PHAN_CONG_PHONG WHERE MaNguoiDung = @userId
                                UNION ALL
                                SELECT 1 FROM CAP_NHAT_PHONG WHERE MaNguoiDung = @userId
                                UNION ALL
                                SELECT 1 FROM CAP_NHAT_MAY WHERE MaNguoiDung = @userId
                            ) T";
                return db.ExecuteScalar<int>(sql, new { userId }) > 0;
            }
        }

        public void DeleteUserAndRelatedData(int userId)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            {
                // conn.Open(); - already opened by GetConnection()
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"
                            DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE NguoiTao = @userId);
                            DELETE FROM PHAN_CONG_PHONG WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE NguoiTao = @userId);
                            DELETE FROM PHAN_CONG_PHONG WHERE MaNguoiDung = @userId;
                            DELETE FROM LICH_THUC_HANH WHERE NguoiTao = @userId;
                            DELETE FROM CAP_NHAT_PHONG WHERE MaNguoiDung = @userId;
                            DELETE FROM CAP_NHAT_MAY WHERE MaNguoiDung = @userId;
                            DELETE FROM NGUOI_DUNG WHERE MaNguoiDung = @userId;";
                        
                        conn.Execute(sql, new { userId }, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}

