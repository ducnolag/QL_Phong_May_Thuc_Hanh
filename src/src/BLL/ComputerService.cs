using System.Collections.Generic;
using src.DAL;
using src.DTO;

namespace src.BLL
{
    public class ComputerService
    {
        private readonly IComputerRepository _computerRepository;

        public ComputerService()
        {
            _computerRepository = new ComputerRepository();
        }

        public List<MayTinhDTO> GetAllComputers()
        {
            return _computerRepository.GetAllComputers();
        }

        public (bool IsSuccess, string Message) AddComputer(MayTinhDTO computer)
        {
            if (string.IsNullOrEmpty(computer.TenMay) || string.IsNullOrEmpty(computer.CPU))
                return (false, "Mã máy và CPU không được để trống!");

            bool success = _computerRepository.AddComputer(computer);
            if (success)
                return (true, "Đã thêm máy tính thành công!");
            else
                return (false, "Lỗi hệ thống khi thêm máy tính.");
        }

        public (bool IsSuccess, string Message) UpdateComputer(MayTinhDTO computer)
        {
            if (string.IsNullOrEmpty(computer.TenMay) || string.IsNullOrEmpty(computer.CPU))
                return (false, "Mã máy và CPU không được để trống!");

            bool success = _computerRepository.UpdateComputer(computer);
            if (success)
                return (true, "Đã cập nhật máy tính thành công!");
            else
                return (false, "Lỗi hệ thống khi cập nhật máy tính.");
        }

        public (bool IsSuccess, string Message) DeleteComputer(int maMay)
        {
            bool success = _computerRepository.DeleteComputer(maMay);
            if (success)
                return (true, "Đã xóa máy tính thành công!");
            else
                return (false, "Lỗi hệ thống khi xóa máy tính.");
        }
    }
}
