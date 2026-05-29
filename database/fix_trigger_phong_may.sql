-- ============================================================
-- FIX: Trigger trg_LogTrangThaiPhongMay
-- Vấn đề: Cột MaNguoiDung trong CAP_NHAT_PHONG NOT NULL
--         nhưng trigger đang INSERT NULL -> lỗi constraint
-- Giải pháp: Đọc MaNguoiDung từ CONTEXT_INFO
--            (C# set CONTEXT_INFO trước mỗi lệnh UPDATE)
-- ============================================================

USE QuanLyPhongMay;
GO

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
        -- Đọc MaNguoiDung từ CONTEXT_INFO (C# set CONVERT(VARBINARY(128), CONVERT(BINARY(4), uid)) trước UPDATE)
        -- Nếu không có context, dùng 1 (admin hệ thống) làm fallback
        DECLARE @MaNguoiDung INT;
        DECLARE @ctx VARBINARY(128) = CONTEXT_INFO();
        IF @ctx IS NOT NULL
            SET @MaNguoiDung = CONVERT(INT, CONVERT(BINARY(4), SUBSTRING(@ctx, 1, 4)));
        IF @MaNguoiDung IS NULL OR @MaNguoiDung <= 0
            SET @MaNguoiDung = 1;  -- fallback: admin hệ thống

        INSERT INTO CAP_NHAT_PHONG (MaNguoiDung, MaPhong, HanhDong, ThoiGianCapNhat)
        SELECT
            @MaNguoiDung,
            i.MaPhong,
            N'Đổi trạng thái: ' +
                ISNULL((SELECT TenTrangThaiPhong FROM TRANG_THAI_PHONG WHERE MaTTPhong = d.MaTTPhong), CAST(d.MaTTPhong AS NVARCHAR(10))) +
                N' -> ' +
                ISNULL((SELECT TenTrangThaiPhong FROM TRANG_THAI_PHONG WHERE MaTTPhong = i.MaTTPhong), CAST(i.MaTTPhong AS NVARCHAR(10))),
            SYSDATETIME()
        FROM inserted i
        INNER JOIN deleted d ON i.MaPhong = d.MaPhong
        WHERE i.MaTTPhong <> d.MaTTPhong;
    END
END;
GO

PRINT N'✓ Trigger trg_LogTrangThaiPhongMay đã được cập nhật thành công!';
GO
