using System;
using System.Collections.Generic;
using src.DAL;
using src.DTO;
using src.Helpers;

namespace src.BLL
{
    public class NguoiDungService
    {
        private readonly INguoiDungRepository _NguoiDungRepository;

        public NguoiDungService(INguoiDungRepository NguoiDungRepository)
        {
            _NguoiDungRepository = NguoiDungRepository;
        }

        public NguoiDungService()
        {
            _NguoiDungRepository = new NguoiDungRepository();
        }

        public (bool IsSuccess, string ErrorMessage, TaiKhoanDTO User) Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return (false, "⚠ Vui lòng nhập đầy đủ thông tin!", null);
            }

            try
            {
                var user = _NguoiDungRepository.GetUserByUsername(username);

                if (user == null)
                {
                    return (false, "⚠ Tên đăng nhập không tồn tại!", null);
                }

                if (!user.TrangThai)
                {
                    return (false, "⚠ Tài khoản chưa được kích hoạt!\nVui lòng liên hệ Admin để được mở quyền.", null);
                }

                if (!DatabaseHelper.VerifyPassword(password, user.MatKhauDaMaHoa))
                {
                    return (false, "⚠ Mật khẩu không đúng!", null);
                }

                return (true, string.Empty, user);
            }
            catch (Exception ex)
            {
                return (false, "⚠ Lỗi kết nối: " + ex.Message, null);
            }
        }

        public IEnumerable<TaiKhoanDTO> GetAllUsers()
        {
            return _NguoiDungRepository.GetAllUsers();
        }

        public void CreateUser(string username, string password, string hoTen, string email, string phone, bool active)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Vui lòng điền đầy đủ thông tin bắt buộc!");
            }

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                throw new Exception("Họ và tên không được để trống!");
            }

            var existingUser = _NguoiDungRepository.GetUserByUsername(username);
            if (existingUser != null)
            {
                throw new Exception("Tên đăng nhập đã tồn tại!");
            }

            int roleId = _NguoiDungRepository.GetRoleIdByName("NhanVien");
            string hashed = DatabaseHelper.HashPassword(password);

            var user = new TaiKhoanDTO
            {
                TenDangNhap = username,
                MatKhauDaMaHoa = hashed,
                HoTen = hoTen,
                Email = email,
                SoDienThoai = phone,
                TrangThai = active,
                MaVaiTro = roleId
            };

            _NguoiDungRepository.CreateUser(user);
        }

        public void UpdateUser(string username, string newPassword, string hoTen, string email, string phone, bool active)
        {
            var user = new TaiKhoanDTO
            {
                TenDangNhap = username,
                HoTen = string.IsNullOrWhiteSpace(hoTen) ? throw new Exception("Họ và tên không được để trống!") : hoTen,
                Email = email,
                SoDienThoai = phone,
                TrangThai = active
            };

            bool updatePassword = !string.IsNullOrEmpty(newPassword);
            if (updatePassword)
            {
                user.MatKhauDaMaHoa = DatabaseHelper.HashPassword(newPassword);
            }

            _NguoiDungRepository.UpdateUser(user, updatePassword);
        }

        public void DeleteUser(int userId, string username, bool active)
        {
            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Không thể xóa tài khoản admin!");
            }

            bool hasData = _NguoiDungRepository.CheckUserHasData(userId);
            if (hasData && active)
            {
                throw new Exception("Tài khoản này đã được kích hoạt và đang sử dụng nên không thể xóa! Vui lòng ngừng kích hoạt (khóa) tài khoản trước.");
            }

            _NguoiDungRepository.DeleteUserAndRelatedData(userId);
        }
    }
}

