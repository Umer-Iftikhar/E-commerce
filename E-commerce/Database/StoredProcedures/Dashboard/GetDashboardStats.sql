USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetDashboardStats
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            200 AS ResponseCode,
            'Dashboard statistics retrieved successfully.' AS ResponseMessage;

        SELECT
            (SELECT COUNT(*)
             FROM dbo.Products
             WHERE IsDeleted = 0) AS TotalProducts,

            (SELECT COUNT(*)
             FROM dbo.Users
             WHERE IsDeleted = 0) AS TotalUsers,

            (SELECT COUNT(*)
             FROM dbo.Orders) AS TotalOrders;

        SELECT
            P.Id,
            P.Name,
            C.Name AS Category,
            P.Stock
        FROM dbo.Products AS P
        INNER JOIN dbo.Categories AS C
            ON P.CategoryId = C.Id
        WHERE P.IsDeleted = 0
          AND C.IsDeleted = 0
          AND P.Stock < 5
        ORDER BY P.Stock ASC, P.Name ASC;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS TotalProducts,
            CAST(NULL AS INT) AS TotalUsers,
            CAST(NULL AS INT) AS TotalOrders
        WHERE 1 = 0;

        SELECT
            CAST(NULL AS INT) AS Id,
            CAST(NULL AS NVARCHAR(100)) AS Name,
            CAST(NULL AS NVARCHAR(100)) AS Category,
            CAST(NULL AS INT) AS Stock
        WHERE 1 = 0;

    END CATCH
END
GO