using System;

namespace src.DTO
{
    public class LichThucHanhDTO
    {
        public int MaLich { get; set; }
        public DateTime NgayThucHanh { get; set; }
        public int SoLuongSinhVien { get; set; }
        public int MaLop { get; set; }
        public int MaMon { get; set; }
        public int MaCa { get; set; }
        public int NguoiTao { get; set; }
        public string TrangThaiLich { get; set; }

        // Additional fields for display/logic
        public string TenMon { get; set; }
        public string TenLop { get; set; }
        public string TenCa { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public string TenPhong { get; set; }
        
        public int RAMToiThieu { get; set; }
        public int LuuTruToiThieu { get; set; }
    }
}
