# Kế Hoạch Triển Khai Trang Admin

## 1. Mục tiêu của trang Admin

Trang `Admin` là khu vực quản trị nội bộ của hệ thống website học lập trình.  
Mục tiêu của trang này là giúp người quản trị:

- quản lý tài khoản người dùng
- quản lý khóa học
- quản lý teacher
- quản lý student
- kiểm soát nội dung được publish
- theo dõi đăng ký học
- theo dõi thanh toán
- xem dashboard tổng quan

Nói ngắn gọn, nếu phần `Public + Student` là nơi người học sử dụng sản phẩm, thì `Admin` là nơi vận hành toàn bộ hệ thống.

---

## 2. Vai trò của Admin trong hệ thống

Admin là tác nhân có quyền cao nhất.  
Admin không trực tiếp đi học và cũng không trực tiếp tạo toàn bộ nội dung như teacher, nhưng Admin là người:

- tạo cấu trúc vận hành
- kiểm soát dữ liệu hệ thống
- quyết định khóa học nào được hiển thị
- kiểm soát teacher nào được phụ trách khóa nào
- theo dõi giao dịch và tình trạng học viên

Admin là người giữ cho toàn hệ thống chạy đúng luồng.

---

## 3. Phạm vi chức năng của trang Admin

## 3.1 Nhóm chức năng bắt buộc

### Dashboard

- xem tổng số khóa học
- xem tổng số teacher
- xem tổng số student
- xem tổng số enrollment
- xem tổng số payment
- xem thống kê khóa học nổi bật hoặc mới nhất

### Quản lý tài khoản

- xem danh sách account
- tạo account
- cập nhật thông tin account
- khóa hoặc mở tài khoản
- phân vai trò `Admin`, `Teacher`, `Student`

### Quản lý teacher

- xem danh sách teacher
- tạo hồ sơ teacher
- cập nhật hồ sơ teacher
- xem teacher đang phụ trách khóa nào

### Quản lý student

- xem danh sách student
- xem thông tin học viên
- xem học viên đã đăng ký khóa nào
- xem tình trạng thanh toán cơ bản

### Quản lý khóa học

- xem danh sách course
- tạo khóa học
- cập nhật khóa học
- đổi trạng thái course
- đánh dấu course nổi bật
- gán teacher cho course

### Quản lý chapter và lesson ở mức giám sát

- xem cấu trúc chapter và lesson của course
- kiểm tra lesson đã publish hay chưa
- hỗ trợ kiểm duyệt nội dung nếu hệ thống yêu cầu

### Quản lý enrollment

- xem danh sách học viên đăng ký theo course
- lọc enrollment theo trạng thái
- xem enrollment chi tiết
- kích hoạt hoặc hủy enrollment nếu có quyền xử lý

### Quản lý payment

- xem danh sách payment
- xem trạng thái thanh toán
- tra cứu mã giao dịch
- đối soát giao dịch thành công, thất bại, hoàn tiền

---

## 3.2 Nhóm chức năng nên có nếu còn thời gian

- tìm kiếm nâng cao
- lọc theo ngày tạo
- lọc theo trạng thái
- xem biểu đồ doanh thu cơ bản
- xem course có nhiều học viên nhất
- xem student mới đăng ký gần đây
- audit log cơ bản

---

## 4. Cách Admin hoạt động trong hệ thống

## 4.1 Luồng làm việc tổng quát của Admin

1. Admin đăng nhập hệ thống.
2. Vào dashboard để xem tổng quan.
3. Quản lý teacher và account.
4. Tạo hoặc chỉnh sửa course.
5. Gán teacher phụ trách course.
6. Kiểm tra chapter và lesson của course.
7. Publish course khi đã sẵn sàng.
8. Theo dõi enrollment của student.
9. Theo dõi payment.
10. Hỗ trợ xử lý các tình huống lỗi dữ liệu hoặc sai trạng thái.

## 4.2 Mục tiêu của luồng này

Người lập trình phải hiểu rằng trang Admin không chỉ là nơi CRUD dữ liệu, mà là nơi:

- điều hành hệ thống
- kiểm soát quyền
- kiểm soát nội dung
- kiểm soát giao dịch

Nếu thiết kế Admin chỉ như một bộ form nhập liệu, thì sẽ thiếu giá trị nghiệp vụ.

---

## 5. Danh sách trang Admin cần có

## 5.1 Dashboard

### Mục tiêu

Cho admin nhìn tổng quan nhanh về hệ thống.

### Nội dung nên hiển thị

- tổng số account
- tổng số teacher
- tổng số student
- tổng số course
- tổng số enrollment
- tổng số payment thành công
- vài course mới tạo
- vài giao dịch gần nhất

---

## 5.2 Quản lý Account

### Mục tiêu

Quản lý danh sách người dùng chung.

### Chức năng

- xem danh sách
- tạo mới
- sửa thông tin
- khóa/mở tài khoản
- gán role

### Cột cần hiển thị

- username
- email
- full name
- role
- is active
- created at

---

## 5.3 Quản lý Teacher

### Mục tiêu

Quản lý giảng viên hoặc người quản lý nội dung.

### Chức năng

- xem danh sách teacher
- tạo teacher từ account có role teacher
- cập nhật specialization, bio, experience
- xem teacher đang phụ trách course nào

### Cột cần hiển thị

- full name
- email
- specialization
- experience years
- số lượng course phụ trách

---

## 5.4 Quản lý Student

### Mục tiêu

Theo dõi người học trong hệ thống.

### Chức năng

- xem danh sách student
- xem profile student
- xem khóa đã đăng ký
- xem trạng thái enrollment

### Cột cần hiển thị

- full name
- email
- phone
- số lượng course đã đăng ký
- ngày tạo tài khoản

---

## 5.5 Quản lý Course

### Mục tiêu

Đây là trang quan trọng nhất của Admin sau dashboard.

### Chức năng

- tạo course
- sửa thông tin course
- đổi trạng thái `Draft`, `Published`, `Archived`
- chỉnh giá course
- gán category, level
- gán teacher
- đánh dấu featured

### Cột cần hiển thị

- course name
- category
- level
- price
- status
- featured
- created at
- published at

---

## 5.6 Quản lý cấu trúc nội dung Course

### Mục tiêu

Giúp admin kiểm tra khóa học có đủ nội dung trước khi publish.

### Chức năng

- xem chapter
- xem lesson theo chapter
- xem lesson ở trạng thái `Draft` hay `Published`
- kiểm tra số lượng lesson

### Ghi chú

Admin không nhất thiết phải sửa lesson thường xuyên, nhưng phải nhìn được cấu trúc để kiểm soát chất lượng nội dung.

---

## 5.7 Quản lý Enrollment

### Mục tiêu

Theo dõi student nào đã đăng ký course nào.

### Chức năng

- xem enrollment list
- lọc theo course
- lọc theo student
- lọc theo trạng thái
- xem chi tiết enrollment

### Cột cần hiển thị

- student
- course
- status
- price snapshot
- activated at
- created at

---

## 5.8 Quản lý Payment

### Mục tiêu

Theo dõi giao dịch thanh toán.

### Chức năng

- xem payment list
- lọc theo payment status
- lọc theo payment method
- xem transaction number
- xem chi tiết payment

### Cột cần hiển thị

- student
- course
- amount
- method
- status
- transaction no
- paid at

---

## 6. Dữ liệu Admin thao tác trực tiếp

Trang Admin sẽ tác động chủ yếu vào các bảng sau:

- `Account`
- `Teacher`
- `Student`
- `Course`
- `CourseTeacher`
- `Chapter`
- `Lesson`
- `Enrollment`
- `Payment`

Các bảng danh mục mà Admin thường dùng để đổ dropdown hoặc quản lý nền:

- `Role`
- `CourseLevel`
- `CourseCategory`
- `CourseStatus`
- `LessonStatus`
- `EnrollmentStatus`
- `PaymentMethod`
- `PaymentStatus`

---

## 7. Rule nghiệp vụ cần chốt trước khi code Admin

## 7.1 Rule về Account

- account bị khóa thì không đăng nhập được
- một account thuộc đúng một role
- teacher và student đều phải có account trước

## 7.2 Rule về Course

- chỉ course `Published` mới xuất hiện public
- course `Draft` chưa được student thấy
- course `Archived` không cho đăng ký mới

## 7.3 Rule về Teacher

- teacher chỉ được quản lý course được giao
- admin là người gán teacher vào course

## 7.4 Rule về Enrollment

- enrollment `ACTIVE` mới có quyền học
- enrollment `PENDING_PAYMENT` thì chưa học được
- enrollment `CANCELLED` thì không còn hiệu lực

## 7.5 Rule về Payment

- payment `SUCCESS` mới được xem là thanh toán xong
- payment `FAILED` không cấp quyền học
- payment `REFUNDED` có thể kéo theo enrollment bị đổi trạng thái

---

## 8. Thiết kế giao diện Admin nên theo hướng nào

Với `ASP.NET`, phần Admin nên dùng dashboard layout kiểu:

- sidebar trái
- top navbar
- content area ở giữa
- card số liệu ở dashboard
- table cho danh sách dữ liệu
- form modal hoặc form riêng cho tạo/sửa

Theo định hướng đã chốt, nên dùng `Tabler` cho phần Admin.

## 8.1 Các thành phần UI nên có

- sidebar menu
- breadcrumb
- summary cards
- data table
- search box
- filter dropdown
- status badge
- modal xác nhận thao tác

## 8.2 Sidebar đề xuất

- Dashboard
- Accounts
- Teachers
- Students
- Courses
- Enrollments
- Payments
- Settings

---

## 9. Kế hoạch triển khai trang Admin

## Giai đoạn 1: Dựng khung Admin

### Mục tiêu

Có layout admin chạy được.

### Việc cần làm

- tích hợp template `Tabler`
- tạo layout admin
- tạo sidebar, navbar, footer
- tạo route bảo vệ cho role admin

### Kết quả đầu ra

- vào được khu vực admin sau đăng nhập
- có khung trang đồng nhất

---

## Giai đoạn 2: Dashboard

### Mục tiêu

Có trang tổng quan để demo.

### Việc cần làm

- lấy số liệu từ database
- hiển thị summary cards
- hiển thị dữ liệu gần đây

### Kết quả đầu ra

- dashboard có số liệu thật

---

## Giai đoạn 3: CRUD dữ liệu nền

### Mục tiêu

Hoàn thành các trang quản lý cơ bản.

### Việc cần làm

- account management
- teacher management
- student management
- course management

### Kết quả đầu ra

- admin có thể tạo/sửa dữ liệu cốt lõi

---

## Giai đoạn 4: Quản lý enrollment và payment

### Mục tiêu

Hoàn thiện luồng quản trị giao dịch.

### Việc cần làm

- tạo danh sách enrollment
- tạo danh sách payment
- hiển thị detail
- thêm filter theo trạng thái

### Kết quả đầu ra

- admin theo dõi được student nào đã học và đã thanh toán

---

## Giai đoạn 5: Hoàn thiện và kiểm thử

### Mục tiêu

Làm Admin ổn định để demo.

### Việc cần làm

- kiểm tra phân quyền
- kiểm tra validate form
- kiểm tra search/filter
- chuẩn bị dữ liệu demo

### Kết quả đầu ra

- trang admin demo được đầy đủ luồng quản trị

---

## 10. API backend cần có cho Admin

## 10.1 Dashboard APIs

- lấy số lượng account
- lấy số lượng teacher
- lấy số lượng student
- lấy số lượng course
- lấy số lượng enrollment
- lấy payment gần đây

## 10.2 Account APIs

- danh sách account
- tạo account
- cập nhật account
- đổi trạng thái active

## 10.3 Teacher APIs

- danh sách teacher
- tạo teacher
- cập nhật teacher
- lấy course của teacher

## 10.4 Student APIs

- danh sách student
- chi tiết student
- enrollment của student

## 10.5 Course APIs

- danh sách course
- tạo course
- cập nhật course
- đổi trạng thái course
- gán teacher cho course
- lấy chapter/lesson theo course

## 10.6 Enrollment APIs

- danh sách enrollment
- chi tiết enrollment
- lọc theo course/student/status

## 10.7 Payment APIs

- danh sách payment
- chi tiết payment
- lọc theo status/method

---

## 11. Phân chia việc cho nhóm nếu có người phụ trách Admin

Nếu một người phụ trách chính phần Admin thì người đó nên làm:

- layout admin
- dashboard
- CRUD account
- CRUD course
- enrollment list
- payment list

Người này cũng nên phối hợp chặt với:

- người làm teacher để thống nhất course/chapter/lesson
- người làm student để thống nhất enrollment/payment

---

## 12. Những lỗi rất dễ gặp khi làm Admin

- chỉ làm giao diện mà chưa nối dữ liệu thật
- không kiểm tra role admin ở backend
- cho admin sửa dữ liệu nhưng không validate
- không đồng bộ trạng thái payment và enrollment
- bảng danh sách không có filter nên khó demo
- thiếu dữ liệu seed dẫn đến dashboard trống

---

## 13. Mức ưu tiên thực tế khi làm Admin

### Bắt buộc

- login admin
- admin layout
- dashboard
- course management
- account management
- payment management

### Nên có

- teacher management
- student management
- enrollment management

### Có thể để sau

- audit log
- biểu đồ nâng cao
- settings nâng cao

---

## 14. Kết luận

Trang Admin là trung tâm vận hành của toàn bộ hệ thống.

Nếu làm tốt phần Admin thì nhóm sẽ:

- quản lý được dữ liệu demo
- kiểm soát được toàn bộ hệ thống
- dễ thuyết trình vì có màn hình rõ ràng
- chứng minh được tư duy quản trị và nghiệp vụ

Thứ tự làm đúng nên là:

1. Dựng layout admin
2. Làm dashboard
3. Làm account và course
4. Làm teacher, student
5. Làm enrollment và payment
6. Kiểm thử phân quyền và dữ liệu demo
