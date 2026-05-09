# Tài liệu giải thích `QLKH_LTW`

## 1. Phạm vi tài liệu

`QLKH_LTW` hiện có khoảng `21,145` file. Phần lớn trong số đó không phải mã nghiệp vụ do nhóm viết mà là:

- file build/restore của .NET trong `obj/`, `build_obj/`, `bin/`
- thư viện giao diện/vendor trong `wwwroot/assets`, `wwwroot/admin/assets`, `wwwroot/admin/assets/assets`
- rất nhiều icon/font/template file đi kèm

Vì vậy tài liệu này chia làm 2 lớp:

- lớp 1: giải thích **chi tiết từng file nghiệp vụ đang quyết định hành vi hệ thống**, kèm **giải thích hàm** trong các file đó
- lớp 2: giải thích **theo nhóm thư mục** cho phần generated/vendor, vì mô tả riêng từng `.svg` hay từng file `.gz` không giúp hiểu dự án

## 2. Kiến trúc tổng thể của `QLKH_LTW`

Luồng chính của dự án:

`Request -> Controller (UI MVC hoặc API) -> IAdminService -> IAdminRepository -> SQL Server hoặc JSON -> DTO/ViewModel -> Razor View hoặc JSON response`

Các lớp chính:

- `Program.cs`: khởi động ứng dụng, đăng ký DI, middleware lỗi, middleware kiểm tra role admin
- `Domain/`: định nghĩa enum, entity, store, exception
- `Dtos/`: dữ liệu trao đổi giữa service với controller/UI/API
- `Infrastructure/`: tầng truy cập dữ liệu; hỗ trợ cả JSON file và SQL Server
- `Services/`: toàn bộ luật nghiệp vụ quản trị
- `Controllers/`: REST API `/api/admin/*`
- `Areas/Admin/Controllers/`: controller giao diện MVC cho khu vực `/Admin`
- `Areas/Admin/Views/`: giao diện Razor cho dashboard, accounts, teachers, students, courses, enrollments, payments
- `wwwroot/admin/css/admin-premium.css` và `wwwroot/admin/js/admin-premium.js`: CSS/JS custom thực sự đang được layout hiện tại dùng

Điểm quan trọng:

- theo `appsettings.json`, dự án đang có `DefaultConnection`, nên repository mặc định sẽ ưu tiên backend SQL Server
- nếu bỏ `DefaultConnection`, `AdminRepository` có thể hoạt động ở chế độ JSON file với `App_Data/admin-module-data.json`
- các endpoint `/api/admin/*` bị chặn nếu request không gửi header `X-Role: ADMIN`

---

## 3. Giải thích chi tiết từng file nghiệp vụ

### `QLKH_LTW/QLKH_LTW.csproj`

- Chức năng: file cấu hình project ASP.NET Core Web.
- Ý nghĩa chính:
  - đặt `TargetFramework` là `net10.0`
  - bật `Nullable` và `ImplicitUsings`
  - loại trừ nhiều thư mục generated khỏi item mặc định như `obj`, `bin`, `build_*`
  - thêm package `Microsoft.Data.SqlClient` để truy cập SQL Server
- Hàm: không có.

### `QLKH_LTW/Program.cs`

- Chức năng: file bootstrap toàn bộ ứng dụng.
- Khối logic chính:
  - `builder.Services.AddControllersWithViews().AddJsonOptions(...)`
    - bật MVC + Razor
    - thêm `JsonStringEnumConverter` để enum serialize ra chuỗi thay vì số
  - `builder.Services.AddSingleton<IAdminRepository, AdminRepository>()`
    - đăng ký repository duy nhất cho toàn app
  - `builder.Services.AddSingleton<IAdminService, AdminService>()`
    - đăng ký service nghiệp vụ duy nhất
  - middleware `try/catch`
    - bắt `AdminModuleException` và trả JSON lỗi có `StatusCode` đúng
    - bắt lỗi thường và trả `500`
  - middleware kiểm tra header `X-Role`
    - chỉ áp dụng cho path bắt đầu bằng `/api/admin`
    - nếu không có `X-Role: ADMIN` thì trả `403`
  - `app.MapControllers()`
    - bật toàn bộ controller API
  - `app.MapAreaControllerRoute(...)`
    - map UI khu vực admin vào `/Admin/{controller}/{action}/{id?}`
  - `app.MapControllerRoute(...)`
    - route MVC mặc định ngoài area
  - `app.MapGet("/", ...)`
    - truy cập root sẽ redirect sang `/Admin`
- Hàm tường minh: không dùng kiểu method truyền thống vì file dùng top-level statements, nhưng các khối trên chính là logic khởi động cốt lõi.

### `QLKH_LTW/appsettings.json`

- Chức năng: cấu hình runtime mặc định.
- Nội dung chính:
  - `Logging.LogLevel`: chỉnh mức log
  - `ConnectionStrings.DefaultConnection`: chuỗi kết nối SQL Server
  - `AllowedHosts`
- Tác động:
  - vì có `DefaultConnection`, `AdminRepository` sẽ dùng SQL backend thay vì JSON backend
- Hàm: không có.

### `QLKH_LTW/appsettings.Development.json`

- Chức năng: cấu hình override cho môi trường Development.
- Ở bản hiện tại file chỉ override mức log.
- Hàm: không có.

### `QLKH_LTW/Properties/launchSettings.json`

- Chức năng: cấu hình profile chạy local bằng `dotnet run`/Visual Studio.
- Ý nghĩa:
  - profile `http` chạy ở `http://localhost:5035`
  - profile `https` chạy ở `https://localhost:7035;http://localhost:5035`
  - đặt `ASPNETCORE_ENVIRONMENT=Development`
- Hàm: không có.

### `QLKH_LTW/README.md`

- Chức năng: hướng dẫn ngắn để chạy dự án và liệt kê route chính.
- Ý nghĩa:
  - mô tả module admin hỗ trợ dashboard, account, teacher, student, course, enrollment, payment
  - nhắc rõ API `/api/admin/*` cần header `X-Role: ADMIN`
- Hàm: không có.

### `QLKH_LTW/run*.log`, `QLKH_LTW/smoke*.log`

- Chức năng: log tạm sinh ra khi chạy thử hoặc smoke test local.
- Ý nghĩa:
  - `run*.log`, `run-qlkh*.log`: log startup/request của các lần chạy thử
  - `smoke*.log`: log kiểm tra nhanh, có bản ghi lỗi startup do logging/webroot ở một lần chạy cũ
- Đây không phải mã nguồn, không ảnh hưởng runtime nếu không được đọc thủ công.
- Hàm: không có.

### `QLKH_LTW/Views/_ViewImports.cshtml`

- Chức năng: bật Tag Helper MVC cho các view ngoài area.
- Vai trò thực tế:
  - hiện route root redirect vào `/Admin`, nên file này gần như là phần hạ tầng dự phòng
- Hàm: không có.

---

## 4. Domain và DTO

### `QLKH_LTW/Domain/AdminEntities.cs`

- Chức năng: chứa toàn bộ enum, entity domain, store in-memory, helper nhãn và exception nghiệp vụ.
- Thành phần chính:
  - enum:
    - `RoleCode`
    - `CourseStatusCode`
    - `LessonStatusCode`
    - `EnrollmentStatusCode`
    - `PaymentMethodCode`
    - `PaymentStatusCode`
  - entity:
    - `AccountEntity`
    - `TeacherEntity`
    - `StudentEntity`
    - `CourseLevelEntity`
    - `CourseCategoryEntity`
    - `CourseTeacherAssignmentEntity`
    - `CourseEntity`
    - `ChapterEntity`
    - `LessonEntity`
    - `EnrollmentEntity`
    - `PaymentEntity`
  - `AdminDataStore`
    - là “snapshot dữ liệu admin” chứa toàn bộ list entity và các bộ đếm `Next*Id`
  - `AdminLabels`
    - helper để đổi enum thành nhãn hiển thị
  - `AdminModuleException`
    - exception nghiệp vụ có kèm HTTP status code
- Hàm:
  - `RoleName(RoleCode role)`
    - đổi enum role sang tên hiển thị `"Admin"`, `"Teacher"`, `"Student"`
  - `CourseStatusName(CourseStatusCode status)`
    - đổi trạng thái course sang nhãn tiếng Việt
  - `LessonStatusName(LessonStatusCode status)`
    - đổi trạng thái lesson sang nhãn tiếng Việt
  - `EnrollmentStatusName(EnrollmentStatusCode status)`
    - đổi trạng thái enrollment sang nhãn tiếng Việt
  - `PaymentStatusName(PaymentStatusCode status)`
    - đổi trạng thái payment sang nhãn tiếng Việt
  - `PaymentMethodName(PaymentMethodCode method)`
    - đổi phương thức thanh toán sang nhãn tiếng Việt
  - `AdminModuleException(string message, int statusCode = 400)`
    - tạo lỗi nghiệp vụ có thông điệp và mã HTTP đi kèm

### `QLKH_LTW/Dtos/AdminDtos.cs`

- Chức năng: định nghĩa DTO cho UI và API.
- Nhóm DTO:
  - lookup:
    - `LookupItemDto`
    - `LookupsDto`
  - dashboard:
    - `DashboardMetricDto`
    - `WeeklyRevenueDto`
    - `CourseAttentionDto`
    - `RecentAccountDto`
    - `RecentPaymentDto`
    - `AdminDashboardDto`
  - account:
    - `AccountSummaryDto`
    - `AccountDetailDto`
    - `CreateAccountRequest`
    - `UpdateAccountRequest`
    - `UpdateAccountStatusRequest`
  - teacher:
    - `TeacherSummaryDto`
    - `TeacherCourseAssignmentDto`
    - `TeacherDetailDto`
    - `CreateTeacherRequest`
    - `UpdateTeacherRequest`
  - student:
    - `StudentEnrollmentPreviewDto`
    - `StudentSummaryDto`
    - `StudentDetailDto`
    - `UpdateStudentRequest`
  - course:
    - `CourseTeacherDto`
    - `CourseSummaryDto`
    - `CourseChapterDto`
    - `CourseLessonDto`
    - `CourseDetailDto`
    - `CreateCourseRequest`
    - `UpdateCourseRequest`
    - `AssignTeacherRequest`
    - `UpdateCourseStatusRequest`
    - `ToggleFeaturedRequest`
  - enrollment:
    - `EnrollmentSummaryDto`
    - `EnrollmentPaymentDto`
    - `EnrollmentDetailDto`
    - `UpdateEnrollmentStatusRequest`
  - payment:
    - `PaymentSummaryDto`
    - `PaymentDetailDto`
    - `UpdatePaymentStatusRequest`
- Hàm: không có; file này là “hợp đồng dữ liệu”.

---

## 5. Infrastructure

### `QLKH_LTW/Infrastructure/IAdminRepository.cs`

- Chức năng: interface trừu tượng cho tầng lưu trữ.
- Hàm:
  - `QueryAsync<T>(Func<AdminDataStore, T> query)`
    - chạy hàm đọc dữ liệu trên `AdminDataStore`
    - không làm thay đổi store
  - `UpdateAsync<T>(Func<AdminDataStore, T> action)`
    - chạy hàm thay đổi dữ liệu
    - sau đó persist xuống backend

### `QLKH_LTW/Infrastructure/AdminRepository.cs`

- Chức năng: repository chính; chịu trách nhiệm nạp/lưu dữ liệu và quyết định dùng SQL hay JSON.
- Ý tưởng thiết kế:
  - class là `partial`, phần JSON/file nằm ở đây, phần SQL nằm trong `AdminRepository.Sql.cs`
  - dùng `SemaphoreSlim _lock` để mọi thao tác đọc/ghi tuần tự, tránh race condition
- Hàm:
  - `AdminRepository(...)`
    - xác định `App_Data/admin-module-data.json`
    - đọc `DefaultConnection`
    - nếu có connection string thì bật `_useSqlBackend`
    - cấu hình JSON serializer
    - nạp dữ liệu ban đầu bằng `LoadStoreAsync()`
  - `QueryAsync<T>(...)`
    - khóa truy cập
    - nếu đang dùng SQL thì reload dữ liệu mới nhất từ DB
    - chạy delegate đọc dữ liệu và trả kết quả
  - `UpdateAsync<T>(...)`
    - khóa truy cập
    - reload dữ liệu nếu đang dùng SQL
    - chạy delegate sửa dữ liệu
    - chuẩn hóa bộ đếm `Next*Id`
    - lưu store lại
    - nếu đang dùng SQL thì reload lại sau khi save để đồng bộ
  - `LoadStoreAsync()`
    - nếu dùng SQL: gọi `LoadStoreFromSqlAsync()`
    - nếu dùng JSON: đọc file JSON
    - nếu chưa có file: tạo seed mặc định bằng `CreateSeedStore()`
  - `SaveStoreAsync()`
    - nếu dùng SQL: gọi `SaveStoreToSqlAsync(_store)`
    - nếu dùng JSON: ghi lại file JSON
  - `EnsureNextIds(AdminDataStore store)`
    - tính lại `NextAccountId`, `NextTeacherId`, ... dựa trên ID lớn nhất hiện có
    - tránh đụng độ ID khi thêm bản ghi mới
  - `CreateSeedStore()`
    - tạo dữ liệu mẫu gốc cho app khi chưa có dữ liệu
    - gọi chuỗi hàm seed từng bảng
  - `SeedAccounts(...)`
    - seed tài khoản admin/teacher/student mẫu
  - `SeedTeachers(...)`
    - seed hồ sơ teacher
  - `SeedStudents(...)`
    - seed hồ sơ student
  - `SeedCourses(...)`
    - seed course mẫu
  - `SeedCourseAssignments(...)`
    - seed quan hệ course-teacher
  - `SeedChapters(...)`
    - seed chapter của course
  - `SeedLessons(...)`
    - seed lesson theo chapter
  - `SeedEnrollments(...)`
    - seed lượt ghi danh
  - `SeedPayments(...)`
    - seed payment tương ứng enrollment
  - `HashPassword(string value)`
    - băm SHA256 password seed trước khi lưu

### `QLKH_LTW/Infrastructure/AdminRepository.Sql.cs`

- Chức năng: nửa còn lại của `AdminRepository`, chuyên xử lý SQL Server.
- Ý nghĩa tổng thể:
  - đọc toàn bộ dữ liệu từ SQL vào `AdminDataStore`
  - ghi toàn bộ snapshot `AdminDataStore` ngược lại vào SQL
  - có cơ chế thử nhiều biến thể connection string cho SQL local named instance
  - tự bổ sung cột `IsVisible` cho `Teacher` và `Student` nếu DB cũ chưa có
- Nhóm hàm đọc dữ liệu:
  - `LoadStoreFromSqlAsync()`
    - đọc lần lượt `CourseLevel`, `CourseCategory`, `Account`, `Teacher`, `Student`, `Course`, `CourseTeacher`, `Chapter`, `Lesson`, `Enrollment`, `Payment`
    - ánh xạ dữ liệu SQL sang entity
    - cuối cùng gọi `EnsureNextIds`
  - `ReadListAsync<T>(SqlConnection connection, string sql, Func<SqlDataReader, T> map)`
    - helper chung để chạy query và map nhiều dòng về list
  - `ReadNullableString(...)`
    - đọc chuỗi nullable
  - `ReadNullableInt32(...)`
    - đọc số nguyên nullable
  - `ReadNullableDateTime(...)`
    - đọc `DateTime?`
  - `ReadNullableDateOnly(...)`
    - đọc `DateOnly?`
  - `ParseRoleCode(...)`
  - `ParseCourseStatusCode(...)`
  - `ParseLessonStatusCode(...)`
  - `ParseEnrollmentStatusCode(...)`
  - `ParsePaymentMethodCode(...)`
  - `ParsePaymentStatusCode(...)`
    - toàn bộ nhóm `Parse*Code` đổi mã string trong DB về enum domain
- Nhóm hàm kết nối SQL:
  - `OpenConnectionAsync()`
    - thử mở kết nối SQL bằng nhiều candidate connection string
    - nếu tất cả thất bại thì tạo exception dễ hiểu hơn
  - `BuildConnectionStringCandidates(string connectionString)`
    - tạo nhiều biến thể data source từ named instance local
    - ví dụ thử `localhost\\INSTANCE`, `.\\INSTANCE`, `(local)\\INSTANCE`, hoặc TCP port thật
  - `AddConnectionStringCandidate(...)`
    - tránh thêm candidate trùng
  - `CreateSqlConnectionException(...)`
    - gói lỗi kết nối thành thông điệp dễ chẩn đoán
  - `IsSspiContextFailure(Exception? exception)`
    - nhận diện lỗi Windows Authentication kiểu SSPI/Kerberos
  - `BuildSqlLoginConnectionExample(SqlConnectionStringBuilder builder)`
    - sinh ví dụ connection string dùng SQL Login qua TCP
  - `TryParseNamedInstance(...)`
    - tách `serverHost` và `instanceName` từ data source dạng `SERVER\\INSTANCE`
  - `IsLocalSqlServer(string serverHost)`
    - kiểm tra data source có phải SQL Server local không
  - `GetLocalInstanceTcpPorts(string instanceName)`
    - cố lấy port TCP từ registry Windows cho instance local
  - `TryGetPreferredLocalTcpPort(string dataSource)`
    - lấy port ưu tiên nếu data source là named instance local
  - `TryReadRegistryTcpPort(string instanceName, string valueName)`
    - đọc registry SQL Server để lấy `TcpPort` hoặc `TcpDynamicPorts`
- Nhóm hàm ghi dữ liệu:
  - `SaveStoreToSqlAsync(AdminDataStore store)`
    - mở transaction
    - lần lượt upsert account, teacher, student, course, course-teacher, chapter, lesson, enrollment, payment
    - commit nếu thành công
  - `UpsertAccountsAsync(...)`
    - insert/update bảng `Account`
  - `UpsertTeachersAsync(...)`
    - insert/update bảng `Teacher`
  - `UpsertStudentsAsync(...)`
    - insert/update bảng `Student`
  - `EnsureVisibilityColumnsAsync(...)`
    - nếu DB cũ thiếu `IsVisible` ở `Teacher` hoặc `Student` thì tự `ALTER TABLE`
  - `SyncStudentsAsync(...)`
    - xóa student không còn tồn tại trong store rồi upsert lại danh sách hiện tại
  - `UpsertCoursesAsync(...)`
    - insert/update `Course`, resolve foreign key theo code level/category/status
  - `ReplaceCourseTeachersAsync(...)`
    - xóa toàn bộ gán teacher cũ của course rồi insert danh sách mới
  - `UpsertChaptersAsync(...)`
    - insert/update `Chapter`
  - `UpsertLessonsAsync(...)`
    - insert/update `Lesson`
  - `UpsertEnrollmentsAsync(...)`
    - insert/update `Enrollment`
  - `UpsertPaymentsAsync(...)`
    - insert/update `Payment`
  - `ExecuteNonQueryAsync(...)`
    - helper chạy câu lệnh SQL không trả data trong transaction
  - `DbValue(string? value)`, `DbValue(DateTime? value)`, `DbValue(int? value)`
    - đổi `null` sang `DBNull.Value`
  - `ToRoleCode(...)`
  - `ToCourseStatusCode(...)`
  - `ToLessonStatusCode(...)`
  - `ToEnrollmentStatusCode(...)`
  - `ToPaymentMethodCode(...)`
  - `ToPaymentStatusCode(...)`
    - nhóm hàm đổi enum domain sang string code để ghi DB

---

## 6. Service

### `QLKH_LTW/Services/IAdminService.cs`

- Chức năng: interface mô tả toàn bộ nghiệp vụ admin.
- Nhóm hàm:
  - dashboard/lookups
  - accounts
  - teachers
  - students
  - courses
  - enrollments
  - payments
- File này không có cài đặt; chỉ khai báo hợp đồng.

### `QLKH_LTW/Services/AdminService.cs`

- Chức năng: cài đặt nghiệp vụ dashboard, lookups, account, teacher, student và một số thao tác “wrapper”.
- Hàm:
  - `AdminService(IAdminRepository repository)`
    - inject repository
  - `GetLookupsAsync()`
    - dựng danh sách role, course status, enrollment status, payment status, payment method
    - lấy level/category trực tiếp từ store
  - `GetDashboardAsync()`
    - tính metric dashboard
    - tính doanh thu 30 ngày gần đây
    - tính thống kê 6 tuần gần nhất
    - xác định course cần chú ý
    - lấy 5 account mới nhất và 5 payment mới nhất
  - `GetAccountsAsync(string? search, string? roleCode, bool? isActive)`
    - lọc account theo từ khóa, role, trạng thái active
    - trả list đã map sang `AccountSummaryDto`
  - `GetAccountAsync(long accountId)`
    - lấy chi tiết một account
  - `CreateAccountAsync(CreateAccountRequest request)`
    - validate field bắt buộc
    - kiểm tra username/email duy nhất
    - parse role
    - băm password
    - tạo account mới
  - `UpdateAccountAsync(long accountId, UpdateAccountRequest request)`
    - validate field bắt buộc
    - kiểm tra email không trùng
    - cập nhật thông tin account và role
  - `UpdateAccountStatusAsync(long accountId, UpdateAccountStatusRequest request)`
    - khóa/mở khóa account
  - `GetTeachersAsync(string? search, bool includeHidden = false)`
    - lọc teacher theo `search`
    - mặc định ẩn teacher có `IsVisible = false`
  - `GetTeacherAsync(long teacherId)`
    - lấy chi tiết teacher kèm course đang phụ trách
  - `CreateTeacherAsync(CreateTeacherRequest request)`
    - chỉ cho phép tạo hồ sơ teacher từ account có role `TEACHER`
    - cấm tạo 2 profile teacher cho cùng một account
  - `UpdateTeacherAsync(long teacherId, UpdateTeacherRequest request)`
    - cập nhật điện thoại, chuyên môn, bio, số năm kinh nghiệm, cờ public profile
  - `UpdateTeacherVisibilityAsync(long teacherId, bool isVisible)`
    - ẩn/hiện teacher khỏi danh sách
  - `GetStudentsAsync(string? search, bool includeHidden = false)`
    - lọc student theo tên, email, phone
    - mặc định ẩn student có `IsVisible = false`
  - `GetStudentAsync(long studentId)`
    - lấy student kèm danh sách enrollment
  - `UpdateStudentAsync(long studentId, UpdateStudentRequest request)`
    - cập nhật phone, birthday, bio
  - `UpdateStudentVisibilityAsync(long studentId, bool isVisible)`
    - ẩn/hiện student
  - `DeleteTeacherAsync(long teacherId)`
    - thực chất là soft-delete bằng cách gọi `UpdateTeacherVisibilityAsync(..., false)`
  - `DeleteStudentAsync(long studentId)`
    - soft-delete bằng `UpdateStudentVisibilityAsync(..., false)`
  - `ArchiveCourseAsync(long courseId)`
    - wrapper gọi `UpdateCourseStatusAsync(..., "ARCHIVED")`

### `QLKH_LTW/Services/AdminService.CourseOps.cs`

- Chức năng: cài đặt nghiệp vụ course, enrollment, payment.
- Hàm:
  - `GetCoursesAsync(...)`
    - lọc course theo từ khóa, trạng thái, category, teacher
    - nếu không chọn trạng thái và `includeArchived = false` thì tự loại course archived
  - `GetCourseAsync(long courseId)`
    - lấy chi tiết một course kèm chapter/lesson/checklist
  - `CreateCourseAsync(CreateCourseRequest request)`
    - validate field bắt buộc
    - kiểm tra trùng `CourseCode` và `Slug`
    - xác minh `LevelCode`, `CategoryCode`, `TeacherId`, `CreatedByAccountId`
    - course mới luôn bắt đầu ở trạng thái `Draft`
    - sau khi tạo sẽ gán teacher chính
  - `UpdateCourseAsync(long courseId, UpdateCourseRequest request)`
    - validate field bắt buộc
    - kiểm tra slug không trùng
    - cập nhật metadata, giá, level/category, cờ featured (Đã fix lỗi binding - xem mục 14)
  - `AssignTeacherAsync(long courseId, AssignTeacherRequest request)`
    - thay teacher chính của course
  - `UpdateCourseStatusAsync(long courseId, UpdateCourseStatusRequest request)`
    - đổi trạng thái course
    - nếu chuyển sang `Published` và chưa có `PublishedAt` thì gắn thời điểm xuất bản
  - `ToggleFeaturedAsync(long courseId, ToggleFeaturedRequest request)`
    - bật/tắt cờ course nổi bật (Đã fix logic truyền dữ liệu từ View - xem mục 14)
  - `GetEnrollmentsAsync(string? statusCode, long? courseId, long? studentId)`
    - lọc enrollment theo trạng thái, course, student
  - `GetEnrollmentAsync(long enrollmentId)`
    - lấy chi tiết enrollment kèm payment liên quan
  - `UpdateEnrollmentStatusAsync(long enrollmentId, UpdateEnrollmentStatusRequest request)`
    - đổi trạng thái enrollment
    - nếu chuyển `Active` thì set `ActivatedAt`
    - nếu chuyển `Cancelled` thì set `CancelledAt` và `CancelReason`
    - nếu trạng thái khác `Refunded` thì dọn dữ liệu hủy cũ
  - `GetPaymentsAsync(string? search, string? statusCode, string? methodCode)`
    - lọc payment theo transaction/note/tên học viên
    - lọc thêm theo status/method
  - `GetPaymentAsync(long paymentId)`
    - lấy chi tiết payment
  - `UpdatePaymentStatusAsync(long paymentId, UpdatePaymentStatusRequest request)`
    - đổi trạng thái payment
    - đồng bộ trạng thái enrollment liên quan:
      - `Success` -> enrollment thành `Active`
      - `Refunded` -> enrollment thành `Refunded`
      - `Pending`/`Failed` -> nếu course có phí và enrollment chưa cancelled/refunded thì enrollment về `PendingPayment`

### `QLKH_LTW/Services/AdminService.Helpers.cs`

- Chức năng: gom các hàm phụ trợ map dữ liệu, validate, tìm entity và dựng checklist.
- Hàm map và dựng dữ liệu:
  - `BuildCourseAttention(...)`
    - tạo DTO cảnh báo course cần xử lý, gồm teacher, số lesson draft, checklist xuất bản
  - `MapAccountSummary(...)`
  - `MapAccountDetail(...)`
  - `MapTeacherSummary(...)`
  - `MapTeacherDetail(...)`
  - `MapStudentSummary(...)`
  - `MapStudentDetail(...)`
  - `MapCourseSummary(...)`
  - `MapCourseDetail(...)`
  - `MapEnrollmentSummary(...)`
  - `MapEnrollmentDetail(...)`
  - `MapPaymentSummary(...)`
  - `MapPaymentDetail(...)`
    - nhóm `Map*` đổi entity store sang DTO cho controller/UI/API
- Hàm validate và integrity:
  - `EnsureUniqueAccount(...)`
    - đảm bảo username/email không trùng
  - `EnsureCourseCodeAndSlugUnique(...)`
    - đảm bảo `CourseCode` và `Slug` không trùng
  - `EnsureCourseMetadataExists(...)`
    - đảm bảo `LevelCode` và `CategoryCode` tồn tại
  - `ReplaceCourseTeacher(...)`
    - xóa teacher cũ của course và gắn teacher mới làm primary
  - `GetCourseLessons(...)`
    - gom toàn bộ lesson của một course qua chapter
  - `BuildPublishChecklist(...)`
    - sinh checklist trước khi xuất bản:
      - còn lesson draft hay không
      - thiếu intro video không
      - thiếu summary lesson không
      - thiếu thumbnail không
      - có lesson nào chưa
  - `TryGetPrimaryTeacher(...)`
    - trả teacher chính của course nếu có
- Hàm tìm entity:
  - `FindAccount(...)`
  - `FindTeacher(...)`
  - `FindStudent(...)`
  - `FindCourse(...)`
  - `FindEnrollment(...)`
  - `FindPayment(...)`
  - `FindLevel(...)`
  - `FindCategory(...)`
    - nếu không thấy sẽ ném `AdminModuleException`
- Hàm tiện ích:
  - `Contains(string? value, string keyword)`
    - tìm kiếm không phân biệt hoa thường
  - `ValidateRequired(string? value, string fieldName)`
    - kiểm tra field bắt buộc
  - `TrimOrNull(string? value)`
    - trim chuỗi hoặc trả `null`
  - `Hash(string value)`
    - băm SHA256 password hoặc chuỗi cần lưu

### `QLKH_LTW/Services/AdminService.Codes.cs`

- Chức năng: chuyển đổi qua lại giữa enum nội bộ và string code cho request/response.
- Hàm:
  - `ParseRole(string value)`
  - `ParseCourseStatus(string value)`
  - `ParseEnrollmentStatus(string value)`
  - `ParsePaymentStatus(string value)`
  - `ParsePaymentMethod(string value)`
    - nhóm `Parse*` đổi chuỗi request như `"ADMIN"`, `"PUBLISHED"`, `"VNPAY"` sang enum
  - `ToCode(RoleCode role)`
  - `ToCode(CourseStatusCode status)`
  - `ToCode(LessonStatusCode status)`
  - `ToCode(EnrollmentStatusCode status)`
  - `ToCode(PaymentStatusCode status)`
  - `ToCode(PaymentMethodCode method)`
    - nhóm `ToCode` đổi enum về string code ổn định cho UI/API

---

## 7. API Controllers (`QLKH_LTW/Controllers`)

### `Controllers/DashboardController.cs`

- Chức năng: API dashboard và lookup.
- Hàm:
  - `GetDashboard()`
    - trả `AdminDashboardDto`
  - `GetLookups()`
    - trả các danh mục lookup để UI/API client dùng filter/form

### `Controllers/AccountsController.cs`

- Chức năng: REST API quản lý account.
- Hàm:
  - `GetAccounts(...)`
    - lấy danh sách account có lọc
  - `GetAccount(long accountId)`
    - lấy chi tiết account
  - `CreateAccount(CreateAccountRequest request)`
    - tạo account mới và trả `201 Created`
  - `UpdateAccount(long accountId, UpdateAccountRequest request)`
    - cập nhật account
  - `UpdateStatus(long accountId, UpdateAccountStatusRequest request)`
    - khóa/mở khóa account

### `Controllers/TeachersController.cs`

- Chức năng: REST API teacher.
- Hàm:
  - `GetTeachers(...)`
  - `GetTeacher(long teacherId)`
  - `CreateTeacher(CreateTeacherRequest request)`
  - `UpdateTeacher(long teacherId, UpdateTeacherRequest request)`

### `Controllers/StudentsController.cs`

- Chức năng: REST API student.
- Hàm:
  - `GetStudents(string? search)`
  - `GetStudent(long studentId)`
  - `UpdateStudent(long studentId, UpdateStudentRequest request)`

### `Controllers/CoursesController.cs`

- Chức năng: REST API course.
- Hàm:
  - `GetCourses(...)`
    - lấy list course có filter
  - `GetCourse(long courseId)`
  - `CreateCourse(CreateCourseRequest request)`
  - `UpdateCourse(long courseId, UpdateCourseRequest request)`
  - `AssignTeacher(long courseId, AssignTeacherRequest request)`
  - `UpdateStatus(long courseId, UpdateCourseStatusRequest request)`
  - `ToggleFeatured(long courseId, ToggleFeaturedRequest request)`

### `Controllers/EnrollmentsController.cs`

- Chức năng: REST API enrollment.
- Hàm:
  - `GetEnrollments(...)`
  - `GetEnrollment(long enrollmentId)`
  - `UpdateStatus(long enrollmentId, UpdateEnrollmentStatusRequest request)`

### `Controllers/PaymentsController.cs`

- Chức năng: REST API payment.
- Hàm:
  - `GetPayments(...)`
  - `GetPayment(long paymentId)`
  - `UpdateStatus(long paymentId, UpdatePaymentStatusRequest request)`

---

## 8. Admin MVC Models và Component

### `QLKH_LTW/Areas/Admin/Models/AdminMenuItem.cs`

- Chức năng: model đại diện một mục menu sidebar.
- Dùng để dựng menu điều hướng trong component `AdminMenuViewComponent`.
- Hàm: không có.

### `QLKH_LTW/Areas/Admin/Models/AdminViewModels.cs`

- Chức năng: chứa toàn bộ ViewModel cho Razor Admin và các helper factory/format.
- Nhóm lớp dữ liệu:
  - hằng số:
    - `AdminUiDefaults`
  - view model cho dashboard:
    - `AdminDashboardPageViewModel`
  - accounts:
    - `AccountFilterViewModel`
    - `AccountIndexViewModel`
    - `AccountDetailsViewModel`
    - `AccountFormViewModel`
  - teachers:
    - `TeacherIndexViewModel`
    - `TeacherFormViewModel`
    - `TeacherDetailsViewModel`
  - students:
    - `StudentIndexViewModel`
    - `StudentFormViewModel`
    - `StudentDetailsViewModel`
  - courses:
    - `CourseFilterViewModel`
    - `CourseIndexViewModel`
    - `CourseFormViewModel`
    - `CourseStatusFormViewModel`
    - `CourseTeacherFormViewModel`
    - `CourseDetailsViewModel`
  - enrollments:
    - `EnrollmentFilterViewModel`
    - `EnrollmentIndexViewModel`
    - `EnrollmentStatusFormViewModel`
    - `EnrollmentDetailsViewModel`
  - payments:
    - `PaymentFilterViewModel`
    - `PaymentIndexViewModel`
    - `PaymentStatusFormViewModel`
    - `PaymentDetailsViewModel`
- Hàm trong `AdminViewModelFactory`:
  - `ToSelectList(...)`
    - đổi lookup DTO thành `SelectListItem`
  - `ToTeacherOptions(...)`
    - build combobox teacher
  - `ToAccountOptions(...)`
    - build combobox account teacher
  - `ToCourseOptions(...)`
    - build combobox course
  - `ToStudentOptions(...)`
    - build combobox student
  - `ToBoolOptions(bool? selectedValue)`
    - build option lọc trạng thái active/inactive
  - `ToAccountForm(...)`
    - map `AccountDetailDto` -> `AccountFormViewModel`
  - `ToTeacherForm(...)`
    - map `TeacherDetailDto` -> `TeacherFormViewModel`
  - `ToStudentForm(...)`
    - map `StudentDetailDto` -> `StudentFormViewModel`
  - `ToCourseForm(...)`
    - map `CourseDetailDto` -> `CourseFormViewModel`
  - `ToEnrollmentStatusForm(...)`
    - map `EnrollmentDetailDto` -> form đổi trạng thái enrollment
  - `ToCourseStatusForm(...)`
    - map `CourseDetailDto` -> form đổi trạng thái course
  - `ToCourseTeacherForm(...)`
    - map `CourseDetailDto` -> form đổi teacher chính
  - `ToPaymentStatusForm(...)`
    - map `PaymentDetailDto` -> form cập nhật payment
- Hàm trong `AdminDisplay`:
  - `BadgeClass(string? code)`
    - đổi status/role code sang tên class badge CSS
  - `Currency(decimal amount)`
    - format tiền kiểu `1,234,000đ`
  - `DateTimeOrDash(DateTime? value)`
    - format thời gian local, null thì `-`
  - `DateOnlyOrDash(DateOnly? value)`
    - format ngày, null thì `-`

### `QLKH_LTW/Areas/Admin/Components/AdminMenuViewComponent.cs`

- Chức năng: component dựng sidebar menu admin.
- Hàm:
  - `Invoke()`
    - tạo danh sách menu tĩnh: Dashboard, Accounts, Teachers, Students, Courses, Enrollments, Payments
    - trả view `Shared/Components/AdminMenu/Default.cshtml`

---

## 9. Admin MVC Controllers (`QLKH_LTW/Areas/Admin/Controllers`)

### `Areas/Admin/Controllers/AdminControllerBase.cs`

- Chức năng: base class dùng chung cho toàn bộ controller UI admin.
- Hàm:
  - `SetSuccess(string message)`
    - ghi thông báo thành công vào `TempData`
  - `AddServiceError(AdminModuleException exception)`
    - thêm lỗi service vào `ModelState`
  - `HandleGetException(AdminModuleException exception)`
    - nếu lỗi `404` thì trả `NotFound`, còn lại trả `BadRequest`
  - `GetLookupsAsync()`
    - gọi service lấy lookups
  - `GetRoleOptionsAsync(...)`
  - `GetCourseStatusOptionsAsync(...)`
  - `GetEnrollmentStatusOptionsAsync(...)`
  - `GetPaymentStatusOptionsAsync(...)`
  - `GetPaymentMethodOptionsAsync(...)`
  - `GetCategoryOptionsAsync(...)`
  - `GetLevelOptionsAsync(...)`
  - `GetTeacherOptionsAsync(...)`
  - `GetCourseOptionsAsync(...)`
  - `GetStudentOptionsAsync(...)`
  - `GetTeacherAccountOptionsAsync(...)`
    - nhóm hàm này chuẩn hóa việc build option cho dropdown trong form/filter

### `Areas/Admin/Controllers/HomeController.cs`

- Chức năng: controller UI dashboard.
- Hàm:
  - `Index()`
    - lấy `AdminDashboardDto`
    - gắn vào `AdminDashboardPageViewModel`
    - render dashboard `/Admin`

### `Areas/Admin/Controllers/AccountsController.cs`

- Chức năng: UI quản lý account.
- Hàm:
  - `Index(AccountFilterViewModel filter)`
    - hiển thị danh sách account theo filter
  - `Details(long id)`
    - hiển thị chi tiết một account
  - `Create()` (GET)
    - mở form tạo account, nạp `RoleOptions`
  - `Create(AccountFormViewModel form)` (POST)
    - validate form
    - gọi service tạo account
    - redirect sang trang chi tiết nếu thành công
  - `Edit(long id)` (GET)
    - lấy dữ liệu account và fill vào form sửa
  - `Edit(long id, AccountFormViewModel form)` (POST)
    - validate form
    - gọi service cập nhật account
  - `ToggleStatus(long id, bool isActive)`
    - khóa hoặc mở khóa account
    - ưu tiên quay lại trang trước theo `Referer`
  - `BuildIndexViewModelAsync(AccountFilterViewModel filter)`
    - helper dựng ViewModel cho trang index, kể cả trong trường hợp service ném lỗi

### `Areas/Admin/Controllers/TeachersController.cs`

- Chức năng: UI teacher.
- Hàm:
  - `Index(string? search, bool includeHidden = false)`
    - trang danh sách teacher
  - `Details(long id)`
    - trang chi tiết teacher
  - `Create()` (GET)
    - mở form tạo hồ sơ teacher
  - `Create(TeacherFormViewModel form)` (POST)
    - tạo teacher profile cho một account teacher chưa có hồ sơ
  - `Edit(long id)` (GET)
    - mở form sửa
  - `Edit(long id, TeacherFormViewModel form)` (POST)
    - cập nhật hồ sơ teacher
  - `Delete(long id)`
    - soft-delete teacher bằng cách ẩn khỏi danh sách
  - `ToggleVisibility(long id, bool isVisible)`
    - ẩn/hiện teacher

### `Areas/Admin/Controllers/StudentsController.cs`

- Chức năng: UI student.
- Hàm:
  - `Index(string? search, bool includeHidden = false)`
    - trang danh sách student
  - `Details(long id)`
    - trang chi tiết student
  - `Edit(long id)` (GET)
    - mở form sửa student
  - `Edit(long id, StudentFormViewModel form)` (POST)
    - cập nhật thông tin student
  - `Delete(long id)`
    - soft-delete bằng ẩn khỏi danh sách
  - `ToggleVisibility(long id, bool isVisible)`
    - ẩn/hiện student

### `Areas/Admin/Controllers/CoursesController.cs`

- Chức năng: UI course; đây là controller UI nhiều nghiệp vụ nhất.
- Hàm:
  - `Index(CourseFilterViewModel filter)`
    - hiển thị danh sách course theo filter
  - `Details(long id)`
    - hiển thị chi tiết course
  - `Create()` (GET)
    - tạo form rỗng và nạp select options
  - `Create(CourseFormViewModel form)` (POST)
    - validate form
    - gọi service tạo course
  - `Edit(long id)` (GET)
    - lấy course hiện có và fill form sửa
  - `Edit(long id, CourseFormViewModel form)` (POST)
    - cập nhật metadata course
    - cập nhật status riêng
    - gán lại teacher nếu có
  - `UpdateStatus(long id, CourseStatusFormViewModel form)`
    - đổi trạng thái course ngay tại trang chi tiết
  - `AssignTeacher(long id, CourseTeacherFormViewModel form)`
    - đổi teacher chính từ trang chi tiết
  - `ToggleFeatured(long id, bool isFeatured)`
    - bật/tắt featured
  - `Archive(long id)`
    - đưa course về `ARCHIVED`
  - `Restore(long id)`
    - hiện lại course, đồng thời đưa trạng thái về `DRAFT`
  - `Delete(long id)`
    - hiện tại không xóa cứng; chỉ gọi `ArchiveCourseAsync`
  - `BuildIndexViewModelAsync(CourseFilterViewModel filter)`
    - helper dựng dữ liệu trang index
  - `BuildCourseFormAsync(CourseFormViewModel form)`
    - helper nạp dropdown options vào form
  - `BuildDetailsViewModelAsync(...)`
    - helper tạo data cho trang chi tiết course, gồm form status và form teacher

### `Areas/Admin/Controllers/EnrollmentsController.cs`

- Chức năng: UI enrollment.
- Hàm:
  - `Index(EnrollmentFilterViewModel filter)`
    - hiển thị danh sách enrollment theo trạng thái/course/student
  - `Details(long id)`
    - hiển thị chi tiết enrollment
  - `UpdateStatus(long id, EnrollmentStatusFormViewModel form)`
    - cập nhật trạng thái enrollment
  - `BuildDetailsViewModelAsync(...)`
    - helper dựng form status với dữ liệu hiện tại hoặc dữ liệu bị lỗi submit

### `Areas/Admin/Controllers/PaymentsController.cs`

- Chức năng: UI payment.
- Hàm:
  - `Index(PaymentFilterViewModel filter)`
    - hiển thị danh sách payment với filter theo search/status/method
  - `Details(long id)`
    - hiển thị chi tiết payment
  - `UpdateStatus(long id, PaymentStatusFormViewModel form)`
    - cập nhật payment và đồng bộ enrollment qua service
  - `BuildDetailsViewModelAsync(...)`
    - helper dựng dữ liệu cho trang chi tiết payment và form cập nhật trạng thái

### `Areas/Admin/Controllers/SeedController.cs`

- Chức năng: seed dữ liệu demo lớn hơn bộ seed cơ bản trong repository.
- Ý nghĩa:
  - route `/Admin/Seed`
  - dùng `UpdateAsync` để ghi trực tiếp vào store hiện tại
  - tạo hoặc cập nhật 7 bản ghi mẫu cho từng nhóm dữ liệu chính
  - có tính idempotent tương đối vì tìm theo username, course code, quan hệ sẵn có
- Hàm:
  - `Index()`
    - điều phối toàn bộ quy trình seed
    - trả JSON báo thành công/thất bại
  - `SeedAccounts(...)`
    - tạo/cập nhật 7 account mẫu
  - `SeedTeachers(...)`
    - tạo/cập nhật 7 teacher profile
  - `SeedStudents(...)`
    - tạo/cập nhật 7 student profile
  - `SeedCourses(...)`
    - tạo/cập nhật 7 course mẫu giàu nội dung hơn seed mặc định
  - `SeedCourseTeachers(...)`
    - gán teacher chính cho từng course
  - `SeedChapters(...)`
    - tạo chapter đầu tiên cho mỗi course
  - `SeedLessons(...)`
    - tạo lesson đầu tiên cho từng chapter
  - `SeedEnrollments(...)`
    - tạo enrollment với nhiều trạng thái khác nhau
  - `SeedPayments(...)`
    - tạo payment tương ứng, bao phủ success/pending/failed/refunded
  - `HashPassword(string value)`
    - băm password seed bằng SHA256

---

## 10. Razor Views (`QLKH_LTW/Areas/Admin/Views`)

### View hạ tầng dùng chung

- `Areas/Admin/Views/_ViewImports.cshtml`
  - import namespace `QLKH_LTW.Areas.Admin.Models`, `QLKH_LTW.Dtos`
  - bật Tag Helpers
  - Hàm: không có.
- `Areas/Admin/Views/_ViewStart.cshtml`
  - đặt layout mặc định là `_LayoutAdmin`
  - Hàm: không có.
- `Areas/Admin/Views/Shared/_LayoutAdmin.cshtml`
  - layout chính của admin
  - nạp Bootstrap 5, Tabler Icons, `~/admin/css/admin-premium.css`, `~/admin/js/admin-premium.js`
  - chứa topbar, sidebar component, vùng render body, footer
  - Hàm: không có.
- `Areas/Admin/Views/Shared/_FlashMessages.cshtml`
  - đọc `TempData["SuccessMessage"]` và hiển thị alert thành công
  - Hàm: không có.
- `Areas/Admin/Views/Shared/Components/AdminMenu/Default.cshtml`
  - view của `AdminMenuViewComponent`
  - render sidebar, tô active item theo controller hiện tại
  - Hàm: không có.

### Dashboard

- `Areas/Admin/Views/Home/Index.cshtml`
  - trang dashboard chính
  - hiển thị metric card, chart tăng trưởng, course cần chú ý, account mới, payment gần đây
  - Hàm/JS:
    - `window.initDashboardCharts = function() { ... }`
      - lấy dữ liệu tuần từ model Razor
      - cấu hình ApexCharts
      - render chart vào `#growthChart`
    - khối `if (typeof ApexCharts !== 'undefined') { ... }`
      - tự render chart nếu thư viện đã sẵn sàng

### Accounts

- `Areas/Admin/Views/Accounts/_Form.cshtml`
  - partial form dùng chung cho create/edit account
  - create mode cho nhập `Username` và `Password`
  - edit mode khóa `Username`
  - Hàm: không có.
- `Areas/Admin/Views/Accounts/Create.cshtml`
  - bọc `_Form` trong card tạo mới
  - Hàm: không có.
- `Areas/Admin/Views/Accounts/Edit.cshtml`
  - bọc `_Form` trong card chỉnh sửa
  - Hàm: không có.
- `Areas/Admin/Views/Accounts/Details.cshtml`
  - hiển thị metadata account, role, trạng thái, avatar, timestamps
  - có nút khóa/mở khóa account
  - Hàm: không có.
- `Areas/Admin/Views/Accounts/Index.cshtml`
  - danh sách account
  - có ô tìm kiếm, filter role, filter active/inactive
  - render badge role/trạng thái và menu thao tác
  - Hàm: không có.

### Teachers

- `Areas/Admin/Views/Teachers/_Form.cshtml`
  - partial form tạo/sửa teacher
  - create mode cho chọn account teacher; edit mode khóa account hiện có
  - Hàm: không có.
- `Areas/Admin/Views/Teachers/Create.cshtml`
  - trang tạo hồ sơ teacher
  - Hàm: không có.
- `Areas/Admin/Views/Teachers/Edit.cshtml`
  - trang sửa teacher
  - Hàm: không có.
- `Areas/Admin/Views/Teachers/Details.cshtml`
  - hiển thị hồ sơ teacher và danh sách course phụ trách
  - có nút ẩn/hiện teacher
  - Hàm: không có.
- `Areas/Admin/Views/Teachers/Index.cshtml`
  - danh sách teacher, filter theo search và `includeHidden`
  - hiển thị chuyên môn, số khóa học, trạng thái hiển thị
  - Hàm: không có.

### Students

- `Areas/Admin/Views/Students/_Form.cshtml`
  - partial form sửa student
  - `FullName` và `Email` chỉ đọc
  - cho sửa `Phone`, `Birthday`, `Bio`
  - Hàm: không có.
- `Areas/Admin/Views/Students/Edit.cshtml`
  - trang sửa student
  - Hàm: không có.
- `Areas/Admin/Views/Students/Details.cshtml`
  - hiển thị hồ sơ student và bảng enrollment của học viên
  - có nút ẩn/hiện student
  - Hàm: không có.
- `Areas/Admin/Views/Students/Index.cshtml`
  - danh sách student, filter theo search và `includeHidden`
  - hiển thị số khóa học, trạng thái enrollment gần nhất
  - Hàm: không có.

### Courses

- `Areas/Admin/Views/Courses/_Form.cshtml`
  - partial form lớn nhất, chứa:
    - thông tin cơ bản
    - taxonomy và pricing
    - media/link
    - mô tả
    - trạng thái, featured, teacher
  - create mode cho nhập `CourseCode`, edit mode khóa `CourseCode`
  - Hàm: không có.
- `Areas/Admin/Views/Courses/Create.cshtml`
  - trang tạo course
  - Hàm: không có.
- `Areas/Admin/Views/Courses/Edit.cshtml`
  - trang sửa course
  - Hàm: không có.
- `Areas/Admin/Views/Courses/Details.cshtml`
  - hiển thị metadata course, chapter/lesson, checklist xuất bản
  - có form đổi trạng thái, form đổi teacher, nút archive/restore/featured
  - Hàm: không có.
- `Areas/Admin/Views/Courses/Index.cshtml`
  - danh sách course với filter theo search/status/category/teacher
  - hiển thị số lesson, số draft lesson, featured icon, menu thao tác
  - Hàm: không có.

### Enrollments

- `Areas/Admin/Views/Enrollments/Index.cshtml`
  - danh sách enrollment với filter theo trạng thái/course/student
  - có menu thao tác để vào chi tiết hoặc hủy nhanh
  - Hàm: không có.
- `Areas/Admin/Views/Enrollments/Details.cshtml`
  - hiển thị chi tiết enrollment, payment liên quan và form cập nhật trạng thái
  - Hàm: không có.

### Payments

- `Areas/Admin/Views/Payments/Index.cshtml`
  - danh sách payment với filter theo search/status/method
  - có thao tác vào chi tiết hoặc hoàn tiền nhanh
  - Hàm: không có.
- `Areas/Admin/Views/Payments/Details.cshtml`
  - hiển thị chi tiết payment và form đổi trạng thái, transaction no, gateway code, note
  - Hàm: không có.

---

## 11. Custom static files đang thực sự được layout dùng

### `QLKH_LTW/wwwroot/admin/css/admin-premium.css`

- Chức năng: stylesheet custom chính cho giao diện admin hiện tại.
- Phần chính:
  - `:root`
    - khai báo design tokens: màu, shadow, radius, font
  - `body`, `h1-h4`, `.logo-text`
    - thiết lập typography
  - `.sidebar-premium`, `.sidebar-collapsed`
    - style sidebar desktop/mobile
  - `#content-wrapper`, `.overlay`, `.topbar-glass`
    - layout chính, overlay và topbar
  - `.card-premium`, `.metric-card-content`, `.icon-box-premium`
    - style card dashboard
  - `.badge-*`
    - badge trạng thái
  - `.btn-subtle-primary`, `.admin-filter-area`, `.table-premium`
    - đồng nhất UI cho button, filter box, table
  - `.dropdown-menu`, `.action-dropup`
    - sửa clipping của dropdown trong table
  - `@keyframes fadeIn`, `slideUp`, `staggeredFade`, `actionMenuRise`, `skeleton-loading`
    - animation cho page/cards/table/dropdown/skeleton
  - `.animate-fade-in`, `.animate-slide-up`
    - class utility cho animation
  - `.form-control:focus`, `.form-select:focus`
    - focus style cho form
  - `.empty-state`
    - empty state dùng chung
- Hàm: không có; đây là file CSS thuần.

### `QLKH_LTW/wwwroot/admin/js/admin-premium.js`

- Chức năng: JS custom cho tương tác của admin layout.
- Hàm/khối:
  - `document.addEventListener('DOMContentLoaded', function() { ... })`
    - điểm vào chính của JS custom
  - xử lý nút `toggleBtn`
    - desktop sidebar collapse/expand bằng cách toggle class `sidebar-collapsed`
  - xử lý nút `mobileBtn`
    - mở sidebar mobile và bật overlay
  - xử lý `overlay`
    - click overlay sẽ đóng sidebar mobile
  - `if (window.initDashboardCharts) { window.initDashboardCharts(); }`
    - nếu dashboard có đăng ký hàm render chart thì gọi nó
  - xử lý submit button của form
    - khi form hợp lệ và submit, đổi nội dung nút thành spinner/loading text
  - auto-hide flash message
    - alert thành công sẽ mờ dần và bị xóa sau 5 giây
  - animate rows trong `.table-premium`
    - gắn hiệu ứng fade có delay cho từng dòng
  - cấu hình `bootstrap.Dropdown`
    - ép dropdown trong `.action-dropup` mở theo hướng `top-end`
    - tắt flip mặc định để tránh menu bị lật hướng ngoài ý muốn

### `QLKH_LTW/wwwroot/admin/inapp/img/*`

- Chức năng: ảnh/logo/icon demo cho giao diện admin.
- Nhóm file:
  - `avatar/avatar-1.jpg` đến `avatar/avatar-6.jpg`
    - avatar mẫu cho admin/teacher/student cards hoặc header
  - `logo.svg`, `logo-icon.svg`, `logo-1.svg`
    - biến thể logo của bộ giao diện
  - `product-1.png` đến `product-10.png`
    - ảnh minh họa từ template; hiện không phải logic nghiệp vụ cốt lõi
  - `favicon_io/android-chrome-192x192.png`
  - `favicon_io/android-chrome-512x512.png`
  - `favicon_io/apple-touch-icon.png`
  - `favicon_io/favicon-16x16.png`
  - `favicon_io/favicon-32x32.png`
  - `favicon_io/favicon.ico`
  - `favicon_io/site.webmanifest`
    - bộ favicon/PWA manifest cho icon trình duyệt
- Hàm: không có.

### `QLKH_LTW/App_Data/admin-module-data.json`

- Chức năng: file store JSON fallback của module admin.
- Nội dung:
  - chứa các mảng `accounts`, `teachers`, `students`, `levels`, `categories`, `courses`, `courseTeachers`, `chapters`, `lessons`, `enrollments`, `payments`
  - chứa các `next*Id`
- Vai trò thực tế:
  - nếu không có `DefaultConnection`, repository sẽ đọc/ghi vào file này
  - khi có SQL connection string, file này đóng vai trò dữ liệu dự phòng và snapshot tham chiếu
- Hàm: không có.

### `QLKH_LTW/testsprite_tests/*`

- Chức năng: artifact phục vụ kiểm thử/gợi ý test do TestSprite sinh.
- File:
  - `testsprite_frontend_test_plan.json`
    - danh sách test case giao diện
  - `standard_prd.json`
    - tài liệu PRD/tóm tắt sản phẩm theo format TestSprite
  - `tmp/code_summary.yaml`
    - tóm tắt codebase cho tool
  - `tmp/config.json`
    - cấu hình chạy test
  - `tmp/execution.lock`
    - lock file của lần chạy
  - `tmp/mcp.log`
    - log runtime rất lớn của tool
  - `tmp/prd_files/plan1.md`
    - bản copy/tài liệu đầu vào để TestSprite dùng
- Hàm: không có.

---

## 12. Nhóm file generated/vendor còn lại

Các nhóm dưới đây vẫn là một phần của thư mục `QLKH_LTW`, nhưng về mặt kỹ thuật chúng không chứa logic nghiệp vụ riêng của dự án:

### `QLKH_LTW/obj/` khoảng `15,336` file

- Chức năng: output restore/build của .NET.
- Thành phần thường gặp:
  - `project.assets.json`
  - `*.nuget.g.props`
  - `*.nuget.g.targets`
  - `*.dgspec.json`
  - file cache, tmp, compressed asset, metadata
- Ý nghĩa:
  - không sửa tay
  - có thể sinh lại bằng restore/build

### `QLKH_LTW/build_obj/` khoảng `20` file

- Chức năng: một nhánh artifact build/restore khác.
- Ý nghĩa:
  - tương tự `obj`
  - dùng cho các lần build hoặc kiểm thử trước đó

### `QLKH_LTW/bin/` khoảng `106` file

- Chức năng: file output sau khi biên dịch/chạy.
- Thường gồm:
  - `.dll`, `.pdb`, `.deps.json`, `.runtimeconfig.json`, static assets copy ra output
- Ý nghĩa:
  - không phải source code

### `QLKH_LTW/wwwroot/assets/` khoảng `1,855` file

- Chức năng: bộ asset theme cũ/legacy, gồm:
  - `css/`, `js/`, `scss/`, `img/`, `vendor/`
- Quan sát quan trọng:
  - tìm kiếm tham chiếu trong controller/view/service/program hiện tại không thấy layout nào gọi trực tiếp thư mục này
  - nghĩa là đây chủ yếu là bundle template hoặc bản sao theme cũ

### `QLKH_LTW/wwwroot/admin/assets/` khoảng `3,711` file

- Chức năng: một bộ asset admin kiểu `sb-admin-2` + vendor đầy đủ hơn.
- Thành phần:
  - `css`, `js`, `scss`, `vendor`, ảnh minh họa
- Vai trò thực tế:
  - phần lớn không được layout `_LayoutAdmin.cshtml` hiện tại dùng trực tiếp
  - đây là asset pack nền hoặc dữ liệu di sản còn lưu trong repo

### `QLKH_LTW/wwwroot/admin/assets/assets/` khoảng `1,855` file

- Chức năng: bản sao lồng thêm một cấp của asset pack ở trên.
- Ý nghĩa:
  - gần như chắc chắn là copy dư hoặc bản xuất template
  - không nên xem đây là nguồn logic thật của UI hiện tại

### Những file vendor tiêu biểu trong các thư mục asset legacy

- `sb-admin-2.css`, `sb-admin-2.min.css`
  - CSS theme admin mẫu
- `sb-admin-2.js`, `sb-admin-2.min.js`
  - JS tương tác của theme admin mẫu
- `demo/chart-*.js`, `demo/datatables-demo.js`
  - file demo chart/table của theme
- `scss/*.scss`, `scss/utilities/*.scss`, `scss/navs/*.scss`
  - mã nguồn Sass của theme
- `vendor/bootstrap/**`, `vendor/fontawesome-free/**`
  - Bootstrap, Font Awesome và hàng nghìn icon/font đi kèm

Những file này có chức năng thư viện, không phải nghiệp vụ riêng của `QLKH_LTW`.

---

## 13. Kết luận ngắn

Nếu đọc dự án theo thứ tự để hiểu nhanh nhất, nên đi như sau:

1. `Program.cs`
2. `Domain/AdminEntities.cs`
3. `Dtos/AdminDtos.cs`
4. `Infrastructure/IAdminRepository.cs`
5. `Infrastructure/AdminRepository.cs`
6. `Infrastructure/AdminRepository.Sql.cs`
7. `Services/IAdminService.cs`
8. `Services/AdminService.cs`
9. `Services/AdminService.CourseOps.cs`
10. `Services/AdminService.Helpers.cs`
11. `Areas/Admin/Controllers/*`
12. `Areas/Admin/Views/*`
13. `wwwroot/admin/css/admin-premium.css` và `wwwroot/admin/js/admin-premium.js`

Tóm gọn về kiến trúc:

- `Controllers` ở root là API
- `Areas/Admin/Controllers` là UI MVC
- `Services` chứa toàn bộ luật nghiệp vụ
- `Infrastructure` lo lưu trữ dữ liệu
- `Domain` là model nội bộ
- `Dtos` và `ViewModels` là dữ liệu trao đổi/hiển thị
- `Views` là phần trình bày
- `wwwroot/admin/*` là style và hành vi giao diện custom đang dùng

Tóm gọn về điểm mạnh kỹ thuật:

- có tách tầng tương đối rõ
- service chứa nghiệp vụ khá tập trung
- repository hỗ trợ cả JSON và SQL
- UI Admin và API dùng chung service nên giữ được logic nhất quán

Tóm gọn về điểm cần lưu ý khi đọc code:

- rất nhiều asset vendor/legacy nằm trong `wwwroot`, không phải file nghiệp vụ thật
- `Delete` ở UI hiện tại chủ yếu là soft-hide/archive, không phải xóa cứng dữ liệu
- trạng thái `payment` và `enrollment` có quan hệ đồng bộ chặt trong `UpdatePaymentStatusAsync`
- **Quan trọng:** Binding dữ liệu boolean (như `IsFeatured`) cần dùng cặp input hidden + checkbox tường minh để tránh mất dữ liệu khi dùng layout custom.

## 14. Bản ghi Sửa lỗi (Fix Log)

### Lỗi 013: Trạng thái Nổi bật (IsFeatured) không lưu vào DB

- **Vấn đề:** Khi bật/tắt trạng thái Nổi bật của khóa học, UI thông báo thành công nhưng DB không cập nhật. Nguyên nhân do parser Razor và bộ Model Binder của ASP.NET Core không nhận diện đúng giá trị boolean `"true"`/`"false"` được render từ biểu thức `@(!course.IsFeatured).ToString().ToLower()`.
- **Giải pháp:** 
  - Tại `Details.cshtml`: Thay thế hàm render giá trị bằng toán tử điều kiện tường minh: `value="@(course.IsFeatured ? "false" : "true")"`.
  - Tại `_Form.cshtml`: Thay thế `asp-for="IsFeatured"` bằng cặp input `hidden` (giá trị false) và `checkbox` (giá trị true) thủ công để ép Model Binder luôn nhận được giá trị đúng bất kể checkbox có được check hay không.
- **Kết quả:** Trạng thái `IsFeatured` hiện đã lưu và hiển thị nhất quán 100% trong mọi kịch bản thao tác.
## 15. Tự động sinh dữ liệu và Cải thiện UX (Auto-Generation)

### Vấn đề (Validation Loop):
- Trong quá trình kiểm thử tự động (TC003), hệ thống test không điền `CourseCode` và `Slug` vì giả định chúng sẽ được hệ thống tự động sinh ra hoặc không bắt buộc ở bước nhập liệu đầu tiên.
- Ràng buộc `[Required]` trong `CourseFormViewModel` khiến `ModelState.IsValid` trả về `false`, dẫn đến vòng lặp báo lỗi và không thể hoàn tất tạo khóa học.

### Giải pháp triển khai:
1. **ViewModel (`CourseFormViewModel`):**
   - Loại bỏ thuộc tính `[Required]` cho `CourseCode` và `Slug`.
   - Chuyển kiểu dữ liệu sang `string?` (nullable) để chấp nhận giá trị trống từ form.

2. **Controller (`CoursesController`):**
   - **Logic Server-side:** Khi nhận request `Create` (POST), nếu `CourseCode` hoặc `Slug` trống, hệ thống sẽ tự động sinh dựa trên `CourseName`.
     - `CourseCode`: Lấy 3 ký tự đầu của tên + chuỗi định danh duy nhất (Guid). VD: `PRO-A1B2C3`.
     - `Slug`: Chuyển đổi tên khóa học sang dạng không dấu, viết thường, gạch nối. VD: `Lập trình .NET` -> `lap-trinh-dotnet`.
   - **Hàm hỗ trợ:** Thêm `GenerateSlug(string phrase)` để xử lý chuẩn hóa chuỗi.

3. **Giao diện (`_Form.cshtml`):**
   - **Logic Client-side:** Thêm JavaScript lắng nghe sự kiện nhập `CourseName`.
   - Tự động điền vào ô `Slug` thời gian thực nếu người dùng chưa can thiệp thủ công (manual input).
   - Chỉ áp dụng trong chế độ **Create** để tránh vô tình đổi link của khóa học cũ.

### Lợi ích:
- Giải quyết triệt để lỗi TC003 trong TestSprite.
- Giảm công sức nhập liệu cho người quản trị.
- Đảm bảo tính nhất quán của dữ liệu (SEO friendly slugs).

---
