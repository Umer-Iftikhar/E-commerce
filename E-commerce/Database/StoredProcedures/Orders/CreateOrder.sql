USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.CreateOrder
(
    @UserId INT,
    @Address NVARCHAR(255),
    @PhoneNumber NVARCHAR(20),
    @PaymentMethodId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        -- Validate cart exists
        DECLARE @CartId INT;

        SELECT @CartId = Id
        FROM dbo.Carts
        WHERE UserId = @UserId;

        IF @CartId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT
                404 AS ResponseCode,
                'Cart not found.' AS ResponseMessage;

            SELECT
                CAST(NULL AS INT) AS ProductId
            WHERE 1 = 0;

            RETURN;
        END

        -- Validate cart has items
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.CartItems
            WHERE CartId = @CartId
        )
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT
                400 AS ResponseCode,
                'Your cart is empty.' AS ResponseMessage;

            SELECT
                CAST(NULL AS INT) AS ProductId
            WHERE 1 = 0;

            RETURN;
        END

        -- Validate products
        DECLARE @InvalidProducts TABLE
        (
            ProductId INT
        );

        INSERT INTO @InvalidProducts (ProductId)
        SELECT
            ci.ProductId
        FROM dbo.CartItems ci
        LEFT JOIN dbo.Products p WITH (UPDLOCK, HOLDLOCK)
            ON p.Id = ci.ProductId
        WHERE
            ci.CartId = @CartId
            AND
            (
                p.Id IS NULL
                OR p.Stock < ci.Quantity
                OR p.IsDeleted = 1
            );

        IF EXISTS (SELECT 1 FROM @InvalidProducts)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT
                400 AS ResponseCode,
                'One or more products are unavailable.' AS ResponseMessage;

            SELECT ProductId
            FROM @InvalidProducts;

            RETURN;
        END

        -- Create order
        INSERT INTO dbo.Orders
        (
            UserId,
            Address,
            PhoneNumber,
            PaymentMethodId,
            Status
        )
        VALUES
        (
            @UserId,
            @Address,
            @PhoneNumber,
            @PaymentMethodId,
            1
        );

        DECLARE @OrderId INT = SCOPE_IDENTITY();

        -- Create order items
        INSERT INTO dbo.OrderItems
        (
            OrderId,
            ProductId,
            ProductName,
            Price,
            Quantity
        )
        SELECT
            @OrderId,
            p.Id,
            p.Name,
            p.Price,
            ci.Quantity
        FROM dbo.CartItems ci
        INNER JOIN dbo.Products p
            ON p.Id = ci.ProductId
        WHERE ci.CartId = @CartId;

        -- Reduce stock
        UPDATE p
        SET
            p.Stock = p.Stock - ci.Quantity
        FROM dbo.Products p 
        INNER JOIN dbo.CartItems ci
            ON ci.ProductId = p.Id
        WHERE ci.CartId = @CartId;

        -- Clear cart
        DELETE
        FROM dbo.CartItems
        WHERE CartId = @CartId;

        DELETE
        FROM dbo.Carts
        WHERE Id = @CartId;

        COMMIT TRANSACTION;

        SELECT
            200 AS ResponseCode,
            'Order placed successfully.' AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS ProductId
        WHERE 1 = 0;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS ProductId
        WHERE 1 = 0;

    END CATCH
END;