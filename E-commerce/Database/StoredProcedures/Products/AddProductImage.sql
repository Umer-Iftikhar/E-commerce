USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.AddProductImage
(
    @ProductId INT,
    @StoredFileName VARCHAR(255),
    @FileExtension VARCHAR(20),
    @MimeType VARCHAR(100),
    @Width INT,
    @Height INT,
    @FileSizeBytes INT,
    @IsPrimary BIT
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
            RETURN;
        END

        IF @IsPrimary = 1
            BEGIN
                UPDATE dbo.ProductImages
                SET IsPrimary = 0
                WHERE ProductId = @ProductId AND IsPrimary = 1
            END

        INSERT INTO dbo.ProductImages
        (
            ProductId,
            StoredFileName,
            FileExtension,
            MimeType,
            Width,
            Height,
            FileSizeBytes,
            IsPrimary
        )
        VALUES
        (
            @ProductId,
            @StoredFileName,
            @FileExtension,
            @MimeType,
            @Width,
            @Height,
            @FileSizeBytes,
            @IsPrimary
        );
        SELECT
            200 AS ResponseCode,
            'Product image added successfully.' AS ResponseMessage;
    END TRY
    BEGIN CATCH
        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO