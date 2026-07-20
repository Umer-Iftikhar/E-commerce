USE ECommerce;
GO


CREATE OR ALTER PROCEDURE dbo.UpdateProduct
(
    @ProductId INT,
    @CategoryId INT,
    @Name VARCHAR(200),
    @Description VARCHAR(2000) = NULL,
    @Price DECIMAL(18,2),
    @Stock INT
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

        UPDATE dbo.Products
        SET
            CategoryId = @CategoryId,
            Name = @Name,
            Description = @Description,
            Price = @Price,
            Stock = @Stock
        WHERE Id = @ProductId;

        SELECT
            200 AS ResponseCode,
            'Product updated successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO