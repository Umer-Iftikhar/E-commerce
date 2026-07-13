CREATE OR ALTER PROCEDURE dbo.MarkImageUploadCompleted
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

        -- Update only if the token is still valid
        UPDATE dbo.ImageUploadAttempts
        SET
            Status = 'Completed',
            CompletedAt = SYSUTCDATETIME()
        WHERE UploadToken = @UploadToken
          AND Status = 'Pending'
          AND ExpiresAt > SYSUTCDATETIME();

        -- Token exists but is no longer valid
        IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                410 AS ResponseCode,
                'Upload Token Has Expired Or Already Been Used' AS ResponseMessage;

            RETURN;
        END

        SELECT
            200 AS ResponseCode,
            'Image Upload Marked As Completed' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO