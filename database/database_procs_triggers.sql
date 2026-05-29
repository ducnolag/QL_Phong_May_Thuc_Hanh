-- ============================================================
--  FILE: database_procs_triggers.sql
--  Dự án: Quản Lý Phòng Máy Thực Hành
--  Mô tả: Stored Procedures và Triggers bổ sung cho database
--
--  SCHEMA THỰC TẾ (đã kiểm tra):
--  - MAY_TINH     : có UpdatedAt (datetime)
--  - PHONG_MAY    : có UpdatedAt (datetime)
--  - NGUOI_DUNG   : KHÔNG có UpdatedAt (chỉ có CreatedAt) => bỏ trigger UpdatedAt
--  - CAP_NHAT_MAY : cột gồm MaCapNhatMay, MaNguoiDung, MaMay,
--                   HanhDong (nvarchar), ThoiGianCapNhat (datetime2)
--                   => KHÔNG có MaTTMay hay NgayCapNhat
--  - CAP_NHAT_PHONG: cột gồm MaCapNhatPhong, MaNguoiDung, MaPhong,
--                   HanhDong (nvarchar), ThoiGianCapNhat (datetime2)
--                   => KHÔNG có MaTTPhong hay NgayCapNhat
--
--  TRIGGERS TẠO:
--  1) trg_UpdatedAt_MayTinh   - tự cập nhật UpdatedAt cho MAY_TINH
--  2) trg_UpdatedAt_PhongMay  - tự cập nhật UpdatedAt cho PHONG_MAY
--  3) trg_LogTrangThaiMayTinh - log đổi trạng thái máy -> CAP_NHAT_MAY
--  4) trg_LogTrangThaiPhongMay- log đổi trạng thái phòng -> CAP_NHAT_PHONG
--
--  STORED PROCEDURES TẠO:
--  1) sp_CapNhatChotSoLieu  - snapshot báo cáo theo ngày
--  2) sp_AutoUpdateLichCu  - đóng lịch quá hạn
--  3) sp_XoaPhongMay       - xóa phòng + cascade an toàn
--  4) sp_XoaNguoiDung      - xóa user + cascade an toàn
--  5) sp_TimPhongChoLich   - tìm phòng phù hợp cho lịch
--  6) sp_ThongKeTongQuan   - thống kê tổng hợp
-- ============================================================

USE QuanLyPhongMay;
GO

-- ============================================================
-- PHẦN 1: TRIGGERS
-- ============================================================

-- ------------------------------------------------------------
-- TRIGGER 1: Tự cập nhật UpdatedAt cho bảng MAY_TINH
-- Ghi chú: MayTinhRepository.UpdateComputer() đã set UpdatedAt=GETDATE()
-- trong câu UPDATE. Trigger này sẽ đảm bảo UpdatedAt luôn được cập nhật
-- ngay cả khi có câu UPDATE nào khác không set tay.
-- KHÔNG gây lỗi vì trigger chỉ ghi đè UpdatedAt => idempotent.
-- ------------------------------------------------------------
IF OBJECT_ID('trg_UpdatedAt_MayTinh', 'TR') IS NOT NULL
    DROP TRIGGER trg_UpdatedAt_MayTinh;
GO

CREATE TRIGGER trg_UpdatedAt_MayTinh
ON MAY_TINH
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    -- Chỉ cập nhật nếu UpdatedAt chưa được set bởi câu lệnh UPDATE
    -- (tránh vòng lặp trigger gọi trigger)
    IF NOT UPDATE(UpdatedAt)
    BEGIN
        UPDATE MAY_TINH
        SET UpdatedAt = GETDATE()
        WHERE MaMay IN (SELECT MaMay FROM inserted);
    END
END;
GO

-- ------------------------------------------------------------
-- TRIGGER 2: Tự cập nhật UpdatedAt cho bảng PHONG_MAY
-- Ghi chú: PhongMayRepository không có SET UpdatedAt trong UPDATE
-- => Trigger này sẽ tự động fill khi có bất kỳ UPDATE nào
-- ------------------------------------------------------------
IF OBJECT_ID('trg_UpdatedAt_PhongMay', 'TR') IS NOT NULL
    DROP TRIGGER trg_UpdatedAt_PhongMay;
GO

CREATE TRIGGER trg_UpdatedAt_PhongMay
ON PHONG_MAY
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT UPDATE(UpdatedAt)
    BEGIN
        UPDATE PHONG_MAY
        SET UpdatedAt = GETDATE()
        WHERE MaPhong IN (SELECT MaPhong FROM inserted);
    END
END;
GO

-- ------------------------------------------------------------
-- TRIGGER 3: Ghi log thay đổi trạng thái máy tính vào CAP_NHAT_MAY
-- Schema thực tế của CAP_NHAT_MAY:
--   MaCapNhatMay (bigint), MaNguoiDung (int), MaMay (int),
--   HanhDong (nvarchar), ThoiGianCapNhat (datetime2)
-- Ghi chú: Không có cột MaTTMay hay NgayCapNhat
-- => Ghi nội dung thay đổi vào cột HanhDong dạng text
-- ------------------------------------------------------------
IF OBJECT_ID('trg_LogTrangThaiMayTinh', 'TR') IS NOT NULL
    DROP TRIGGER trg_LogTrangThaiMayTinh;
GO

CREATE TRIGGER trg_LogTrangThaiMayTinh
ON MAY_TINH
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    -- Chỉ ghi log khi trạng thái thực sự thay đổi
    IF UPDATE(MaTTMay)
    BEGIN
        INSERT INTO CAP_NHAT_MAY (MaNguoiDung, MaMay, HanhDong, ThoiGianCapNhat)
        SELECT
            NULL,   -- system trigger, không biết user đang login
            i.MaMay,
            N'[Trigger] Đổi trạng thái: ' +
                CAST(d.MaTTMay AS NVARCHAR(10)) + N' -> ' +
                CAST(i.MaTTMay AS NVARCHAR(10)),
            SYSDATETIME()
        FROM inserted i
        INNER JOIN deleted d ON i.MaMay = d.MaMay
        WHERE i.MaTTMay <> d.MaTTMay;  -- chỉ khi TT thực sự đổi
    END
END;
GO

-- ------------------------------------------------------------
-- TRIGGER 4: Ghi log thay đổi trạng thái phòng vào CAP_NHAT_PHONG
-- Schema thực tế của CAP_NHAT_PHONG:
--   MaCapNhatPhong (bigint), MaNguoiDung (int), MaPhong (int),
--   HanhDong (nvarchar), ThoiGianCapNhat (datetime2)
-- ------------------------------------------------------------
IF OBJECT_ID('trg_LogTrangThaiPhongMay', 'TR') IS NOT NULL
    DROP TRIGGER trg_LogTrangThaiPhongMay;
GO

CREATE TRIGGER trg_LogTrangThaiPhongMay
ON PHONG_MAY
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(MaTTPhong)
    BEGIN
        INSERT INTO CAP_NHAT_PHONG (MaNguoiDung, MaPhong, HanhDong, ThoiGianCapNhat)
        SELECT
            NULL,   -- system trigger
            i.MaPhong,
            N'[Trigger] Đổi trạng thái: ' +
                CAST(d.MaTTPhong AS NVARCHAR(10)) + N' -> ' +
                CAST(i.MaTTPhong AS NVARCHAR(10)),
            SYSDATETIME()
        FROM inserted i
        INNER JOIN deleted d ON i.MaPhong = d.MaPhong
        WHERE i.MaTTPhong <> d.MaTTPhong;
    END
END;
GO


-- ============================================================
-- PHẦN 2: STORED PROCEDURES
-- ============================================================

-- ------------------------------------------------------------
-- SP 1: sp_CapNhatChotSoLieu
-- Mục đích: Gom logic snapshot CHOT_SO_LIEU hiện đang bị lặp
--   ở 2 nơi trong C#:
--   1) DatabaseHelper.SeedInitialData() (dòng 226-262)
--   2) BaoCaoThongKeRepository.GetThongKeTongQuan() (dòng 32-44)
-- Ghi chú: C# vẫn chạy bình thường; SP này bổ sung, không thay thế.
--   Sau khi test OK, có thể gọi SP từ C# thay vì inline SQL.
-- ------------------------------------------------------------
IF OBJECT_ID('sp_CapNhatChotSoLieu', 'P') IS NOT NULL
    DROP PROCEDURE sp_CapNhatChotSoLieu;
GO

CREATE PROCEDURE sp_CapNhatChotSoLieu
    @NgayChot DATE = NULL   -- NULL = hôm nay
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @NgayChot IS NULL
        SET @NgayChot = CAST(GETDATE() AS DATE);

    -- Xóa snapshot cũ của ngày này để tính lại
    DELETE FROM CHOT_SO_LIEU WHERE NgayChot = @NgayChot;

    -- Insert snapshot mới
    INSERT INTO CHOT_SO_LIEU (NgayChot, TotalRooms, ActiveRooms, TotalMay, MayTot, MayHong, TotalUsers)
    SELECT
        @NgayChot,
        (SELECT COUNT(*) FROM PHONG_MAY),
        (SELECT COUNT(*) FROM PHONG_MAY p
            JOIN TRANG_THAI_PHONG t ON p.MaTTPhong = t.MaTTPhong
            WHERE t.TenTrangThaiPhong = N'Hoạt động'),
        (SELECT COUNT(*) FROM MAY_TINH),
        (SELECT COUNT(*) FROM MAY_TINH m
            JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
            WHERE t.TenTrangThaiMay = N'Tốt'),
        (SELECT COUNT(*) FROM MAY_TINH m
            JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
            WHERE t.TenTrangThaiMay = N'Hỏng'),
        (SELECT COUNT(*) FROM NGUOI_DUNG);

    -- Backfill lịch sử còn thiếu
    INSERT INTO CHOT_SO_LIEU (NgayChot, TotalRooms, ActiveRooms, TotalMay, MayTot, MayHong, TotalUsers)
    SELECT DISTINCT d, 0, 0, 0, 0, 0, 0
    FROM (
        SELECT CAST(CreatedAt AS DATE) AS d FROM NGUOI_DUNG
        UNION SELECT CAST(CreatedAt AS DATE) FROM PHONG_MAY
        UNION SELECT CAST(CreatedAt AS DATE) FROM MAY_TINH
    ) dates
    WHERE d < @NgayChot AND d NOT IN (SELECT NgayChot FROM CHOT_SO_LIEU);

    -- Cập nhật running total cho các ngày backfill (có giá trị 0)
    UPDATE C
    SET
        TotalRooms  = (SELECT COUNT(*) FROM PHONG_MAY WHERE CAST(CreatedAt AS DATE) <= C.NgayChot),
        ActiveRooms = (SELECT COUNT(*) FROM PHONG_MAY p
                           JOIN TRANG_THAI_PHONG t ON p.MaTTPhong = t.MaTTPhong
                           WHERE t.TenTrangThaiPhong = N'Hoạt động'
                             AND CAST(p.CreatedAt AS DATE) <= C.NgayChot),
        TotalMay    = (SELECT COUNT(*) FROM MAY_TINH WHERE CAST(CreatedAt AS DATE) <= C.NgayChot),
        MayTot      = (SELECT COUNT(*) FROM MAY_TINH m
                           JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                           WHERE t.TenTrangThaiMay = N'Tốt'
                             AND CAST(m.CreatedAt AS DATE) <= C.NgayChot),
        MayHong     = (SELECT COUNT(*) FROM MAY_TINH m
                           JOIN TRANG_THAI_MAY t ON m.MaTTMay = t.MaTTMay
                           WHERE t.TenTrangThaiMay = N'Hỏng'
                             AND CAST(m.CreatedAt AS DATE) <= C.NgayChot),
        TotalUsers  = (SELECT COUNT(*) FROM NGUOI_DUNG WHERE CAST(CreatedAt AS DATE) <= C.NgayChot)
    FROM CHOT_SO_LIEU C
    WHERE NgayChot < @NgayChot
      AND TotalUsers = 0 AND TotalRooms = 0;

    PRINT N'✓ CHOT_SO_LIEU đã được cập nhật cho ngày: ' + CAST(@NgayChot AS NVARCHAR(20));
END;
GO

-- ------------------------------------------------------------
-- SP 2: sp_AutoUpdateLichCu
-- Mục đích: Đóng gói logic tự động cập nhật lịch quá hạn
--   hiện đang dùng inline SQL trong:
--   LichThucHanhRepository.AutoUpdateOldSchedules() (dòng 50-55)
-- Ghi chú: C# vẫn chạy bình thường. SP này chuẩn bị sẵn
--   để có thể gọi từ SQL Agent Job (chạy hàng đêm).
-- ------------------------------------------------------------
IF OBJECT_ID('sp_AutoUpdateLichCu', 'P') IS NOT NULL
    DROP PROCEDURE sp_AutoUpdateLichCu;
GO

CREATE PROCEDURE sp_AutoUpdateLichCu
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SoLichCapNhat INT;

    UPDATE LICH_THUC_HANH
    SET TrangThaiLich = N'Không được xếp'
    WHERE TrangThaiLich = N'Chờ xếp phòng'
      AND (
            NgayThucHanh < CAST(GETDATE() AS DATE)
            OR (
                NgayThucHanh = CAST(GETDATE() AS DATE)
                AND MaCa IN (
                    SELECT MaCa FROM CA_HOC
                    WHERE GioKetThuc < CAST(GETDATE() AS TIME)
                )
            )
          );

    SET @SoLichCapNhat = @@ROWCOUNT;
    PRINT N'✓ Đã cập nhật ' + CAST(@SoLichCapNhat AS NVARCHAR(10)) + N' lịch quá hạn thành "Không được xếp"';
END;
GO

-- ------------------------------------------------------------
-- SP 3: sp_XoaPhongMay
-- Mục đích: Xóa phòng + máy tính + log liên quan an toàn
--   Tương đương với PhongMayRepository.DeleteRoomWithTransaction()
--   nhưng chạy hoàn toàn trong DB (transaction phía DB)
-- Ghi chú: C# vẫn dùng transaction của riêng nó => KHÔNG xung đột.
--   SP này độc lập, dùng khi muốn gọi thẳng từ SQL Management Studio.
-- OUTPUT: @KetQua = 1 thành công, 0 thất bại
-- ------------------------------------------------------------
IF OBJECT_ID('sp_XoaPhongMay', 'P') IS NOT NULL
    DROP PROCEDURE sp_XoaPhongMay;
GO

CREATE PROCEDURE sp_XoaPhongMay
    @MaPhong    INT,
    @KetQua     BIT OUTPUT,
    @ThongBao   NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @KetQua   = 0;
    SET @ThongBao = N'';

    -- Kiểm tra phòng có lịch hiện tại/tương lai không (giống BLL PhongMayService.DeleteRoom)
    DECLARE @SoLich INT;
    SELECT @SoLich = COUNT(*)
    FROM PHAN_CONG_PHONG pc
    JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
    JOIN CA_HOC c ON l.MaCa = c.MaCa
    WHERE pc.MaPhong = @MaPhong
      AND l.TrangThaiLich NOT IN (N'Đã hủy', N'Không được xếp')
      AND (
            l.NgayThucHanh > CAST(GETDATE() AS DATE)
            OR (l.NgayThucHanh = CAST(GETDATE() AS DATE)
                AND c.GioKetThuc >= CAST(GETDATE() AS TIME))
          );

    IF @SoLich > 0
    BEGIN
        SET @ThongBao = N'Phòng đang có lịch thực hành trong hiện tại hoặc tương lai, không thể xóa!';
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

            -- 1. Xóa phân công phòng
            DELETE FROM PHAN_CONG_PHONG WHERE MaPhong = @MaPhong;

            -- 2. Xóa log cập nhật phòng
            DELETE FROM CAP_NHAT_PHONG WHERE MaPhong = @MaPhong;

            -- 3. Xóa log máy thuộc phòng
            DELETE FROM CAP_NHAT_MAY
            WHERE MaMay IN (SELECT MaMay FROM MAY_TINH WHERE MaPhong = @MaPhong);

            -- 4. Xóa máy tính thuộc phòng
            DELETE FROM MAY_TINH WHERE MaPhong = @MaPhong;

            -- 5. Xóa phòng
            DELETE FROM PHONG_MAY WHERE MaPhong = @MaPhong;

        COMMIT TRANSACTION;
        SET @KetQua   = 1;
        SET @ThongBao = N'Đã xóa phòng thành công!';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @KetQua   = 0;
        SET @ThongBao = N'Lỗi xóa phòng: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO

-- ------------------------------------------------------------
-- SP 4: sp_XoaNguoiDung
-- Mục đích: Xóa user + toàn bộ dữ liệu liên quan an toàn
--   Tương đương NguoiDungRepository.DeleteUserAndRelatedData()
-- Ghi chú: C# vẫn dùng transaction của riêng nó => KHÔNG xung đột.
--   SP này dùng độc lập khi cần admin query thẳng vào DB.
-- OUTPUT: @KetQua = 1 thành công, 0 thất bại
-- ------------------------------------------------------------
IF OBJECT_ID('sp_XoaNguoiDung', 'P') IS NOT NULL
    DROP PROCEDURE sp_XoaNguoiDung;
