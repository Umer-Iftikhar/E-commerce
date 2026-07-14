CREATE OR ALTER PROCEDURE dbo.GetRefreshToken
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

            SELECT
                Id,
                UserId,
                Token,
                ExpiresAt,
                CreatedAt,
                IsRevoked
            FROM dbo.RefreshTokens
            WHERE 1 = 0;

            RETURN;
        END


        SELECT
            200 AS ResponseCode,
            'Refresh Token Retrieved Successfully' AS ResponseMessage;


        SELECT
            Id,
            UserId,
            Token,
            ExpiresAt,
            CreatedAt,
            IsRevoked
        FROM dbo.RefreshTokens
        WHERE Token = @Token;


    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            Id,
            UserId,
            Token,
            ExpiresAt,
            CreatedAt,
            IsRevoked
        FROM dbo.RefreshTokens
        WHERE 1 = 0;

    END CATCH
END
GO