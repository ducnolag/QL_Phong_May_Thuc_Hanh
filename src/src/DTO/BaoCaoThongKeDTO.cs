using System;

namespace src.DTO
{
    public class ThongKeTongQuanDTO
    {
        public int TotalRooms { get; set; }
        public int ActiveRooms { get; set; }
        public int ClosedRooms { get; set; }
        
        public int TotalMay { get; set; }
        public int MayTot { get; set; }
        public int MayHong { get; set; }
        
        public int TotalLich { get; set; }
        public int LichDaXep { get; set; }
        public int LichChoXep { get; set; }
        public int LichKhongDuocXep { get; set; }
        public int LichDaHuy { get; set; }
        
        public int TotalUsers { get; set; }
    }

    public class ThongKeMayTheoPhongDTO
    {
        public string TenPhong { get; set; }
        public int Tong { get; set; }
        public int Tot { get; set; }
        public int Hong { get; set; }
    }

    public class ThongKeLichDTO
    {
        public DateTime NgayThucHanh { get; set; }
        public string TenMon { get; set; }
        public string TenCa { get; set; }
        public int SoLuongSinhVien { get; set; }
        public string TrangThaiLich { get; set; }
        public string TenPhong { get; set; }
    }
}
