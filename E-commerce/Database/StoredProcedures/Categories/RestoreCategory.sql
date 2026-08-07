USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.RestoreCategory
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
              AND IsDeleted = 1
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Deleted category not found.' AS ResponseMessage;

            RETURN;
        END

        UPDATE dbo.Categories
        SET
            IsDeleted = 0
        WHERE Id = @CategoryId;

        SELECT
            200 AS ResponseCode,
            'Category restored successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO