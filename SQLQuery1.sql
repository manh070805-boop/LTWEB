create database TrungTamLapTrinh  
use TrungTamLapTrinh  
CREATE TABLE Role (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName VARCHAR(30) NOT NULL UNIQUE,
    Description NVARCHAR(255) NULL
);
GO

CREATE TABLE Account (
    AccountId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    FullName NVARCHAR(250) NOT NULL,
    RoleId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_Account_Role
        FOREIGN KEY (RoleId) REFERENCES Role(RoleId)
);
GO

CREATE TABLE Student (
    StudentId CHAR(5) PRIMARY KEY,
    AccountId BIGINT NOT NULL UNIQUE,
    Phone VARCHAR(15) NOT NULL,
    Address NVARCHAR(250) NULL,
    Birthday DATE NULL,
    CONSTRAINT FK_Student_Account
        FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
);
GO

CREATE TABLE Teacher (
    TeacherId CHAR(5) PRIMARY KEY,
    AccountId BIGINT NOT NULL UNIQUE,
    Phone VARCHAR(15) NOT NULL,
    Specialization NVARCHAR(150) NULL,
    Bio NVARCHAR(1000) NULL,
    CONSTRAINT FK_Teacher_Account
        FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
);
GO

CREATE TABLE CourseLevel (
    LevelId INT IDENTITY(1,1) PRIMARY KEY,
    LevelCode VARCHAR(30) NOT NULL UNIQUE,
    LevelName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Course (
    CourseId CHAR(5) PRIMARY KEY,
    CourseName NVARCHAR(250) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    ImageUrl VARCHAR(255) NULL,
    Price DECIMAL(15,2) NOT NULL DEFAULT 0,
    DurationMonths INT NULL,
    Roadmap NVARCHAR(MAX) NULL,
    LevelId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Course_CourseLevel
        FOREIGN KEY (LevelId) REFERENCES CourseLevel(LevelId)
);
GO

CREATE TABLE ClassStatus (
    StatusId INT IDENTITY(1,1) PRIMARY KEY,
    StatusCode VARCHAR(30) NOT NULL UNIQUE,
    StatusName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE CenterClass (
    ClassId CHAR(5) PRIMARY KEY,
    ClassName NVARCHAR(250) NOT NULL,
    RoomName NVARCHAR(100) NULL,
    CourseId CHAR(5) NOT NULL,
    TeacherId CHAR(5) NOT NULL,
    StatusId INT NOT NULL,
    StartDate DATE NULL,
    EndDate DATE NULL,
    MaxStudents INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_CenterClass_Course
        FOREIGN KEY (CourseId) REFERENCES Course(CourseId),
    CONSTRAINT FK_CenterClass_Teacher
        FOREIGN KEY (TeacherId) REFERENCES Teacher(TeacherId),
    CONSTRAINT FK_CenterClass_Status
        FOREIGN KEY (StatusId) REFERENCES ClassStatus(StatusId)
);
GO

CREATE TABLE Schedule (
    ScheduleId BIGINT IDENTITY(1,1) PRIMARY KEY,
    ClassId CHAR(5) NOT NULL,
    DayOfWeek TINYINT NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    RoomName NVARCHAR(100) NULL,
    CONSTRAINT FK_Schedule_CenterClass
        FOREIGN KEY (ClassId) REFERENCES CenterClass(ClassId),
    CONSTRAINT CK_Schedule_DayOfWeek
        CHECK (DayOfWeek BETWEEN 2 AND 8),
    CONSTRAINT CK_Schedule_Time
        CHECK (EndTime > StartTime)
);
GO

CREATE TABLE Lesson (
    LessonId BIGINT IDENTITY(1,1) PRIMARY KEY,
    ClassId CHAR(5) NOT NULL,
    Title NVARCHAR(250) NOT NULL,
    VideoUrl VARCHAR(MAX) NULL,
    OrderIndex INT NOT NULL,
    CONSTRAINT FK_Lesson_CenterClass
        FOREIGN KEY (ClassId) REFERENCES CenterClass(ClassId),
    CONSTRAINT UQ_Lesson_Class_Order
        UNIQUE (ClassId, OrderIndex)
);
GO

CREATE TABLE EnrollmentStatus (
    StatusId INT IDENTITY(1,1) PRIMARY KEY,
    StatusCode VARCHAR(30) NOT NULL UNIQUE,
    StatusName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Enrollment (
    EnrollmentId BIGINT IDENTITY(1,1) PRIMARY KEY,
    StudentId CHAR(5) NOT NULL,
    CourseId CHAR(5) NOT NULL,
    ClassId CHAR(5) NULL,
    StatusId INT NOT NULL,
    ApprovedByTeacherId CHAR(5) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    ApprovedAt DATETIME2 NULL,
    Note NVARCHAR(500) NULL,
    CONSTRAINT FK_Enrollment_Student
        FOREIGN KEY (StudentId) REFERENCES Student(StudentId),
    CONSTRAINT FK_Enrollment_Course
        FOREIGN KEY (CourseId) REFERENCES Course(CourseId),
    CONSTRAINT FK_Enrollment_Class
        FOREIGN KEY (ClassId) REFERENCES CenterClass(ClassId),
    CONSTRAINT FK_Enrollment_ApprovedTeacher
        FOREIGN KEY (ApprovedByTeacherId) REFERENCES Teacher(TeacherId),
    CONSTRAINT FK_Enrollment_Status
        FOREIGN KEY (StatusId) REFERENCES EnrollmentStatus(StatusId),
    CONSTRAINT UQ_Enrollment_Student_Course
        UNIQUE (StudentId, CourseId)
);
GO

CREATE TABLE PaymentMethod (
    MethodId INT IDENTITY(1,1) PRIMARY KEY,
    MethodCode VARCHAR(30) NOT NULL UNIQUE,
    MethodName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE PaymentStatus (
    StatusId INT IDENTITY(1,1) PRIMARY KEY,
    StatusCode VARCHAR(30) NOT NULL UNIQUE,
    StatusName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Payment (
    PaymentId BIGINT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentId BIGINT NOT NULL,
    Amount DECIMAL(15,2) NOT NULL,
    MethodId INT NOT NULL,
    StatusId INT NOT NULL,
    TransactionNo VARCHAR(255) NULL,
    PaidAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Payment_Enrollment
        FOREIGN KEY (EnrollmentId) REFERENCES Enrollment(EnrollmentId),
    CONSTRAINT FK_Payment_Method
        FOREIGN KEY (MethodId) REFERENCES PaymentMethod(MethodId),
    CONSTRAINT FK_Payment_Status
        FOREIGN KEY (StatusId) REFERENCES PaymentStatus(StatusId)
);
GO

ALTER TABLE CenterClass
ADD CONSTRAINT CK_CenterClass_Date
CHECK (EndDate IS NULL OR StartDate IS NULL OR EndDate >= StartDate);

ALTER TABLE CenterClass
ADD CONSTRAINT CK_CenterClass_MaxStudents
CHECK (MaxStudents IS NULL OR MaxStudents > 0);

ALTER TABLE Course
ADD CONSTRAINT CK_Course_Price
CHECK (Price >= 0);

ALTER TABLE Payment
ADD CONSTRAINT CK_Payment_Amount
CHECK (Amount > 0);