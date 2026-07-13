USE ECommerce;
GO
/*==============================================================*/
/* Users                                                        */
/*==============================================================*/
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name VARCHAR(100) NOT NULL,
        Email VARCHAR(250) NOT NULL,
        PasswordHash VARCHAR(300) NOT NULL,
        IsActive BIT NOT NULL
            CONSTRAINT DF_Users_IsActive DEFAULT (1),
        IsDeleted BIT NOT NULL
            CONSTRAINT DF_Users_IsDeleted DEFAULT (0),
        CreatedAt DATETIME2 NOT NULL
            CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Users_Email
            UNIQUE (Email)
    );
END
GO

/*==============================================================*/
/* UserAvatars                                                  */
/*==============================================================*/

IF OBJECT_ID('dbo.UserAvatars', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserAvatars
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        OriginalFileName VARCHAR(255) NOT NULL,
        StoredFileName VARCHAR(255) NOT NULL,
        FileExtension VARCHAR(20) NOT NULL,
        MimeType VARCHAR(100) NOT NULL,
        Width INT NOT NULL,
        Height INT NOT NULL,
        FileSizeBytes INT NOT NULL,
        UploadedAt DATETIME2 NOT NULL
            CONSTRAINT DF_UserAvatars_UploadedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT FK_UserAvatars_Users
            FOREIGN KEY (UserId)
            REFERENCES dbo.Users(Id),
        CONSTRAINT UQ_UserAvatars_UserId
            UNIQUE (UserId)
    );
END
GO

/*==============================================================*/
/* ImageUploadAttempts                                          */
/*==============================================================*/

IF OBJECT_ID('dbo.ImageUploadAttempts', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ImageUploadAttempts
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UploadToken UNIQUEIDENTIFIER NOT NULL,
        TempFileName VARCHAR(255) NOT NULL,
        Status VARCHAR(20) NOT NULL
            CONSTRAINT DF_ImageUploadAttempts_Status DEFAULT ('Pending'),
        CreatedAt DATETIME2 NOT NULL
            CONSTRAINT DF_ImageUploadAttempts_CreatedAt DEFAULT (SYSUTCDATETIME()),
        ExpiresAt DATETIME2 NOT NULL,
        CompletedAt DATETIME2 NULL,
        CONSTRAINT UQ_ImageUploadAttempts_UploadToken
            UNIQUE (UploadToken),
        CONSTRAINT CK_ImageUploadAttempts_Status
            CHECK (Status IN ('Pending', 'Completed', 'Failed', 'Expired'))
    );
END
GO