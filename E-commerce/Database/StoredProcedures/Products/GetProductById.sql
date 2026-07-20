USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetProductById
(
    @ProductId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Products
            WHERE Id = @ProductId
              AND IsDeleted = 0
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Product not found.' AS ResponseMessage;

            SELECT
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.CreatedAt,
                c.Id AS CategoryId,
                c.Name AS CategoryName
            FROM dbo.Products AS p
            INNER JOIN dbo.Categories AS c
                ON c.Id = p.CategoryId
            WHERE 1 = 0;

            SELECT
                Id,
                ProductId,
                StoredFileName,
                FileExtension,
                MimeType,
                Width,
                Height,
                FileSizeBytes,
                UploadedAt,
                IsPrimary
            FROM dbo.ProductImages
            WHERE 1 = 0;

            RETURN;
        END

        SELECT
            200 AS ResponseCode,
            'Product retrieved successfully.' AS ResponseMessage;

        SELECT
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.Stock,
            p.CreatedAt,
            c.Id AS CategoryId,
            c.Name AS CategoryName
        FROM dbo.Products AS p
        INNER JOIN dbo.Categories AS c
            ON c.Id = p.CategoryId
        WHERE p.Id = @ProductId
          AND p.IsDeleted = 0
          AND c.IsDeleted = 0;

        SELECT
            Id,
            ProductId,
            StoredFileName,
            FileExtension,
            MimeType,
            Width,
            Height,
            FileSizeBytes,
            UploadedAt,
            IsPrimary
        FROM dbo.ProductImages
        WHERE ProductId = @ProductId
        ORDER BY
            IsPrimary DESC,
            UploadedAt;

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
            c.Name AS CategoryName
        FROM dbo.Products AS p
        INNER JOIN dbo.Categories AS c
            ON c.Id = p.CategoryId
        WHERE 1 = 0;

        SELECT
            Id,
            ProductId,
            StoredFileName,
            FileExtension,
            MimeType,
            Width,
            Height,
            FileSizeBytes,
            UploadedAt,
            IsPrimary
        FROM dbo.ProductImages
        WHERE 1 = 0;

    END CATCH
END
GO