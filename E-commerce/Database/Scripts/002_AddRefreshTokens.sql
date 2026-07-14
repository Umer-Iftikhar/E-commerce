USE ECommerce;
GO

/*==============================================================*/
/* RefreshTokens                                                */
/*==============================================================*/

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


