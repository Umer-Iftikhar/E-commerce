USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.UpdateCartItemQuantity
    @UserId INT,
    @CartItemId INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF @Quantity <= 0
        BEGIN
            SELECT
                400 AS ResponseCode,
                'Quantity must be greater than zero.' AS ResponseMessage;

            RETURN;
        END;

        DECLARE
            @Stock INT,
            @ProductId INT;

        SELECT
            @ProductId = ci.ProductId,
            @Stock = p.Stock
        FROM dbo.CartItems ci
        INNER JOIN dbo.Carts c
            ON ci.CartId = c.Id
        INNER JOIN dbo.Products p
            ON ci.ProductId = p.Id
        WHERE ci.Id = @CartItemId
          AND c.UserId = @UserId;

        IF @ProductId IS NULL
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Cart item not found.' AS ResponseMessage;

            RETURN;
        END;

        IF @Quantity > @Stock
        BEGIN
            SELECT
                400 AS ResponseCode,
                'Requested quantity exceeds available stock.' AS ResponseMessage;

            RETURN;
        END;

        UPDATE dbo.CartItems
        SET Quantity = @Quantity
        WHERE Id = @CartItemId;

        SELECT
            200 AS ResponseCode,
            'Cart updated successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END;
GO