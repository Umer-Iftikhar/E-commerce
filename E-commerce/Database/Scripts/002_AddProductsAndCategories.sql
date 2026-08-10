USE ECommerce;
GO

IF OBJECT_ID('dbo.Categories', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.Categories
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		Name NVARCHAR(100) NOT NULL,
		IsDeleted BIT NOT NULL
			CONSTRAINT DF_Categories_IsDeleted DEFAULT (0),
		IsActive BIT NOT NULL
			CONSTRAINT DF_Categories_IsActive DEFAULT (1),
		CreatedAt DATETIME2 NOT NULL
			CONSTRAINT DF_Categories_CreatedAt DEFAULT (SYSUTCDATETIME()),
		CONSTRAINT UQ_Categories_Name UNIQUE (Name)
	);
END;
GO


IF OBJECT_ID('dbo.Products', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.Products
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		CategoryId INT NOT NULL,
		Name NVARCHAR(100) NOT NULL,
		Description NVARCHAR(1000) NULL,
		Price DECIMAL(18,2) NOT NULL,
		Stock INT NOT NULL,
		CoverImagePath NVARCHAR(500) NULL,
		IsDeleted BIT NOT NULL
			CONSTRAINT DF_Products_IsDeleted DEFAULT (0),
		IsActive BIT NOT NULL
			CONSTRAINT DF_Products_IsActive DEFAULT (1),
		CreatedAt DATETIME2 NOT NULL
			CONSTRAINT DF_Products_CreatedAt DEFAULT (SYSUTCDATETIME()),

		CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId)
			REFERENCES dbo.Categories(Id),
		
		CONSTRAINT CK_Products_Price
			CHECK (Price >= 0),

		CONSTRAINT CK_Products_Stock
			CHECK (Stock >= 0)
	);

	CREATE INDEX IX_Products_CategoryId
		ON dbo.Products(CategoryId);

	CREATE INDEX IX_Products_IsDeleted
        ON dbo.Products(IsDeleted);
END;
GO
