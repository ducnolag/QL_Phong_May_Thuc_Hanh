using System;
using System.Collections.Generic;
using src.DAL;
using src.DTO;
using src.Helpers;

namespace src.BLL
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public (bool IsSuccess, string ErrorMessage, TaiKhoanDTO User) Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return (false, "⚠ Vui lòng nhập đầy đủ thông tin!", null);
            }

            try
            {
                var user = _userRepository.GetUserByUsername(username);

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
            return _userRepository.GetAllUsers();
        }

        public void CreateUser(string username, string password, string hoTen, string email, string role, bool active)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Vui lòng điền đầy đủ thông tin bắt buộc!");
            }

            var existingUser = _userRepository.GetUserByUsername(username);
            if (existingUser != null)
            {
                throw new Exception("Tên đăng nhập đã tồn tại!");
            }

            int roleId = _userRepository.GetRoleIdByName(role);
            string hashed = DatabaseHelper.HashPassword(password);

            var user = new TaiKhoanDTO
            {
                TenDangNhap = username,
                MatKhauDaMaHoa = hashed,
                HoTen = string.IsNullOrEmpty(hoTen) ? username : hoTen,
                Email = email,
                SoDienThoai = password, // Using SoDienThoai as hint (as original code did)
                TrangThai = active,
                MaVaiTro = roleId
            };

            _userRepository.CreateUser(user);
        }

        public void UpdateUser(string username, string newPassword, string hoTen, string email, string role, bool active)
        {
            int roleId = _userRepository.GetRoleIdByName(role);
            
            var user = new TaiKhoanDTO
            {
                TenDangNhap = username,
                HoTen = string.IsNullOrEmpty(hoTen) ? username : hoTen,
                Email = email,
                TrangThai = active,
                MaVaiTro = roleId
            };

            bool updatePassword = !string.IsNullOrEmpty(newPassword);
            if (updatePassword)
            {
                user.MatKhauDaMaHoa = DatabaseHelper.HashPassword(newPassword);
                user.SoDienThoai = newPassword;
            }

            _userRepository.UpdateUser(user, updatePassword);
        }

        public void DeleteUser(int userId, string username, bool active)
        {
            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Không thể xóa tài khoản admin!");
            }

            bool hasData = _userRepository.CheckUserHasData(userId);
            if (hasData && active)
            {
                throw new Exception("Tài khoản này đã được kích hoạt và đang sử dụng nên không thể xóa! Vui lòng ngừng kích hoạt (khóa) tài khoản trước.");
            }

            _userRepository.DeleteUserAndRelatedData(userId);
        }
    }
}
