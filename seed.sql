USE QuanLyPhongMay;

-- Insert Vai Tro
INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES ('Admin', N'Quản trị viên hệ thống');
INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES ('NhanVien', N'Nhân viên quản lý phòng máy');
INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES ('GiangVien', N'Giảng viên đăng ký lịch');

-- Insert Admin User (Password is 'admin' hashed with MD5 - or whatever the system uses, wait, usually it's plain text or standard hash, I will put 'admin' if it hashes on the fly, wait, what is the hash for 'admin'?)
-- Actually, let's insert standard data for CA_HOC, MON_HOC, LOP_HOC
INSERT INTO CA_HOC (TenCa, GioBatDau, GioKetThuc) VALUES (N'Ca 1', '07:00', '09:30');
INSERT INTO CA_HOC (TenCa, GioBatDau, GioKetThuc) VALUES (N'Ca 2', '09:30', '12:00');
INSERT INTO CA_HOC (TenCa, GioBatDau, GioKetThuc) VALUES (N'Ca 3', '13:00', '15:30');
INSERT INTO CA_HOC (TenCa, GioBatDau, GioKetThuc) VALUES (N'Ca 4', '15:30', '18:00');

INSERT INTO MON_HOC (MaHocPhan, TenMon) VALUES ('IT01', N'Lập trình C#');
INSERT INTO MON_HOC (MaHocPhan, TenMon) VALUES ('IT02', N'Lập trình Web');

INSERT INTO LOP_HOC (MaLopHocPhan, TenLop, SiSo, MaHocPhan) VALUES ('KTPM01-01', 'KTPM01-01', 30, 'IT01');
INSERT INTO LOP_HOC (MaLopHocPhan, TenLop, SiSo, MaHocPhan) VALUES ('KTPM02-01', 'KTPM02-01', 30, 'IT01');
INSERT INTO LOP_HOC (MaLopHocPhan, TenLop, SiSo, MaHocPhan) VALUES ('CNTT01-01', 'CNTT01-01', 35, 'IT02');
