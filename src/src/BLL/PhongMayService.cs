using System;
using System.Collections.Generic;
using src.DAL;
using src.DTO;
using src.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace src.BLL
{
    public class PhongMayService
    {
        private readonly IPhongMayRepository _PhongMayRepository;

        public PhongMayService()
        {
            _PhongMayRepository = new PhongMayRepository();
        }

        public List<PhongMayDTO> GetAllRooms()
        {
            return _PhongMayRepository.GetAllRooms();
        }

        public (int TotalRooms, int Available, int Occupied) GetRoomStats()
        {
            try
            {
                int totalRooms = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM PHONG_MAY"));
                int available = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong=N'Hoạt động'"));
                int occupied = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM PHONG_MAY p JOIN TRANG_THAI_PHONG t ON p.MaTTPhong=t.MaTTPhong WHERE t.TenTrangThaiPhong!=N'Hoạt động'"));
                return (totalRooms, available, occupied);
            }
            catch
            {
                return (0, 0, 0);
            }
        }

        public (bool IsSuccess, string Message) DeleteRoom(int roomId)
        {
            // Check if room has active (future or ongoing) schedules
            var count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM PHAN_CONG_PHONG pc
                  JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                  JOIN CA_HOC c ON l.MaCa = c.MaCa
                  WHERE pc.MaPhong=@id AND l.TrangThaiLich != N'Đã hủy'
                    AND (l.NgayThucHanh > CAST(GETDATE() AS DATE) 
                         OR (l.NgayThucHanh = CAST(GETDATE() AS DATE) AND c.GioKetThuc >= CAST(GETDATE() AS TIME)))",
                new SqlParameter("@id", roomId)));
            
            if (count > 0)
            {
                return (false, "Phòng đang có lịch thực hành không thể xóa!");
            }

            bool success = _PhongMayRepository.DeleteRoomWithTransaction(roomId);
            if (success)
                return (true, "Đã xóa phòng thành công!");
            else
                return (false, "Xóa phòng thất bại do lỗi hệ thống (Đã Rollback).");
        }
    }
}

