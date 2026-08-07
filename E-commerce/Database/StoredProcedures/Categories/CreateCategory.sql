USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.CreateCategory
(
    @Name NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

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
        )
        BEGIN
            IF EXISTS
            (
                SELECT 1
                FROM dbo.Categories
                WHERE Name = @Name
                  AND IsDeleted = 1
            )
            BEGIN
                SELECT
                    409 AS ResponseCode,
                    'Category already exists but is deleted. Restore it instead.' AS ResponseMessage;

                RETURN;
            END

            SELECT
                409 AS ResponseCode,
                'Category already exists.' AS ResponseMessage;

            RETURN;
        END

        INSERT INTO dbo.Categories
        (
            Name
        )
        VALUES
        (
            @Name
        );

        SELECT
            200 AS ResponseCode,
            'Category created successfully.' AS ResponseMessage,
            CAST(SCOPE_IDENTITY() AS INT) AS CategoryId;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO