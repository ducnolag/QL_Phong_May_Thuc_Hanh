namespace src.DTO
{
    public class TaiKhoanDTO
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhauDaMaHoa { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public bool TrangThai { get; set; }
        public int MaVaiTro { get; set; }
        
        // Joined fields
        public string TenVaiTro { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
