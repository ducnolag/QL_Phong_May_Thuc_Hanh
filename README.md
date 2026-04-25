Xây dựng hệ thống quản lý phòng máy thực hành cho trường đại học
1.  Yêu cầu về Chức năng (5 điểm):
1.1. Quản lý người dùng (1 điểm)
•	Quản lý tài khoản: Admin, nhân viên phòng máy  
•	Đăng nhập, phân quyền người dùng
1.2. Quản Danh mục (1 điểm)
•	Admin: Được phép thêm, xoá, sửa các loại danh mục 
•	Nhân phòng máy: Được phép thêm, sửa, xoá, tìm kiếm các loại danh mục liên quan tới xếp lịch cho thực hành
•	Tìm kiếm phòng máy trống theo ngày, ca, thứ 
•	Lọc danh sách máy theo cấu hình: ram, màn hình, bộ vi xử lý,..
1.3. Quản lý lịch thực hành (2 điểm)
•	Tạo lịch thực hành (lớp, ngày thực hành, thứ trong tuần, ca thực hành, số lượng sinh viên)
•	Gán phòng máy cho lịch thực hành, tự động gợi ý phòng máy phù hợp
•	Chỉnh sửa lịch thực hành
1.4. Báo cáo & thống kê (dành cho Admin) (1 điểm)
•	Thống kê số lượng phòng theo trạng thái ngày/tháng/năm.
•	Thống kê tình trạng máy.
•	Thống kê tình trạng xử dụng phòng máy.
•	Biểu đồ minh họa.
2.	Yêu cầu kỹ thuật (2.5 điểm)
•	Sử dụng ngôn ngữ C#, áp dụng các thư viện giao diện như Guna.UI2 WinForms, Krypton Toolkit, Live Charts.   (1 điểm)
•	Sử dụng cơ sở dữ liệu quan hệ (MySQL/SQL server) (0.25 điểm)
•	Sử dụng các thư viện kết nối cơ sở dữ liệu: Entity Framwork, Dapper (0.5 điểm)
•	Mã hóa mật khẩu khi lưu trữ (0.25 điểm)
•	Giao diện dễ dùng, hỗ trợ tìm kiếm & lọc (0.5 điểm)
Hãy đọc các tài liệu dưới đây để hiểu rõ hơn về cấu trúc dự án hiện tại và các bước cần làm tiếp theo.

---

## 3. Hướng dẫn Dành cho Lập trình viên (Kiến trúc chuẩn kéo-thả Designer)

### 3.1. Cấu trúc hoạt động của dự án (Mô hình Single Page Application)
Dự án áp dụng mô hình **Single Page Application (SPA)** thu nhỏ trên Desktop để tránh việc mở quá nhiều cửa sổ:
- **`MainForm`**: Đóng vai trò là khung sườn ngoài cùng (Shell). Giao diện của MainForm đã được thiết kế bằng các Control chuẩn (Button, Label, Panel). Phía bên trái là thanh Menu, bên phải là một khoảng trống `pnlContent` dùng để chứa các màn hình con.
- **Thư mục `Views`**: Chứa các màn hình chức năng dưới dạng **UserControl** (như `ComputerManageView`, `UserManageView`...). Mỗi màn hình này hoạt động như một trang độc lập.
- **Cơ chế chuyển trang**: Khi bạn bấm vào một nút trên Menu ở MainForm, hệ thống sẽ xóa màn hình cũ đang hiển thị trong `pnlContent` và tải `UserControl` mới vào khoảng trống đó.

### 3.2. Cách sử dụng Visual Studio Designer (Kéo thả giao diện)
Toàn bộ dự án đã được **tối ưu hóa 100% cho Visual Studio Designer**, không còn sử dụng code để vẽ giao diện ngầm (GDI+). Những gì bạn thấy ở màn hình Designer chính là những gì sẽ chạy:
- **Sửa khung chính**: Nhấp đúp vào `MainForm.cs` để mở chế độ Designer. Bạn có thể kéo thả, đổi tên, đổi màu trực tiếp các nút Menu hoặc Avatar.
- **Sửa các màn hình con**: Mở các file trong thư mục `Views` (ví dụ `ComputerManageView.cs`). Bạn có thể mở Toolbox, tìm kiếm `DataGridView`, `TextBox`, `Button` và kéo thả vào giao diện một cách thoải mái. 
- **Thiết kế lại Dashboard & Báo cáo**: Hiện tại `DashboardView.cs` và `ReportsView.cs` đang là các khung trống (do đã gỡ bỏ code tự vẽ biểu đồ để trả lại quyền kéo thả cho bạn). Bạn cần mở Toolbox, kéo thả các `Panel`, `Label` hoặc sử dụng thư viện **LiveCharts** (theo yêu cầu đề bài) để thiết kế lại màn hình thống kê.

### 3.3. Các bước tiếp theo cần thực hiện để hoàn thiện đồ án
Để đạt điểm tối đa dựa trên các yêu cầu (Mục 1 & 2), bạn cần thực hiện các công việc sau:

**A. Xử lý Giao diện (UI)**
- Sử dụng thư viện **Guna.UI2** hoặc **Krypton Toolkit**: Hiện tại dự án đang dùng Control mặc định của WinForms. Bạn hãy cài đặt gói NuGet `Guna.UI2.WinForms`, sau đó xóa các nút bấm mặc định và kéo thả `Guna2Button`, `Guna2TextBox` vào để giao diện đẹp hơn và đúng yêu cầu.
- Tích hợp **Live Charts**: Kéo thả control của LiveCharts vào `ReportsView.cs` để vẽ biểu đồ thống kê.

**B. Xử lý Cơ sở dữ liệu (Database)**
- Chuyển đổi sang **Entity Framework / Dapper**: Hiện tại hàm `DatabaseHelper.cs` (nằm trong thư mục `Helpers`) đang sử dụng `SqlCommand` thuần. Bạn cần cài đặt Entity Framework hoặc Dapper, sau đó cấu hình lại kết nối.
- Viết các hàm **CRUD (Thêm/Sửa/Xóa)**: Ở các file code-behind như `ComputerManageView.cs`, bạn cần viết sự kiện `Click` cho các nút Thêm/Sửa/Xóa. Các sự kiện này sẽ gọi xuống CSDL để thay đổi dữ liệu, sau đó gọi hàm `LoadData()` để làm mới lại `DataGridView`.
- **Mã hóa mật khẩu**: Trong file `LoginForm.cs` và phần tạo người dùng, bạn cần bổ sung hàm băm mật khẩu (VD: dùng SHA256 hoặc BCrypt) khi lưu trữ và kiểm tra.

**C. Hoàn thiện Logic nghiệp vụ**
- **Xếp lịch thực hành**: Code logic tự động gợi ý phòng máy trống dựa trên số lượng sinh viên và cấu hình máy yêu cầu.
- Phân quyền chi tiết: Ẩn/hiện các nút Thêm/Sửa/Xóa tùy thuộc vào người đang đăng nhập là Admin hay Nhân viên phòng máy.