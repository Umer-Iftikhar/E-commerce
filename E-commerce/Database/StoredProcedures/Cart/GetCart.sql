CREATE OR ALTER PROCEDURE dbo.GetCart
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        DECLARE @CartId INT;

        SELECT @CartId = Id
        FROM dbo.Carts
        WHERE UserId = @UserId;

        IF @CartId IS NULL
        BEGIN
            SELECT
                200 AS ResponseCode,
                'Cart is empty.' AS ResponseMessage;

            SELECT
                ci.Id AS CartItemId,
                p.Id AS ProductId,
                p.Name AS ProductName,
                p.Price,
                ci.Quantity,
                p.CoverImagePath
                FROM dbo.CartItems ci
                INNER JOIN dbo.Products p
                    ON ci.ProductId = p.Id
            WHERE 1 = 0;

            RETURN;
        END;

        SELECT
            200 AS ResponseCode,
            'Cart retrieved successfully.' AS ResponseMessage;

        SELECT
            ci.Id AS CartItemId,
            p.Id AS ProductId,
            p.Name AS ProductName,
            p.Price,
            ci.Quantity,
            p.CoverImagePath
        FROM dbo.CartItems ci
        INNER JOIN dbo.Products p
            ON ci.ProductId = p.Id
        WHERE ci.CartId = @CartId
        AND p.IsDeleted = 0
        ORDER BY ci.AddedAt DESC;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

            SELECT
                ci.Id AS CartItemId,
                p.Id AS ProductId,
                p.Name AS ProductName,
                p.Price,
                ci.Quantity,
                p.CoverImagePath
                FROM dbo.CartItems ci
                INNER JOIN dbo.Products p
                    ON ci.ProductId = p.Id
            WHERE 1 = 0;

    END CATCH
END;
GO

