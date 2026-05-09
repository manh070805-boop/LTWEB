/*
    Seed dữ liệu mẫu cho TrungTamLapTrinh.

    Gợi ý chạy bằng sqlcmd để không lỗi tiếng Việt:
    sqlcmd -S ".\SQLEXPRESS01" -d "TrungTamLapTrinh" -E -f 65001 -i "ADMINLTW\Data\SeedData_7Records.sql"

    Lưu ý:
    - Chuỗi tiếng Việt đều dùng N'...' để ghi đúng vào NVARCHAR.
    - Mã, username, email, URL dùng ASCII vì các cột này là VARCHAR/CHAR.
    - Script có thể chạy lại nhiều lần, dữ liệu seed sẽ được cập nhật hoặc bỏ qua thay vì chèn trùng.
*/

USE [TrungTamLapTrinh];
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

MERGE [Role] AS target
USING (VALUES
    ('Admin', N'Quản trị hệ thống'),
    ('Teacher', N'Giảng viên phụ trách lớp học'),
    ('Student', N'Học viên tham gia khóa học'),
    ('Academic', N'Điều phối học vụ'),
    ('Accountant', N'Nhân sự phụ trách học phí'),
    ('Counselor', N'Tư vấn tuyển sinh'),
    ('Support', N'Hỗ trợ vận hành lớp học')
) AS source (RoleName, Description)
ON target.RoleName = source.RoleName
WHEN MATCHED THEN
    UPDATE SET Description = source.Description
WHEN NOT MATCHED THEN
    INSERT (RoleName, Description)
    VALUES (source.RoleName, source.Description);

MERGE CourseLevel AS target
USING (VALUES
    ('BEGINNER', N'Cơ bản'),
    ('BASIC', N'Nhập môn'),
    ('INTERMEDIATE', N'Trung cấp'),
    ('ADVANCED', N'Nâng cao'),
    ('FULLSTACK', N'Fullstack'),
    ('KIDS', N'Trẻ em'),
    ('CAREER', N'Định hướng nghề nghiệp')
) AS source (LevelCode, LevelName)
ON target.LevelCode = source.LevelCode
WHEN MATCHED THEN
    UPDATE SET LevelName = source.LevelName
WHEN NOT MATCHED THEN
    INSERT (LevelCode, LevelName)
    VALUES (source.LevelCode, source.LevelName);

MERGE ClassStatus AS target
USING (VALUES
    ('OPEN', N'Mở đăng ký'),
    ('UPCOMING', N'Sắp khai giảng'),
    ('ONGOING', N'Đang học'),
    ('PAUSED', N'Tạm nghỉ'),
    ('CLOSED', N'Đã đóng'),
    ('FINISHED', N'Đã kết thúc'),
    ('CANCELLED', N'Đã hủy')
) AS source (StatusCode, StatusName)
ON target.StatusCode = source.StatusCode
WHEN MATCHED THEN
    UPDATE SET StatusName = source.StatusName
WHEN NOT MATCHED THEN
    INSERT (StatusCode, StatusName)
    VALUES (source.StatusCode, source.StatusName);

MERGE EnrollmentStatus AS target
USING (VALUES
    ('PENDING', N'Chờ duyệt'),
    ('WAITLIST', N'Danh sách chờ'),
    ('APPROVED', N'Đã duyệt'),
    ('ACTIVE', N'Đang học'),
    ('COMPLETED', N'Hoàn thành'),
    ('REJECTED', N'Từ chối'),
    ('CANCELLED', N'Đã hủy')
) AS source (StatusCode, StatusName)
ON target.StatusCode = source.StatusCode
WHEN MATCHED THEN
    UPDATE SET StatusName = source.StatusName
WHEN NOT MATCHED THEN
    INSERT (StatusCode, StatusName)
    VALUES (source.StatusCode, source.StatusName);

MERGE PaymentMethod AS target
USING (VALUES
    ('CASH', N'Tiền mặt'),
    ('BANK', N'Chuyển khoản'),
    ('VNPAY', N'VNPay'),
    ('MOMO', N'MoMo'),
    ('ZALOPAY', N'ZaloPay'),
    ('CREDIT_CARD', N'Thẻ tín dụng'),
    ('INSTALLMENT', N'Trả góp')
) AS source (MethodCode, MethodName)
ON target.MethodCode = source.MethodCode
WHEN MATCHED THEN
    UPDATE SET MethodName = source.MethodName
WHEN NOT MATCHED THEN
    INSERT (MethodCode, MethodName)
    VALUES (source.MethodCode, source.MethodName);

MERGE PaymentStatus AS target
USING (VALUES
    ('PENDING', N'Chờ thanh toán'),
    ('PARTIAL', N'Thanh toán một phần'),
    ('SUCCESS', N'Thành công'),
    ('FAILED', N'Thất bại'),
    ('REVIEW', N'Đang đối soát'),
    ('REFUNDED', N'Hoàn tiền'),
    ('CANCELLED', N'Đã hủy')
) AS source (StatusCode, StatusName)
ON target.StatusCode = source.StatusCode
WHEN MATCHED THEN
    UPDATE SET StatusName = source.StatusName
WHEN NOT MATCHED THEN
    INSERT (StatusCode, StatusName)
    VALUES (source.StatusCode, source.StatusName);

DECLARE @AdminRoleId INT = (SELECT RoleId FROM [Role] WHERE RoleName = 'Admin');
DECLARE @TeacherRoleId INT = (SELECT RoleId FROM [Role] WHERE RoleName = 'Teacher');
DECLARE @StudentRoleId INT = (SELECT RoleId FROM [Role] WHERE RoleName = 'Student');

MERGE Account AS target
USING (VALUES
    ('admin_seed', '123456', 'admin.seed@trungtam.dev', N'Quản trị viên Mẫu', @AdminRoleId, CAST(1 AS BIT)),
    ('gv01', '123456', 'gv01@trungtam.dev', N'Nguyễn Minh Khang', @TeacherRoleId, CAST(1 AS BIT)),
    ('gv02', '123456', 'gv02@trungtam.dev', N'Trần Hà Linh', @TeacherRoleId, CAST(1 AS BIT)),
    ('gv03', '123456', 'gv03@trungtam.dev', N'Lê Quốc Bảo', @TeacherRoleId, CAST(1 AS BIT)),
    ('gv04', '123456', 'gv04@trungtam.dev', N'Phạm Thùy Dương', @TeacherRoleId, CAST(1 AS BIT)),
    ('gv05', '123456', 'gv05@trungtam.dev', N'Võ Anh Tuấn', @TeacherRoleId, CAST(1 AS BIT)),
    ('gv06', '123456', 'gv06@trungtam.dev', N'Đặng Ngọc Mai', @TeacherRoleId, CAST(1 AS BIT)),
    ('gv07', '123456', 'gv07@trungtam.dev', N'Hoàng Gia Huy', @TeacherRoleId, CAST(1 AS BIT)),
    ('sv01', '123456', 'sv01@trungtam.dev', N'Nguyễn Thảo Vy', @StudentRoleId, CAST(1 AS BIT)),
    ('sv02', '123456', 'sv02@trungtam.dev', N'Trần Đức Anh', @StudentRoleId, CAST(1 AS BIT)),
    ('sv03', '123456', 'sv03@trungtam.dev', N'Lê Bảo Ngọc', @StudentRoleId, CAST(1 AS BIT)),
    ('sv04', '123456', 'sv04@trungtam.dev', N'Phạm Minh Châu', @StudentRoleId, CAST(1 AS BIT)),
    ('sv05', '123456', 'sv05@trungtam.dev', N'Vũ Hoài Nam', @StudentRoleId, CAST(1 AS BIT)),
    ('sv06', '123456', 'sv06@trungtam.dev', N'Đỗ Khánh Linh', @StudentRoleId, CAST(1 AS BIT)),
    ('sv07', '123456', 'sv07@trungtam.dev', N'Bùi Gia Phúc', @StudentRoleId, CAST(1 AS BIT))
) AS source (Username, PasswordHash, Email, FullName, RoleId, IsActive)
ON target.Username = source.Username
WHEN MATCHED THEN
    UPDATE SET
        PasswordHash = source.PasswordHash,
        Email = source.Email,
        FullName = source.FullName,
        RoleId = source.RoleId,
        IsActive = source.IsActive,
        UpdatedAt = SYSDATETIME()
WHEN NOT MATCHED THEN
    INSERT (Username, PasswordHash, Email, FullName, RoleId, IsActive)
    VALUES (source.Username, source.PasswordHash, source.Email, source.FullName, source.RoleId, source.IsActive);

MERGE Teacher AS target
USING (VALUES
    ('GV001', 'gv01', '0901001001', N'Lập trình C# và ASP.NET Core', N'Có kinh nghiệm xây dựng hệ thống quản trị và hướng dẫn học viên triển khai dự án thực tế.'),
    ('GV002', 'gv02', '0901001002', N'Frontend React và UI/UX', N'Tập trung vào giao diện rõ ràng, thao tác mượt và trải nghiệm học tập dễ tiếp cận.'),
    ('GV003', 'gv03', '0901001003', N'Cơ sở dữ liệu SQL Server', N'Hướng dẫn thiết kế dữ liệu, truy vấn tối ưu và xử lý báo cáo cho doanh nghiệp.'),
    ('GV004', 'gv04', '0901001004', N'Kiểm thử phần mềm', N'Phụ trách kiểm thử tự động, quy trình QA và quản lý lỗi trong dự án nhóm.'),
    ('GV005', 'gv05', '0901001005', N'Java Spring Boot', N'Giảng dạy backend Java, REST API và tích hợp bảo mật ứng dụng.'),
    ('GV006', 'gv06', '0901001006', N'Python và phân tích dữ liệu', N'Kết nối kiến thức lập trình Python với bài toán xử lý dữ liệu thực tế.'),
    ('GV007', 'gv07', '0901001007', N'DevOps căn bản', N'Hướng dẫn Git, CI/CD, Docker và quy trình triển khai ứng dụng web.')
) AS source (TeacherId, Username, Phone, Specialization, Bio)
ON target.TeacherId = source.TeacherId
WHEN MATCHED THEN
    UPDATE SET
        AccountId = (SELECT AccountId FROM Account WHERE Username = source.Username),
        Phone = source.Phone,
        Specialization = source.Specialization,
        Bio = source.Bio
WHEN NOT MATCHED THEN
    INSERT (TeacherId, AccountId, Phone, Specialization, Bio)
    VALUES (source.TeacherId, (SELECT AccountId FROM Account WHERE Username = source.Username), source.Phone, source.Specialization, source.Bio);

MERGE Student AS target
USING (VALUES
    ('SV001', 'sv01', '0912001001', N'12 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh', CAST('2004-03-12' AS DATE)),
    ('SV002', 'sv02', '0912001002', N'25 Lê Lợi, Quận 3, TP. Hồ Chí Minh', CAST('2003-07-24' AS DATE)),
    ('SV003', 'sv03', '0912001003', N'48 Trần Hưng Đạo, Quận 5, TP. Hồ Chí Minh', CAST('2005-01-18' AS DATE)),
    ('SV004', 'sv04', '0912001004', N'7 Phan Đình Phùng, Phú Nhuận, TP. Hồ Chí Minh', CAST('2002-11-05' AS DATE)),
    ('SV005', 'sv05', '0912001005', N'103 Nguyễn Văn Cừ, Quận 10, TP. Hồ Chí Minh', CAST('2004-09-30' AS DATE)),
    ('SV006', 'sv06', '0912001006', N'66 Cách Mạng Tháng Tám, Quận 3, TP. Hồ Chí Minh', CAST('2003-05-16' AS DATE)),
    ('SV007', 'sv07', '0912001007', N'18 Điện Biên Phủ, Bình Thạnh, TP. Hồ Chí Minh', CAST('2005-12-02' AS DATE))
) AS source (StudentId, Username, Phone, Address, Birthday)
ON target.StudentId = source.StudentId
WHEN MATCHED THEN
    UPDATE SET
        AccountId = (SELECT AccountId FROM Account WHERE Username = source.Username),
        Phone = source.Phone,
        Address = source.Address,
        Birthday = source.Birthday
WHEN NOT MATCHED THEN
    INSERT (StudentId, AccountId, Phone, Address, Birthday)
    VALUES (source.StudentId, (SELECT AccountId FROM Account WHERE Username = source.Username), source.Phone, source.Address, source.Birthday);

MERGE Course AS target
USING (VALUES
    ('KH001', N'C# nền tảng cho người mới', N'Học cú pháp C#, tư duy lập trình và cách xây dựng ứng dụng console nhỏ.', NULL, CAST(2500000 AS DECIMAL(15,2)), 2, N'Biến và kiểu dữ liệu; điều kiện; vòng lặp; hàm; bài tập cuối khóa', 'BEGINNER', CAST(1 AS BIT)),
    ('KH002', N'ASP.NET Core MVC thực chiến', N'Xây dựng website quản trị bằng MVC, Razor, Entity Framework Core và SQL Server.', NULL, CAST(4200000 AS DECIMAL(15,2)), 3, N'MVC; Razor View; Entity Framework Core; xác thực; triển khai', 'INTERMEDIATE', CAST(1 AS BIT)),
    ('KH003', N'ReactJS hiện đại', N'Thiết kế giao diện component, quản lý state và kết nối API cho ứng dụng web.', NULL, CAST(3900000 AS DECIMAL(15,2)), 3, N'Component; hook; routing; form; gọi API; tối ưu giao diện', 'INTERMEDIATE', CAST(1 AS BIT)),
    ('KH004', N'SQL Server từ cơ bản đến tối ưu', N'Nắm vững thiết kế bảng, khóa ngoại, truy vấn, stored procedure và tối ưu hiệu năng.', NULL, CAST(3100000 AS DECIMAL(15,2)), 2, N'Thiết kế CSDL; join; index; view; procedure; transaction', 'BASIC', CAST(1 AS BIT)),
    ('KH005', N'Fullstack .NET và React', N'Kết hợp ASP.NET Core Web API, ReactJS và SQL Server để hoàn thiện sản phẩm.', NULL, CAST(6500000 AS DECIMAL(15,2)), 5, N'API; JWT; React; quản lý trạng thái; phân quyền; deploy', 'FULLSTACK', CAST(1 AS BIT)),
    ('KH006', N'Python phân tích dữ liệu', N'Làm sạch dữ liệu, trực quan hóa và xây dựng báo cáo bằng Python.', NULL, CAST(3600000 AS DECIMAL(15,2)), 3, N'Python căn bản; pandas; biểu đồ; báo cáo; mini project', 'CAREER', CAST(1 AS BIT)),
    ('KH007', N'Lập trình tư duy cho thiếu nhi', N'Giúp học viên nhỏ tuổi làm quen thuật toán qua trò chơi và dự án sáng tạo.', NULL, CAST(2800000 AS DECIMAL(15,2)), 2, N'Tư duy logic; Scratch; trò chơi; thuyết trình sản phẩm', 'KIDS', CAST(1 AS BIT))
) AS source (CourseId, CourseName, Description, ImageUrl, Price, DurationMonths, Roadmap, LevelCode, IsActive)
ON target.CourseId = source.CourseId
WHEN MATCHED THEN
    UPDATE SET
        CourseName = source.CourseName,
        Description = source.Description,
        ImageUrl = source.ImageUrl,
        Price = source.Price,
        DurationMonths = source.DurationMonths,
        Roadmap = source.Roadmap,
        LevelId = (SELECT LevelId FROM CourseLevel WHERE LevelCode = source.LevelCode),
        IsActive = source.IsActive
