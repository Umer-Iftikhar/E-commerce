CREATE OR ALTER PROCEDURE dbo.GetUserById
(
    @Id INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Users
            WHERE Id = @Id
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'User Not Found' AS ResponseMessage;

            SELECT
                Id,
                Name,
                Email,
                PasswordHash,
                IsActive,
                IsDeleted,
                CreatedAt
            FROM dbo.Users
            WHERE 1 = 0;

            RETURN;
        END


        SELECT
            200 AS ResponseCode,
            'User Retrieved Successfully' AS ResponseMessage;


        SELECT
            Id,
            Name,
            Email,
            PasswordHash,
            IsActive,
            IsDeleted,
            CreatedAt
        FROM dbo.Users
        WHERE Id = @Id;


    END TRY
    BEGIN CATCH

        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;

        SELECT
            Id,
            Name,
            Email,
            PasswordHash,
            IsActive,
            IsDeleted,
            CreatedAt
        FROM dbo.Users
        WHERE 1 = 0;

    END CATCH
END
GO