-- ============================================================
-- FIX: Trigger trg_LogTrangThaiMayTinh
-- Vấn đề: Cột MaNguoiDung trong CAP_NHAT_MAY NOT NULL
--         nhưng trigger đang INSERT NULL -> lỗi constraint
-- Giải pháp: Đọc MaNguoiDung từ CONTEXT_INFO
--            (C# set CONTEXT_INFO trước mỗi lệnh UPDATE)
-- ============================================================

USE QuanLyPhongMay;
GO

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
        -- Đọc MaNguoiDung từ CONTEXT_INFO (C# set CONVERT(VARBINARY(128), CONVERT(BINARY(4), uid)) trước UPDATE)
        -- Nếu không có context, dùng 1 (admin hệ thống) làm fallback
        DECLARE @MaNguoiDung INT;
        DECLARE @ctx VARBINARY(128) = CONTEXT_INFO();
        IF @ctx IS NOT NULL
            SET @MaNguoiDung = CONVERT(INT, CONVERT(BINARY(4), SUBSTRING(@ctx, 1, 4)));
        IF @MaNguoiDung IS NULL OR @MaNguoiDung <= 0
            SET @MaNguoiDung = 1;  -- fallback: admin hệ thống

        INSERT INTO CAP_NHAT_MAY (MaNguoiDung, MaMay, HanhDong, ThoiGianCapNhat)
        SELECT
            @MaNguoiDung,
            i.MaMay,
            N'Đổi trạng thái: ' +
                ISNULL((SELECT TenTrangThaiMay FROM TRANG_THAI_MAY WHERE MaTTMay = d.MaTTMay), CAST(d.MaTTMay AS NVARCHAR(10))) +
                N' -> ' +
                ISNULL((SELECT TenTrangThaiMay FROM TRANG_THAI_MAY WHERE MaTTMay = i.MaTTMay), CAST(i.MaTTMay AS NVARCHAR(10))),
            SYSDATETIME()
        FROM inserted i
        INNER JOIN deleted d ON i.MaMay = d.MaMay
        WHERE i.MaTTMay <> d.MaTTMay;  -- chỉ khi TT thực sự đổi
    END
END;
GO

PRINT N'✓ Trigger trg_LogTrangThaiMayTinh đã được cập nhật thành công!';
GO
