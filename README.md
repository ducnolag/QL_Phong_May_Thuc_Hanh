# Hệ thống Quản lý Phòng máy Thực hành (University Lab Management System)

## 📌 Giới thiệu & Mục tiêu Dự án
Dự án **Hệ thống Quản lý Phòng máy Thực hành** được xây dựng nhằm mục đích tin học hóa và tối ưu hóa quy trình quản lý, sắp xếp lịch thực hành cho các phòng máy vi tính tại trường đại học. 

Hệ thống giải quyết các bài toán phức tạp như theo dõi tình trạng thiết bị (RAM, CPU, Màn hình), chống chồng chéo lịch học, và tự động hóa việc gợi ý phòng máy sao cho phù hợp với số lượng sinh viên và yêu cầu cấu hình của môn học.

## 🚀 Các Tính Năng Chính (Key Features)

### 1. Phân quyền và Bảo mật (User Management)
- Hệ thống hỗ trợ 2 vai trò chính: **Quản trị viên (Admin)** và **Nhân viên phòng máy**.
- Cơ chế đăng nhập an toàn, mật khẩu được **mã hoá một chiều** (SHA-256 / BCrypt) trong cơ sở dữ liệu.
- Menu và các tính năng thao tác trên UI sẽ động thay đổi hiển thị tùy thuộc vào quyền của người đăng nhập.

### 2. Quản lý Danh mục Hệ thống (Categories Management)
- **Quản trị viên:** Toàn quyền Thêm/Sửa/Xóa mọi danh mục nền tảng như: Danh mục phần cứng, Danh mục phần mềm, Danh mục phòng máy, Danh mục môn học/chuẩn đầu ra.
- **Nhân viên:** Được phép quản lý các danh mục phục vụ trực tiếp công tác xếp lịch như Lớp học phần, hoặc tìm kiếm phòng trống theo bộ lọc (Ngày, Ca, Thứ trong tuần). Đặc biệt hỗ trợ bộ lọc phòng theo cấu hình phần cứng (Bao nhiêu GB RAM, vi xử lý gì, loại màn hình cần thiết).

### 3. Xếp Lịch Thực Hành (Lab Scheduling)
- Là tính năng cốt lõi, tiếp nhận đầu vào gồm: Lớp học phần, Ngày thực hành, Thứ trong tuần, Ca thực hành và Khối lượng sinh viên.
- **Tự động gợi ý:** Thuật toán truy xuất CSDL nhanh chóng quét các phòng trống trong cùng thời điểm, sau đó lọc tiếp các phòng đáp ứng đủ số lượng máy cho sinh viên và yêu cầu cấu hình của môn học.
- Dễ dàng thao tác chuyển đổi lịch, cập nhật và xoá buổi thực hành.

### 4. Thống Kê & Báo Cáo (Dashboard & Reports)
- **Admin Dashboard** trực quan hiển thị tổng quan:
  - Tỉ lệ phần trăm lấp đầy các phòng máy theo tuần/tháng.
  - Tình trạng máy móc: Hoạt động, đang bảo trì, hoặc hỏng hóc cần thay thế.
- Ứng dụng **Live Charts** để vẽ các biểu đồ động minh họa tình hình sử dụng nhằm hỗ trợ ban quản trị đưa ra các quyết định nâng cấp kịp thời.

---

## 🛠 Nền tảng Kỹ thuật & Kiến trúc

Dự án được xây dựng hoàn toàn trên nền tảng .NET Framework (hoặc .NET Core tùy chiến lược), vận hành với kiến trúc phân tầng chuyên nghiệp:

1. **Ngôn ngữ lập trình:** C#
2. **Kiến trúc áp dụng:** Mô hình 3 lớp (3-Tier Architecture: Presentation - Business - DataAccess).
3. **Giao diện (Presentation):** 
   - Windows Forms (WinForms).
   - Tích hợp **Guna.UI2** và **Krypton Toolkit** để tạo ra các Modern UI (Bo góc, Shadow, Dark/Light theme chuyển đổi linh hoạt).
   - Sử dụng **Live Charts** cho giao diện phân tích số liệu.
4. **Cơ sở dữ liệu:** Microsoft SQL Server / MySQL.
5. **Truy cập dữ liệu (Data Access Layer):** Sử dụng ORM **Dapper** (cho hiệu năng cao, viết SQL thô nhanh) hoặc **Entity Framework**.

---

## 📂 Cấu trúc Thư mục Dự án

```text
📦FInal_dotnet
 ┣ 📂Database             # Chứa toàn bộ Script SQL tạo Schema, Tables, Stored Procedures và Dữ liệu mẫu khởi tạo.
 ┣ 📂Docs                 # Lưu trữ tài liệu liên quan đến dự án (Word, PDF, Excel).
 ┃ ┣ 📂Analysis           # File phân tích yêu cầu phần mềm, Thiết kế DB (ERD Diagram).
 ┃ ┗ 📂Reports            # Cấu trúc báo cáo đồ án, Test Cases, Hướng dẫn sử dụng.
 ┣ 📂Source               # Chứa Source Code chính của dự án C#.
 ┃ ┣ 📂Models             # (Entities/DTO) Chứa cấu trúc class đại diện cho các bảng trong CSDL.
 ┃ ┣ 📂DataAccess         # (DAL) Xử lý logic truy vấn dữ liệu (CRUD) xuống DB bằng Dapper/EF.
 ┃ ┣ 📂BusinessLogic      # (BLL) Nơi đặt các Business Rule (ví dụ: Thuật toán gợi ý phòng, rule bắt lỗi trống dữ liệu).
 ┃ ┣ 📂Views              # (GUI) Các Form giao diện WinForms, User Controls, Assets hình ảnh, icon.
 ┃ ┗ 📂Utilities          # (Helper) Chứa các hàm dùng chung: Mã hóa mật khẩu, Format tiền tệ, Email sender, v.v.
 ┣ 📜.gitignore           # Cấu hình bỏ qua các thư mục biên dịch (bin, obj, .vs) khỏi Git.
 ┗ 📜README.md            # Tài liệu điều hướng (chính là file bạn đang đọc).
```

---

## 📅 Lộ trình Triển khai (How we will execute this)

Để đảm bảo dự án chạy xuyên suốt theo mục tiêu và dễ dàng làm việc nhóm qua Github, lộ trình thực hiện được chia làm 4 giai đoạn cụ thể:

### Giai đoạn 1: Thiết kế Kiến trúc và Cơ sở dữ liệu
- Phân tích và tạo ERD (Entity Relationship Diagram) để thấy được mối liên kết giữa Phòng máy, Chi tiết máy, Cấu hình, Người dùng, Lịch thực hành.
- Viết file script `.sql` định nghĩa chuẩn các ràng buộc (Foreign keys, Indexes) nhằm tối ưu hoá truy vấn xếp lịch.
- Chuẩn bị sẵn bộ Mockup Data để giai đoạn chạy thử nghiệm có dữ liệu mẫu.

### Giai đoạn 2: Base UI & Cấu trúc Mô hình 3 lớp
- Khởi tạo File Solution `.sln`, map 3 Class Library (DAL, BLL, Models) và 1 Windows Application (Views).
- Setting up các package bắt buộc qua NuGet: Dapper, GunaUI2.
- Xây dựng Form Đăng nhập, viết hàm mã hoá mật khẩu. Tùy thuộc vào Database gửi lên role gì để render Master Menu Dashboard tương ứng.

### Giai đoạn 3: Nghiệp vụ Quản lý & Xếp Lịch (Core Functionality)
- Phát triển các Form (CRUD) quản lý Danh mục đơn giản (Xong trước).
- Tích hợp bộ lọc phức tạp: Cung cấp giao diện lọc cấu hình phòng (RAM, CPU).
- Phát triển Core module "Xếp lịch học": Giao diện trực quan chọn ô giờ, gọi xuồng BLL để tính toán chống xung đột (Conflict resolution) trên các phòng để in ra gợi ý tự động cho người dùng.

### Giai đoạn 4: Thống kê & Packaging Báo Cáo
- Hoàn thiện luồng lấy data cho Dashboard. Sử dụng Live Graphs gắn vào view.
- Test hệ thống: Dựa vào chức năng điền các form Test case căn bản.
- Chụp ảnh màn hình mọi tính năng và đẩy vào thư mục `Docs` làm căn cứ viết Báo cáo nộp bài.

---
> **Trạng thái hiện tại:** Đã khởi tạo cấu trúc thư mục, bổ sung Gitignore và thống nhất quy trình làm việc. Sẵn sàng tới bước tiếp theo: Thiết kế hạ tầng cơ sở dữ liệu.
