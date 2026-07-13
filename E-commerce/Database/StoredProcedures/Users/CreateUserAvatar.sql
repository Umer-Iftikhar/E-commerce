CREATE OR ALTER PROCEDURE dbo.CreateUserAvatar
(
    @UserId INT,
    @OriginalFileName VARCHAR(255),
    @StoredFileName VARCHAR(255),
    @FileExtension VARCHAR(20),
    @MimeType VARCHAR(100),
    @Width INT,
    @Height INT,
    @FileSizeBytes BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.UserAvatars
        (
            UserId,
            OriginalFileName,
            StoredFileName,
            FileExtension,
            MimeType,
            Width,
            Height,
            FileSizeBytes
        )
        VALUES
        (
            @UserId,
            @OriginalFileName,
            @StoredFileName,
            @FileExtension,
            @MimeType,
            @Width,
            @Height,
            @FileSizeBytes
        );
        SELECT
            200 AS ResponseCode,
            'User Avatar Created Successfully' AS ResponseMessage;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627)
        BEGIN
            SELECT
                409 AS ResponseCode,
                'User Already Has A Profile Image' AS ResponseMessage;
            RETURN;
        END
        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO