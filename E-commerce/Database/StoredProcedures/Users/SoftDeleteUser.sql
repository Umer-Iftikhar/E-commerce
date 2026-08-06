USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.SoftDeleteUser
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        DECLARE @ProfileImagePath NVARCHAR(1000);

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Users
            WHERE Id = @UserId
              AND IsDeleted = 0
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'User not found.' AS ResponseMessage,
                CAST(NULL AS NVARCHAR(1000)) AS ProfileImagePath;

            RETURN;
        END

        SELECT
            @ProfileImagePath = ProfileImagePath
        FROM dbo.Users
        WHERE Id = @UserId;

        UPDATE dbo.Users
        SET
            IsDeleted = 1
        WHERE Id = @UserId;

        SELECT
            200 AS ResponseCode,
            'User deleted successfully.' AS ResponseMessage,
            @ProfileImagePath AS ProfileImagePath;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage,
            CAST(NULL AS NVARCHAR(1000)) AS ProfileImagePath;

    END CATCH
END
GO