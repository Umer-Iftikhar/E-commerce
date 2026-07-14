CREATE OR ALTER PROCEDURE dbo.RevokeRefreshToken
(
    @Token VARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.RefreshTokens
            WHERE Token = @Token
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'Refresh Token Not Found' AS ResponseMessage;

            RETURN;
        END


        UPDATE dbo.RefreshTokens
        SET
            IsRevoked = 1
        WHERE Token = @Token;


        SELECT
            200 AS ResponseCode,
            'Refresh Token Revoked Successfully' AS ResponseMessage;


    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

    END CATCH
END
GO