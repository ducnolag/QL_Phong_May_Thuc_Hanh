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
                    _repository.CreateLopHoc("KTPM01-01", "KTPM01", 30, csharp.MaMon);
                    _repository.CreateLopHoc("KTPM02-01", "KTPM02", 30, csharp.MaMon);
                }
                var web = list.FirstOrDefault(x => x.TenMon == "Lập trình Web");
                if (web != null)
                {
                    _repository.CreateLopHoc("CNTT01-01", "CNTT01", 35, web.MaMon);
                    _repository.CreateLopHoc("CNTT02-01", "CNTT02", 35, web.MaMon);
                }
            }
            return list;
        }

        public void CreateLopHoc(string maLopHocPhan, string name, int siSo, int? maMon)
        {
            if (string.IsNullOrWhiteSpace(maLopHocPhan)) throw new Exception("Mã lớp học phần không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");

            var allLops = GetAllLopHoc();
            if (allLops.Any(l => l.MaLopHocPhan?.Equals(maLopHocPhan, StringComparison.OrdinalIgnoreCase) == true))
                throw new Exception("Mã lớp học phần đã tồn tại.");

            _repository.CreateLopHoc(maLopHocPhan, name, siSo, maMon);
        }

        public void CreateMonHoc(string maHocPhan, string name)
        {
            if (string.IsNullOrWhiteSpace(maHocPhan)) throw new Exception("Mã học phần không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");

            var allMons = GetAllMonHoc();
            if (allMons.Any(m => m.MaHocPhan?.Equals(maHocPhan, StringComparison.OrdinalIgnoreCase) == true))
                throw new Exception("Mã học phần đã tồn tại.");

            _repository.CreateMonHoc(maHocPhan, name);
        }

        public void UpdateLopHoc(int id, string maLopHocPhan, string name, int siSo, int? maMon)
        {
            if (string.IsNullOrWhiteSpace(maLopHocPhan)) throw new Exception("Mã lớp học phần không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");

            var allLops = GetAllLopHoc();
            if (allLops.Any(l => l.MaLopHocPhan?.Equals(maLopHocPhan, StringComparison.OrdinalIgnoreCase) == true && l.MaLop != id))
                throw new Exception("Mã lớp học phần đã tồn tại.");

            _repository.UpdateLopHoc(id, maLopHocPhan, name, siSo, maMon);
        }

        public void UpdateMonHoc(int id, string maHocPhan, string name)
        {
            if (string.IsNullOrWhiteSpace(maHocPhan)) throw new Exception("Mã học phần không hợp lệ.");
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");

            var allMons = GetAllMonHoc();
            if (allMons.Any(m => m.MaHocPhan?.Equals(maHocPhan, StringComparison.OrdinalIgnoreCase) == true && m.MaMon != id))
                throw new Exception("Mã học phần đã tồn tại.");

            _repository.UpdateMonHoc(id, maHocPhan, name);
        }

        public void DeleteLopHoc(int id)
        {
            try
            {
                _repository.DeleteLopHoc(id);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE") || ex.Message.Contains("FK_"))
                    throw new Exception("Không thể xóa vì đang được sử dụng trong lịch thực hành.");
                throw new Exception("Lỗi: " + ex.Message);
            }
        }

        public void DeleteMonHoc(int id)
        {
            try
            {
                _repository.DeleteMonHoc(id);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE") || ex.Message.Contains("FK_"))
                    throw new Exception("Không thể xóa vì đang được sử dụng trong lịch thực hành.");
                throw new Exception("Lỗi: " + ex.Message);
            }
        }
    }
}

