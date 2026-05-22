namespace src.DTO
{
    public class PhongMayDTO
    {
        public int MaPhong { get; set; }
        public string TenPhong { get; set; }
        public string ViTri { get; set; }
        public int SucChua { get; set; }
        public int MaTTPhong { get; set; }
        
        // Joined fields for view
        public string TenTrangThaiPhong { get; set; }
        public int SoMay { get; set; }
        
        // Derived fields for view
        public string StatusEng => TenTrangThaiPhong?.Contains("Hoạt") == true ? "available" : TenTrangThaiPhong?.Contains("Bảo") == true ? "maintenance" : "occupied";
    }
}
