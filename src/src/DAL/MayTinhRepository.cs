using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using src.DTO;
using src.Helpers;


namespace src.DAL
{
    public interface IMayTinhRepository
    {
        List<MayTinhDTO> GetAllComputers();
        bool AddComputer(MayTinhDTO computer);
        bool UpdateComputer(MayTinhDTO computer);
        bool DeleteComputer(int maMay);
        bool IsRoomInUseNow(int roomId);
        List<string> GetRoomNames();
        DataTable GetRoomListForComboBox();
        MayTinhDTO GetComputerById(int maMay);
        int? GetRoomIdByName(string roomName);
        bool IsComputerNameExists(string name, int? excludeId = null);
    }

    public class MayTinhRepository : IMayTinhRepository
    {
        public List<MayTinhDTO> GetAllComputers()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT m.MaMay, m.MaPhong, m.TenMay, p.TenPhong, m.CPU, m.RAM,
                                      m.KichThuocManHinh, t.TenTrangThaiMay, m.DungLuongLuuTru
                               FROM MAY_TINH m
                               JOIN PHONG_MAY p   ON m.MaPhong  = p.MaPhong
                               JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                               ORDER BY p.TenPhong, m.TenMay";
                return db.Query<MayTinhDTO>(sql).ToList();
            }
        }

        private int GetStatusId(string statusName, IDbConnection db)
        {
            return db.QueryFirstOrDefault<int>("SELECT MaTTMay FROM TRANG_THAI_MAY WHERE TenTrangThaiMay=@t", new { t = statusName });
        }

        public bool AddComputer(MayTinhDTO computer)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                int ttId = GetStatusId(computer.TenTrangThaiMay, db);
                string sql = @"INSERT INTO MAY_TINH (TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, MaPhong, MaTTMay)
                               VALUES (@TenMay, @CPU, @RAM, @DungLuongLuuTru, @KichThuocManHinh, @MaPhong, @MaTTMay)";
                int rows = db.Execute(sql, new { 
                    computer.TenMay, computer.CPU, computer.RAM, 
                    computer.DungLuongLuuTru, computer.KichThuocManHinh, 
                    computer.MaPhong, MaTTMay = ttId 
                });
                return rows > 0;
            }
        }

        public bool UpdateComputer(MayTinhDTO computer)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                int ttId = GetStatusId(computer.TenTrangThaiMay, db);

                // Truyền MaNguoiDung vào CONTEXT_INFO để trigger trg_LogTrangThaiMayTinh đọc được
                int maNguoiDung = computer.MaNguoiDung > 0 ? computer.MaNguoiDung : AppSession.MaNguoiDung;
                // Dùng BINARY(4) làm buffer trung gian: INT -> BINARY(4) -> VARBINARY(128)
                db.Execute("DECLARE @ctx VARBINARY(128) = CONVERT(VARBINARY(128), CONVERT(BINARY(4), @uid)); SET CONTEXT_INFO @ctx",
                    new { uid = maNguoiDung });

                string sql = @"UPDATE MAY_TINH SET TenMay=@TenMay, CPU=@CPU, RAM=@RAM, DungLuongLuuTru=@DungLuongLuuTru,
                               KichThuocManHinh=@KichThuocManHinh, MaPhong=@MaPhong, MaTTMay=@MaTTMay, UpdatedAt=GETDATE()
                               WHERE MaMay=@MaMay";
                int rows = db.Execute(sql, new { 
                    computer.TenMay, computer.CPU, computer.RAM, 
                    computer.DungLuongLuuTru, computer.KichThuocManHinh, 
                    computer.MaPhong, MaTTMay = ttId, computer.MaMay 
                });
                return rows > 0;
            }
        }

        public bool DeleteComputer(int maMay)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    DELETE FROM CAP_NHAT_MAY WHERE MaMay=@id;
                    DELETE FROM MAY_TINH WHERE MaMay=@id;";
                int rows = db.Execute(sql, new { id = maMay });
                return rows > 0;
            }
        }

        public bool IsRoomInUseNow(int roomId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT COUNT(*) FROM PHAN_CONG_PHONG pc
                               JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                               JOIN CA_HOC c ON l.MaCa = c.MaCa
                               WHERE pc.MaPhong = @roomId
                                 AND l.NgayThucHanh = CAST(GETDATE() AS DATE)
                                 AND l.TrangThaiLich != N'Đã hủy'
                                 AND CAST(GETDATE() AS TIME) BETWEEN c.GioBatDau AND c.GioKetThuc";
                return db.ExecuteScalar<int>(sql, new { roomId }) > 0;
            }
        }

        /// <summary>
        /// Lấy danh sách tên phòng (cho filter combobox).
        /// </summary>
        public List<string> GetRoomNames()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<string>("SELECT TenPhong FROM PHONG_MAY ORDER BY TenPhong").ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách phòng (MaPhong, TenPhong) dạng DataTable cho ComboBox DataSource.
        /// </summary>
        public DataTable GetRoomListForComboBox()
        {
            using (var conn = new Microsoft.Data.SqlClient.SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                using (var cmd = new Microsoft.Data.SqlClient.SqlCommand("SELECT MaPhong, TenPhong FROM PHONG_MAY ORDER BY TenPhong", conn))
                using (var adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Lấy thông tin 1 máy tính theo MaMay (kèm tên trạng thái).
        /// </summary>
        public MayTinhDTO GetComputerById(int maMay)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT m.MaMay, m.TenMay, m.CPU, m.RAM, m.DungLuongLuuTru, m.KichThuocManHinh,
                                      m.MaPhong, p.TenPhong, m.MaTTMay, t.TenTrangThaiMay
                               FROM MAY_TINH m
                               JOIN PHONG_MAY p ON m.MaPhong = p.MaPhong
                               JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                               WHERE m.MaMay=@maMay";
                return db.QueryFirstOrDefault<MayTinhDTO>(sql, new { maMay });
            }
        }

        /// <summary>
        /// Lấy MaPhong từ TenPhong.
        /// </summary>
        public int? GetRoomIdByName(string roomName)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int?>(
                    "SELECT MaPhong FROM PHONG_MAY WHERE TenPhong=@roomName",
                    new { roomName });
            }
        }

        /// <summary>
        /// Kiểm tra tên máy đã tồn tại chưa (bỏ qua máy có excludeId nếu đang sửa).
        /// </summary>
        public bool IsComputerNameExists(string name, int? excludeId = null)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                if (excludeId.HasValue)
                {
                    return db.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM MAY_TINH WHERE TenMay=@name AND MaMay!=@id",
                        new { name, id = excludeId.Value }) > 0;
                }
                return db.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM MAY_TINH WHERE TenMay=@name",
                    new { name }) > 0;
            }
        }
    }
}

