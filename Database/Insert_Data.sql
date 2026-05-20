USE QuanLyPhongMay;
GO

/* ============================================================
   DU LIEU MAU - QUAN LY PHONG MAY THUC HANH
   Chay file nay SAU KHI da chay Create_Table.sql
   ============================================================ */

/* ===== XOA DU LIEU CU (theo thu tu FK nguoc) ===== */
DELETE FROM CAP_NHAT_MAY;
DELETE FROM CAP_NHAT_PHONG;
DELETE FROM PHAN_CONG_PHONG;
DELETE FROM YEU_CAU_CAU_HINH;
DELETE FROM LICH_THUC_HANH;
DELETE FROM MAY_TINH;
DELETE FROM PHONG_MAY;
DELETE FROM NGUOI_DUNG;
DELETE FROM PHAN_QUYEN;
DELETE FROM LOP_HOC;
DELETE FROM MON_HOC;
DELETE FROM CA_HOC;
DELETE FROM CHUC_NANG;
DELETE FROM TRANG_THAI_MAY;
DELETE FROM TRANG_THAI_PHONG;
DELETE FROM VAI_TRO;
GO

/* ===== RESET IDENTITY SEED ===== */
DBCC CHECKIDENT ('VAI_TRO', RESEED, 0);
DBCC CHECKIDENT ('CHUC_NANG', RESEED, 0);
DBCC CHECKIDENT ('TRANG_THAI_PHONG', RESEED, 0);
DBCC CHECKIDENT ('TRANG_THAI_MAY', RESEED, 0);
DBCC CHECKIDENT ('NGUOI_DUNG', RESEED, 0);
DBCC CHECKIDENT ('PHONG_MAY', RESEED, 0);
DBCC CHECKIDENT ('MAY_TINH', RESEED, 0);
DBCC CHECKIDENT ('CA_HOC', RESEED, 0);
DBCC CHECKIDENT ('MON_HOC', RESEED, 0);
DBCC CHECKIDENT ('LOP_HOC', RESEED, 0);
DBCC CHECKIDENT ('LICH_THUC_HANH', RESEED, 0);
DBCC CHECKIDENT ('YEU_CAU_CAU_HINH', RESEED, 0);
DBCC CHECKIDENT ('PHAN_CONG_PHONG', RESEED, 0);
DBCC CHECKIDENT ('CAP_NHAT_PHONG', RESEED, 0);
DBCC CHECKIDENT ('CAP_NHAT_MAY', RESEED, 0);
GO

/* ----- 1. VAI TRO ----- */
SET IDENTITY_INSERT VAI_TRO ON;
INSERT INTO VAI_TRO (MaVaiTro, TenVaiTro, MoTa) VALUES
(1, N'Admin',    N'Quản trị viên hệ thống'),
(2, N'NhanVien', N'Nhân viên phòng máy');
SET IDENTITY_INSERT VAI_TRO OFF;
GO

/* ----- 2. CHUC NANG ----- */
SET IDENTITY_INSERT CHUC_NANG ON;
INSERT INTO CHUC_NANG (MaChucNang, TenChucNang, MoTa) VALUES
(1, N'Quản lý người dùng',     N'Thêm, sửa, xóa, khóa tài khoản'),
(2, N'Quản lý phòng máy',      N'Thêm, sửa, xóa phòng máy'),
(3, N'Quản lý máy tính',       N'Thêm, sửa, xóa, cập nhật trạng thái máy'),
(4, N'Quản lý lịch thực hành', N'Tạo, sửa, hủy lịch, gợi ý phòng'),
(5, N'Báo cáo & thống kê',     N'Xem biểu đồ, xuất báo cáo'),
(6, N'Quản lý danh mục',       N'Quản lý lớp, môn, ca học');
SET IDENTITY_INSERT CHUC_NANG OFF;
GO

