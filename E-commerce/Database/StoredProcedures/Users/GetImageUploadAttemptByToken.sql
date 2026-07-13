CREATE OR ALTER PROCEDURE dbo.GetImageUploadAttemptByToken
(
    @UploadToken UNIQUEIDENTIFIER
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Token does not exist
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.ImageUploadAttempts
            WHERE UploadToken = @UploadToken
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Upload Token Not Found' AS ResponseMessage;

            RETURN;
        END

        -- Token exists but is no longer valid
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.ImageUploadAttempts
            WHERE UploadToken = @UploadToken
              AND Status = 'Pending'
              AND ExpiresAt > SYSUTCDATETIME()
        )
        BEGIN
            SELECT
                410 AS ResponseCode,
                'Upload Token Has Expired Or Already Been Used' AS ResponseMessage;

            RETURN;
        END

        -- Success response
        SELECT
            200 AS ResponseCode,
            'Upload Token Retrieved Successfully' AS ResponseMessage;

        -- Upload details
        SELECT
            Id,
            UploadToken,
            TempFileName,
            Status,
            CreatedAt,
            ExpiresAt,
            CompletedAt
        FROM dbo.ImageUploadAttempts
        WHERE UploadToken = @UploadToken
          AND Status = 'Pending'
          AND ExpiresAt > SYSUTCDATETIME();

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO