using System.Collections.Generic;
using src.DAL;
using src.DTO;

namespace src.BLL
{
    public interface IBaoCaoThongKeService
    {
        ThongKeTongQuanDTO GetThongKeTongQuan(int thang, int nam);
        List<ThongKeMayTheoPhongDTO> GetThongKeMayTheoPhong();
        List<ThongKeLichDTO> GetThongKeLich(int thang, int nam);
    }

    public class BaoCaoThongKeService : IBaoCaoThongKeService
    {
        private readonly IBaoCaoThongKeRepository _repo;

        public BaoCaoThongKeService()
        {
            _repo = new BaoCaoThongKeRepository();
        }

        public ThongKeTongQuanDTO GetThongKeTongQuan(int thang, int nam)
        {
            return _repo.GetThongKeTongQuan(thang, nam);
        }

        public List<ThongKeMayTheoPhongDTO> GetThongKeMayTheoPhong()
        {
            return _repo.GetThongKeMayTheoPhong();
        }

        public List<ThongKeLichDTO> GetThongKeLich(int thang, int nam)
        {
            return _repo.GetThongKeLich(thang, nam);
        }
    }
}
