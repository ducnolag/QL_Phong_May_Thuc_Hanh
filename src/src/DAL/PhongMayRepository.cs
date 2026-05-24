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
    }
}

