using System;
using System.Collections.Generic;
using src.DAL;
using src.DTO;
using System.Linq;

namespace src.BLL
{
    public class CatalogService
    {
        private readonly ICatalogRepository _repository;

        public CatalogService()
        {
            _repository = new CatalogRepository();
        }

        public IEnumerable<LopHocDTO> GetAllLopHoc() => _repository.GetAllLopHoc().ToList();
        public IEnumerable<MonHocDTO> GetAllMonHoc() => _repository.GetAllMonHoc().ToList();

        public void CreateLopHoc(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            _repository.CreateLopHoc(name);
        }

        public void CreateMonHoc(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên môn không hợp lệ.");
            _repository.CreateMonHoc(name);
        }

        public void UpdateLopHoc(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên lớp không hợp lệ.");
            _repository.UpdateLopHoc(id, name);
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
