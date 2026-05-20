namespace src.Helpers
{
    /// <summary>
    /// Lưu thông tin người dùng đang đăng nhập (singleton đơn giản qua static).
    /// </summary>
    public static class AppSession
    {
        public static int    MaNguoiDung { get; set; } = 1;
        public static string HoTen       { get; set; } = "";
        public static bool   IsAdmin     { get; set; } = false;
    }
}