/* ----- 3. PHAN QUYEN ----- */
-- Admin: full quyền tất cả chức năng
INSERT INTO PHAN_QUYEN (MaVaiTro, MaChucNang, Xem, Them, Sua, Xoa) VALUES
(1, 1, 1, 1, 1, 1),  -- QL Người dùng
(1, 2, 1, 1, 1, 1),  -- QL Phòng máy
(1, 3, 1, 1, 1, 1),  -- QL Máy tính
(1, 4, 1, 1, 1, 1),  -- QL Lịch TH
(1, 5, 1, 1, 1, 1),  -- Báo cáo
(1, 6, 1, 1, 1, 1);  -- QL Danh mục
GO

-- Nhân viên: hạn chế quyền
INSERT INTO PHAN_QUYEN (MaVaiTro, MaChucNang, Xem, Them, Sua, Xoa) VALUES
(2, 1, 0, 0, 0, 0),  -- Không quản lý người dùng
(2, 2, 1, 0, 0, 0),  -- Chỉ xem phòng máy
(2, 3, 1, 0, 1, 0),  -- Xem + sửa trạng thái máy (không thêm/xóa)
(2, 4, 1, 1, 1, 1),  -- Full quyền lịch TH
(2, 5, 0, 0, 0, 0),  -- Không xem báo cáo
(2, 6, 1, 1, 1, 1);  -- QL danh mục liên quan xếp lịch
GO

/* ----- 4. TRANG THAI PHONG ----- */
SET IDENTITY_INSERT TRANG_THAI_PHONG ON;
INSERT INTO TRANG_THAI_PHONG (MaTTPhong, TenTrangThaiPhong) VALUES
(1, N'Hoạt động'),
(2, N'Bảo trì'),
(3, N'Đóng cửa');
SET IDENTITY_INSERT TRANG_THAI_PHONG OFF;
GO

/* ----- 5. TRANG THAI MAY ----- */
SET IDENTITY_INSERT TRANG_THAI_MAY ON;
INSERT INTO TRANG_THAI_MAY (MaTTMay, TenTrangThaiMay) VALUES
(1, N'Tốt'),
(2, N'Bảo trì'),
(3, N'Hỏng');
SET IDENTITY_INSERT TRANG_THAI_MAY OFF;
GO

/* ----- 6. NGUOI DUNG -----
   Luu y: Mat khau tam thoi dung SHA256 hash cua "admin123" va "nv123"
   Khi tich hop BCrypt, can chay lai SeedInitialData() de cap nhat hash
*/
SET IDENTITY_INSERT NGUOI_DUNG ON;
INSERT INTO NGUOI_DUNG (MaNguoiDung, TenDangNhap, MatKhauDaMaHoa, HoTen, Email, SoDienThoai, TrangThai, MaVaiTro) VALUES
(1, 'admin',   '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 
    N'Nguyễn Văn Admin', 'admin@lab.edu.vn', '0901234567', 1, 1),
(2, 'nhanvien1', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
    N'Trần Thị Bình', 'binh@lab.edu.vn', '0912345678', 1, 2),
(3, 'nhanvien2', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
    N'Lê Hoàng Nam', 'nam@lab.edu.vn', '0923456789', 1, 2);
SET IDENTITY_INSERT NGUOI_DUNG OFF;
GO

/* ----- 7. PHONG MAY (6 phong) ----- */
SET IDENTITY_INSERT PHONG_MAY ON;
INSERT INTO PHONG_MAY (MaPhong, TenPhong, ViTri, SucChua, MoTa, MaTTPhong) VALUES
(1, N'P201', N'Tầng 2 - Tòa A', 40, N'Phòng máy cơ bản',          1),
(2, N'P202', N'Tầng 2 - Tòa A', 35, N'Phòng máy cơ bản',          1),
(3, N'P301', N'Tầng 3 - Tòa A', 40, N'Phòng Lab chuyên dụng',     1),
(4, N'P302', N'Tầng 3 - Tòa A', 45, N'Phòng đồ họa',             1),
(5, N'P401', N'Tầng 4 - Tòa B', 50, N'Phòng máy lớn',            1),
(6, N'P402', N'Tầng 4 - Tòa B', 30, N'Phòng máy nhỏ - đang sửa', 2);
SET IDENTITY_INSERT PHONG_MAY OFF;
GO

/* ----- 8. MAY TINH -----
   Tao may tinh cho moi phong, so luong = SucChua
   De ngan gon, chi tao 5 may mau cho moi phong (tong 30 may)
   Trong thuc te, SeedInitialData() se auto-generate day du
*/
SET IDENTITY_INSERT MAY_TINH ON;

-- Phong P201 (MaPhong=1): 5 may, cau hinh co ban
INSERT INTO MAY_TINH (MaMay, TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, GhiChu, ViTriMayTrongPhong, MaPhong, MaTTMay) VALUES
(1,  N'P201-01', N'Intel Core i5-12400',  8,  256, 24.0, NULL, N'Dãy 1-01', 1, 1),
(2,  N'P201-02', N'Intel Core i5-12400',  8,  256, 24.0, NULL, N'Dãy 1-02', 1, 1),
(3,  N'P201-03', N'Intel Core i5-12400',  8,  256, 24.0, NULL, N'Dãy 1-03', 1, 1),
(4,  N'P201-04', N'Intel Core i5-12400',  8,  256, 24.0, N'Lỗi bàn phím', N'Dãy 2-01', 1, 3),
(5,  N'P201-05', N'Intel Core i5-12400',  8,  256, 24.0, NULL, N'Dãy 2-02', 1, 1);

-- Phong P202 (MaPhong=2): 5 may
INSERT INTO MAY_TINH (MaMay, TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, GhiChu, ViTriMayTrongPhong, MaPhong, MaTTMay) VALUES
(6,  N'P202-01', N'Intel Core i5-12400',  8,  256, 24.0, NULL, N'Dãy 1-01', 2, 1),
(7,  N'P202-02', N'Intel Core i5-12400',  8,  256, 24.0, NULL, N'Dãy 1-02', 2, 1),
(8,  N'P202-03', N'Intel Core i5-12400',  8,  512, 24.0, NULL, N'Dãy 1-03', 2, 1),
(9,  N'P202-04', N'Intel Core i5-12400',  8,  256, 24.0, N'Đang bảo trì nguồn', N'Dãy 2-01', 2, 2),
(10, N'P202-05', N'Intel Core i5-12400',  8,  256, 24.0, NULL, N'Dãy 2-02', 2, 1);

-- Phong P301 (MaPhong=3): 5 may, cau hinh kha
INSERT INTO MAY_TINH (MaMay, TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, GhiChu, ViTriMayTrongPhong, MaPhong, MaTTMay) VALUES
(11, N'P301-01', N'Intel Core i7-12700',  16, 512, 27.0, NULL, N'Dãy 1-01', 3, 1),
(12, N'P301-02', N'Intel Core i7-12700',  16, 512, 27.0, NULL, N'Dãy 1-02', 3, 1),
(13, N'P301-03', N'Intel Core i7-12700',  16, 512, 27.0, NULL, N'Dãy 1-03', 3, 1),
(14, N'P301-04', N'Intel Core i7-12700',  16, 512, 27.0, NULL, N'Dãy 2-01', 3, 1),
(15, N'P301-05', N'Intel Core i7-12700',  16, 512, 27.0, N'Cháy nguồn', N'Dãy 2-02', 3, 3);

-- Phong P302 (MaPhong=4): 5 may, cau hinh cao (do hoa)
INSERT INTO MAY_TINH (MaMay, TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, GhiChu, ViTriMayTrongPhong, MaPhong, MaTTMay) VALUES
(16, N'P302-01', N'Intel Core i7-13700K', 32, 1024, 27.0, NULL, N'Dãy 1-01', 4, 1),
(17, N'P302-02', N'Intel Core i7-13700K', 32, 1024, 27.0, NULL, N'Dãy 1-02', 4, 1),
(18, N'P302-03', N'Intel Core i7-13700K', 32, 1024, 27.0, NULL, N'Dãy 1-03', 4, 1),
(19, N'P302-04', N'Intel Core i7-13700K', 32, 1024, 27.0, NULL, N'Dãy 2-01', 4, 1),
(20, N'P302-05', N'Intel Core i7-13700K', 32, 1024, 27.0, NULL, N'Dãy 2-02', 4, 1);

