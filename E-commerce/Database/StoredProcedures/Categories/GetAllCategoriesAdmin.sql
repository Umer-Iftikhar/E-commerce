USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetAllCategoriesAdmin
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            200 AS ResponseCode,
            'Categories retrieved successfully.' AS ResponseMessage;

        SELECT
            Id,
            Name,
            IsDeleted
        FROM dbo.Categories
        ORDER BY
            IsDeleted,
            Name;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS Id,
            CAST(NULL AS VARCHAR(100)) AS Name,
            CAST(NULL AS BIT) AS IsDeleted
        WHERE 1 = 0;

    END CATCH
END
GO