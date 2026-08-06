USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.SoftDeleteProduct
(
    @ProductId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        DECLARE @CoverImagePath NVARCHAR(500);

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Products
            WHERE Id = @ProductId
              AND IsDeleted = 0
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Product not found.' AS ResponseMessage,
                CAST(NULL AS NVARCHAR(500)) AS CoverImagePath;

            RETURN;
        END

        SELECT
            @CoverImagePath = CoverImagePath
        FROM dbo.Products
        WHERE Id = @ProductId;

        UPDATE dbo.Products
        SET
            IsDeleted = 1
        WHERE Id = @ProductId;

        SELECT
            200 AS ResponseCode,
            'Product deleted successfully.' AS ResponseMessage,
            @CoverImagePath AS CoverImagePath;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage,
            CAST(NULL AS NVARCHAR(500)) AS CoverImagePath;

    END CATCH
END
GO