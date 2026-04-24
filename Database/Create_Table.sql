CREATE DATABASE QuanLyPhongMay;
GO

USE QuanLyPhongMay;
GO

/* =========================
   1. BANG VAI TRO
   ========================= */
CREATE TABLE VAI_TRO (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL
);
GO

/* =========================
   2. BANG CHUC NANG
   ========================= */
CREATE TABLE CHUC_NANG (
    MaChucNang INT IDENTITY(1,1) PRIMARY KEY,
    TenChucNang NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL
);
GO

/* =========================
   3. BANG PHAN QUYEN
   Quan he N-N giua VAI_TRO va CHUC_NANG
   ========================= */
CREATE TABLE PHAN_QUYEN (
    MaVaiTro INT NOT NULL,
    MaChucNang INT NOT NULL,
    Xem BIT NOT NULL DEFAULT 0,
    Them BIT NOT NULL DEFAULT 0,
    Sua BIT NOT NULL DEFAULT 0,
    Xoa BIT NOT NULL DEFAULT 0,
    CONSTRAINT PK_PHAN_QUYEN PRIMARY KEY (MaVaiTro, MaChucNang),
    CONSTRAINT FK_PHAN_QUYEN_VAI_TRO FOREIGN KEY (MaVaiTro)
        REFERENCES VAI_TRO(MaVaiTro),
    CONSTRAINT FK_PHAN_QUYEN_CHUC_NANG FOREIGN KEY (MaChucNang)
        REFERENCES CHUC_NANG(MaChucNang)
);
GO

/* =========================
   4. BANG NGUOI DUNG
   ========================= */
CREATE TABLE NGUOI_DUNG (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhauDaMaHoa VARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    SoDienThoai VARCHAR(20) NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    MaVaiTro INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_NGUOI_DUNG_VAI_TRO FOREIGN KEY (MaVaiTro)
        REFERENCES VAI_TRO(MaVaiTro)
);
GO

/* =========================
   5. BANG TRANG THAI PHONG
   ========================= */
CREATE TABLE TRANG_THAI_PHONG (
    MaTTPhong INT IDENTITY(1,1) PRIMARY KEY,
    TenTrangThaiPhong NVARCHAR(50) NOT NULL UNIQUE
);
GO

/* =========================
   6. BANG PHONG MAY
   ========================= */
CREATE TABLE PHONG_MAY (
    MaPhong INT IDENTITY(1,1) PRIMARY KEY,
    TenPhong NVARCHAR(100) NOT NULL,
    ViTri NVARCHAR(100) NOT NULL,
    SucChua INT NOT NULL,
    MoTa NVARCHAR(255) NULL,
    MaTTPhong INT NOT NULL,
    CONSTRAINT CK_PHONG_MAY_SUCCHUA CHECK (SucChua > 0),
    CONSTRAINT FK_PHONG_MAY_TRANG_THAI FOREIGN KEY (MaTTPhong)
        REFERENCES TRANG_THAI_PHONG(MaTTPhong)
);
GO

/* =========================
   7. BANG TRANG THAI MAY
   ========================= */
CREATE TABLE TRANG_THAI_MAY (
    MaTTMay INT IDENTITY(1,1) PRIMARY KEY,
    TenTrangThaiMay NVARCHAR(50) NOT NULL UNIQUE
);
GO

/* =========================
   8. BANG MAY TINH
   CHUA la quan he 1-N nen dua MaPhong vao MAY_TINH
   ========================= */
CREATE TABLE MAY_TINH (
    MaMay INT IDENTITY(1,1) PRIMARY KEY,
    TenMay NVARCHAR(100) NOT NULL,
    CPU NVARCHAR(100) NOT NULL,
    RAM INT NOT NULL,
    DungLuongLuuTru INT NOT NULL,
    KichThuocManHinh DECIMAL(4,1) NOT NULL,
    GhiChu NVARCHAR(255) NULL,
    ViTriMayTrongPhong NVARCHAR(50) NULL,
    MaPhong INT NOT NULL,
    MaTTMay INT NOT NULL,
    CONSTRAINT CK_MAY_TINH_RAM CHECK (RAM > 0),
    CONSTRAINT CK_MAY_TINH_LUUTRU CHECK (DungLuongLuuTru > 0),
    CONSTRAINT CK_MAY_TINH_MANHINH CHECK (KichThuocManHinh > 0),
    CONSTRAINT FK_MAY_TINH_PHONG FOREIGN KEY (MaPhong)
        REFERENCES PHONG_MAY(MaPhong),
    CONSTRAINT FK_MAY_TINH_TRANG_THAI FOREIGN KEY (MaTTMay)
        REFERENCES TRANG_THAI_MAY(MaTTMay)
);
GO

/* =========================
   9. BANG LOP HOC
   ========================= */
CREATE TABLE LOP_HOC (
    MaLop INT IDENTITY(1,1) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    SiSo INT NOT NULL,
    CONSTRAINT CK_LOP_HOC_SISO CHECK (SiSo > 0)
);
GO

/* =========================
   10. BANG MON HOC
   ========================= */
CREATE TABLE MON_HOC (
    MaMon INT IDENTITY(1,1) PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL
);
GO

/* =========================
   11. BANG CA HOC
   ========================= */
CREATE TABLE CA_HOC (
    MaCa INT IDENTITY(1,1) PRIMARY KEY,
    TenCa NVARCHAR(50) NOT NULL,
    GioBatDau TIME NOT NULL,
    GioKetThuc TIME NOT NULL,
    CONSTRAINT CK_CA_HOC_GIO CHECK (GioKetThuc > GioBatDau)
);
GO

/* =========================
   12. BANG LICH THUC HANH
   Bo ThuTrongTuan vi co the suy ra tu NgayThucHanh
   Tao lich la 1-N nen dua NguoiTao, ThoiGianTao vao day
   ========================= */
CREATE TABLE LICH_THUC_HANH (
    MaLich INT IDENTITY(1,1) PRIMARY KEY,
    NgayThucHanh DATE NOT NULL,
    SoLuongSinhVien INT NOT NULL,
    TrangThaiLich NVARCHAR(30) NOT NULL DEFAULT N'Chờ xếp phòng',
    GhiChu NVARCHAR(255) NULL,
    ThoiGianTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    MaLop INT NOT NULL,
    MaMon INT NOT NULL,
    MaCa INT NOT NULL,
    NguoiTao INT NOT NULL,
    CONSTRAINT CK_LICH_SOSV CHECK (SoLuongSinhVien > 0),
    CONSTRAINT FK_LICH_LOP FOREIGN KEY (MaLop)
        REFERENCES LOP_HOC(MaLop),
    CONSTRAINT FK_LICH_MON FOREIGN KEY (MaMon)
        REFERENCES MON_HOC(MaMon),
    CONSTRAINT FK_LICH_CA FOREIGN KEY (MaCa)
        REFERENCES CA_HOC(MaCa),
    CONSTRAINT FK_LICH_NGUOITAO FOREIGN KEY (NguoiTao)
        REFERENCES NGUOI_DUNG(MaNguoiDung)
);
GO

/* =========================
   13. BANG YEU CAU CAU HINH
   Moi lich co 1 yeu cau cau hinh
   ========================= */
