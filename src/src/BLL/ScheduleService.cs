using System;
using System.Collections.Generic;
using System.Linq;
using src.DAL;
using src.DTO;

namespace src.BLL
{
    public class ScheduleService
    {
        private readonly IScheduleRepository _repository;

        public ScheduleService()
        {
            _repository = new ScheduleRepository();
        }

        public (int total, int assigned, int pending, int canceled) GetStatistics()
        {
            return _repository.GetStatistics();
        }

        public IEnumerable<ScheduleDTO> GetActiveSchedules()
        {
            return _repository.GetActiveSchedules();
        }

        public ScheduleDTO GetScheduleById(int id)
        {
            return _repository.GetScheduleById(id);
        }

        public (int RAMToiThieu, int LuuTruToiThieu) GetScheduleRequirements(int id)
        {
            return _repository.GetScheduleRequirements(id);
        }

        public (int MaPhong, string TenPhong, int SucChua) GetAssignedRoom(int scheduleId)
        {
            return _repository.GetAssignedRoom(scheduleId);
        }

        public IEnumerable<dynamic> GetRoomsForAssignment(int soSV, int reqRam, int reqStorage, DateTime date, int caId, int currentScheduleId = 0)
        {
            return _repository.GetRoomsForAssignment(soSV, reqRam, reqStorage, date, caId, currentScheduleId);
        }

        public IEnumerable<CaHocDTO> GetAllCaHoc()
        {
            return _repository.GetAllCaHoc().ToList();
        }

        public void ValidateAndCreateSchedule(DateTime date, string lopName, string monName, int caId, int soSV, int reqRam, int reqStorage, int? roomId, int creatorId)
        {
            if (date.Date < DateTime.Today) throw new Exception("Không thể đặt lịch vào ngày trong quá khứ! Vui lòng chọn lại ngày.");
            if (string.IsNullOrWhiteSpace(lopName) || string.IsNullOrWhiteSpace(monName)) throw new Exception("Vui lòng nhập đầy đủ thông tin Lớp, Môn và chọn Ca học!");

            int lopId = _repository.GetLopIdByName(lopName);
            if (lopId == 0) lopId = _repository.CreateLop(lopName);
            int monId = _repository.GetMonIdByName(monName);
            if (monId == 0) monId = _repository.CreateMon(monName);

            if (_repository.CheckDuplicateClassSchedule(lopId, date, caId) > 0)
            {
                throw new Exception("Lớp này đã có lịch thực hành vào cùng ngày và ca học đó rồi! Vui lòng chọn ngày hoặc ca khác.");
            }

            if (roomId.HasValue && roomId.Value > 0)
            {
                int mayDatYeuCau = _repository.CountAvailableComputers(roomId.Value, reqRam, reqStorage);
                if (mayDatYeuCau < soSV)
                {
                    throw new Exception($"Phòng máy được chọn chỉ có {mayDatYeuCau} máy đáp ứng cấu hình (RAM ≥ {reqRam}GB, Lưu trữ ≥ {reqStorage}GB), không đủ cho {soSV} sinh viên!\nVui lòng chọn phòng khác hoặc giảm yêu cầu cấu hình.");
                }

                if (_repository.CheckRoomConflict(roomId.Value, date, caId) > 0)
                {
                    throw new Exception("Rất tiếc, phòng máy này vừa được người khác đặt trước cho ca học và ngày này. Vui lòng chọn phòng khác!");
                }
            }

            var schedule = new ScheduleDTO
            {
                NgayThucHanh = date,
                SoLuongSinhVien = soSV,
                MaLop = lopId,
                MaMon = monId,
                MaCa = caId,
                NguoiTao = creatorId
            };

            _repository.CreateSchedule(schedule, reqRam, reqStorage, roomId > 0 ? roomId : null);
        }

        public void ValidateAndUpdateSchedule(int scheduleId, DateTime date, string lopName, string monName, int caId, int soSV, int reqRam, int reqStorage, int? roomId, int updaterId)
        {
            if (date.Date < DateTime.Today) throw new Exception("Không thể đặt lịch vào ngày trong quá khứ! Vui lòng chọn lại ngày.");
            if (string.IsNullOrWhiteSpace(lopName) || string.IsNullOrWhiteSpace(monName)) throw new Exception("Vui lòng nhập đầy đủ thông tin Lớp, Môn và chọn Ca học!");

            int lopId = _repository.GetLopIdByName(lopName);
            if (lopId == 0) lopId = _repository.CreateLop(lopName);
            int monId = _repository.GetMonIdByName(monName);
            if (monId == 0) monId = _repository.CreateMon(monName);

            if (_repository.CheckDuplicateClassSchedule(lopId, date, caId, scheduleId) > 0)
            {
                throw new Exception("Lớp này đã có lịch thực hành vào cùng ngày và ca học đó rồi! Vui lòng chọn ngày hoặc ca khác.");
            }

            if (roomId.HasValue && roomId.Value > 0)
            {
                int mayDatYeuCau = _repository.CountAvailableComputers(roomId.Value, reqRam, reqStorage);
                if (mayDatYeuCau < soSV)
                {
                    throw new Exception($"Phòng máy được chọn chỉ có {mayDatYeuCau} máy đáp ứng cấu hình (RAM ≥ {reqRam}GB, Lưu trữ ≥ {reqStorage}GB), không đủ cho {soSV} sinh viên!\nVui lòng chọn phòng khác hoặc giảm yêu cầu cấu hình.");
                }

                if (_repository.CheckRoomConflict(roomId.Value, date, caId, scheduleId) > 0)
                {
                    throw new Exception("Rất tiếc, phòng máy này vừa được người khác đặt trước cho ca học và ngày này. Vui lòng chọn phòng khác!");
                }
            }

            var schedule = new ScheduleDTO
            {
                MaLich = scheduleId,
                NgayThucHanh = date,
                SoLuongSinhVien = soSV,
                MaLop = lopId,
                MaMon = monId,
                MaCa = caId,
                NguoiTao = updaterId // In update, this might not strictly overwrite creator, but serves as context for PHAN_CONG_PHONG
            };

            _repository.UpdateSchedule(schedule, reqRam, reqStorage, roomId > 0 ? roomId : null);
        }

        public void CancelSchedule(int id)
        {
            _repository.CancelSchedule(id);
        }

        public void DeleteSchedule(int id)
        {
            _repository.DeleteSchedule(id);
        }
    }
}