GO

CREATE PROCEDURE sp_XoaNguoiDung
    @MaNguoiDung INT,
    @KetQua      BIT OUTPUT,
    @ThongBao    NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @KetQua   = 0;
    SET @ThongBao = N'';

    -- Kiểm tra không được xóa admin
    IF EXISTS (SELECT 1 FROM NGUOI_DUNG WHERE MaNguoiDung = @MaNguoiDung AND TenDangNhap = 'admin')
    BEGIN
        SET @ThongBao = N'Không thể xóa tài khoản admin!';
        RETURN;
    END

    -- Kiểm tra user tồn tại
    IF NOT EXISTS (SELECT 1 FROM NGUOI_DUNG WHERE MaNguoiDung = @MaNguoiDung)
    BEGIN
        SET @ThongBao = N'Tài khoản không tồn tại!';
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

            -- Xóa theo thứ tự phụ thuộc FK (giống C# DeleteUserAndRelatedData)
            DELETE FROM YEU_CAU_CAU_HINH
            WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE NguoiTao = @MaNguoiDung);

            DELETE FROM PHAN_CONG_PHONG
            WHERE MaLich IN (SELECT MaLich FROM LICH_THUC_HANH WHERE NguoiTao = @MaNguoiDung);

            DELETE FROM PHAN_CONG_PHONG WHERE MaNguoiDung = @MaNguoiDung;

            DELETE FROM LICH_THUC_HANH WHERE NguoiTao = @MaNguoiDung;

            DELETE FROM CAP_NHAT_PHONG WHERE MaNguoiDung = @MaNguoiDung;

            DELETE FROM CAP_NHAT_MAY WHERE MaNguoiDung = @MaNguoiDung;

            DELETE FROM NGUOI_DUNG WHERE MaNguoiDung = @MaNguoiDung;

        COMMIT TRANSACTION;
        SET @KetQua   = 1;
        SET @ThongBao = N'Đã xóa tài khoản thành công!';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @KetQua   = 0;
        SET @ThongBao = N'Lỗi xóa tài khoản: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO

-- ------------------------------------------------------------
-- SP 5: sp_TimPhongChoLich
-- Mục đích: Tìm phòng phù hợp để xếp lịch thực hành
--   Gom logic phức tạp từ LichThucHanhRepository.GetRoomsForAssignment()
-- Ghi chú: C# vẫn dùng inline SQL. SP này song song, dùng để test.
-- ------------------------------------------------------------
IF OBJECT_ID('sp_TimPhongChoLich', 'P') IS NOT NULL
    DROP PROCEDURE sp_TimPhongChoLich;
GO

CREATE PROCEDURE sp_TimPhongChoLich
    @SoSinhVien      INT,
    @ReqRAM          INT,
    @ReqLuuTru       INT,
    @ReqManHinh      INT,
    @ReqCPU          NVARCHAR(100),
    @NgayThucHanh    DATE,
    @MaCa            INT,
    @MaLichHienTai   INT = 0   -- để loại trừ lịch hiện tại khi edit
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.MaPhong,
        p.TenPhong,
        p.SucChua,
        (
            SELECT COUNT(*)
            FROM MAY_TINH m
            JOIN TRANG_THAI_MAY tm ON m.MaTTMay = tm.MaTTMay
            WHERE m.MaPhong = p.MaPhong
              AND tm.TenTrangThaiMay = N'Tốt'
              AND m.RAM              >= @ReqRAM
              AND m.DungLuongLuuTru  >= @ReqLuuTru
              AND ISNULL(m.KichThuocManHinh, 0) >= @ReqManHinh
              AND (@ReqCPU = '' OR ISNULL(m.CPU, '') = @ReqCPU)
        ) AS MayTot
    FROM PHONG_MAY p
    JOIN TRANG_THAI_PHONG ttp ON p.MaTTPhong = ttp.MaTTPhong
    WHERE ttp.TenTrangThaiPhong = N'Hoạt động'
      AND p.SucChua >= @SoSinhVien
      AND NOT EXISTS (
            SELECT 1 FROM PHAN_CONG_PHONG pc
            JOIN LICH_THUC_HANH l ON pc.MaLich = l.MaLich
            WHERE pc.MaPhong       = p.MaPhong
              AND l.NgayThucHanh   = @NgayThucHanh
              AND l.MaCa           = @MaCa
              AND l.TrangThaiLich  != N'Đã hủy'
              AND l.MaLich         != @MaLichHienTai
          )
    ORDER BY MayTot DESC, p.TenPhong;
