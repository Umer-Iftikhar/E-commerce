USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.SoftDeleteCategory
(
    @CategoryId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Categories
            WHERE Id = @CategoryId
              AND IsDeleted = 0
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Category not found.' AS ResponseMessage;

            RETURN;
        END

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Products
            WHERE CategoryId = @CategoryId
              AND IsDeleted = 0
        )
        BEGIN
            SELECT
                409 AS ResponseCode,
                'Cannot delete category because active products exist under this category.' AS ResponseMessage;

            RETURN;
        END

        UPDATE dbo.Categories
        SET
            IsDeleted = 1
        WHERE Id = @CategoryId;

        SELECT
            200 AS ResponseCode,
            'Category deleted successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO