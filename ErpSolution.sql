
CREATE TABLE Genders (
    GenderID INT IDENTITY(1,1) PRIMARY KEY,
    GenderName VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255),
	IsActive BIT DEFAULT 1
);

CREATE TABLE AcademicYears (
    AcademicYearID INT IDENTITY(1,1) PRIMARY KEY,
    YearName VARCHAR(20) NOT NULL, -- e.g. 2025-2026
	YearStart DATE NOT NULL,
    YearEnd DATE NOT NULL,
    IsActive BIT DEFAULT 1
);

CREATE TABLE Classes (
    ClassId INT PRIMARY KEY IDENTITY,
    ClassName NVARCHAR(50),   -- e.g. 10, 9
);

CREATE TABLE CourseFees (
    CourseFeeId INT PRIMARY KEY IDENTITY,
    ClassId INT NOT NULL,                  -- Which class
    AcademicYearId INT NOT NULL,           -- Which year
    TuitionFee DECIMAL(10,2) NOT NULL,    -- Main tuition fee
    LabFee DECIMAL(10,2) NULL,            -- Optional
    LibraryFee DECIMAL(10,2) NULL,        -- Optional
    OtherFee DECIMAL(10,2) NULL,          -- Optional
    TotalFee AS (TuitionFee + ISNULL(LabFee,0) + ISNULL(LibraryFee,0) + ISNULL(OtherFee,0)),  -- Computed column

    CONSTRAINT FK_CF_Class FOREIGN KEY (ClassId)
        REFERENCES Classes(ClassId),
    CONSTRAINT FK_CF_AcademicYear FOREIGN KEY (AcademicYearId)
        REFERENCES AcademicYears(AcademicYearId)
);

CREATE TABLE Sections (
    SectionId INT PRIMARY KEY IDENTITY,
    SectionName NVARCHAR(5)  -- A, B, C
);

CREATE TABLE ClassSections (
    Id INT PRIMARY KEY IDENTITY,
    ClassId INT,
    SectionId INT,
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (SectionId) REFERENCES Sections(SectionId)
);


CREATE TABLE Subjects (
    SubjectId INT IDENTITY(1,1),
    SubjectName NVARCHAR(150) NOT NULL,	-- e.g., "Physics", "Math"
	SubjectCode VARCHAR(10) NULL,		-- e.g., "PHY", "MATH", "CHE"
	Description VARCHAR(150) NULL,
    IsActive BIT DEFAULT 1,
	CONSTRAINT PK_Subjects_SubjectID PRIMARY KEY (SubjectID),
);

CREATE TABLE Teachers (
    TeacherID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL UNIQUE,
    Designation VARCHAR(100),

    FullName VARCHAR(150) NOT NULL,
    GenderID INT NOT NULL,
    DateOfBirth DATE,
    BloodGroup VARCHAR(3),
    FatherName VARCHAR(150),
    MotherName VARCHAR(150),
    EmergencyContact VARCHAR(10),
	IsMarried BIT DEFAULT 0,
	SpouseName VARCHAR(100),
	SpouseContact VARCHAR(10),
    Aadhaar VARCHAR(30),
    Phone VARCHAR(15),
    EmailID VARCHAR(100),
    PresentAddress VARCHAR(250),
    PermanentAddress VARCHAR(250), 

    Qualification VARCHAR(100),
	Specialization VARCHAR(250),
    Experience NUMERIC(3,1),   
    JoiningDate DATE,
    LastWorkingDate DATE,

    IsActive BIT DEFAULT 1,

    CONSTRAINT FK_Teacher_Users FOREIGN KEY (UserID)
        REFERENCES Users(UserID),
    CONSTRAINT FK_Teacher_Genders FOREIGN KEY (GenderID)
        REFERENCES Genders(GenderID)
);

CREATE TABLE ClassSubjects (
    Id INT PRIMARY KEY IDENTITY,
    ClassId INT,
    SubjectId INT,
    TeacherId INT,
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId),
    FOREIGN KEY (TeacherId) REFERENCES Teachers(TeacherId)
);

--Students (Basic Info)
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL UNIQUE,
   
	ClassSectionId INT,
    -- Student Information
    StudentName VARCHAR(150) NOT NULL,
    GenderID INT NOT NULL,
    PermanentAddress VARCHAR(250),
    PhoneNo VARCHAR(15),
    EmailID VARCHAR(150),
    DateOfBirth DATE,
    BloodGroup VARCHAR(3),
    FatherName VARCHAR(150),
    MotherName VARCHAR(150),
    EmergencyContact VARCHAR(10),
    Aadhaar VARCHAR(30),

    -- Academic Info
    RollNumber VARCHAR(50) NOT NULL UNIQUE, --RollNumber = $"STU-{dto.AdmissionYear}-{student.StudentID}";
    AdmissionYear INT,
    Semester INT,
	AcademicYearID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_Students_Users FOREIGN KEY (UserID)
        REFERENCES Users(UserID),
    FOREIGN KEY (AcademicYearID) REFERENCES AcademicYears(AcademicYearID),
    CONSTRAINT FK_Students_Genders FOREIGN KEY (GenderID)
        REFERENCES Genders(GenderID),
		FOREIGN KEY (ClassSectionId) REFERENCES ClassSections(Id)
);

CREATE TABLE StudentEnrollments (
    EnrollmentId INT PRIMARY KEY IDENTITY,
    StudentId INT,
    ClassId INT,
    AcademicYearId INT,
    RollNumber INT,

    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (AcademicYearId) REFERENCES AcademicYears(AcademicYearId)
);

CREATE TABLE Attendance (
    AttendanceID INT PRIMARY KEY IDENTITY,
    StudentId INT,
	TeacherID INT NULL,
    SubjectID INT NOT NULL,                -- Track attendance for a specific subject
    AttendanceDate DATE NOT NULL,
    Status VARCHAR(10) CHECK (Status IN ('Present','Absent')),
    CONSTRAINT FK_Attendance_Student FOREIGN KEY (StudentID)
        REFERENCES Students(StudentID),
    CONSTRAINT FK_Attendance_Subject FOREIGN KEY (SubjectID)
        REFERENCES Subjects(SubjectID)
);

CREATE TABLE StudentFees (
    FeeId INT PRIMARY KEY IDENTITY,
    StudentId INT,
    PaidAmount DECIMAL(10,2) DEFAULT 0,
    PaymentDate DATE,
	AcademicYearID INT NOT NULL,
	CourseFeeId INT,
    Status VARCHAR(20) CHECK (Status IN ('PAID','PARTIAL','DUE')) DEFAULT 'DUE',
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
	FOREIGN KEY (AcademicYearID) REFERENCES AcademicYears(AcademicYearID),
	FOREIGN KEY (CourseFeeId) REFERENCES CourseFees(CourseFeeId)
);

CREATE TABLE FeePayments (
	PaymentID INT IDENTITY(1,1) PRIMARY KEY,
	FeeId INT NOT NULL,
	PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
	AmountPaid DECIMAL(10,2) NOT NULL,
	PaymentMethod VARCHAR(50),  -- Bank Transfer, Cash
	TransactionID VARCHAR(50) NULL,
	Remarks VARCHAR(250),
	CONSTRAINT FK_FeePayments_StudentFee FOREIGN KEY (FeeId)
		REFERENCES StudentFees(FeeId)
);

CREATE TABLE Exams (
    ExamId INT PRIMARY KEY IDENTITY,
    ExamName NVARCHAR(50),  -- e.g. Half Yearly
    Description NVARCHAR(100),
	AcademicYearID INT NOT NULL,
	FOREIGN KEY (AcademicYearId) REFERENCES AcademicYears(AcademicYearId)
);

CREATE TABLE ExamSchedules (
    ScheduleId INT PRIMARY KEY IDENTITY,
    ExamId INT,
    ClassId INT,
    SubjectId INT,
    ExamDate DATE,
    PassingMarks INT,
    MaxMarks INT,
    FOREIGN KEY (ExamId) REFERENCES Exams(ExamId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId)
);

CREATE TABLE Marks (
    Id INT PRIMARY KEY IDENTITY,
    StudentId INT,
    SubjectId INT,
    ExamId INT,
    MarksObtained INT,
	Grade NVARCHAR(5),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId),
    FOREIGN KEY (ExamId) REFERENCES Exams(ExamId)
);

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    Email VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
	FailedLoginCount INT DEFAULT 0,
    LockoutUntil DATETIME NULL,
    LastLogin DATETIME NULL,
	IsActive BIT DEFAULT 1,
);

CREATE TABLE UserRoles (
    UserRoleID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    RoleID INT NOT NULL,
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserID)
        REFERENCES Users(UserID),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleID)
        REFERENCES Roles(RoleID),
    CONSTRAINT UQ_User_Role UNIQUE (UserID, RoleID)
);

CREATE TABLE LoginAttempts (
    AttemptID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    AttemptTime DATETIME DEFAULT GETDATE(),
    IsSuccessful BIT,
    IPAddress VARCHAR(50),
    CONSTRAINT FK_LoginAttempts_Users FOREIGN KEY (UserID)
        REFERENCES Users(UserID)
);
