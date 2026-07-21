USE ECommerce;
GO


IF OBJECT_ID('dbo.Roles', 'U') IS NULL
BEGIN
CREATE TABLE dbo.Roles
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Name VARCHAR(50) NOT NULL,

    CONSTRAINT UQ_Roles_Name UNIQUE (Name)
);
END
GO


IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
Create table dbo.Users
(
	Id INT Identity(1,1) PRIMARY KEY,
	Name VARCHAR (100) NOT NULL,
	Email VARCHAR(250) NOT NULL,
        PasswordHash VARCHAR(300) NOT NULL,
    IsActive BIT NOT NULL
            CONSTRAINT DF_Users_IsActive DEFAULT (1),
    IsDeleted BIT NOT NULL
            CONSTRAINT DF_Users_IsDeleted DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL
            CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT UQ_Users_Email
            UNIQUE (Email),
    RoleId INT NOT NULL CONSTRAINT FK_Users_Roles FOREIGN KEY REFERENCES dbo.Roles(Id),
    ProfileImagePath VARCHAR(1000) NULL
);
END
GO


IF OBJECT_ID('dbo.RefreshTokens', 'U') IS NULL
BEGIN
CREATE TABLE dbo.RefreshTokens
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,

        UserId INT NOT NULL,

        Token VARCHAR(255) NOT NULL,

        ExpiresAt DATETIME2 NOT NULL,

        CreatedAt DATETIME2 NOT NULL
            CONSTRAINT DF_RefreshTokens_CreatedAt
                DEFAULT (SYSUTCDATETIME()),

        IsRevoked BIT NOT NULL
            CONSTRAINT DF_RefreshTokens_IsRevoked
                DEFAULT (0),

        CONSTRAINT FK_RefreshTokens_Users
            FOREIGN KEY (UserId)
            REFERENCES dbo.Users(Id),

        CONSTRAINT UQ_RefreshTokens_Token
            UNIQUE (Token)
    );

    CREATE INDEX IX_RefreshTokens_UserId
    ON dbo.RefreshTokens(UserId);
END
GO

INSERT INTO dbo.Roles (Name)
SELECT 'Admin'
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Roles
    WHERE Name = 'Admin'
);

INSERT INTO dbo.Roles (Name)
SELECT 'Customer'
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Roles
    WHERE Name = 'Customer'
);