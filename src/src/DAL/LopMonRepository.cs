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
        void CreateLopHoc(string maLopHocPhan, string tenLop, int siSo, int? maMon);
        void CreateMonHoc(string maHocPhan, string tenMon);
        void UpdateLopHoc(int maLop, string maLopHocPhan, string tenLop, int siSo, int? maMon);
        void UpdateMonHoc(int maMon, string maHocPhan, string tenMon);
        void DeleteLopHoc(int maLop);
        void DeleteMonHoc(int maMon);

        // Kiem tra con lich hien tai/tuong lai chua huy
        bool HasActiveOrFutureSchedule_Lop(int maLop);
        bool HasActiveOrFutureSchedule_Mon(int maMon);

        // Xoa cascade (lich qua khu/da huy + lop/mon)
        void DeleteLopHocWithCascade(int maLop);
        void DeleteMonHocWithCascade(int maMon);
    }

    public class LopMonRepository : ILopMonRepository
    {
        public IEnumerable<LopHocDTO> GetAllLopHoc()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<LopHocDTO>(@"
                    SELECT l.MaLop, l.MaLopHocPhan, l.TenLop, l.SiSo, l.MaMon, m.TenMon 
                    FROM LOP_HOC l 
                    LEFT JOIN MON_HOC m ON l.MaMon = m.MaMon 
                    ORDER BY l.TenLop");
            }
        }

        public IEnumerable<MonHocDTO> GetAllMonHoc()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<MonHocDTO>("SELECT MaMon, MaHocPhan, TenMon FROM MON_HOC ORDER BY TenMon");
            }
        }

        public void CreateLopHoc(string maLopHocPhan, string tenLop, int siSo, int? maMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO LOP_HOC (MaLopHocPhan, TenLop, SiSo, MaMon) VALUES (@maLopHocPhan, @tenLop, @siSo, @maMon)", new { maLopHocPhan, tenLop, siSo, maMon });
            }
        }

        public void CreateMonHoc(string maHocPhan, string tenMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO MON_HOC (MaHocPhan, TenMon) VALUES (@maHocPhan, @tenMon)", new { maHocPhan, tenMon });
            }
        }

        public void UpdateLopHoc(int maLop, string maLopHocPhan, string tenLop, int siSo, int? maMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE LOP_HOC SET MaLopHocPhan=@maLopHocPhan, TenLop=@tenLop, SiSo=@siSo, MaMon=@maMon WHERE MaLop=@maLop", new { maLopHocPhan, tenLop, siSo, maMon, maLop });
            }
        }

        public void UpdateMonHoc(int maMon, string maHocPhan, string tenMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE MON_HOC SET MaHocPhan=@maHocPhan, TenMon=@tenMon WHERE MaMon=@maMon", new { maHocPhan, tenMon, maMon });
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

        // ── Kiem tra con lich hien tai / tuong lai chua huy ──────────────
        public bool HasActiveOrFutureSchedule_Lop(int maLop)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                // Chi chan neu con lich CHUA HUY va CHUA QUA
                int count = db.ExecuteScalar<int>(@"
                    SELECT COUNT(*) FROM LICH_THUC_HANH l
                    JOIN CA_HOC c ON l.MaCa = c.MaCa
                    WHERE l.MaLop = @maLop
                      AND l.TrangThaiLich NOT IN (N'Da huy', N'Khong duoc xep')
                      AND l.TrangThaiLich NOT IN (N'Đã hủy', N'Không được xếp')
                      AND (
                            l.NgayThucHanh > CAST(GETDATE() AS DATE)
                            OR (l.NgayThucHanh = CAST(GETDATE() AS DATE)
                                AND c.GioKetThuc >= CAST(GETDATE() AS TIME))
                          )",
                    new { maLop });
                return count > 0;
            }
        }

        public bool HasActiveOrFutureSchedule_Mon(int maMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                int count = db.ExecuteScalar<int>(@"
                    SELECT COUNT(*) FROM LICH_THUC_HANH l
                    JOIN CA_HOC c ON l.MaCa = c.MaCa
                    WHERE l.MaMon = @maMon
                      AND l.TrangThaiLich NOT IN (N'Da huy', N'Khong duoc xep')
                      AND l.TrangThaiLich NOT IN (N'Đã hủy', N'Không được xếp')
                      AND (
                            l.NgayThucHanh > CAST(GETDATE() AS DATE)
                            OR (l.NgayThucHanh = CAST(GETDATE() AS DATE)
                                AND c.GioKetThuc >= CAST(GETDATE() AS TIME))
                          )",
                    new { maMon });
                return count > 0;
            }
        }

        // ── Cascade xoa (lich qua khu / da huy) roi xoa lop/mon ─────────
        public void DeleteLopHocWithCascade(int maLop)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    // 1. Xoa YEU_CAU_CAU_HINH cua cac lich thuoc lop
                    conn.Execute(
                        "DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLop = @maLop)",
                        new { maLop }, trans);

                    // 2. Xoa PHAN_CONG_PHONG cua cac lich thuoc lop
                    conn.Execute(
                        "DELETE FROM PHAN_CONG_PHONG WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLop = @maLop)",
                        new { maLop }, trans);

                    // 3. Xoa cac lich cua lop (tat ca - qua khu, da huy)
                    conn.Execute(
                        "DELETE FROM LICH_THUC_HANH WHERE MaLop = @maLop",
                        new { maLop }, trans);

                    // 4. Xoa lop
                    conn.Execute(
                        "DELETE FROM LOP_HOC WHERE MaLop = @maLop",
                        new { maLop }, trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void DeleteMonHocWithCascade(int maMon)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    // 1. Xoa YEU_CAU_CAU_HINH cho lich truc tiep theo mon
                    conn.Execute(
                        "DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaMon = @maMon)",
                        new { maMon }, trans);

                    // 2. Xoa PHAN_CONG_PHONG cho lich theo mon
                    conn.Execute(
                        "DELETE FROM PHAN_CONG_PHONG WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaMon = @maMon)",
                        new { maMon }, trans);

                    // 3. Xoa lich truc tiep theo mon
                    conn.Execute(
                        "DELETE FROM LICH_THUC_HANH WHERE MaMon = @maMon",
                        new { maMon }, trans);

                    // 4. Xoa YEU_CAU_CAU_HINH cho lich thuoc cac lop cua mon
                    conn.Execute(
                        "DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLop IN (SELECT MaLop FROM LOP_HOC WHERE MaMon = @maMon))",
                        new { maMon }, trans);

                    // 5. Xoa PHAN_CONG_PHONG cho lich thuoc cac lop
                    conn.Execute(
                        "DELETE FROM PHAN_CONG_PHONG WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLop IN (SELECT MaLop FROM LOP_HOC WHERE MaMon = @maMon))",
                        new { maMon }, trans);

                    // 6. Xoa lich thuoc cac lop cua mon
                    conn.Execute(
                        "DELETE FROM LICH_THUC_HANH WHERE MaLop IN (SELECT MaLop FROM LOP_HOC WHERE MaMon = @maMon)",
                        new { maMon }, trans);

                    // 7. Xoa cac lop cua mon
                    conn.Execute(
                        "DELETE FROM LOP_HOC WHERE MaMon = @maMon",
                        new { maMon }, trans);

                    // 8. Xoa mon
                    conn.Execute(
                        "DELETE FROM MON_HOC WHERE MaMon = @maMon",
                        new { maMon }, trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}