END;
GO

-- ------------------------------------------------------------
-- SP 6: sp_ThongKeTongQuan
-- Mục đích: Lấy toàn bộ thống kê tổng quan 1 lần gọi
--   thay vì nhiều subquery rải rác trong BaoCaoThongKeRepository
-- Ghi chú: C# vẫn chạy bình thường, SP bổ sung để dùng sau
-- ------------------------------------------------------------
IF OBJECT_ID('sp_ThongKeTongQuan', 'P') IS NOT NULL
    DROP PROCEDURE sp_ThongKeTongQuan;
GO

CREATE PROCEDURE sp_ThongKeTongQuan
    @StartDate DATE = NULL,
    @EndDate   DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @EndDate IS NULL SET @EndDate = CAST(GETDATE() AS DATE);
    IF @StartDate IS NULL SET @StartDate = DATEADD(MONTH, -1, @EndDate);

    -- Cập nhật snapshot hôm nay trước
    EXEC sp_CapNhatChotSoLieu @NgayChot = NULL;

    -- Lấy thống kê
    SELECT
        ISNULL((SELECT TOP 1 TotalRooms  FROM CHOT_SO_LIEU WHERE NgayChot <= @EndDate ORDER BY NgayChot DESC), 0) AS TotalRooms,
        ISNULL((SELECT TOP 1 ActiveRooms FROM CHOT_SO_LIEU WHERE NgayChot <= @EndDate ORDER BY NgayChot DESC), 0) AS ActiveRooms,
        ISNULL((SELECT TOP 1 TotalMay    FROM CHOT_SO_LIEU WHERE NgayChot <= @EndDate ORDER BY NgayChot DESC), 0) AS TotalMay,
        ISNULL((SELECT TOP 1 MayTot      FROM CHOT_SO_LIEU WHERE NgayChot <= @EndDate ORDER BY NgayChot DESC), 0) AS MayTot,
        ISNULL((SELECT TOP 1 MayHong     FROM CHOT_SO_LIEU WHERE NgayChot <= @EndDate ORDER BY NgayChot DESC), 0) AS MayHong,
        ISNULL((SELECT TOP 1 TotalUsers  FROM CHOT_SO_LIEU WHERE NgayChot <= @EndDate ORDER BY NgayChot DESC), 0) AS TotalUsers,

        (SELECT COUNT(*) FROM LICH_THUC_HANH l
         WHERE l.NgayThucHanh >= @StartDate AND l.NgayThucHanh <= @EndDate) AS TotalLich,

        (SELECT COUNT(*) FROM LICH_THUC_HANH l
         WHERE l.TrangThaiLich != N'Đã hủy'
           AND l.MaLich IN (SELECT MaLich FROM PHAN_CONG_PHONG)
           AND l.NgayThucHanh >= @StartDate AND l.NgayThucHanh <= @EndDate) AS LichDaXep,

        (SELECT COUNT(*) FROM LICH_THUC_HANH l
         WHERE l.TrangThaiLich = N'Đã hủy'
           AND l.NgayThucHanh >= @StartDate AND l.NgayThucHanh <= @EndDate) AS LichDaHuy;
END;
GO


-- ============================================================
-- PHẦN 3: KIỂM TRA SAU KHI CHẠY
-- ============================================================
-- Chạy các lệnh sau để xác nhận đã tạo thành công:

SELECT name, type_desc
FROM sys.objects
WHERE type IN ('TR', 'P')
  AND name IN (
    'trg_UpdatedAt_MayTinh',
    'trg_UpdatedAt_PhongMay',
    'trg_UpdatedAt_NguoiDung',
    'trg_LogTrangThaiMayTinh',
    'trg_LogTrangThaiPhongMay',
    'sp_CapNhatChotSoLieu',
    'sp_AutoUpdateLichCu',
    'sp_XoaPhongMay',
    'sp_XoaNguoiDung',
    'sp_TimPhongChoLich',
    'sp_ThongKeTongQuan'
  )
ORDER BY type_desc, name;
GO

-- Test nhanh SP snapshot:
-- EXEC sp_CapNhatChotSoLieu;
-- EXEC sp_AutoUpdateLichCu;
-- EXEC sp_ThongKeTongQuan;
GO
