using System;
using System.Collections.Generic;
using src.DAL;
using src.DTO;

namespace src.BLL
{
    public interface IBaoCaoThongKeService
    {
        ThongKeTongQuanDTO GetThongKeTongQuan(DateTime? startDate, DateTime? endDate);
        List<ThongKeMayTheoPhongDTO> GetThongKeMayTheoPhong();
        List<ThongKeLichDTO> GetThongKeLich(DateTime? startDate, DateTime? endDate);
    }

    public class BaoCaoThongKeService : IBaoCaoThongKeService
    {
        private readonly IBaoCaoThongKeRepository _repo;

        public BaoCaoThongKeService()
        {
            _repo = new BaoCaoThongKeRepository();
        }

        public ThongKeTongQuanDTO GetThongKeTongQuan(DateTime? startDate, DateTime? endDate)
        {
            return _repo.GetThongKeTongQuan(startDate, endDate);
        }

        public List<ThongKeMayTheoPhongDTO> GetThongKeMayTheoPhong()
        {
            return _repo.GetThongKeMayTheoPhong();
        }

        public List<ThongKeLichDTO> GetThongKeLich(DateTime? startDate, DateTime? endDate)
        {
            return _repo.GetThongKeLich(startDate, endDate);
        }
    }
}
