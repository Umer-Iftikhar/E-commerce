USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.UpdateProfile
(
    @UserId INT,
    @Name NVARCHAR(100) = NULL,
    @Email NVARCHAR(255) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Email IS NOT NULL AND EXISTS
        (
            SELECT 1 FROM dbo.Users
            WHERE Email = @Email
                AND Id <> @UserId
        )
        BEGIN

            SELECT 409 AS ResponseCode,
                'Email is not available' AS ResponseMessage;

            RETURN;
        END

        UPDATE dbo.Users
        SET
            Name = COALESCE(@Name, Name),
            Email = COALESCE(@Email, Email)
        WHERE Id = @UserId;

        SELECT 200 AS ResponseCode,
            'Profile updated successfully' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
                ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO