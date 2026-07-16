CREATE OR ALTER PROCEDURE dbo.CreateUserAvatar
(
    @UserId INT,
    @StoredFileName VARCHAR(255),
    @FileExtension VARCHAR(20),
    @MimeType VARCHAR(100),
    @Width INT,
    @Height INT,
    @FileSizeBytes INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Users
            WHERE Id = @UserId
              AND IsActive = 1
              AND IsDeleted = 0
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'User Not Found' AS ResponseMessage;
            RETURN;
        END
        INSERT INTO dbo.UserAvatars
        (
            UserId,
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
                    'User Already Has An Avatar' AS ResponseMessage;
                    RETURN;
            END
            SELECT
                500 AS ResponseCode,
                ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO