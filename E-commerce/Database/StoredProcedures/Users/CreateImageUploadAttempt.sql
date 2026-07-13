CREATE OR ALTER PROCEDURE dbo.CreateImageUploadAttempt
(
    @UploadToken UNIQUEIDENTIFIER,
    @TempFileName VARCHAR(255),
    @ExpiresAt DATETIME2
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.ImageUploadAttempts
        (
            UploadToken,
            TempFileName,
            ExpiresAt
        )
        VALUES
        (
            @UploadToken,
            @TempFileName,
            @ExpiresAt
        );
        SELECT
            200 AS ResponseCode,
            'Image Upload Attempt Created Successfully' AS ResponseMessage;
    END TRY
    BEGIN CATCH
        -- Duplicate UploadToken
        IF ERROR_NUMBER() IN (2601, 2627)
        BEGIN
            SELECT
                409 AS ResponseCode,
                'Upload Token Already Exists' AS ResponseMessage;
            RETURN;
        END
        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO
