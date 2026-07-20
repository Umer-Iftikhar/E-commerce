

USE ECommerce;
GO

/*==============================================================*/
/* Roles                                                        */
/*==============================================================*/

IF OBJECT_ID('dbo.Roles', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,

        Name VARCHAR(50) NOT NULL,

        CONSTRAINT UQ_Roles_Name
            UNIQUE (Name)
    );
END
GO

/*==============================================================*/
/* UserRoles                                                    */
/*==============================================================*/

IF OBJECT_ID('dbo.UserRoles', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserRoles
    (
        UserId INT NOT NULL,

        RoleId INT NOT NULL,

        AssignedAt DATETIME2 NOT NULL
            CONSTRAINT DF_UserRoles_AssignedAt
                DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_UserRoles
            PRIMARY KEY (UserId, RoleId),

        CONSTRAINT FK_UserRoles_Users
            FOREIGN KEY (UserId)
            REFERENCES dbo.Users(Id),

        CONSTRAINT FK_UserRoles_Roles
            FOREIGN KEY (RoleId)
            REFERENCES dbo.Roles(Id)
    );

    CREATE INDEX IX_UserRoles_RoleId
    ON dbo.UserRoles(RoleId);
END
GO

/*==============================================================*/
/* Seed Roles                                                   */
/*==============================================================*/

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Roles
    WHERE Name = 'Admin'
)
BEGIN
    INSERT INTO dbo.Roles (Name)
    VALUES ('Admin');
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Roles
    WHERE Name = 'Customer'
)
BEGIN
    INSERT INTO dbo.Roles (Name)
    VALUES ('Customer');
END
GO