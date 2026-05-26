using System.Collections.Generic;
using System.Data;
using Dapper;
using src.DTO;
using src.Helpers;

namespace src.DAL
{
    public interface ILopMonRepository
    {
        IEnumerable<LopHocDTO> GetAllLopHoc();
        IEnumerable<MonHocDTO> GetAllMonHoc();
        void CreateLopHoc(string tenLop, int siSo, int? maMon);
        void CreateMonHoc(string tenMon);
        void UpdateLopHoc(int maLop, string tenLop, int siSo, int? maMon);
        void UpdateMonHoc(int maMon, string tenMon);
        void DeleteLopHoc(int maLop);
        void DeleteMonHoc(int maMon);
    }

    public class LopMonRepository : ILopMonRepository
    {
        public IEnumerable<LopHocDTO> GetAllLopHoc()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<LopHocDTO>(@"
                    SELECT l.MaLop, l.TenLop, l.SiSo, l.MaMon, m.TenMon 
                    FROM LOP_HOC l 
                    LEFT JOIN MON_HOC m ON l.MaMon = m.MaMon 
                    ORDER BY l.TenLop");
            }
        }

        public IEnumerable<MonHocDTO> GetAllMonHoc()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<MonHocDTO>("SELECT MaMon, TenMon FROM MON_HOC ORDER BY TenMon");
            }
        }

        public void CreateLopHoc(string tenLop, int siSo, int? maMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO LOP_HOC (TenLop, SiSo, MaMon) VALUES (@tenLop, @siSo, @maMon)", new { tenLop, siSo, maMon });
            }
        }

        public void CreateMonHoc(string tenMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO MON_HOC (TenMon) VALUES (@tenMon)", new { tenMon });
            }
        }

        public void UpdateLopHoc(int maLop, string tenLop, int siSo, int? maMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE LOP_HOC SET TenLop=@tenLop, SiSo=@siSo, MaMon=@maMon WHERE MaLop=@maLop", new { tenLop, siSo, maMon, maLop });
            }
        }

        public void UpdateMonHoc(int maMon, string tenMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE MON_HOC SET TenMon=@tenMon WHERE MaMon=@maMon", new { tenMon, maMon });
            }
        }

        public void DeleteLopHoc(int maLop)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("DELETE FROM LOP_HOC WHERE MaLop=@maLop", new { maLop });
            }
        }

        public void DeleteMonHoc(int maMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("DELETE FROM MON_HOC WHERE MaMon=@maMon", new { maMon });
            }
        }
    }
}

