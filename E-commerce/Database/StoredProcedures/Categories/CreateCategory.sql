USE ECommerce;
GO



CREATE OR ALTER PROCEDURE dbo.CreateCategory
(
    @Name VARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Categories
            WHERE Name = @Name
              AND IsDeleted = 0
        )
        BEGIN
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