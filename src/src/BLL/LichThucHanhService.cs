using System;
using System.Collections.Generic;
using System.Linq;
using src.DAL;
using src.DTO;

namespace src.BLL
{
    public class LichThucHanhService
    {
        private readonly ILichThucHanhRepository _repository;

        public LichThucHanhService()
        {
            _repository = new LichThucHanhRepository();
        }

        public (int total, int assigned, int pending, int canceled) GetStatistics(DateTime? start = null, DateTime? end = null)
        {
            return _repository.GetStatistics(start, end);
        }

        public IEnumerable<LichThucHanhDTO> GetActiveSchedules(DateTime? start = null, DateTime? end = null, bool includePast = false)
        {
            return _repository.GetActiveSchedules(start, end, includePast);
        }

        public LichThucHanhDTO GetScheduleById(int id)
        {
            return _repository.GetScheduleById(id);
        }

        public (int RAMToiThieu, int LuuTruToiThieu, int ManHinhToiThieu, string CPUToiThieu) GetScheduleRequirements(int id)
        {
            return _repository.GetScheduleRequirements(id);
        }

        public (int MaPhong, string TenPhong, int SucChua) GetAssignedRoom(int scheduleId)
        {
            return _repository.GetAssignedRoom(scheduleId);
        }

        public IEnumerable<dynamic> GetRoomsForAssignment(int soSV, int reqRam, int reqStorage, int reqMonitor, string reqCpu, DateTime date, int caId, int currentScheduleId = 0)
        {
            return _repository.GetRoomsForAssignment(soSV, reqRam, reqStorage, reqMonitor, reqCpu, date, caId, currentScheduleId);
        }

        public IEnumerable<CaHocDTO> GetAllCaHoc()
        {
            return _repository.GetAllCaHoc().ToList();
        }

        public void ValidateAndCreateSchedule(DateTime date, string lopName, string monName, int caId, int soSV, int reqRam, int reqStorage, int reqMonitor, string reqCpu, int? roomId, int creatorId)
        {
            if (date.Date < DateTime.Today) throw new Exception("Không thể đặt lịch vào ngày trong quá khứ! Vui lòng chọn lại ngày.");
            
            var ca = _repository.GetAllCaHoc().FirstOrDefault(c => c.MaCa == caId);
            if (date.Date == DateTime.Today && ca != null && DateTime.Now.TimeOfDay > ca.GioKetThuc)
            {
                throw new Exception("Ca học này đã kết thúc trong ngày hôm nay. Không thể thêm lịch!");
            }

            if (string.IsNullOrWhiteSpace(lopName) || string.IsNullOrWhiteSpace(monName)) throw new Exception("Vui lòng nhập đầy đủ thông tin Lớp, Môn và chọn Ca học!");

            string lopId = _repository.GetLopIdByName(lopName);
            if (string.IsNullOrEmpty(lopId)) lopId = _repository.CreateLop(lopName);
            string monId = _repository.GetMonIdByName(monName);
            if (string.IsNullOrEmpty(monId)) monId = _repository.CreateMon(monName);

            if (_repository.CheckDuplicateClassSchedule(lopId, date, caId) > 0)
            {
                throw new Exception("Lớp này đã có lịch thực hành vào cùng ngày và ca học đó rồi! Vui lòng chọn ngày hoặc ca khác.");
            }

            if (roomId.HasValue && roomId.Value > 0)
            {
                int mayDatYeuCau = _repository.CountAvailableComputers(roomId.Value, reqRam, reqStorage, reqMonitor, reqCpu);
                if (mayDatYeuCau < soSV)
                {
                    throw new Exception($"Phòng máy được chọn chỉ có {mayDatYeuCau} máy đáp ứng cấu hình (RAM ≥ {reqRam}GB, Lưu trữ ≥ {reqStorage}GB, Màn hình ≥ {reqMonitor}\", CPU: {reqCpu}), không đủ cho {soSV} sinh viên!\nVui lòng chọn phòng khác hoặc giảm yêu cầu cấu hình.");
                }

                if (_repository.CheckRoomConflict(roomId.Value, date, caId) > 0)
                {
                    throw new Exception("Rất tiếc, phòng máy này vừa được người khác đặt trước cho ca học và ngày này. Vui lòng chọn phòng khác!");
                }
            }

            var schedule = new LichThucHanhDTO
            {
                NgayThucHanh = date,
                SoLuongSinhVien = soSV,
                MaLopHocPhan = lopId,
                MaHocPhan = monId,
                MaCa = caId,
                NguoiTao = creatorId
            };

            _repository.CreateSchedule(schedule, reqRam, reqStorage, reqMonitor, reqCpu, roomId > 0 ? roomId : null);
        }

        public void ValidateAndUpdateSchedule(int scheduleId, DateTime date, string lopName, string monName, int caId, int soSV, int reqRam, int reqStorage, int reqMonitor, string reqCpu, int? roomId, int updaterId)
        {
            if (date.Date < DateTime.Today) throw new Exception("Không thể đặt lịch vào ngày trong quá khứ! Vui lòng chọn lại ngày.");

            var ca = _repository.GetAllCaHoc().FirstOrDefault(c => c.MaCa == caId);
            if (date.Date == DateTime.Today && ca != null && DateTime.Now.TimeOfDay > ca.GioKetThuc)
            {
                throw new Exception("Ca học này đã kết thúc trong ngày hôm nay. Không thể cập nhật lịch vào ca này!");
            }

            if (string.IsNullOrWhiteSpace(lopName) || string.IsNullOrWhiteSpace(monName)) throw new Exception("Vui lòng nhập đầy đủ thông tin Lớp, Môn và chọn Ca học!");

            string lopId = _repository.GetLopIdByName(lopName);
            if (string.IsNullOrEmpty(lopId)) lopId = _repository.CreateLop(lopName);
            string monId = _repository.GetMonIdByName(monName);
            if (string.IsNullOrEmpty(monId)) monId = _repository.CreateMon(monName);

            if (_repository.CheckDuplicateClassSchedule(lopId, date, caId, scheduleId) > 0)
            {
                throw new Exception("Lớp này đã có lịch thực hành vào cùng ngày và ca học đó rồi! Vui lòng chọn ngày hoặc ca khác.");
            }

            if (roomId.HasValue && roomId.Value > 0)
            {
                int mayDatYeuCau = _repository.CountAvailableComputers(roomId.Value, reqRam, reqStorage, reqMonitor, reqCpu);
                if (mayDatYeuCau < soSV)
                {
                    throw new Exception($"Phòng máy được chọn chỉ có {mayDatYeuCau} máy đáp ứng cấu hình (RAM ≥ {reqRam}GB, Lưu trữ ≥ {reqStorage}GB, Màn hình ≥ {reqMonitor}\", CPU: {reqCpu}), không đủ cho {soSV} sinh viên!\nVui lòng chọn phòng khác hoặc giảm yêu cầu cấu hình.");
                }

                if (_repository.CheckRoomConflict(roomId.Value, date, caId, scheduleId) > 0)
                {
                    throw new Exception("Rất tiếc, phòng máy này vừa được người khác đặt trước cho ca học và ngày này. Vui lòng chọn phòng khác!");
                }
            }

            var schedule = new LichThucHanhDTO
            {
                MaLich = scheduleId,
                NgayThucHanh = date,
                SoLuongSinhVien = soSV,
                MaLopHocPhan = lopId,
                MaHocPhan = monId,
                MaCa = caId,
                NguoiTao = updaterId // In update, this might not strictly overwrite creator, but serves as context for PHAN_CONG_PHONG
            };

            _repository.UpdateSchedule(schedule, reqRam, reqStorage, reqMonitor, reqCpu, roomId > 0 ? roomId : null);
        }

        public void CancelSchedule(int id)
        {
            var sch = _repository.GetScheduleById(id);
            if (sch != null)
            {
                var ca = _repository.GetAllCaHoc().FirstOrDefault(c => c.MaCa == sch.MaCa);
                if (sch.NgayThucHanh.Date == DateTime.Today && ca != null && DateTime.Now.TimeOfDay > ca.GioKetThuc)
                {
                    throw new Exception("Ca học này đã kết thúc trong ngày hôm nay. Không thể hủy lịch!");
                }
            }
            _repository.CancelSchedule(id);
        }

        public void DeleteSchedule(int scheduleId)
        {
            var sch = _repository.GetScheduleById(scheduleId);
            if (sch != null)
            {
                var ca = _repository.GetAllCaHoc().FirstOrDefault(c => c.MaCa == sch.MaCa);
                if (sch.NgayThucHanh.Date == DateTime.Today && ca != null && DateTime.Now.TimeOfDay > ca.GioKetThuc)
                {
                    throw new Exception("Ca học này đã kết thúc trong ngày hôm nay. Không thể xóa lịch!");
                }
            }
            _repository.DeleteSchedule(scheduleId);
        }
    }
}
