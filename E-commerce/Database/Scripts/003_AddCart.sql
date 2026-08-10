USE ECommerce;
go

IF OBJECT_ID('dbo.Carts','U') IS NULL
BEGIN
	CREATE TABLE Carts
	(
		Id INT IDENTITY (1,1) PRIMARY KEY,
		UserId INT NOT NULL,
		CreatedAt DATETIME2 NOT NULL
			CONSTRAINT DF_Carts_CreatedAt DEFAULT (SYSUTCDATETIME()),
				
		CONSTRAINT FK_Carts_Users
			FOREIGN KEY (UserId) REFERENCES dbo.Users(Id),

		CONSTRAINT UQ_Carts_UserId UNIQUE (UserId)
	);
END;
GO

IF OBJECT_ID('dbo.CartItems','U') IS NULL
BEGIN
	CREATE TABLE CartItems
	(
		ID INT IDENTITY (1,1) PRIMARY KEY,
		CartId INT NOT NULL,
		ProductId INT NOT NULL,
		Quantity INT NOT NULL
			CONSTRAINT DF_CartItems_Quantity DEFAULT (1),
		CONSTRAINT CK_CartItems_Quantity
			CHECK (Quantity > 0),

		AddedAt DATETIME2 NOT NULL
			CONSTRAINT DF_CartItems_AddedAt DEFAULT (SYSUTCDATETIME()),

		CONSTRAINT FK_CartItems_Carts
			FOREIGN KEY (CartId) REFERENCES dbo.Carts(Id),
		CONSTRAINT FK_CartItems_Products
			FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id),

		CONSTRAINT UQ_CartItems_CartProduct UNIQUE (CartId, ProductId)
	);
END
GO
