using System.Collections.Generic;
using src.DAL;
using src.DTO;

namespace src.BLL
{
    public class MayTinhService
    {
        private readonly IMayTinhRepository _MayTinhRepository;

        public MayTinhService()
        {
            _MayTinhRepository = new MayTinhRepository();
        }

        public List<MayTinhDTO> GetAllComputers()
        {
            return _MayTinhRepository.GetAllComputers();
        }

        public (bool IsSuccess, string Message) AddComputer(MayTinhDTO computer)
        {
            if (string.IsNullOrEmpty(computer.TenMay) || string.IsNullOrEmpty(computer.CPU))
                return (false, "Mã máy và CPU không được để trống!");

            bool success = _MayTinhRepository.AddComputer(computer);
            if (success)
                return (true, "Đã thêm máy tính thành công!");
            else
                return (false, "Lỗi hệ thống khi thêm máy tính.");
        }

        public (bool IsSuccess, string Message) UpdateComputer(MayTinhDTO computer)
        {
            if (string.IsNullOrEmpty(computer.TenMay) || string.IsNullOrEmpty(computer.CPU))
                return (false, "Mã máy và CPU không được để trống!");

            bool success = _MayTinhRepository.UpdateComputer(computer);
            if (success)
                return (true, "Đã cập nhật máy tính thành công!");
            else
                return (false, "Lỗi hệ thống khi cập nhật máy tính.");
        }

        public (bool IsSuccess, string Message) DeleteComputer(int maMay)
        {
            var computers = _MayTinhRepository.GetAllComputers();
            var computer = computers.Find(c => c.MaMay == maMay);
            if (computer != null && computer.TenTrangThaiMay != "Hỏng")
            {
                return (false, "Chỉ được phép xóa máy tính khi máy đang ở trạng thái 'Hỏng'!");
            }

            bool success = _MayTinhRepository.DeleteComputer(maMay);
            if (success)
                return (true, "Đã xóa máy tính thành công!");
            else
                return (false, "Lỗi hệ thống khi xóa máy tính.");
        }
    }
}

