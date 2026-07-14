CREATE OR ALTER PROCEDURE dbo.GetExpiredImageUploads
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- First result set
        SELECT
            200 AS ResponseCode,
            'Expired Uploads Retrieved Successfully' AS ResponseMessage;
        -- Second result set
        SELECT
            UploadToken,
            TempFileName
        FROM dbo.ImageUploadAttempts
        WHERE Status = 'Pending'
        AND ExpiresAt <= SYSUTCDATETIME();
    END TRY
    BEGIN CATCH
        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO