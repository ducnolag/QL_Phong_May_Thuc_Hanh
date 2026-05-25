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
        public IEnumerable<MonHocDTO> GetAllMonHoc() => _repository.GetAllMonHoc().ToList();

        public void CreateLopHoc(string name, int siSo)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");
            _repository.CreateLopHoc(name, siSo);
        }

        public void CreateMonHoc(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");
            _repository.CreateMonHoc(name);
        }

        public void UpdateLopHoc(int id, string name, int siSo)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            if (siSo <= 0) throw new Exception("Sĩ số phải lớn hơn 0.");
            _repository.UpdateLopHoc(id, name, siSo);
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

