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
                u.Id,
                u.Name,
                u.Email,
                u.PasswordHash,
                u.IsActive,
                u.IsDeleted,
                u.CreatedAt,
                r.Id AS RoleId,
                r.Name AS RoleName
            FROM dbo.Users AS u
            LEFT JOIN dbo.UserRoles AS ur
                ON ur.UserId = u.Id
            LEFT JOIN dbo.Roles AS r
                ON r.Id = ur.RoleId
            WHERE 1 = 0;
            
            RETURN;
        END


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
            r.Id AS RoleId,
            r.Name AS RoleName
        FROM dbo.Users AS u
        LEFT JOIN dbo.UserRoles AS ur
            ON ur.UserId = u.Id
        LEFT JOIN dbo.Roles AS r
            ON r.Id = ur.RoleId
        WHERE u.Id = @Id;


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
            r.Id AS RoleId,
            r.Name AS RoleName
        FROM dbo.Users AS u
        LEFT JOIN dbo.UserRoles AS ur
            ON ur.UserId = u.Id
        LEFT JOIN dbo.Roles AS r
            ON r.Id = ur.RoleId
        WHERE 1 = 0;

    END CATCH
END
GO

Use ECommerce