CREATE TABLE YEU_CAU_CAU_HINH (
    MaYeuCau INT IDENTITY(1,1) PRIMARY KEY,
    RAMToiThieu INT NULL,
    CPUToiThieu NVARCHAR(100) NULL,
    ManHinhToiThieu DECIMAL(4,1) NULL,
    LuuTruToiThieu INT NULL,
    MaLich INT NOT NULL UNIQUE,
    CONSTRAINT CK_YEUCAU_RAM CHECK (RAMToiThieu IS NULL OR RAMToiThieu >= 0),
    CONSTRAINT CK_YEUCAU_MANHINH CHECK (ManHinhToiThieu IS NULL OR ManHinhToiThieu >= 0),
    CONSTRAINT CK_YEUCAU_LUUTRU CHECK (LuuTruToiThieu IS NULL OR LuuTruToiThieu >= 0),
    CONSTRAINT FK_YEU_CAU_LICH FOREIGN KEY (MaLich)
        REFERENCES LICH_THUC_HANH(MaLich)
        ON DELETE CASCADE
);
GO

/* =========================
   14. BANG PHAN CONG PHONG
   Tach rieng de luu lich su phan cong phong
   ========================= */
CREATE TABLE PHAN_CONG_PHONG (
    MaPhanCong INT IDENTITY(1,1) PRIMARY KEY,
    MaLich INT NOT NULL,
    MaPhong INT NOT NULL,
    ThoiDiemPhanCong DATETIME2 NOT NULL DEFAULT GETDATE(),
    GhiChuXepPhong NVARCHAR(255) NULL,
    MaNguoiDung INT NOT NULL,
    CONSTRAINT FK_PHAN_CONG_LICH FOREIGN KEY (MaLich)
        REFERENCES LICH_THUC_HANH(MaLich),
    CONSTRAINT FK_PHAN_CONG_PHONG FOREIGN KEY (MaPhong)
        REFERENCES PHONG_MAY(MaPhong),
    CONSTRAINT FK_PHAN_CONG_NGUOI FOREIGN KEY (MaNguoiDung)
        REFERENCES NGUOI_DUNG(MaNguoiDung)
);
GO

/* =========================
   15. BANG LOG CAP NHAT PHONG
   Mo rong neu can audit
   ========================= */
CREATE TABLE CAP_NHAT_PHONG (
    MaCapNhatPhong BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaPhong INT NOT NULL,
    HanhDong NVARCHAR(50) NOT NULL,
    ThoiGianCapNhat DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_CAP_NHAT_PHONG_NGUOI FOREIGN KEY (MaNguoiDung)
        REFERENCES NGUOI_DUNG(MaNguoiDung),
    CONSTRAINT FK_CAP_NHAT_PHONG_PHONG FOREIGN KEY (MaPhong)
        REFERENCES PHONG_MAY(MaPhong)
);
GO

/* =========================
   16. BANG LOG CAP NHAT MAY
   Mo rong neu can audit
   ========================= */
CREATE TABLE CAP_NHAT_MAY (
    MaCapNhatMay BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaMay INT NOT NULL,
    HanhDong NVARCHAR(50) NOT NULL,
    ThoiGianCapNhat DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_CAP_NHAT_MAY_NGUOI FOREIGN KEY (MaNguoiDung)
        REFERENCES NGUOI_DUNG(MaNguoiDung),
    CONSTRAINT FK_CAP_NHAT_MAY_MAY FOREIGN KEY (MaMay)
        REFERENCES MAY_TINH(MaMay)
);
GO

/* =========================
   17. CAC INDEX HUU ICH
   ========================= */
CREATE INDEX IX_NGUOI_DUNG_MaVaiTro ON NGUOI_DUNG(MaVaiTro);
CREATE INDEX IX_PHONG_MAY_MaTTPhong ON PHONG_MAY(MaTTPhong);
CREATE INDEX IX_MAY_TINH_MaPhong ON MAY_TINH(MaPhong);
CREATE INDEX IX_MAY_TINH_MaTTMay ON MAY_TINH(MaTTMay);
CREATE INDEX IX_LICH_Ngay_Ca ON LICH_THUC_HANH(NgayThucHanh, MaCa);
CREATE INDEX IX_PHAN_CONG_LICH ON PHAN_CONG_PHONG(MaLich);
CREATE INDEX IX_PHAN_CONG_PHONG ON PHAN_CONG_PHONG(MaPhong);
GO