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
        void CreateLopHoc(string tenLop);
        void CreateMonHoc(string tenMon);
        void UpdateLopHoc(int maLop, string tenLop);
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
                return db.Query<LopHocDTO>("SELECT MaLop, TenLop FROM LOP_HOC ORDER BY TenLop");
            }
        }

        public IEnumerable<MonHocDTO> GetAllMonHoc()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<MonHocDTO>("SELECT MaMon, TenMon FROM MON_HOC ORDER BY TenMon");
            }
        }

        public void CreateLopHoc(string tenLop)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO LOP_HOC (TenLop, SiSo) VALUES (@tenLop, 30)", new { tenLop });
            }
        }

        public void CreateMonHoc(string tenMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO MON_HOC (TenMon) VALUES (@tenMon)", new { tenMon });
            }
        }

        public void UpdateLopHoc(int maLop, string tenLop)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE LOP_HOC SET TenLop=@tenLop WHERE MaLop=@maLop", new { tenLop, maLop });
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

