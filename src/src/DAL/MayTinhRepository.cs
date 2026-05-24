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
                string sql = "DELETE FROM MAY_TINH WHERE MaMay=@id";
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
    }
}

