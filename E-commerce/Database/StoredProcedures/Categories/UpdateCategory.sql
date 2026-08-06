USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.UpdateCategory
(
    @CategoryId INT,
    @Name NVARCHAR(100)
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

        SET @Name = LTRIM(RTRIM(@Name));

        IF NULLIF(@Name, '') IS NULL
        BEGIN
            SELECT
                400 AS ResponseCode,
                'Category name is required.' AS ResponseMessage;

            RETURN;
        END

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Categories
            WHERE Name = @Name
              AND Id <> @CategoryId
        )
        BEGIN
            SELECT
                409 AS ResponseCode,
                'A category with this name already exists.' AS ResponseMessage;

            RETURN;
        END

        UPDATE dbo.Categories
        SET
            Name = @Name
        WHERE Id = @CategoryId;

        SELECT
            200 AS ResponseCode,
            'Category updated successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO