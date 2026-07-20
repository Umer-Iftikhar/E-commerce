CREATE OR ALTER PROCEDURE dbo.AssignRoleToUser
(
    @UserId INT,
    @RoleName VARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleId INT;

    SELECT @RoleId = Id
    FROM dbo.Roles
    WHERE Name = @RoleName;

    IF @RoleId IS NULL
    BEGIN
        SELECT
            404 AS ResponseCode,
            'Role not found.' AS ResponseMessage;

        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM dbo.UserRoles
        WHERE UserId = @UserId
    )
    BEGIN
        SELECT
            409 AS ResponseCode,
            'User already has a role.' AS ResponseMessage;

        RETURN;
    END

    INSERT INTO dbo.UserRoles
    (
        UserId,
        RoleId
    )
    VALUES
    (
        @UserId,
        @RoleId
    );

    SELECT
        200 AS ResponseCode,
        'Role assigned successfully.' AS ResponseMessage;
END;
GO