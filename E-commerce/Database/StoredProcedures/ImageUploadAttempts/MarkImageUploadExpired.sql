CREATE OR ALTER PROCEDURE dbo.MarkImageUploadExpired
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

        -- Only pending uploads can expire
        UPDATE dbo.ImageUploadAttempts
        SET
            Status = 'Expired'
        WHERE UploadToken = @UploadToken
          AND Status = 'Pending';

        -- Token exists but is no longer eligible to expire
        IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                410 AS ResponseCode,
                'Upload Token Has Already Been Processed' AS ResponseMessage;

            RETURN;
        END

        SELECT
            200 AS ResponseCode,
            'Image Upload Marked As Expired' AS ResponseMessage;

    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO