-- DDL Updates

-- Add CreatedAt and UpdatedAt to PHONG_MAY if not exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'PHONG_MAY') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE PHONG_MAY ADD CreatedAt DATETIME DEFAULT GETDATE();
    ALTER TABLE PHONG_MAY ADD UpdatedAt DATETIME DEFAULT GETDATE();
    EXEC('UPDATE PHONG_MAY SET CreatedAt = GETDATE(), UpdatedAt = GETDATE() WHERE CreatedAt IS NULL');
END

-- Add CreatedAt and UpdatedAt to MAY_TINH if not exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MAY_TINH') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE MAY_TINH ADD CreatedAt DATETIME DEFAULT GETDATE();
    ALTER TABLE MAY_TINH ADD UpdatedAt DATETIME DEFAULT GETDATE();
    EXEC('UPDATE MAY_TINH SET CreatedAt = GETDATE(), UpdatedAt = GETDATE() WHERE CreatedAt IS NULL');
END

-- Add CreatedAt and UpdatedAt to NGUOI_DUNG if not exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'NGUOI_DUNG') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE NGUOI_DUNG ADD CreatedAt DATETIME DEFAULT GETDATE();
    ALTER TABLE NGUOI_DUNG ADD UpdatedAt DATETIME DEFAULT GETDATE();
    EXEC('UPDATE NGUOI_DUNG SET CreatedAt = GETDATE(), UpdatedAt = GETDATE() WHERE CreatedAt IS NULL');
END

-- Thêm trường MaHocPhan vào bảng LOP_HOC để biến thành Lớp Học Phần
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'LOP_HOC') AND name = 'MaHocPhan')
BEGIN
    ALTER TABLE LOP_HOC ADD MaHocPhan INT NULL;
    ALTER TABLE LOP_HOC ADD CONSTRAINT FK_LOP_HOC_MON_HOC FOREIGN KEY (MaHocPhan) REFERENCES MON_HOC(MaHocPhan);
END

-- Thêm trường MaHocPhan vào MON_HOC
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'MON_HOC') AND name = 'MaHocPhan')
BEGIN
    ALTER TABLE MON_HOC ADD MaHocPhan NVARCHAR(50) NULL;
END

-- Thêm trường MaLopHocPhan vào LOP_HOC
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'LOP_HOC') AND name = 'MaLopHocPhan')
BEGIN
    ALTER TABLE LOP_HOC ADD MaLopHocPhan NVARCHAR(50) NULL;
END

-- Create CHOT_SO_LIEU table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'CHOT_SO_LIEU') AND type in (N'U'))
BEGIN
    CREATE TABLE CHOT_SO_LIEU (
        NgayChot DATE PRIMARY KEY,
        TotalRooms INT,
        ActiveRooms INT,
        TotalMay INT,
        MayTot INT,
        MayHong INT,
        TotalUsers INT
    )
END
