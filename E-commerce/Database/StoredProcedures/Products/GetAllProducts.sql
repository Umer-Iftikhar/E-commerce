USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetAllProducts
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
            p.CreatedAt,

            c.Id AS CategoryId,
            c.Name AS CategoryName,

            pi.StoredFileName,
            pi.FileExtension,
            pi.MimeType

        FROM dbo.Products AS p

        INNER JOIN dbo.Categories AS c
            ON c.Id = p.CategoryId

        LEFT JOIN dbo.ProductImages AS pi
            ON pi.ProductId = p.Id
           AND pi.IsPrimary = 1

        WHERE p.IsDeleted = 0
          AND c.IsDeleted = 0

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
            p.CreatedAt,

            c.Id AS CategoryId,
            c.Name AS CategoryName,

            pi.StoredFileName,
            pi.FileExtension,
            pi.MimeType

        FROM dbo.Products AS p

        INNER JOIN dbo.Categories AS c
            ON c.Id = p.CategoryId

        LEFT JOIN dbo.ProductImages AS pi
            ON pi.ProductId = p.Id
           AND pi.IsPrimary = 1

        WHERE 1 = 0;

    END CATCH
END
GO