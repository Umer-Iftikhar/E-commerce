
CREATE OR ALTER PROCEDURE dbo.CheckEmailExists
    @Email VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF EXISTS (SELECT 1 FROM dbo.Users WHERE Email = @Email)
        BEGIN
            SELECT 
                409 AS ResponseCode,
                'Email Already Exists' AS ResponseMessage;
        END
        ELSE
        BEGIN
            SELECT 
                200 AS ResponseCode,
                'Email Is Available' AS ResponseMessage;
        END

    END TRY
    BEGIN CATCH

        SELECT 
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO