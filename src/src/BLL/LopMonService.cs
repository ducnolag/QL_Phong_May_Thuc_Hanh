using System;
using System.Collections.Generic;
using src.DAL;
using src.DTO;
using System.Linq;

namespace src.BLL
{
    public class LopMonService
    {
        private readonly ILopMonRepository _repository;

        public LopMonService()
        {
            _repository = new LopMonRepository();
        }

        public IEnumerable<LopHocDTO> GetAllLopHoc() => _repository.GetAllLopHoc().ToList();
        public IEnumerable<MonHocDTO> GetAllMonHoc()
        {
            var list = _repository.GetAllMonHoc().ToList();
            if (!list.Any())
            {
                // Seed data if empty
                _repository.CreateMonHoc("IT01", "Lập trình C#");
                _repository.CreateMonHoc("IT02", "Lập trình Web");
                _repository.CreateMonHoc("IT03", "Cơ sở dữ liệu");
                list = _repository.GetAllMonHoc().ToList();
                
                var csharp = list.FirstOrDefault(x => x.TenMon == "Lập trình C#");
                if (csharp != null)
                {
                    _repository.CreateLopHoc("KTPM01-01", "KTPM01", 30, csharp.MaHocPhan);
                    _repository.CreateLopHoc("KTPM02-01", "KTPM02", 30, csharp.MaHocPhan);
                }
                var web = list.FirstOrDefault(x => x.TenMon == "Lập trình Web");
                if (web != null)
                {
                    _repository.CreateLopHoc("CNTT01-01", "CNTT01", 35, web.MaHocPhan);
                    _repository.CreateLopHoc("CNTT02-01", "CNTT02", 35, web.MaHocPhan);
                }
            }
            return list;
        }

        public void CreateLopHoc(string maLopHocPhan, string name, int siSo, string MaHocPhan)
        {
            if (string.IsNullOrWhiteSpace(maLopHocPhan)) throw new Exception("Mã lớp học phần không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");

            var allLops = GetAllLopHoc();
            if (allLops.Any(l => l.MaLopHocPhan?.Equals(maLopHocPhan, StringComparison.OrdinalIgnoreCase) == true))
                throw new Exception("Mã lớp học phần đã tồn tại.");

            _repository.CreateLopHoc(maLopHocPhan, name, siSo, MaHocPhan);
        }

        public void CreateMonHoc(string MaHocPhan, string name)
        {
            if (string.IsNullOrWhiteSpace(MaHocPhan)) throw new Exception("Mã môn không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");

            var allMons = GetAllMonHoc();
            if (allMons.Any(m => m.MaHocPhan?.Equals(MaHocPhan, StringComparison.OrdinalIgnoreCase) == true))
                throw new Exception("Mã môn đã tồn tại.");

            _repository.CreateMonHoc(MaHocPhan, name);
        }

        public void UpdateLopHoc(string oldMaLopHocPhan, string maLopHocPhan, string name, int siSo, string MaHocPhan)
        {
            if (string.IsNullOrWhiteSpace(maLopHocPhan)) throw new Exception("Mã lớp học phần không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");

            var allLops = GetAllLopHoc();
            if (allLops.Any(l => l.MaLopHocPhan?.Equals(maLopHocPhan, StringComparison.OrdinalIgnoreCase) == true && !l.MaLopHocPhan.Equals(oldMaLopHocPhan, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Mã lớp học phần đã tồn tại.");

            _repository.UpdateLopHoc(maLopHocPhan, name, siSo, MaHocPhan);
        }

        public void UpdateMonHoc(string oldMaMon, string MaHocPhan, string name)
        {
            if (string.IsNullOrWhiteSpace(MaHocPhan)) throw new Exception("Mã môn không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");

            var allMons = GetAllMonHoc();
            if (allMons.Any(m => m.MaHocPhan?.Equals(MaHocPhan, StringComparison.OrdinalIgnoreCase) == true && !m.MaHocPhan.Equals(oldMaMon, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Mã môn đã tồn tại.");

            _repository.UpdateMonHoc(MaHocPhan, name);
        }

        public void DeleteLopHoc(string maLopHocPhan)
        {
            // Kiểm tra nếu lớp còn lịch HIỆN TẠI hoặc TƯƠNG LAI chưa hủy => chặn
            bool hasFutureSched = _repository.HasActiveOrFutureSchedule_Lop(maLopHocPhan);
            if (hasFutureSched)
                throw new Exception("Lớp này còn lịch thực hành hiện tại hoặc tương lai chưa hủy.\nVui lòng hủy hoặc xóa các lịch đó trước!");

            // Cascade xóa lịch quá khứ/đã hủy liên quan, sau đó xóa lớp
            _repository.DeleteLopHocWithCascade(maLopHocPhan);
        }

        public void DeleteMonHoc(string MaHocPhan)
        {
            // Kiểm tra nếu môn còn lịch HIỆN TẠI hoặc TƯƠNG LAI chưa hủy => chặn
            bool hasFutureSched = _repository.HasActiveOrFutureSchedule_Mon(MaHocPhan);
            if (hasFutureSched)
                throw new Exception("Môn học này còn lịch thực hành hiện tại hoặc tương lai chưa hủy.\nVui lòng hủy hoặc xóa các lịch đó trước!");

            // Cascade xóa lịch quá khứ/đã hủy của tất cả lớp thuộc môn, sau đó xóa môn
            _repository.DeleteMonHocWithCascade(MaHocPhan);
        }
    }
}

