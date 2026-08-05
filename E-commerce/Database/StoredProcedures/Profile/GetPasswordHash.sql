USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetPasswordHash
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            200 AS ResponseCode,
            'Password hash retrieved successfully.' AS ResponseMessage;

        SELECT
            PasswordHash
        FROM dbo.Users
        WHERE Id = @UserId;

    END TRY

    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS VARCHAR(300)) AS PasswordHash
        WHERE 1 = 0;

    END CATCH

END
GO