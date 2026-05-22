using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using src.DTO;
using src.Helpers;
using Microsoft.Data.SqlClient;

namespace src.DAL
{
    public interface IScheduleRepository
    {
        (int total, int assigned, int pending, int canceled) GetStatistics();
        IEnumerable<ScheduleDTO> GetActiveSchedules();
        ScheduleDTO GetScheduleById(int id);
        (int RAMToiThieu, int LuuTruToiThieu) GetScheduleRequirements(int id);
        (int MaPhong, string TenPhong, int SucChua) GetAssignedRoom(int scheduleId);
        
        IEnumerable<dynamic> GetRoomsForAssignment(int soSV, int reqRam, int reqStorage, DateTime date, int caId, int currentScheduleId = 0);
        
        int GetLopIdByName(string name);
        int CreateLop(string name);
        int GetMonIdByName(string name);
        int CreateMon(string name);
        
        int CheckDuplicateClassSchedule(int lopId, DateTime date, int caId, int excludeScheduleId = 0);
        int CountAvailableComputers(int roomId, int reqRam, int reqStorage);
        int CheckRoomConflict(int roomId, DateTime date, int caId, int excludeScheduleId = 0);

        int CreateSchedule(ScheduleDTO schedule, int reqRam, int reqStorage, int? roomId);
        void UpdateSchedule(ScheduleDTO schedule, int reqRam, int reqStorage, int? roomId);
        void CancelSchedule(int id);
        void DeleteSchedule(int id);
        
        IEnumerable<CaHocDTO> GetAllCaHoc();
    }

    public class CaHocDTO
    {
        public int MaCa { get; set; }
        public string TenCa { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
    }

    public class ScheduleRepository : IScheduleRepository
    {
        public (int total, int assigned, int pending, int canceled) GetStatistics()
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                int total = db.ExecuteScalar<int>("SELECT COUNT(*) FROM LICH_THUC_HANH");
                int assigned = db.ExecuteScalar<int>("SELECT COUNT(*) FROM LICH_THUC_HANH WHERE TrangThaiLich != N'Đã hủy' AND MaLich IN (SELECT MaLich FROM PHAN_CONG_PHONG)");
                int pending = db.ExecuteScalar<int>("SELECT COUNT(*) FROM LICH_THUC_HANH WHERE TrangThaiLich != N'Đã hủy' AND MaLich NOT IN (SELECT MaLich FROM PHAN_CONG_PHONG)");
                int canceled = db.ExecuteScalar<int>("SELECT COUNT(*) FROM LICH_THUC_HANH WHERE TrangThaiLich = N'Đã hủy'");
                return (total, assigned, pending, canceled);
            }
        }

