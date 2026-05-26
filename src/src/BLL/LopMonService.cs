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
                _repository.CreateMonHoc("Lập trình C#");
                _repository.CreateMonHoc("Lập trình Web");
                _repository.CreateMonHoc("Cơ sở dữ liệu");
                list = _repository.GetAllMonHoc().ToList();
                
                var csharp = list.FirstOrDefault(x => x.TenMon == "Lập trình C#");
                if (csharp != null)
                {
                    _repository.CreateLopHoc("KTPM01", 30, csharp.MaMon);
                    _repository.CreateLopHoc("KTPM02", 30, csharp.MaMon);
                }
                var web = list.FirstOrDefault(x => x.TenMon == "Lập trình Web");
                if (web != null)
                {
                    _repository.CreateLopHoc("CNTT01", 35, web.MaMon);
                    _repository.CreateLopHoc("CNTT02", 35, web.MaMon);
                }
            }
            return list;
        }

        public void CreateLopHoc(string name, int siSo, int? maMon)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");

            var allLops = GetAllLopHoc();
            if (allLops.Any(l => l.TenLop.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Tên lớp đã tồn tại.");

            _repository.CreateLopHoc(name, siSo, maMon);
        }

        public void CreateMonHoc(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");
            _repository.CreateMonHoc(name);
        }

        public void UpdateLopHoc(int id, string name, int siSo, int? maMon)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");

            var allLops = GetAllLopHoc();
            if (allLops.Any(l => l.TenLop.Equals(name, StringComparison.OrdinalIgnoreCase) && l.MaLop != id))
                throw new Exception("Tên lớp đã tồn tại.");

            _repository.UpdateLopHoc(id, name, siSo, maMon);
        }

        public void UpdateMonHoc(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");
            _repository.UpdateMonHoc(id, name);
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

