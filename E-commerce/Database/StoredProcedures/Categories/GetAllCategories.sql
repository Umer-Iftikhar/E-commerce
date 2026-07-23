USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetAllCategories
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT
            200 AS ResponseCode,
            'Categories retrieved successfully.' AS ResponseMessage;

        SELECT
            Id,
            Name
        FROM dbo.Categories
        WHERE IsDeleted = 0
        ORDER BY Name;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            Id,
            Name
        FROM dbo.Categories
        WHERE 1 = 0;

    END CATCH
END
GO

