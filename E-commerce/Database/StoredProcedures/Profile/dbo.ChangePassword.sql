USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.ChangePassword
(
    @UserId INT,
    @PasswordHash VARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE dbo.Users
        SET PasswordHash = @PasswordHash
        WHERE Id = @UserId;

        SELECT
            200 AS ResponseCode,
            'Password changed successfully.' AS ResponseMessage;

    END TRY

    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH

END
GO