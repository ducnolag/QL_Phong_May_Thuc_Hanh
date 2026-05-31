using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using src.DTO;
using src.Helpers;

namespace src.DAL
{
    public interface IPhongMayRepository
    {
        List<PhongMayDTO> GetAllRooms();
        bool DeleteRoomWithTransaction(int roomId);
        bool IsRoomNameExists(string name, int? excludeId = null);
        int AddRoomWithComputers(PhongMayDTO room, string cpu, int ram, int storage, int monitor);
        bool UpdateRoom(PhongMayDTO room, int userId);
        (int TotalRooms, int Available, int Occupied) GetRoomStats();
        bool HasActiveSchedule(int roomId);
        PhongMayDTO GetRoomWithStatus(int roomId);
    }

    public class PhongMayRepository : IPhongMayRepository
    {
        public List<PhongMayDTO> GetAllRooms()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT p.MaPhong, p.TenPhong, p.ViTri, p.SucChua, p.MaTTPhong,
                           t.TenTrangThaiPhong,
                           (SELECT COUNT(*) FROM MAY_TINH m JOIN TRANG_THAI_MAY ttm ON m.MaTTMay = ttm.MaTTMay WHERE m.MaPhong = p.MaPhong AND (ttm.TenTrangThaiMay = N'Tốt' OR m.MaTTMay = 1)) AS SoMay
                    FROM PHONG_MAY p
                    JOIN TRANG_THAI_PHONG t ON p.MaTTPhong = t.MaTTPhong
                    ORDER BY p.TenPhong";
                
                return db.Query<PhongMayDTO>(sql).ToList();
            }
        }

        /// <summary>
        /// Lấy thông tin phòng kèm tên trạng thái (dùng cho dialog sửa).
        /// </summary>
        public PhongMayDTO GetRoomWithStatus(int roomId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT p.MaPhong, p.TenPhong, p.ViTri, p.SucChua, p.MaTTPhong, t.TenTrangThaiPhong
                               FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong
                               WHERE p.MaPhong=@roomId";
                return db.QueryFirstOrDefault<PhongMayDTO>(sql, new { roomId });
            }
        }

        /// <summary>
        /// Kiểm tra tên phòng đã tồn tại chưa (bỏ qua phòng có excludeId nếu đang sửa).
        /// </summary>
        public bool IsRoomNameExists(string name, int? excludeId = null)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                if (excludeId.HasValue)
                {
                    return db.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM PHONG_MAY WHERE TenPhong=@name AND MaPhong!=@id",
                        new { name, id = excludeId.Value }) > 0;
                }
                return db.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM PHONG_MAY WHERE TenPhong=@name",
                    new { name }) > 0;
            }
        }

        /// <summary>
        /// Thêm phòng mới kèm tự động tạo máy tính (transaction).
        /// Trả về MaPhong vừa tạo.
        /// </summary>
        public int AddRoomWithComputers(PhongMayDTO room, string cpu, int ram, int storage, int monitor)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                if (db.State != ConnectionState.Open) db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        int statusId = db.ExecuteScalar<int>(
                            "SELECT MaTTPhong FROM TRANG_THAI_PHONG WHERE TenTrangThaiPhong=@s",
                            new { s = room.TenTrangThaiPhong }, transaction);

                        int maPhong = db.ExecuteScalar<int>(
                            @"INSERT INTO PHONG_MAY (TenPhong, ViTri, SucChua, MaTTPhong)
                              VALUES (@TenPhong, @ViTri, @SucChua, @statusId);
                              SELECT CAST(SCOPE_IDENTITY() AS INT);",
                            new { room.TenPhong, room.ViTri, room.SucChua, statusId }, transaction);

                        int ttMayId = db.ExecuteScalar<int?>(
                            "SELECT MaTTMay FROM TRANG_THAI_MAY WHERE TenTrangThaiMay=N'Tốt'",
                            transaction: transaction) ?? 1;

                        for (int i = 1; i <= room.SucChua; i++)
                        {
                            string tenMay = $"{room.TenPhong}-PC{i:D2}";
                            db.Execute(
                                @"INSERT INTO MAY_TINH (TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, MaPhong, MaTTMay)
                                  VALUES (@tenMay, @cpu, @ram, @storage, @monitor, @maPhong, @ttMayId)",
                                new { tenMay, cpu, ram, storage, monitor, maPhong, ttMayId }, transaction);
                        }

                        transaction.Commit();
                        return maPhong;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Cập nhật phòng máy (có CONTEXT_INFO cho trigger log).
        /// Đồng thời tự động thêm máy tính (MAY_TINH) nếu sức chứa (SucChua) tăng lên.
        /// </summary>
        public bool UpdateRoom(PhongMayDTO room, int userId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                if (db.State != ConnectionState.Open) db.Open();
                
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        int statusId = db.ExecuteScalar<int>(
                            "SELECT MaTTPhong FROM TRANG_THAI_PHONG WHERE TenTrangThaiPhong=@s",
                            new { s = room.TenTrangThaiPhong }, transaction);

                        db.Execute(
                            "DECLARE @ctx VARBINARY(128) = CONVERT(VARBINARY(128), CONVERT(BINARY(4), @uid)); SET CONTEXT_INFO @ctx",
                            new { uid = userId }, transaction);

                        // Lấy tên phòng cũ để so sánh
                        string oldName = db.ExecuteScalar<string>(
                            "SELECT TenPhong FROM PHONG_MAY WHERE MaPhong=@MaPhong",
                            new { room.MaPhong }, transaction);

                        int rows = db.Execute(
                            @"UPDATE PHONG_MAY SET TenPhong=@TenPhong, ViTri=@ViTri, SucChua=@SucChua, MaTTPhong=@statusId, UpdatedAt=GETDATE()
                              WHERE MaPhong=@MaPhong",
                            new { room.TenPhong, room.ViTri, room.SucChua, statusId, room.MaPhong }, transaction);

                        // Nếu tên phòng thay đổi → cập nhật tiền tố mã máy tương ứng
                        if (!string.IsNullOrEmpty(oldName) && oldName != room.TenPhong)
                        {
                            db.Execute(
                                @"UPDATE MAY_TINH 
                                  SET TenMay = REPLACE(TenMay, @oldPrefix, @newPrefix), UpdatedAt=GETDATE()
                                  WHERE MaPhong=@MaPhong AND TenMay LIKE @oldPrefix + '%'",
                                new { oldPrefix = oldName, newPrefix = room.TenPhong, room.MaPhong }, transaction);
                        }

                        // Tự động thêm máy tính nếu sức chứa mới lớn hơn số lượng máy tính thực tế hiện có trong bảng MAY_TINH
                        int currentComputerCount = db.ExecuteScalar<int>(
                            "SELECT COUNT(*) FROM MAY_TINH WHERE MaPhong=@MaPhong",
                            new { room.MaPhong }, transaction);

                        if (currentComputerCount < room.SucChua)
                        {
                            int diff = room.SucChua - currentComputerCount;
                            
                            // Lấy cấu hình của máy đầu tiên trong phòng làm mẫu
                            var template = db.QueryFirstOrDefault(
                                "SELECT TOP 1 CPU, RAM, DungLuongLuuTru, KichThuocManHinh, MaTTMay FROM MAY_TINH WHERE MaPhong=@MaPhong",
                                new { room.MaPhong }, transaction);

                            string cpu = template?.CPU ?? "Intel Core i5";
                            int ram = template?.RAM ?? 8;
                            int storage = template?.DungLuongLuuTru ?? 256;
                            double monitor = template?.KichThuocManHinh ?? 24.0;
                            int ttMayId = template?.MaTTMay ?? 1;

                            for (int i = 1; i <= diff; i++)
                            {
                                int nextIndex = currentComputerCount + i;
                                string tenMay = $"{room.TenPhong}-PC{nextIndex:D2}";
                                db.Execute(
                                    @"INSERT INTO MAY_TINH (TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, MaPhong, MaTTMay)
                                      VALUES (@tenMay, @cpu, @ram, @storage, @monitor, @MaPhong, @ttMayId)",
                                    new { tenMay, cpu, ram, storage, monitor, room.MaPhong, ttMayId }, transaction);
                            }
                        }
                        
                        transaction.Commit();
                        return rows > 0;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Thống kê phòng: tổng, hoạt động, đóng cửa.
        /// </summary>
        public (int TotalRooms, int Available, int Occupied) GetRoomStats()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                int totalRooms = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PHONG_MAY");
                int available = db.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong=N'Hoạt động'");
                int occupied = db.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong!=N'Hoạt động'");
                return (totalRooms, available, occupied);
            }
        }

        /// <summary>
        /// Kiểm tra phòng có lịch thực hành đang hoạt động (hiện tại hoặc tương lai).
        /// </summary>
        public bool HasActiveSchedule(int roomId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                int count = db.ExecuteScalar<int>(
                    @"SELECT COUNT(*) FROM PHAN_CONG_PHONG pc
                      JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                      JOIN CA_HOC c ON l.MaCa = c.MaCa
                      WHERE pc.MaPhong=@roomId AND l.TrangThaiLich NOT IN (N'Đã hủy', N'Không được xếp')
                        AND (l.NgayThucHanh > CAST(GETDATE() AS DATE) 
                             OR (l.NgayThucHanh = CAST(GETDATE() AS DATE) AND c.GioKetThuc >= CAST(GETDATE() AS TIME)))",
                    new { roomId });
                return count > 0;
            }
        }

        /// <summary>
        /// Deletes a room and its associated computers using a database transaction.
        /// Fulfills Level 3 Requirement: Giao dịch và Hoàn tác.
        /// </summary>
        public bool DeleteRoomWithTransaction(int roomId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                if (db.State != ConnectionState.Open) db.Open();
                
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. Delete associated records referencing the room
                        db.Execute("DELETE FROM PHAN_CONG_PHONG WHERE MaPhong = @roomId", new { roomId }, transaction);
                        db.Execute("DELETE FROM CAP_NHAT_PHONG WHERE MaPhong = @roomId", new { roomId }, transaction);
                        
                        // 2. Delete computers in the room
                        db.Execute("DELETE FROM CAP_NHAT_MAY WHERE MaMay IN (SELECT MaMay FROM MAY_TINH WHERE MaPhong = @roomId)", new { roomId }, transaction);
                        db.Execute("DELETE FROM MAY_TINH WHERE MaPhong = @roomId", new { roomId }, transaction);

                        // 3. Delete the room
                        db.Execute("DELETE FROM PHONG_MAY WHERE MaPhong = @roomId", new { roomId }, transaction);

                        // If both succeed, commit the transaction
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        // If any error occurs, rollback all changes
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}

