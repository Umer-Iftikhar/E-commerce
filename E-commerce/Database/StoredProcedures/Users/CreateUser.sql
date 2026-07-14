CREATE OR ALTER PROCEDURE dbo.CreateUser
(
    @Name VARCHAR(100),
    @Email VARCHAR(250),
    @PasswordHash VARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF EXISTS (SELECT 1 FROM dbo.Users WHERE Email = @Email)
        BEGIN
            SELECT 
                409 AS ResponseCode,
                'Email Already Exists' AS ResponseMessage;

            RETURN;
        END


        DECLARE @UserId INT;

        INSERT INTO dbo.Users
        (
            Name,
            Email,
            PasswordHash
        )
        VALUES
        (
            @Name,
            @Email,
            @PasswordHash
        );


        SET @UserId = CAST(SCOPE_IDENTITY() AS INT);


        SELECT
            200 AS ResponseCode,
            'User Created Successfully' AS ResponseMessage,
            @UserId AS UserId;
            

    END TRY
    BEGIN CATCH

        IF ERROR_NUMBER() IN (2601,2627)
        BEGIN
            SELECT
                409 AS ResponseCode,
                'Email Already Exists' AS ResponseMessage;

            RETURN;
        END


        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO