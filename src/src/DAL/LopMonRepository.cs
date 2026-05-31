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
        void CreateLopHoc(string maLopHocPhan, string tenLop, int siSo, string MaHocPhan);
        void CreateMonHoc(string MaHocPhan, string tenMon);
        void UpdateLopHoc(string oldMaLopHocPhan, string maLopHocPhan, string tenLop, int siSo, string MaHocPhan);
        void UpdateMonHoc(string oldMaHocPhan, string MaHocPhan, string tenMon);
        void DeleteLopHoc(string maLopHocPhan);
        void DeleteMonHoc(string MaHocPhan);

        // Kiem tra con lich hien tai/tuong lai chua huy
        bool HasActiveOrFutureSchedule_Lop(string maLopHocPhan);
        bool HasActiveOrFutureSchedule_Mon(string MaHocPhan);

        // Xoa cascade (lich qua khu/da huy + lop/mon)
        void DeleteLopHocWithCascade(string maLopHocPhan);
        void DeleteMonHocWithCascade(string MaHocPhan);
    }

    public class LopMonRepository : ILopMonRepository
    {
        public IEnumerable<LopHocDTO> GetAllLopHoc()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<LopHocDTO>(@"
                    SELECT l.MaLopHocPhan, l.TenLop, l.SiSo, l.MaHocPhan, m.TenMon 
                    FROM LOP_HOC l 
                    LEFT JOIN MON_HOC m ON l.MaHocPhan = m.MaHocPhan 
                    ORDER BY l.TenLop");
            }
        }

        public IEnumerable<MonHocDTO> GetAllMonHoc()
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<MonHocDTO>("SELECT MaHocPhan, TenMon FROM MON_HOC ORDER BY TenMon");
            }
        }

        public void CreateLopHoc(string maLopHocPhan, string tenLop, int siSo, string MaHocPhan)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO LOP_HOC (MaLopHocPhan, TenLop, SiSo, MaHocPhan) VALUES (@maLopHocPhan, @tenLop, @siSo, @MaHocPhan)", new { maLopHocPhan, tenLop, siSo, MaHocPhan });
            }
        }

        public void CreateMonHoc(string MaHocPhan, string tenMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("INSERT INTO MON_HOC (MaHocPhan, TenMon) VALUES (@MaHocPhan, @tenMon)", new { MaHocPhan, tenMon });
            }
        }

        public void UpdateLopHoc(string oldMaLopHocPhan, string maLopHocPhan, string tenLop, int siSo, string MaHocPhan)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE LOP_HOC SET MaLopHocPhan=@maLopHocPhan, TenLop=@tenLop, SiSo=@siSo, MaHocPhan=@MaHocPhan WHERE MaLopHocPhan=@oldMaLopHocPhan", new { maLopHocPhan, tenLop, siSo, MaHocPhan, oldMaLopHocPhan });
            }
        }

        public void UpdateMonHoc(string oldMaHocPhan, string MaHocPhan, string tenMon)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("UPDATE MON_HOC SET MaHocPhan=@MaHocPhan, TenMon=@tenMon WHERE MaHocPhan=@oldMaHocPhan", new { MaHocPhan, tenMon, oldMaHocPhan });
            }
        }

        public void DeleteLopHoc(string maLopHocPhan)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("DELETE FROM LOP_HOC WHERE MaLopHocPhan=@maLopHocPhan", new { maLopHocPhan });
            }
        }

        public void DeleteMonHoc(string MaHocPhan)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("DELETE FROM MON_HOC WHERE MaHocPhan=@MaHocPhan", new { MaHocPhan });
            }
        }

        // ── Kiem tra con lich hien tai / tuong lai chua huy ──────────────
        public bool HasActiveOrFutureSchedule_Lop(string maLopHocPhan)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                // Chi chan neu con lich CHUA HUY va CHUA QUA
                int count = db.ExecuteScalar<int>(@"
                    SELECT COUNT(*) FROM LICH_THUC_HANH l
                    JOIN CA_HOC c ON l.MaCa = c.MaCa
                    WHERE l.MaLopHocPhan = @maLopHocPhan
                      AND l.TrangThaiLich NOT IN (N'Da huy', N'Khong duoc xep')
                      AND l.TrangThaiLich NOT IN (N'Đã hủy', N'Không được xếp')
                      AND (
                            l.NgayThucHanh > CAST(GETDATE() AS DATE)
                            OR (l.NgayThucHanh = CAST(GETDATE() AS DATE)
                                AND c.GioKetThuc >= CAST(GETDATE() AS TIME))
                          )",
                    new { maLopHocPhan });
                return count > 0;
            }
        }

        public bool HasActiveOrFutureSchedule_Mon(string MaHocPhan)
        {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                int count = db.ExecuteScalar<int>(@"
                    SELECT COUNT(*) FROM LICH_THUC_HANH l
                    JOIN CA_HOC c ON l.MaCa = c.MaCa
                    WHERE l.MaHocPhan = @MaHocPhan
                      AND l.TrangThaiLich NOT IN (N'Da huy', N'Khong duoc xep')
                      AND l.TrangThaiLich NOT IN (N'Đã hủy', N'Không được xếp')
                      AND (
                            l.NgayThucHanh > CAST(GETDATE() AS DATE)
                            OR (l.NgayThucHanh = CAST(GETDATE() AS DATE)
                                AND c.GioKetThuc >= CAST(GETDATE() AS TIME))
                          )",
                    new { MaHocPhan });
                return count > 0;
            }
        }

        // ── Cascade xoa (lich qua khu / da huy) roi xoa lop/mon ─────────
        public void DeleteLopHocWithCascade(string maLopHocPhan)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    // 1. Xoa YEU_CAU_CAU_HINH cua cac lich thuoc lop
                    conn.Execute(
                        "DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLopHocPhan = @maLopHocPhan)",
                        new { maLopHocPhan }, trans);

                    // 2. Xoa PHAN_CONG_PHONG cua cac lich thuoc lop
                    conn.Execute(
                        "DELETE FROM PHAN_CONG_PHONG WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLopHocPhan = @maLopHocPhan)",
                        new { maLopHocPhan }, trans);

                    // 3. Xoa cac lich cua lop (tat ca - qua khu, da huy)
                    conn.Execute(
                        "DELETE FROM LICH_THUC_HANH WHERE MaLopHocPhan = @maLopHocPhan",
                        new { maLopHocPhan }, trans);

                    // 4. Xoa lop
                    conn.Execute(
                        "DELETE FROM LOP_HOC WHERE MaLopHocPhan = @maLopHocPhan",
                        new { maLopHocPhan }, trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void DeleteMonHocWithCascade(string MaHocPhan)
        {
            using (var conn = DatabaseHelper.GetConnection() as Microsoft.Data.SqlClient.SqlConnection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    // 1. Xoa YEU_CAU_CAU_HINH cho lich truc tiep theo mon
                    conn.Execute(
                        "DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaHocPhan = @MaHocPhan)",
                        new { MaHocPhan }, trans);

                    // 2. Xoa PHAN_CONG_PHONG cho lich theo mon
                    conn.Execute(
                        "DELETE FROM PHAN_CONG_PHONG WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaHocPhan = @MaHocPhan)",
                        new { MaHocPhan }, trans);

                    // 3. Xoa lich truc tiep theo mon
                    conn.Execute(
                        "DELETE FROM LICH_THUC_HANH WHERE MaHocPhan = @MaHocPhan",
                        new { MaHocPhan }, trans);

                    // 4. Xoa YEU_CAU_CAU_HINH cho lich thuoc cac lop cua mon
                    conn.Execute(
                        "DELETE FROM YEU_CAU_CAU_HINH WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLopHocPhan IN (SELECT MaLopHocPhan FROM LOP_HOC WHERE MaHocPhan = @MaHocPhan))",
                        new { MaHocPhan }, trans);

                    // 5. Xoa PHAN_CONG_PHONG cho lich thuoc cac lop
                    conn.Execute(
                        "DELETE FROM PHAN_CONG_PHONG WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE MaLopHocPhan IN (SELECT MaLopHocPhan FROM LOP_HOC WHERE MaHocPhan = @MaHocPhan))",
                        new { MaHocPhan }, trans);

                    // 6. Xoa lich thuoc cac lop cua mon
                    conn.Execute(
                        "DELETE FROM LICH_THUC_HANH WHERE MaLopHocPhan IN (SELECT MaLopHocPhan FROM LOP_HOC WHERE MaHocPhan = @MaHocPhan)",
                        new { MaHocPhan }, trans);

                    // 7. Xoa cac lop cua mon
                    conn.Execute(
                        "DELETE FROM LOP_HOC WHERE MaHocPhan = @MaHocPhan",
                        new { MaHocPhan }, trans);

                    // 8. Xoa mon
                    conn.Execute(
                        "DELETE FROM MON_HOC WHERE MaHocPhan = @MaHocPhan",
                        new { MaHocPhan }, trans);

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

