USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetAllProductsAdmin
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            200 AS ResponseCode,
            'Products retrieved successfully.' AS ResponseMessage;

        SELECT
            P.Id,
            P.Name,
            P.Description,
            P.Price,
            P.Stock,
            P.CoverImagePath,
            P.CategoryId,
            C.Name AS CategoryName,
            P.IsDeleted
        FROM dbo.Products P
        INNER JOIN dbo.Categories C
            ON P.CategoryId = C.Id
        ORDER BY P.CreatedAt DESC;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS Id,
            CAST(NULL AS NVARCHAR(100)) AS Name,
            CAST(NULL AS NVARCHAR(1000)) AS Description,
            CAST(NULL AS DECIMAL(18,2)) AS Price,
            CAST(NULL AS INT) AS Stock,
            CAST(NULL AS NVARCHAR(500)) AS CoverImagePath,
            CAST(NULL AS INT) AS CategoryId,
            CAST(NULL AS NVARCHAR(100)) AS CategoryName,
            CAST(NULL AS BIT) AS IsDeleted
        WHERE 1 = 0;

    END CATCH
END
GO