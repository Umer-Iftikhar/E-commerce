USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.RemoveFromCart
    @UserId INT,
    @CartItemId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.CartItems ci
            INNER JOIN dbo.Carts c
                ON ci.CartId = c.Id
            WHERE ci.Id = @CartItemId
              AND c.UserId = @UserId
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Cart item not found.' AS ResponseMessage;

            RETURN;
        END;

        DELETE ci
        FROM dbo.CartItems ci
        INNER JOIN dbo.Carts c
            ON ci.CartId = c.Id
        WHERE ci.Id = @CartItemId
          AND c.UserId = @UserId;

        SELECT
            200 AS ResponseCode,
            'Item removed from cart successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END;
GO