WHEN NOT MATCHED THEN
    INSERT (CourseId, CourseName, Description, ImageUrl, Price, DurationMonths, Roadmap, LevelId, IsActive)
    VALUES (source.CourseId, source.CourseName, source.Description, source.ImageUrl, source.Price, source.DurationMonths, source.Roadmap, (SELECT LevelId FROM CourseLevel WHERE LevelCode = source.LevelCode), source.IsActive);

MERGE CenterClass AS target
USING (VALUES
    ('LH001', N'C# nền tảng K01', N'Phòng 201', 'KH001', 'GV001', 'OPEN', CAST('2026-05-06' AS DATE), CAST('2026-07-06' AS DATE), 24),
    ('LH002', N'ASP.NET Core MVC K02', N'Phòng 302', 'KH002', 'GV001', 'UPCOMING', CAST('2026-05-12' AS DATE), CAST('2026-08-12' AS DATE), 22),
    ('LH003', N'ReactJS hiện đại K03', N'Phòng 203', 'KH003', 'GV002', 'ONGOING', CAST('2026-04-08' AS DATE), CAST('2026-07-08' AS DATE), 25),
    ('LH004', N'SQL Server tối ưu K01', N'Phòng 305', 'KH004', 'GV003', 'OPEN', CAST('2026-05-20' AS DATE), CAST('2026-07-20' AS DATE), 20),
    ('LH005', N'Fullstack .NET React K01', N'Phòng Lab 1', 'KH005', 'GV005', 'UPCOMING', CAST('2026-06-01' AS DATE), CAST('2026-11-01' AS DATE), 18),
    ('LH006', N'Python dữ liệu K02', N'Phòng Lab 2', 'KH006', 'GV006', 'ONGOING', CAST('2026-04-15' AS DATE), CAST('2026-07-15' AS DATE), 20),
    ('LH007', N'Tư duy lập trình thiếu nhi K01', N'Phòng Sáng tạo', 'KH007', 'GV007', 'OPEN', CAST('2026-06-08' AS DATE), CAST('2026-08-08' AS DATE), 16)
) AS source (ClassId, ClassName, RoomName, CourseId, TeacherId, StatusCode, StartDate, EndDate, MaxStudents)
ON target.ClassId = source.ClassId
WHEN MATCHED THEN
    UPDATE SET
        ClassName = source.ClassName,
        RoomName = source.RoomName,
        CourseId = source.CourseId,
        TeacherId = source.TeacherId,
        StatusId = (SELECT StatusId FROM ClassStatus WHERE StatusCode = source.StatusCode),
        StartDate = source.StartDate,
        EndDate = source.EndDate,
        MaxStudents = source.MaxStudents
WHEN NOT MATCHED THEN
    INSERT (ClassId, ClassName, RoomName, CourseId, TeacherId, StatusId, StartDate, EndDate, MaxStudents)
    VALUES (source.ClassId, source.ClassName, source.RoomName, source.CourseId, source.TeacherId, (SELECT StatusId FROM ClassStatus WHERE StatusCode = source.StatusCode), source.StartDate, source.EndDate, source.MaxStudents);