-- Phong P401 (MaPhong=5): 5 may
INSERT INTO MAY_TINH (MaMay, TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, GhiChu, ViTriMayTrongPhong, MaPhong, MaTTMay) VALUES
(21, N'P401-01', N'AMD Ryzen 5 5600X', 16, 512, 24.0, NULL, N'Dãy 1-01', 5, 1),
(22, N'P401-02', N'AMD Ryzen 5 5600X', 16, 512, 24.0, NULL, N'Dãy 1-02', 5, 1),
(23, N'P401-03', N'AMD Ryzen 5 5600X', 16, 512, 24.0, N'Lỗi màn hình', N'Dãy 1-03', 5, 3),
(24, N'P401-04', N'AMD Ryzen 5 5600X', 16, 512, 24.0, NULL, N'Dãy 2-01', 5, 1),
(25, N'P401-05', N'AMD Ryzen 5 5600X', 16, 512, 24.0, NULL, N'Dãy 2-02', 5, 1);

-- Phong P402 (MaPhong=6): 5 may (phong dang bao tri)
INSERT INTO MAY_TINH (MaMay, TenMay, CPU, RAM, DungLuongLuuTru, KichThuocManHinh, GhiChu, ViTriMayTrongPhong, MaPhong, MaTTMay) VALUES
(26, N'P402-01', N'Intel Core i5-10400', 8, 256, 22.0, N'Đang thay RAM', N'Dãy 1-01', 6, 2),
(27, N'P402-02', N'Intel Core i5-10400', 8, 256, 22.0, N'Đang thay RAM', N'Dãy 1-02', 6, 2),
(28, N'P402-03', N'Intel Core i5-10400', 8, 256, 22.0, NULL, N'Dãy 1-03', 6, 1),
(29, N'P402-04', N'Intel Core i5-10400', 8, 256, 22.0, NULL, N'Dãy 2-01', 6, 1),
(30, N'P402-05', N'Intel Core i5-10400', 8, 256, 22.0, N'Hỏng ổ cứng', N'Dãy 2-02', 6, 3);

SET IDENTITY_INSERT MAY_TINH OFF;
GO

/* ----- 9. CA HOC ----- */
SET IDENTITY_INSERT CA_HOC ON;
INSERT INTO CA_HOC (MaCa, TenCa, GioBatDau, GioKetThuc) VALUES
(1, N'Ca 1', '07:00', '09:15'),
(2, N'Ca 2', '09:30', '11:45'),
(3, N'Ca 3', '13:00', '15:15'),
(4, N'Ca 4', '15:30', '17:45');
SET IDENTITY_INSERT CA_HOC OFF;
GO

/* ----- 10. MON HOC ----- */
SET IDENTITY_INSERT MON_HOC ON;
INSERT INTO MON_HOC (MaMon, TenMon) VALUES
(1, N'Lập trình C#'),
(2, N'Lập trình Web'),
(3, N'Mạng máy tính'),
(4, N'Cơ sở dữ liệu'),
(5, N'Đồ họa máy tính'),
(6, N'An toàn thông tin'),
(7, N'Kiểm thử phần mềm'),
(8, N'Phân tích thiết kế HTTT'),
(9, N'Tin học đại cương'),
(10, N'Trí tuệ nhân tạo');
SET IDENTITY_INSERT MON_HOC OFF;
GO

/* ----- 11. LOP HOC ----- */
SET IDENTITY_INSERT LOP_HOC ON;
INSERT INTO LOP_HOC (MaLop, TenLop, SiSo) VALUES
(1,  N'CNTT01-K20', 40),
(2,  N'CNTT02-K20', 38),
(3,  N'KTPM01-K21', 35),
(4,  N'KTPM02-K21', 42),
(5,  N'HTTT01-K20', 30),
(6,  N'MMT01-K21',  36),
(7,  N'KHMT01-K22', 45),
(8,  N'CNTT03-K22', 40),
(9,  N'KTPM03-K22', 28),
(10, N'HTTT02-K21', 33);
SET IDENTITY_INSERT LOP_HOC OFF;
GO

