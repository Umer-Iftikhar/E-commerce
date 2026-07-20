USE ECommerce;
GO
/*==============================================================*/
/* Categories                                                   */
/*==============================================================*/
IF OBJECT_ID('dbo.Categories', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name VARCHAR(100) NOT NULL,
        IsDeleted BIT NOT NULL
            CONSTRAINT DF_Categories_IsDeleted
                DEFAULT (0),
        CreatedAt DATETIME2 NOT NULL
            CONSTRAINT DF_Categories_CreatedAt
                DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Categories_Name
            UNIQUE (Name)
    );
END
GO
/*==============================================================*/
/* Products                                                     */
/*==============================================================*/
IF OBJECT_ID('dbo.Products', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CategoryId INT NOT NULL,
        Name VARCHAR(200) NOT NULL,
        Description VARCHAR(2000) NULL,
        Price DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL
            CONSTRAINT DF_Products_Stock
                DEFAULT (0),
        IsDeleted BIT NOT NULL
            CONSTRAINT DF_Products_IsDeleted
                DEFAULT (0),
        CreatedAt DATETIME2 NOT NULL
            CONSTRAINT DF_Products_CreatedAt
                DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT FK_Products_Categories
            FOREIGN KEY (CategoryId)
            REFERENCES dbo.Categories(Id)
    );
    CREATE INDEX IX_Products_CategoryId
        ON dbo.Products(CategoryId);
END
GO
/*==============================================================*/
/* ProductImages                                                */
/*==============================================================*/
IF OBJECT_ID('dbo.ProductImages', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductImages
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ProductId INT NOT NULL,
        StoredFileName VARCHAR(255) NOT NULL,
        FileExtension VARCHAR(20) NOT NULL,
        MimeType VARCHAR(100) NOT NULL,
        Width INT NOT NULL,
        Height INT NOT NULL,
        FileSizeBytes INT NOT NULL,
        IsPrimary BIT NOT NULL
            CONSTRAINT DF_ProductImages_IsPrimary
                DEFAULT (0),
        UploadedAt DATETIME2 NOT NULL
            CONSTRAINT DF_ProductImages_UploadedAt
                DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT FK_ProductImages_Products
            FOREIGN KEY (ProductId)
            REFERENCES dbo.Products(Id)
    );
    CREATE INDEX IX_ProductImages_ProductId
        ON dbo.ProductImages(ProductId);
    CREATE UNIQUE INDEX UX_ProductImages_Primary
        ON dbo.ProductImages(ProductId)
        WHERE IsPrimary = 1;
END
GO