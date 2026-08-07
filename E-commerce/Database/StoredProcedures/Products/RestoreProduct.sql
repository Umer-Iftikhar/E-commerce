USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.RestoreProduct
(
    @ProductId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Products
            WHERE Id = @ProductId
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Product not found.' AS ResponseMessage;
            RETURN;
        END

        UPDATE dbo.Products
        SET IsDeleted = 0
        WHERE Id = @ProductId;

        SELECT
            200 AS ResponseCode,
            'Product restored successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO