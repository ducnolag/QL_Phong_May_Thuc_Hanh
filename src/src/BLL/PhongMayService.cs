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
            // Kiểm tra xem phòng có lịch trong hiện tại hoặc tương lai không
            var count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM PHAN_CONG_PHONG pc
                  JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
                  JOIN CA_HOC c ON l.MaCa = c.MaCa
                  WHERE pc.MaPhong=@id AND l.TrangThaiLich NOT IN (N'Đã hủy', N'Không được xếp')
                    AND (l.NgayThucHanh > CAST(GETDATE() AS DATE) 
                         OR (l.NgayThucHanh = CAST(GETDATE() AS DATE) AND c.GioKetThuc >= CAST(GETDATE() AS TIME)))",
                new SqlParameter("@id", roomId)));
            
            if (count > 0)
            {
                return (false, "Phòng đang có lịch thực hành trong hiện tại hoặc tương lai, không thể xóa!");
            }

            bool success = _PhongMayRepository.DeleteRoomWithTransaction(roomId);
            if (success)
                return (true, "Đã xóa phòng thành công!");
            else
                return (false, "Xóa phòng thất bại do lỗi hệ thống (Đã Rollback).");
        }
        public (bool IsSuccess, string Message) AddRoom(PhongMayDTO room)
        {
            if (string.IsNullOrWhiteSpace(room.TenPhong))
                return (false, "Tên phòng không được để trống!");

            if (_PhongMayRepository.CheckRoomNameExists(room.TenPhong))
                return (false, "Tên phòng đã tồn tại. Vui lòng chọn tên khác!");

            bool success = _PhongMayRepository.AddRoom(room);
            return success ? (true, "Đã thêm phòng thành công!") : (false, "Lỗi hệ thống khi thêm phòng.");
        }

        public (bool IsSuccess, string Message) UpdateRoom(PhongMayDTO room)
        {
            if (string.IsNullOrWhiteSpace(room.TenPhong))
                return (false, "Tên phòng không được để trống!");

            if (_PhongMayRepository.CheckRoomNameExists(room.TenPhong, room.MaPhong))
                return (false, "Tên phòng đã tồn tại. Vui lòng chọn tên khác!");

            bool success = _PhongMayRepository.UpdateRoom(room);
            return success ? (true, "Đã cập nhật phòng thành công!") : (false, "Lỗi hệ thống khi cập nhật phòng.");
        }

        public (bool IsWarning, string Message) CheckAndUpdateRoomStatusWarning(int roomId)
        {
            var room = _PhongMayRepository.GetRoomById(roomId);
            if (room == null) return (false, "");

            // Tổng số máy hoạt động trong phòng (trạng thái "Tốt")
            string sql = "SELECT COUNT(*) FROM MAY_TINH m JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay WHERE m.MaPhong = @roomId AND (t.TenTrangThaiMay = N'Tốt' OR m.MaTTMay = 1)";
            int workingComps = Convert.ToInt32(DatabaseHelper.ExecuteScalar(sql, new SqlParameter("@roomId", roomId)));

            if (room.SucChua > 0 && ((double)workingComps / room.SucChua) < 0.7)
            {
                // Tìm "Cần bảo trì" status ID
                int maintenanceStatusId = _PhongMayRepository.GetRoomStatusIdByName("Cần bảo trì");
                if (maintenanceStatusId > 0 && room.MaTTPhong != maintenanceStatusId)
                {
                    _PhongMayRepository.UpdateRoomStatus(roomId, maintenanceStatusId);
                    return (true, $"Cảnh báo: Số máy hoạt động trong phòng {room.TenPhong} đã giảm xuống dưới 70% sức chứa. Trạng thái phòng đã tự động chuyển sang 'Cần bảo trì'.");
                }
            }

            return (false, "");
        }
    }
}