        public IEnumerable<ScheduleDTO> GetActiveSchedules()
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT l.MaLich, mh.TenMon, l.TrangThaiLich, l.NgayThucHanh, 
                                      c.TenCa, c.GioBatDau, c.GioKetThuc,
                                      l.SoLuongSinhVien, ISNULL(p.TenPhong, '---') AS TenPhong
                               FROM LICH_THUC_HANH l
                               JOIN CA_HOC c ON l.MaCa = c.MaCa
                               JOIN MON_HOC mh ON l.MaMon = mh.MaMon
                               LEFT JOIN PHAN_CONG_PHONG pc ON l.MaLich = pc.MaLich
                               LEFT JOIN PHONG_MAY p ON pc.MaPhong = p.MaPhong
                               WHERE l.TrangThaiLich != N'Đã hủy'
                                 AND l.NgayThucHanh >= CAST(GETDATE() AS DATE)
                               ORDER BY l.NgayThucHanh DESC, c.GioBatDau";
                return db.Query<ScheduleDTO>(sql);
            }
        }

        public ScheduleDTO GetScheduleById(int id)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT l.NgayThucHanh, l.SoLuongSinhVien, l.MaLop, l.MaMon, l.MaCa,
                                      lh.TenLop, mh.TenMon, c.TenCa
                               FROM LICH_THUC_HANH l
                               JOIN LOP_HOC lh ON l.MaLop = lh.MaLop
                               JOIN MON_HOC mh ON l.MaMon = mh.MaMon
                               JOIN CA_HOC c   ON l.MaCa  = c.MaCa
                               WHERE l.MaLich = @id";
                return db.QueryFirstOrDefault<ScheduleDTO>(sql, new { id });
            }
        }

        public (int RAMToiThieu, int LuuTruToiThieu) GetScheduleRequirements(int id)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                var row = db.QueryFirstOrDefault("SELECT RAMToiThieu, LuuTruToiThieu FROM YEU_CAU_CAU_HINH WHERE MaLich=@id", new { id });
                if (row == null) return (0, 0);
                return (row.RAMToiThieu ?? 0, row.LuuTruToiThieu ?? 0);
            }
        }

        public (int MaPhong, string TenPhong, int SucChua) GetAssignedRoom(int scheduleId)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                var row = db.QueryFirstOrDefault(@"SELECT pc.MaPhong, p.TenPhong, p.SucChua FROM PHAN_CONG_PHONG pc
                                                   JOIN PHONG_MAY p ON pc.MaPhong = p.MaPhong
                                                   WHERE pc.MaLich = @scheduleId", new { scheduleId });
                if (row == null) return (0, null, 0);
                return (row.MaPhong, row.TenPhong, row.SucChua);
            }
        }

        public IEnumerable<dynamic> GetRoomsForAssignment(int soSV, int reqRam, int reqStorage, DateTime date, int caId, int currentScheduleId = 0)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT p.MaPhong, p.TenPhong, p.SucChua,
                           (SELECT COUNT(*) FROM MAY_TINH m 
                            JOIN TRANG_THAI_MAY tm ON m.MaTTMay = tm.MaTTMay
                            WHERE m.MaPhong = p.MaPhong AND tm.TenTrangThaiMay = N'Tốt'
                              AND m.RAM >= @reqRam AND m.DungLuongLuuTru >= @reqStorage) AS MayTot
                    FROM PHONG_MAY p
                    JOIN TRANG_THAI_PHONG ttp ON p.MaTTPhong = ttp.MaTTPhong
                    WHERE ttp.TenTrangThaiPhong = N'Hoạt động'
                      AND p.SucChua >= @soSV
                      AND NOT EXISTS (
                          SELECT 1 FROM PHAN_CONG_PHONG pc
                          JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                          WHERE pc.MaPhong = p.MaPhong
                            AND l.NgayThucHanh = @date AND l.MaCa = @caId
                            AND l.TrangThaiLich != N'Đã hủy'
                            AND l.MaLich != @currentScheduleId
                      )";
                return db.Query(sql, new { soSV, reqRam, reqStorage, date = date.Date, caId, currentScheduleId });
            }
        }

        public int GetLopIdByName(string name)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>("SELECT MaLop FROM LOP_HOC WHERE TenLop = @name", new { name });
            }
        }

        public int CreateLop(string name)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>("INSERT INTO LOP_HOC (TenLop, SiSo) OUTPUT INSERTED.MaLop VALUES (@name, 30)", new { name });
            }
        }

        public int GetMonIdByName(string name)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>("SELECT MaMon FROM MON_HOC WHERE TenMon = @name", new { name });
            }
        }

        public int CreateMon(string name)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>("INSERT INTO MON_HOC (TenMon) OUTPUT INSERTED.MaMon VALUES (@name)", new { name });
            }
        }

        public int CheckDuplicateClassSchedule(int lopId, DateTime date, int caId, int excludeScheduleId = 0)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>(@"SELECT COUNT(*) FROM LICH_THUC_HANH l
                                               WHERE l.MaLop = @lopId AND l.NgayThucHanh = @date AND l.MaCa = @caId
                                                 AND l.TrangThaiLich != N'Đã hủy' AND l.MaLich != @excludeScheduleId", 
                                               new { lopId, date = date.Date, caId, excludeScheduleId });
            }
        }

        public int CountAvailableComputers(int roomId, int reqRam, int reqStorage)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>(@"SELECT COUNT(*) FROM MAY_TINH m
                                               JOIN TRANG_THAI_MAY tm ON m.MaTTMay = tm.MaTTMay
                                               WHERE m.MaPhong = @roomId AND tm.TenTrangThaiMay = N'Tốt'
                                                 AND m.RAM >= @reqRam AND m.DungLuongLuuTru >= @reqStorage", 
                                               new { roomId, reqRam, reqStorage });
            }
        }

        public int CheckRoomConflict(int roomId, DateTime date, int caId, int excludeScheduleId = 0)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.ExecuteScalar<int>(@"SELECT COUNT(*) FROM PHAN_CONG_PHONG pc
                                               JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                                               WHERE l.NgayThucHanh = @date AND l.MaCa = @caId AND pc.MaPhong = @roomId
                                                 AND l.TrangThaiLich != N'Đã hủy' AND l.MaLich != @excludeScheduleId", 
                                               new { date = date.Date, caId, roomId, excludeScheduleId });
            }
        }

        public int CreateSchedule(ScheduleDTO schedule, int reqRam, int reqStorage, int? roomId)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            {
                // conn.Open(); - Already opened by GetConnection()
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        int newId = conn.ExecuteScalar<int>(
                            @"INSERT INTO LICH_THUC_HANH (NgayThucHanh, SoLuongSinhVien, MaLop, MaMon, MaCa, NguoiTao)
                              OUTPUT INSERTED.MaLich VALUES (@NgayThucHanh, @SoLuongSinhVien, @MaLop, @MaMon, @MaCa, @NguoiTao)",
                            schedule, trans);

                        conn.Execute("INSERT INTO YEU_CAU_CAU_HINH (MaLich, RAMToiThieu, LuuTruToiThieu) VALUES (@newId, @reqRam, @reqStorage)", 
                            new { newId, reqRam, reqStorage }, trans);

                        if (roomId.HasValue)
                        {
                            conn.Execute("INSERT INTO PHAN_CONG_PHONG (MaLich, MaPhong, MaNguoiDung) VALUES (@newId, @roomId, @NguoiTao)", 
                                new { newId, roomId = roomId.Value, schedule.NguoiTao }, trans);
                        }
                        
                        trans.Commit();
                        return newId;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateSchedule(ScheduleDTO schedule, int reqRam, int reqStorage, int? roomId)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            {
                // conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(@"UPDATE LICH_THUC_HANH SET NgayThucHanh=@NgayThucHanh, SoLuongSinhVien=@SoLuongSinhVien,
                                       MaLop=@MaLop, MaMon=@MaMon, MaCa=@MaCa WHERE MaLich=@MaLich", schedule, trans);

                        int countYc = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM YEU_CAU_CAU_HINH WHERE MaLich=@MaLich", new { schedule.MaLich }, trans);
                        if (countYc > 0)
                        {
                            conn.Execute("UPDATE YEU_CAU_CAU_HINH SET RAMToiThieu=@reqRam, LuuTruToiThieu=@reqStorage WHERE MaLich=@MaLich", 
                                new { reqRam, reqStorage, schedule.MaLich }, trans);
                        }
                        else
                        {
                            conn.Execute("INSERT INTO YEU_CAU_CAU_HINH (MaLich, RAMToiThieu, LuuTruToiThieu) VALUES (@MaLich, @reqRam, @reqStorage)", 
                                new { schedule.MaLich, reqRam, reqStorage }, trans);
                        }

                        conn.Execute("DELETE FROM PHAN_CONG_PHONG WHERE MaLich=@MaLich", new { schedule.MaLich }, trans);
                        if (roomId.HasValue)
                        {
                            conn.Execute("INSERT INTO PHAN_CONG_PHONG (MaLich, MaPhong, MaNguoiDung) VALUES (@MaLich, @roomId, @NguoiTao)", 
                                new { schedule.MaLich, roomId = roomId.Value, schedule.NguoiTao }, trans);
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void CancelSchedule(int id)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE LICH_THUC_HANH SET TrangThaiLich=N'Đã hủy' WHERE MaLich=@id", new { id });
            }
        }

        public void DeleteSchedule(int id)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            {
                // conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute("DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich=@id", new { id }, trans);
                        conn.Execute("DELETE FROM PHAN_CONG_PHONG WHERE MaLich=@id", new { id }, trans);
                        conn.Execute("DELETE FROM LICH_THUC_HANH WHERE MaLich=@id", new { id }, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public IEnumerable<CaHocDTO> GetAllCaHoc()
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                return db.Query<CaHocDTO>("SELECT MaCa, TenCa, GioBatDau, GioKetThuc FROM CA_HOC ORDER BY GioBatDau");
            }
        }
    }
}
