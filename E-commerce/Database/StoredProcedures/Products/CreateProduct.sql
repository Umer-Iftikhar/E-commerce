USE ECommerce;
GO



CREATE OR ALTER PROCEDURE dbo.CreateProduct
(
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

        -- Validate category
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

        INSERT INTO dbo.Products
        (
            CategoryId,
            Name,
            Description,
            Price,
            Stock
        )
        VALUES
        (
            @CategoryId,
            @Name,
            @Description,
            @Price,
            @Stock
        );

        SELECT
            200 AS ResponseCode,
            'Product created successfully.' AS ResponseMessage,
            CAST(SCOPE_IDENTITY() AS INT) AS ProductId;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO