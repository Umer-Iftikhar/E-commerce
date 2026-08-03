USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetOrderDetails
(
    @UserId INT,
    @OrderId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Orders
            WHERE Id = @OrderId
              AND UserId = @UserId
        )
        BEGIN

            SELECT
                404 AS ResponseCode,
                'Order not found.' AS ResponseMessage;

            SELECT
                CAST(NULL AS INT) AS OrderId,
                CAST(NULL AS DATETIME2) AS CreatedAt,
                CAST(NULL AS INT) AS Status,
                CAST(NULL AS INT) AS PaymentMethodId,
                CAST(NULL AS VARCHAR(255)) AS Address,
                CAST(NULL AS VARCHAR(20)) AS PhoneNumber,
                CAST(NULL AS DECIMAL(18,2)) AS Total
            WHERE 1 = 0;

            SELECT
                CAST(NULL AS INT) AS ProductId,
                CAST(NULL AS VARCHAR(255)) AS ProductName,
                CAST(NULL AS DECIMAL(18,2)) AS Price,
                CAST(NULL AS INT) AS Quantity
            WHERE 1 = 0;

            RETURN;

        END

        SELECT
            200 AS ResponseCode,
            'Order retrieved successfully.' AS ResponseMessage;

        SELECT
            o.Id AS OrderId,
            o.CreatedAt,
            o.Status,
            o.PaymentMethodId,
            o.Address,
            o.PhoneNumber,
            SUM(oi.Price * oi.Quantity) AS Total
        FROM dbo.Orders o
        INNER JOIN dbo.OrderItems oi
            ON oi.OrderId = o.Id
        WHERE o.Id = @OrderId
          AND o.UserId = @UserId
        GROUP BY
            o.Id,
            o.CreatedAt,
            o.Status,
            o.PaymentMethodId,
            o.Address,
            o.PhoneNumber;

        SELECT
            ProductId,
            ProductName,
            Price,
            Quantity
        FROM dbo.OrderItems
        WHERE OrderId = @OrderId
        ORDER BY Id;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS OrderId,
            CAST(NULL AS DATETIME2) AS CreatedAt,
            CAST(NULL AS INT) AS Status,
            CAST(NULL AS INT) AS PaymentMethodId,
            CAST(NULL AS VARCHAR(255)) AS Address,
            CAST(NULL AS VARCHAR(20)) AS PhoneNumber,
            CAST(NULL AS DECIMAL(18,2)) AS Total
        WHERE 1 = 0;

        SELECT
            CAST(NULL AS INT) AS ProductId,
            CAST(NULL AS VARCHAR(255)) AS ProductName,
            CAST(NULL AS DECIMAL(18,2)) AS Price,
            CAST(NULL AS INT) AS Quantity
        WHERE 1 = 0;

    END CATCH

END;
GO