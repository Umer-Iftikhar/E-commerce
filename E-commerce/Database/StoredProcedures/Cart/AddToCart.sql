CREATE OR ALTER PROCEDURE dbo.AddToCart
    @UserId INT,
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CartId INT;
    DECLARE @Stock INT;
    DECLARE @CurrentQuantity INT;

    BEGIN TRY

        -- Validate product and get stock
        SELECT @Stock = Stock
        FROM dbo.Products
        WHERE Id = @ProductId
          AND IsDeleted = 0;

        IF @Stock IS NULL
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Product not found.' AS ResponseMessage;
            RETURN;
        END

        IF @Stock <= 0
        BEGIN
            SELECT
                409 AS ResponseCode,
                'Product is out of stock.' AS ResponseMessage;
            RETURN;
        END

        BEGIN TRANSACTION;

        -- Get existing cart
        SELECT @CartId = Id
        FROM dbo.Carts
        WHERE UserId = @UserId;

        -- Create cart if it doesn't exist
        IF @CartId IS NULL
        BEGIN
            INSERT INTO dbo.Carts (UserId)
            VALUES (@UserId);

            SET @CartId = SCOPE_IDENTITY();
        END

        -- Get current quantity
        SELECT @CurrentQuantity = Quantity
        FROM dbo.CartItems
        WHERE CartId = @CartId
          AND ProductId = @ProductId;

        -- Product already in cart
        IF @CurrentQuantity IS NOT NULL
        BEGIN
            IF @CurrentQuantity >= @Stock
            BEGIN
                ROLLBACK TRANSACTION;

                SELECT
                    409 AS ResponseCode,
                    'Cannot add more. Stock limit reached.' AS ResponseMessage;

                RETURN;
            END

            UPDATE dbo.CartItems
            SET Quantity = Quantity + 1
            WHERE CartId = @CartId
              AND ProductId = @ProductId;

            COMMIT TRANSACTION;

            SELECT
                200 AS ResponseCode,
                'Product quantity updated in cart.' AS ResponseMessage;

            RETURN;
        END

        -- Add new cart item
        INSERT INTO dbo.CartItems
        (
            CartId,
            ProductId,
            Quantity
        )
        VALUES
        (
            @CartId,
            @ProductId,
            1
        );

        COMMIT TRANSACTION;

        SELECT
            200 AS ResponseCode,
            'Product added to cart.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END;
GO

