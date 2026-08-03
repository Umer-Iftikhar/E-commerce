USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetOrders
(
	@UserId INT
)
AS
BEGIN
	SET NOCOUNT ON;

    BEGIN TRY

        SELECT 200 AS ResponseCode, 'Orders retrieved successfully' AS ResponseMessage;

        SELECT
            o.Id AS OrderId,
            o.CreatedAt,
            o.Status,
            o.PaymentMethodId,
            SUM(oi.Quantity) AS ItemCount,
            SUM(oi.Price * oi.Quantity) AS Total
        FROM dbo.Orders o
        INNER JOIN dbo.OrderItems oi
            ON oi.OrderId = o.Id
        WHERE o.UserId = @UserId
        GROUP BY
            o.Id,
            o.CreatedAt,
            o.Status,
            o.PaymentMethodId
        ORDER BY o.CreatedAt DESC;
    END TRY

    BEGIN CATCH 
        SELECT 500 AS ResponseCode, ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS OrderId,
            CAST(NULL AS DATETIME2) AS CreatedAt,
            CAST(NULL AS INT) AS Status,
            CAST(NULL AS INT) AS PaymentMethodId,
            CAST(NULL AS INT) AS ItemCount,
            CAST(NULL AS DECIMAL(18,2)) AS Total
        WHERE 1 = 0;
    END CATCH

END

GO
