USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetProducts
(
    @SearchTerm NVARCHAR(100) = NULL,
    @CategoryId INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            200 AS ResponseCode,
            'Products retrieved successfully.' AS ResponseMessage;

        SELECT
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.Stock,
            p.CoverImagePath,
            c.Id AS CategoryId,
            c.Name AS CategoryName
        FROM dbo.Products AS p
        INNER JOIN dbo.Categories AS c
            ON c.Id = p.CategoryId
        WHERE p.IsDeleted = 0
          AND c.IsDeleted = 0
          AND (@SearchTerm IS NULL OR p.Name LIKE '%' + @SearchTerm + '%')
          AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
        ORDER BY p.Name;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.Stock,
            p.CoverImagePath,
            c.Id AS CategoryId,
            c.Name AS CategoryName
        FROM dbo.Products AS p
        INNER JOIN dbo.Categories AS c
            ON c.Id = p.CategoryId
        WHERE 1 = 0;

    END CATCH
END
GO