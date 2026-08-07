USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.RestoreUser
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Users
            WHERE Id = @UserId
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'User not found.' AS ResponseMessage;
            RETURN;
        END

        UPDATE dbo.Users
        SET IsDeleted = 0
        WHERE Id = @UserId;

        SELECT
            200 AS ResponseCode,
            'User restored successfully.' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO