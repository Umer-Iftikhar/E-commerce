USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            200 AS ResponseCode,
            'Users retrieved successfully.' AS ResponseMessage;

        SELECT
            U.Id,
            U.Name,
            U.Email,
            R.Name AS Role,
            U.IsActive,
            U.IsDeleted,
            U.CreatedAt,
            U.ProfileImagePath
        FROM dbo.Users U
        INNER JOIN dbo.Roles R
            ON U.RoleId = R.Id
        ORDER BY U.CreatedAt DESC;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS INT) AS Id,
            CAST(NULL AS VARCHAR(100)) AS Name,
            CAST(NULL AS VARCHAR(250)) AS Email,
            CAST(NULL AS VARCHAR(50)) AS Role,
            CAST(NULL AS BIT) AS IsActive,
            CAST(NULL AS BIT) AS IsDeleted,
            CAST(NULL AS DATETIME2) AS CreatedAt,
            CAST(NULL AS VARCHAR(1000)) AS ProfileImagePath
        WHERE 1 = 0;

    END CATCH
END
GO