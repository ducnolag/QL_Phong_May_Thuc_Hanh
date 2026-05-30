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
        PhongMayDTO GetRoomById(int roomId);
        bool DeleteRoomWithTransaction(int roomId);
        bool AddRoom(PhongMayDTO room);
        bool UpdateRoom(PhongMayDTO room);
        bool CheckRoomNameExists(string tenPhong, int? excludeRoomId = null);
        bool UpdateRoomStatus(int roomId, int statusId);
        int GetRoomStatusIdByName(string statusName);
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

        public PhongMayDTO GetRoomById(int roomId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT * FROM PHONG_MAY WHERE MaPhong = @roomId";
                return db.QueryFirstOrDefault<PhongMayDTO>(sql, new { roomId });
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
        public bool AddRoom(PhongMayDTO room)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = "INSERT INTO PHONG_MAY (TenPhong, ViTri, SucChua, MaTTPhong) VALUES (@TenPhong, @ViTri, @SucChua, @MaTTPhong)";
                int rows = db.Execute(sql, new { room.TenPhong, room.ViTri, room.SucChua, room.MaTTPhong });
                return rows > 0;
            }
        }

        public bool UpdateRoom(PhongMayDTO room)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE PHONG_MAY SET TenPhong = @TenPhong, ViTri = @ViTri, SucChua = @SucChua, MaTTPhong = @MaTTPhong WHERE MaPhong = @MaPhong";
                int rows = db.Execute(sql, new { room.TenPhong, room.ViTri, room.SucChua, room.MaTTPhong, room.MaPhong });
                return rows > 0;
            }
        }

        public bool CheckRoomNameExists(string tenPhong, int? excludeRoomId = null)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM PHONG_MAY WHERE TenPhong = @tenPhong";
                if (excludeRoomId.HasValue)
                {
                    sql += " AND MaPhong != @excludeRoomId";
                }
                int count = db.ExecuteScalar<int>(sql, new { tenPhong, excludeRoomId });
                return count > 0;
            }
        }

        public bool UpdateRoomStatus(int roomId, int statusId)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE PHONG_MAY SET MaTTPhong = @statusId WHERE MaPhong = @roomId";
                int rows = db.Execute(sql, new { statusId, roomId });
                return rows > 0;
            }
        }

        public int GetRoomStatusIdByName(string statusName)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT MaTTPhong FROM TRANG_THAI_PHONG WHERE TenTrangThaiPhong = @statusName";
                return db.ExecuteScalar<int>(sql, new { statusName });
            }
        }

    }
}