/* ----- 12. LICH THUC HANH (du lieu mau) ----- */
SET IDENTITY_INSERT LICH_THUC_HANH ON;
INSERT INTO LICH_THUC_HANH (MaLich, NgayThucHanh, SoLuongSinhVien, TrangThaiLich, GhiChu, MaLop, MaMon, MaCa, NguoiTao) VALUES
(1, '2026-05-12', 40, N'Đã xếp phòng', NULL,                1, 1, 1, 1),  -- CNTT01, C#, Ca1
(2, '2026-05-12', 38, N'Đã xếp phòng', NULL,                2, 3, 2, 1),  -- CNTT02, Mạng, Ca2
(3, '2026-05-13', 35, N'Đã xếp phòng', NULL,                3, 7, 1, 2),  -- KTPM01, Kiểm thử, Ca1
(4, '2026-05-13', 30, N'Chờ xếp phòng', N'Cần phòng i7',   5, 8, 3, 1),  -- HTTT01, PTTKHTTT, Ca3
(5, '2026-05-14', 42, N'Chờ xếp phòng', N'Sĩ số lớn',      4, 2, 1, 2),  -- KTPM02, Web, Ca1
(6, '2026-05-14', 36, N'Đã xếp phòng', NULL,                6, 6, 2, 1),  -- MMT01, An toàn, Ca2
(7, '2026-05-15', 45, N'Chờ xếp phòng', N'Lớp đông nhất',  7, 9, 1, 1),  -- KHMT01, Tin đại cương, Ca1
(8, '2026-05-15', 40, N'Đã xếp phòng', NULL,                8, 4, 3, 1);  -- CNTT03, CSDL, Ca3
SET IDENTITY_INSERT LICH_THUC_HANH OFF;
GO

/* ----- 13. YEU CAU CAU HINH (cho cac lich can cau hinh dac biet) ----- */
INSERT INTO YEU_CAU_CAU_HINH (RAMToiThieu, CPUToiThieu, ManHinhToiThieu, LuuTruToiThieu, MaLich) VALUES
(16, N'i7', 27.0, 512, 4),    -- Lịch 4: PTTKHTTT cần i7, 16GB RAM
(8,  NULL,  NULL, 256, 5),     -- Lịch 5: Web cơ bản, cần 8GB
(NULL, NULL, NULL, NULL, 7);   -- Lịch 7: Tin đại cương, không yêu cầu đặc biệt
GO

/* ----- 14. PHAN CONG PHONG (lich da xep phong) ----- */
INSERT INTO PHAN_CONG_PHONG (MaLich, MaPhong, GhiChuXepPhong, MaNguoiDung) VALUES
(1, 1, N'Xếp tự động - phòng P201',  1),  -- Lịch 1 → P201
(2, 2, N'Xếp tự động - phòng P202',  1),  -- Lịch 2 → P202
(3, 3, N'Phòng Lab chuyên dụng',     2),  -- Lịch 3 → P301
(6, 5, N'Phòng lớn đủ chỗ',          2),  -- Lịch 6 → P401
(8, 4, N'Phòng đồ họa',              1);  -- Lịch 8 → P302
GO

PRINT N'✅ Insert dữ liệu mẫu thành công!';
PRINT N'   - 2 vai trò (Admin, Nhân viên)';
PRINT N'   - 6 chức năng + phân quyền';
PRINT N'   - 3 người dùng (1 admin + 2 nhân viên)';
PRINT N'   - 6 phòng máy (5 hoạt động + 1 bảo trì)';
PRINT N'   - 30 máy tính (25 tốt + 3 bảo trì + 2 hỏng)';
PRINT N'   - 4 ca học, 10 môn học, 10 lớp học';
PRINT N'   - 8 lịch thực hành (5 đã xếp + 3 chờ xếp)';
PRINT N'   - 3 yêu cầu cấu hình, 5 phân công phòng';
PRINT N'';
PRINT N'⚠ Mật khẩu tất cả tài khoản: admin123 (SHA256)';
PRINT N'   Cần cập nhật sang BCrypt khi tích hợp!';
GO
