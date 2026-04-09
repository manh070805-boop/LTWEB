# Kế Hoạch Phát Triển Dự Án Web Quản Lý Trung Tâm Lập Trình

## 1. Mục tiêu dự án

Xây dựng hệ thống quản lý trung tâm lập trình phục vụ 3 nhóm người dùng:

- Admin: quản trị tài khoản, khóa học, lớp học, thanh toán, báo cáo.
- Teacher: quản lý lớp phụ trách, lịch học, bài học, duyệt học viên, theo dõi tiến độ.
- Student: xem khóa học, đăng ký học, thanh toán, theo dõi lịch học và bài học.

## 2. Phạm vi chức năng

### Giai đoạn MVP

- Quản lý tài khoản và phân quyền `Admin`, `Teacher`, `Student`.
- Quản lý khóa học và cấp độ khóa học.
- Quản lý lớp học, giảng viên phụ trách, sĩ số, trạng thái lớp.
- Quản lý lịch học cố định theo tuần.
- Quản lý bài học theo từng lớp.
- Đăng ký học và duyệt đăng ký.
- Ghi nhận thanh toán và trạng thái thanh toán.
- Dashboard cơ bản cho Admin.

### Giai đoạn nâng cao

- Điểm danh và chuyên cần.
- Quản lý điểm, bài tập, kết quả học tập.
- Voucher/khuyến mãi.
- Nhắc lịch học qua email/Zalo.
- Báo cáo doanh thu, tỷ lệ chuyển đổi, tỷ lệ hoàn thành khóa học.
- Hoàn tiền, bảo lưu, chuyển lớp.

## 3. Kiến trúc đề xuất

### Frontend

- Web app phân quyền theo vai trò.
- Công nghệ đề xuất: React/Next.js hoặc Vue/Nuxt.
- Module chính:
  - Authentication.
  - Course/Class Management.
  - Enrollment Management.
  - Payment Management.
  - Teacher Portal.
  - Student Portal.
  - Reporting Dashboard.

### Backend

- Kiến trúc REST API hoặc Modular Monolith.
- Công nghệ đề xuất:
  - ASP.NET Core Web API, Java Spring Boot hoặc Node.js NestJS.
- Module backend:
  - Auth & RBAC.
  - Account/Profile.
  - Course/CourseLevel.
  - Class/Schedule/Lesson.
  - Enrollment Workflow.
  - Payment.
  - Notification.
  - Reporting.

### Database

- SQL Server là phù hợp với schema hiện tại.
- Quản lý migration bằng EF Core Migrations, Flyway hoặc Liquibase.
- Chuẩn hóa dữ liệu trạng thái qua bảng master: `Role`, `CourseLevel`, `ClassStatus`, `EnrollmentStatus`, `PaymentMethod`, `PaymentStatus`.

## 4. Lộ trình triển khai theo giai đoạn

## Phase 1. Phân tích nghiệp vụ và đặc tả

- Chốt use case cho từng vai trò.
- Vẽ luồng nghiệp vụ chính:
  - Tạo tài khoản.
  - Tạo khóa học.
  - Mở lớp.
  - Học viên đăng ký.
  - Giảng viên/Admin duyệt.
  - Thanh toán.
  - Bắt đầu học.
- Xác định quy tắc nghiệp vụ:
  - Một học viên có được đăng ký lại cùng khóa hay không.
  - Khi nào `Enrollment` chuyển từ `APPROVED` sang `ACTIVE`.
  - Thanh toán một lần hay nhiều đợt.
  - Có cho phép đổi lớp, hoàn tiền, bảo lưu không.
- Viết BRD hoặc SRS ngắn gọn.

## Phase 2. Thiết kế hệ thống

- Hoàn thiện ERD từ schema hiện tại.
- Thiết kế API contract.
- Thiết kế phân quyền theo role.
- Thiết kế wireframe cho:
  - Admin dashboard.
  - Teacher dashboard.
  - Student portal.
- Chuẩn hóa quy ước mã định danh:
  - `StudentId`, `TeacherId`, `CourseId`, `ClassId`.
- Bổ sung data dictionary cho toàn bộ cột quan trọng.

## Phase 3. Xây dựng nền tảng backend

- Dựng project backend, logging, exception handling, validation.
- Xây dựng authentication:
  - Đăng nhập.
  - JWT/Session.
  - Đổi mật khẩu.
  - Khóa/mở tài khoản.
- Xây dựng CRUD cho:
  - Role, Account.
  - Teacher, Student.
  - CourseLevel, Course.
  - ClassStatus, CenterClass.
  - Schedule, Lesson.
- Viết unit test cho service layer.

## Phase 4. Xây dựng nghiệp vụ lõi

- Enrollment workflow:
  - Học viên đăng ký khóa học/lớp.
  - Admin/Teacher duyệt hoặc từ chối.
  - Gán lớp.
  - Theo dõi lịch sử trạng thái.
- Payment workflow:
  - Tạo payment record.
  - Cập nhật trạng thái.
  - Mapping transaction number.
  - Đối soát kết quả thanh toán.
- Validation quan trọng:
  - Không vượt quá `MaxStudents`.
  - Không trùng `OrderIndex` trong cùng lớp.
  - `EndTime > StartTime`.
  - Chỉ thanh toán cho enrollment hợp lệ.

## Phase 5. Xây dựng frontend

- Màn hình public:
  - Danh sách khóa học.
  - Chi tiết khóa học.
  - Form đăng ký.
- Admin UI:
  - Quản lý người dùng.
  - Quản lý khóa học/lớp học.
  - Quản lý thanh toán.
  - Dashboard.
- Teacher UI:
  - Danh sách lớp phụ trách.
  - Lịch dạy.
  - Bài học.
  - Duyệt học viên.
- Student UI:
  - Hồ sơ cá nhân.
  - Danh sách đăng ký.
  - Lịch học.
  - Thanh toán.

## Phase 6. Tích hợp và kiểm thử

- Tích hợp backend với frontend.
- Tích hợp cổng thanh toán nếu có: VNPay, MoMo.
- Kiểm thử:
  - Unit test.
  - Integration test.
  - API test.
  - UAT theo vai trò người dùng.
- Kiểm tra bảo mật:
  - Authorization.
  - SQL injection.
  - XSS/CSRF.
  - Brute force login.

## Phase 7. Triển khai và vận hành

- Môi trường:
  - Dev.
  - Staging.
  - Production.
- Thiết lập CI/CD.
- Backup database định kỳ.
- Monitoring:
  - API logs.
  - Audit logs.
  - Payment logs.
- Thiết lập tài liệu bàn giao:
  - Hướng dẫn deploy.
  - Hướng dẫn vận hành.
  - Kịch bản xử lý sự cố.

## 5. Thứ tự ưu tiên triển khai

1. Authentication và phân quyền.
2. Quản lý khóa học, lớp học, lịch học.
3. Enrollment workflow.
4. Payment workflow.
5. Dashboard và báo cáo cơ bản.
6. Notification và tính năng mở rộng.

## 6. Deliverables chính

- ERD và data dictionary.
- API specification.
- Source code frontend.
- Source code backend.
- SQL migration và seed data.
- Test cases và báo cáo test.
- Tài liệu triển khai và vận hành.

## 7. Rủi ro cần quản lý

- Thiếu quy tắc nghiệp vụ cho đăng ký lại, chuyển lớp, hoàn tiền.
- Thiếu audit trail cho các thao tác duyệt và thanh toán.
- Thanh toán online cần cơ chế callback/webhook và idempotency.
- Thiếu bảng chuyên cần, điểm số, bài tập nếu muốn mở rộng LMS.
- Thiếu chiến lược backup/restore và bảo mật dữ liệu cá nhân.

## 8. Đề xuất mốc thực hiện

- Tuần 1-2: phân tích nghiệp vụ, chốt ERD, thiết kế API/wireframe.
- Tuần 3-4: hoàn thiện Auth, Account, Course, Class, Schedule, Lesson.
- Tuần 5-6: triển khai Enrollment, Payment, dashboard cơ bản.
- Tuần 7: tích hợp frontend-backend, test end-to-end.
- Tuần 8: UAT, sửa lỗi, staging, chuẩn bị production.