MERGE Schedule AS target
USING (VALUES
    ('LH001', CAST(2 AS TINYINT), CAST('18:00' AS TIME), CAST('20:00' AS TIME), N'Phòng 201'),
    ('LH002', CAST(3 AS TINYINT), CAST('18:30' AS TIME), CAST('20:30' AS TIME), N'Phòng 302'),
    ('LH003', CAST(4 AS TINYINT), CAST('19:00' AS TIME), CAST('21:00' AS TIME), N'Phòng 203'),
    ('LH004', CAST(5 AS TINYINT), CAST('18:00' AS TIME), CAST('20:00' AS TIME), N'Phòng 305'),
    ('LH005', CAST(6 AS TINYINT), CAST('18:30' AS TIME), CAST('21:00' AS TIME), N'Phòng Lab 1'),
    ('LH006', CAST(7 AS TINYINT), CAST('08:00' AS TIME), CAST('10:30' AS TIME), N'Phòng Lab 2'),
    ('LH007', CAST(8 AS TINYINT), CAST('09:00' AS TIME), CAST('11:00' AS TIME), N'Phòng Sáng tạo')
) AS source (ClassId, DayOfWeek, StartTime, EndTime, RoomName)
ON target.ClassId = source.ClassId AND target.DayOfWeek = source.DayOfWeek AND target.StartTime = source.StartTime
WHEN MATCHED THEN
    UPDATE SET EndTime = source.EndTime, RoomName = source.RoomName
WHEN NOT MATCHED THEN
    INSERT (ClassId, DayOfWeek, StartTime, EndTime, RoomName)
    VALUES (source.ClassId, source.DayOfWeek, source.StartTime, source.EndTime, source.RoomName);


MERGE Enrollment AS target
USING (VALUES
    ('SV001', 'KH001', 'LH001', 'APPROVED', 'GV001', CAST('2026-04-20T09:00:00' AS DATETIME2), N'Học viên đăng ký sau buổi tư vấn trực tiếp.'),
    ('SV002', 'KH002', 'LH002', 'PENDING', NULL, NULL, N'Chờ xác nhận lịch học phù hợp.'),
    ('SV003', 'KH003', 'LH003', 'ACTIVE', 'GV002', CAST('2026-04-12T14:30:00' AS DATETIME2), N'Đang tham gia lớp ReactJS hiện đại.'),
    ('SV004', 'KH004', 'LH004', 'APPROVED', 'GV003', CAST('2026-04-21T10:15:00' AS DATETIME2), N'Cần bổ sung bài kiểm tra đầu vào.'),
    ('SV005', 'KH005', 'LH005', 'WAITLIST', NULL, NULL, N'Đang trong danh sách chờ lớp fullstack.'),
    ('SV006', 'KH006', 'LH006', 'ACTIVE', 'GV006', CAST('2026-04-16T16:00:00' AS DATETIME2), N'Học viên quan tâm phân tích dữ liệu.'),
    ('SV007', 'KH007', 'LH007', 'APPROVED', 'GV007', CAST('2026-04-22T08:45:00' AS DATETIME2), N'Phụ huynh đã xác nhận lịch cuối tuần.')
) AS source (StudentId, CourseId, ClassId, StatusCode, ApprovedByTeacherId, ApprovedAt, Note)
ON target.StudentId = source.StudentId AND target.CourseId = source.CourseId
WHEN MATCHED THEN
    UPDATE SET
        ClassId = source.ClassId,
        StatusId = (SELECT StatusId FROM EnrollmentStatus WHERE StatusCode = source.StatusCode),
        ApprovedByTeacherId = source.ApprovedByTeacherId,
        ApprovedAt = source.ApprovedAt,
        Note = source.Note
WHEN NOT MATCHED THEN
    INSERT (StudentId, CourseId, ClassId, StatusId, ApprovedByTeacherId, ApprovedAt, Note)
    VALUES (source.StudentId, source.CourseId, source.ClassId, (SELECT StatusId FROM EnrollmentStatus WHERE StatusCode = source.StatusCode), source.ApprovedByTeacherId, source.ApprovedAt, source.Note);

MERGE Payment AS target
USING (VALUES
    ('SV001', 'KH001', CAST(2500000 AS DECIMAL(15,2)), 'BANK', 'SUCCESS', 'TT20260422001', CAST('2026-04-22T09:30:00' AS DATETIME2)),
    ('SV002', 'KH002', CAST(1000000 AS DECIMAL(15,2)), 'MOMO', 'PENDING', 'TT20260422002', NULL),
    ('SV003', 'KH003', CAST(3900000 AS DECIMAL(15,2)), 'VNPAY', 'SUCCESS', 'TT20260422003', CAST('2026-04-18T19:20:00' AS DATETIME2)),
    ('SV004', 'KH004', CAST(1550000 AS DECIMAL(15,2)), 'CASH', 'PARTIAL', 'TT20260422004', CAST('2026-04-21T11:00:00' AS DATETIME2)),
    ('SV005', 'KH005', CAST(6500000 AS DECIMAL(15,2)), 'INSTALLMENT', 'REVIEW', 'TT20260422005', NULL),
    ('SV006', 'KH006', CAST(3600000 AS DECIMAL(15,2)), 'BANK', 'SUCCESS', 'TT20260422006', CAST('2026-04-17T15:45:00' AS DATETIME2)),
    ('SV007', 'KH007', CAST(2800000 AS DECIMAL(15,2)), 'ZALOPAY', 'SUCCESS', 'TT20260422007', CAST('2026-04-22T10:10:00' AS DATETIME2))
) AS source (StudentId, CourseId, Amount, MethodCode, StatusCode, TransactionNo, PaidAt)
ON target.TransactionNo = source.TransactionNo
WHEN MATCHED THEN
    UPDATE SET
        EnrollmentId = (
            SELECT EnrollmentId
            FROM Enrollment
            WHERE StudentId = source.StudentId AND CourseId = source.CourseId
        ),
        Amount = source.Amount,
        MethodId = (SELECT MethodId FROM PaymentMethod WHERE MethodCode = source.MethodCode),
        StatusId = (SELECT StatusId FROM PaymentStatus WHERE StatusCode = source.StatusCode),
        PaidAt = source.PaidAt
WHEN NOT MATCHED THEN
    INSERT (EnrollmentId, Amount, MethodId, StatusId, TransactionNo, PaidAt)
    VALUES (
        (SELECT EnrollmentId FROM Enrollment WHERE StudentId = source.StudentId AND CourseId = source.CourseId),
        source.Amount,
        (SELECT MethodId FROM PaymentMethod WHERE MethodCode = source.MethodCode),
        (SELECT StatusId FROM PaymentStatus WHERE StatusCode = source.StatusCode),
        source.TransactionNo,
        source.PaidAt
    );

COMMIT TRANSACTION;

SELECT N'Role' AS [Phan], COUNT(*) AS [TongBanGhi] FROM [Role]
UNION ALL SELECT N'CourseLevel', COUNT(*) FROM CourseLevel
UNION ALL SELECT N'ClassStatus', COUNT(*) FROM ClassStatus
UNION ALL SELECT N'EnrollmentStatus', COUNT(*) FROM EnrollmentStatus
UNION ALL SELECT N'PaymentMethod', COUNT(*) FROM PaymentMethod
UNION ALL SELECT N'PaymentStatus', COUNT(*) FROM PaymentStatus
UNION ALL SELECT N'Teacher seed', COUNT(*) FROM Teacher WHERE TeacherId BETWEEN 'GV001' AND 'GV007'
UNION ALL SELECT N'Student seed', COUNT(*) FROM Student WHERE StudentId BETWEEN 'SV001' AND 'SV007'
UNION ALL SELECT N'Course seed', COUNT(*) FROM Course WHERE CourseId BETWEEN 'KH001' AND 'KH007'
UNION ALL SELECT N'Class seed', COUNT(*) FROM CenterClass WHERE ClassId BETWEEN 'LH001' AND 'LH007'
UNION ALL SELECT N'Schedule seed', COUNT(*) FROM Schedule WHERE ClassId BETWEEN 'LH001' AND 'LH007'
UNION ALL SELECT N'Enrollment seed', COUNT(*) FROM Enrollment WHERE StudentId BETWEEN 'SV001' AND 'SV007'
UNION ALL SELECT N'Payment seed', COUNT(*) FROM Payment WHERE TransactionNo BETWEEN 'TT20260422001' AND 'TT20260422007';
