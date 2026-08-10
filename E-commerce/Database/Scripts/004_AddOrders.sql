USE ECommerce;
GO


IF OBJECT_ID('dbo.Orders', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Orders
    (
        Id          INT IDENTITY(1,1) PRIMARY KEY,

        UserId      INT NOT NULL
                    CONSTRAINT FK_Orders_Users
                        FOREIGN KEY REFERENCES dbo.Users(Id),

        Address     NVARCHAR(255) NOT NULL,

        PhoneNumber NVARCHAR(20) NOT NULL,

        PaymentMethodId INT NOT NULL,

        Status      INT NOT NULL
                    CONSTRAINT DF_Orders_Status DEFAULT (1)
                    CONSTRAINT CK_Orders_Status
                        CHECK (Status BETWEEN 1 AND 5),

        CreatedAt   DATETIME2 NOT NULL
                    CONSTRAINT DF_Orders_CreatedAt
                        DEFAULT (SYSUTCDATETIME())
    );

    CREATE INDEX IX_Orders_UserId
    ON dbo.Orders(UserId);
END
GO


IF OBJECT_ID('dbo.OrderItems', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrderItems
    (
        Id          INT IDENTITY(1,1) PRIMARY KEY,

        OrderId     INT NOT NULL
                    CONSTRAINT FK_OrderItems_Orders
                        FOREIGN KEY REFERENCES dbo.Orders(Id),

        ProductId   INT NOT NULL
                    CONSTRAINT FK_OrderItems_Products
                        FOREIGN KEY REFERENCES dbo.Products(Id),

        ProductName NVARCHAR(255) NOT NULL,

        Price       DECIMAL(18,2) NOT NULL,

        Quantity    INT NOT NULL
                        CONSTRAINT CK_OrderItems_Quantity
                            CHECK (Quantity > 0)
    );

    CREATE INDEX IX_OrderItems_OrderId
    ON dbo.OrderItems(OrderId);
END
GO