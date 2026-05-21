# Hệ Thống Quản Lý Phòng Máy Thực Hành 🏫💻

Dự án phần mềm quản lý phòng máy thực hành dành cho trường đại học. Hệ thống giúp tối ưu hóa việc quản lý phòng máy, lịch thực hành, thiết bị và tài khoản người dùng, mang lại sự tiện lợi cho cả Ban Quản Trị (Admin) và Nhân Viên phòng máy.

---

## 🌟 Tính Năng Nổi Bật

### 1. Quản Lý Người Dùng
- **Phân quyền bảo mật**: Hỗ trợ hai vai trò là `Admin` (Quản trị viên) và `Nhân Viên` phòng máy.
- **Bảo mật**: Thông tin tài khoản được lưu trữ an toàn với cơ chế mã hóa mật khẩu.

### 2. Quản Lý Danh Mục
- **Thao tác linh hoạt**: Thêm, sửa, xóa, tìm kiếm danh mục (lớp, môn học, v.v.).
- **Tìm kiếm thông minh**: Tìm kiếm phòng máy trống theo ngày, ca, thứ.
- **Lọc thiết bị nâng cao**: Lọc cấu hình máy tính chi tiết theo RAM, Màn hình, CPU, v.v.

### 3. Quản Lý Lịch Thực Hành
- **Lên lịch nhanh chóng**: Tạo lịch thực hành dễ dàng (theo lớp, ngày, ca, số lượng sinh viên).
- **Gợi ý tự động**: Tự động đề xuất phòng máy phù hợp dựa trên số lượng sinh viên và yêu cầu cấu hình.
- **Tùy chỉnh lịch**: Chỉnh sửa và cập nhật lịch thực hành theo thời gian thực.

### 4. Báo Cáo & Thống Kê (Dành cho Admin)
- Báo cáo số lượng và tình trạng phòng máy theo ngày/tháng/năm.
- Thống kê tình trạng hoạt động của các máy tính.
- Đo lường công suất và tần suất sử dụng phòng máy.
- Trực quan hóa dữ liệu thông qua biểu đồ sinh động (LiveCharts).

---

## 🛠 Công Nghệ Sử Dụng

- **Ngôn ngữ lập trình**: C# (.NET 8.0 Windows Forms)
- **Kiến trúc giao diện**: Mô hình Single Page Application (SPA) thu nhỏ trên Desktop.
- **Giao diện & UI**: Windows Forms tiêu chuẩn kết hợp với các thư viện giao diện nâng cao (như `Guna.UI2` hoặc `Krypton Toolkit`).
- **Biểu đồ thống kê**: `LiveCharts`.
- **Cơ sở dữ liệu**: SQL Server / MySQL (Sử dụng các ORM/Micro-ORM như `Entity Framework` hoặc `Dapper`).

---

## 🚀 Hướng Dẫn Cài Đặt

### Yêu Cầu Hệ Thống
- Visual Studio 2022 (trở lên) hỗ trợ .NET 8.0.
- SQL Server hoặc MySQL.

### Các Bước Cài Đặt
1. **Clone dự án về máy**:
   ```bash
   git clone <đường-dẫn-repo-của-bạn>
   ```
2. **Khôi phục các gói NuGet**:
   Mở file `.sln` bằng Visual Studio, nhấp chuột phải vào Solution và chọn `Restore NuGet Packages`.
3. **Cấu hình Cơ Sở Dữ Liệu**:
   - Tạo Database mới trong hệ quản trị CSDL của bạn.
   - Chạy các script SQL (nếu có) để tạo bảng.
   - Cập nhật chuỗi kết nối (`ConnectionString`) trong mã nguồn (file cấu hình hoặc `DatabaseHelper.cs`).
4. **Chạy dự án**:
   - Nhấn `F5` hoặc chọn nút `Start` trong Visual Studio để chạy ứng dụng.

---

## 📖 Cấu Trúc Dự Án Cơ Bản

- **`MainForm`**: Khung sườn chính của ứng dụng, chứa thanh điều hướng (Sidebar) và vùng hiển thị nội dung (`pnlContent`).
- **`Views/`**: Chứa các màn hình chức năng dưới dạng `UserControl` (SPA), cho phép thao tác kéo-thả bằng Visual Studio Designer.
- **`Helpers/`**: Chứa các lớp tiện ích hỗ trợ kết nối CSDL, xử lý giao diện (UI) và các logic dùng chung.

---

## 🤝 Đóng Góp
Nếu bạn có bất kỳ đóng góp hay cải tiến nào cho dự án, vui lòng tạo **Pull Request** hoặc mở **Issue** để cùng thảo luận. Mọi ý kiến đóng góp đều được trân trọng!