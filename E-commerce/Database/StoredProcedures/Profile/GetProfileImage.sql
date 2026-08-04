USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.GetProfileImage
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            200 AS ResponseCode,
            'Profile image retrieved successfully.' AS ResponseMessage;

        SELECT
            ProfileImagePath AS FilePath
        FROM dbo.Users
        WHERE Id = @UserId;

    END TRY

    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            CAST(NULL AS VARCHAR(1000)) AS ProfileImagePath
        WHERE 1 = 0;

    END CATCH

END
GO