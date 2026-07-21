USE ECommerce;
GO
CREATE OR ALTER PROCEDURE dbo.GetUserByEmail
(
    @Email VARCHAR(250)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Users
            WHERE Email = @Email
        )
        BEGIN
            SELECT
                404 AS ResponseCode,
                'User Not Found' AS ResponseMessage;
            SELECT
                u.Id,
                u.Name,
                u.Email,
                u.PasswordHash,
                u.IsActive,
                u.IsDeleted,
                u.CreatedAt,
                u.ProfileImagePath,
                r.Id AS RoleId,
                r.Name AS RoleName
            FROM dbo.Users AS u
            INNER JOIN dbo.Roles AS r
                ON r.Id = u.RoleId
            WHERE 1 = 0;
            RETURN;
        END;
        SELECT
            200 AS ResponseCode,
            'User Retrieved Successfully' AS ResponseMessage;
        SELECT
            u.Id,
            u.Name,
            u.Email,
            u.PasswordHash,
            u.IsActive,
            u.IsDeleted,
            u.CreatedAt,
            u.ProfileImagePath,
            r.Id AS RoleId,
            r.Name AS RoleName
        FROM dbo.Users AS u
        INNER JOIN dbo.Roles AS r
            ON r.Id = u.RoleId
        WHERE u.Email = @Email;
    END TRY
    BEGIN CATCH
        SELECT
            500 AS ResponseCode,
            ERROR_MESSAGE() AS ResponseMessage;
        SELECT
            u.Id,
            u.Name,
            u.Email,
            u.PasswordHash,
            u.IsActive,
            u.IsDeleted,
            u.CreatedAt,
            u.ProfileImagePath,
            r.Id AS RoleId,
            r.Name AS RoleName
        FROM dbo.Users AS u
        INNER JOIN dbo.Roles AS r
            ON r.Id = u.RoleId
        WHERE 1 = 0;
    END CATCH
END;
GO