using System;
using System.Collections.Generic;
using src.DAL;
using src.DTO;
using src.Helpers;

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
                return _PhongMayRepository.GetRoomStats();
            }
            catch
            {
                return (0, 0, 0);
            }
        }

        /// <summary>
        /// Lấy thông tin phòng kèm tên trạng thái (cho dialog sửa).
        /// </summary>
        public PhongMayDTO GetRoomWithStatus(int roomId)
        {
            return _PhongMayRepository.GetRoomWithStatus(roomId);
        }

        /// <summary>
        /// Kiểm tra tên phòng đã tồn tại chưa.
        /// </summary>
        public bool IsRoomNameExists(string name, int? excludeId = null)
        {
            return _PhongMayRepository.IsRoomNameExists(name, excludeId);
        }

        /// <summary>
        /// Thêm phòng mới kèm tự động tạo máy tính.
        /// </summary>
        public (bool IsSuccess, string Message) AddRoom(PhongMayDTO room, string cpu, int ram, int storage, int monitor)
        {
            if (string.IsNullOrWhiteSpace(room.TenPhong))
                return (false, "Tên phòng không được để trống!");
            if (string.IsNullOrWhiteSpace(room.ViTri))
                return (false, "Vị trí không được để trống!");

            if (_PhongMayRepository.IsRoomNameExists(room.TenPhong))
                return (false, "Tên phòng này đã tồn tại! Vui lòng nhập tên khác.");

            try
            {
                int maPhong = _PhongMayRepository.AddRoomWithComputers(room, cpu, ram, storage, monitor);
                return (true, $"Đã thêm phòng và tự động tạo {room.SucChua} máy tính thành công!");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống khi thêm phòng: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật phòng máy.
        /// </summary>
        public (bool IsSuccess, string Message) UpdateRoom(PhongMayDTO room, int userId)
        {
            if (string.IsNullOrWhiteSpace(room.TenPhong))
                return (false, "Tên phòng không được để trống!");
            if (string.IsNullOrWhiteSpace(room.ViTri))
                return (false, "Vị trí không được để trống!");

            try
            {
                bool success = _PhongMayRepository.UpdateRoom(room, userId);
                if (success)
                    return (true, "Đã cập nhật phòng thành công!");
                else
                    return (false, "Lỗi hệ thống khi cập nhật phòng.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống khi cập nhật phòng: " + ex.Message);
            }
        }

        public (bool IsSuccess, string Message) DeleteRoom(int roomId)
        {
            // Kiểm tra xem phòng có lịch trong hiện tại hoặc tương lai không
            if (_PhongMayRepository.HasActiveSchedule(roomId))
            {
                return (false, "Phòng đang có lịch thực hành trong hiện tại hoặc tương lai, không thể xóa!");
            }

            bool success = _PhongMayRepository.DeleteRoomWithTransaction(roomId);
            if (success)
                return (true, "Đã xóa phòng thành công!");
            else
                return (false, "Xóa phòng thất bại do lỗi hệ thống (Đã Rollback).");
        }
    }
}
