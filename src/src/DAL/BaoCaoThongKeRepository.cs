using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using src.DTO;
using src.Helpers;

namespace src.DAL
{
    public interface IBaoCaoThongKeRepository
    {
        ThongKeTongQuanDTO GetThongKeTongQuan(DateTime? startDate, DateTime? endDate);
        List<ThongKeMayTheoPhongDTO> GetThongKeMayTheoPhong();
        List<ThongKeLichDTO> GetThongKeLich(DateTime? startDate, DateTime? endDate);
    }

    public class BaoCaoThongKeRepository : IBaoCaoThongKeRepository
    {
        public ThongKeTongQuanDTO GetThongKeTongQuan(DateTime? startDate, DateTime? endDate)
        {
            var dto = new ThongKeTongQuanDTO();
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string dtCond = "";
                if (startDate.HasValue && endDate.HasValue)
                {
                    dtCond = " AND l.NgayThucHanh >= @startDate AND l.NgayThucHanh <= @endDate ";
                }
                
                // Ensure today's snapshot is up to date before querying
                db.Execute(@"
                    DECLARE @Today DATE = CAST(GETDATE() AS DATE);
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
                ");

                string sql = $@"
                    SELECT 
                        ISNULL((SELECT TOP 1 TotalRooms FROM CHOT_SO_LIEU WHERE NgayChot <= @endDate ORDER BY NgayChot DESC), 0) as TotalRooms,
                        ISNULL((SELECT TOP 1 ActiveRooms FROM CHOT_SO_LIEU WHERE NgayChot <= @endDate ORDER BY NgayChot DESC), 0) as ActiveRooms,
                        ISNULL((SELECT TOP 1 TotalMay FROM CHOT_SO_LIEU WHERE NgayChot <= @endDate ORDER BY NgayChot DESC), 0) as TotalMay,
                        ISNULL((SELECT TOP 1 MayTot FROM CHOT_SO_LIEU WHERE NgayChot <= @endDate ORDER BY NgayChot DESC), 0) as MayTot,
                        ISNULL((SELECT TOP 1 MayHong FROM CHOT_SO_LIEU WHERE NgayChot <= @endDate ORDER BY NgayChot DESC), 0) as MayHong,
                        (SELECT COUNT(*) FROM LICH_THUC_HANH l WHERE 1=1 {dtCond}) as TotalLich,
                        (SELECT COUNT(*) FROM LICH_THUC_HANH l WHERE l.TrangThaiLich != N'Đã hủy' AND l.MaLich IN (SELECT MaLich FROM PHAN_CONG_PHONG) {dtCond}) as LichDaXep,
                        (SELECT COUNT(*) FROM LICH_THUC_HANH l WHERE l.TrangThaiLich=N'Đã hủy' {dtCond}) as LichDaHuy,
                        ISNULL((SELECT TOP 1 TotalUsers FROM CHOT_SO_LIEU WHERE NgayChot <= @endDate ORDER BY NgayChot DESC), 0) as TotalUsers
                ";

                var result = db.QueryFirstOrDefault(sql, new { startDate = startDate?.Date, endDate = endDate?.Date });
                
                if (result != null)
                {
                    dto.TotalRooms = result.TotalRooms ?? 0;
                    dto.ActiveRooms = result.ActiveRooms ?? 0;
                    dto.ClosedRooms = System.Math.Max(0, dto.TotalRooms - dto.ActiveRooms);

                    dto.TotalMay = result.TotalMay ?? 0;
                    dto.MayTot = result.MayTot ?? 0;
                    dto.MayHong = result.MayHong ?? 0;

                    dto.TotalLich = result.TotalLich ?? 0;
                    dto.LichDaXep = result.LichDaXep ?? 0;
                    dto.LichDaHuy = result.LichDaHuy ?? 0;
                    dto.LichChoXep = dto.TotalLich - dto.LichDaXep - dto.LichDaHuy;

                    dto.TotalUsers = result.TotalUsers ?? 0;
                }
            }
            return dto;
        }

        public List<ThongKeMayTheoPhongDTO> GetThongKeMayTheoPhong()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT 
                        p.TenPhong,
                        COUNT(m.MaMay) AS Tong,
                        SUM(CASE WHEN t.TenTrangThaiMay=N'Tốt' THEN 1 ELSE 0 END) AS Tot,
                        SUM(CASE WHEN t.TenTrangThaiMay=N'Hỏng' THEN 1 ELSE 0 END) AS Hong
                    FROM PHONG_MAY p
                    LEFT JOIN MAY_TINH m ON m.MaPhong = p.MaPhong
                    LEFT JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                    GROUP BY p.TenPhong 
                    ORDER BY p.TenPhong";

                return db.Query<ThongKeMayTheoPhongDTO>(sql).ToList();
            }
        }

        public List<ThongKeLichDTO> GetThongKeLich(DateTime? startDate, DateTime? endDate)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string dtCond = "";
                if (startDate.HasValue && endDate.HasValue)
                {
                    dtCond = " AND l.NgayThucHanh >= @startDate AND l.NgayThucHanh <= @endDate ";
                }

                string sql = $@"
                    SELECT TOP 30 
                        l.NgayThucHanh, 
                        mh.TenMon, 
                        c.TenCa,
                        l.SoLuongSinhVien, 
                        l.TrangThaiLich,
                        ISNULL(p.TenPhong, N'Chưa xếp') AS TenPhong
                    FROM LICH_THUC_HANH l
                    JOIN MON_HOC mh ON l.MaMon = mh.MaMon
                    JOIN CA_HOC c ON l.MaCa = c.MaCa
                    LEFT JOIN PHAN_CONG_PHONG pc ON l.MaLich = pc.MaLich
                    LEFT JOIN PHONG_MAY p ON pc.MaPhong = p.MaPhong
                    WHERE 1=1 {dtCond} 
                    ORDER BY l.NgayThucHanh DESC";

                return db.Query<ThongKeLichDTO>(sql, new { startDate = startDate?.Date, endDate = endDate?.Date }).ToList();
            }
        }
    }
}
