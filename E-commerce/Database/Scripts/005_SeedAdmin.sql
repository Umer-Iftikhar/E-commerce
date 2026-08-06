USE ECommerce;
GO

INSERT INTO dbo.Users
(
    Name,
    Email,
    PasswordHash,
    RoleId
)
SELECT
    'Admin',
    'umeriftikhar981@gmail.com',
    '$2a$11$hQ2OwnmEHvMRMqrJWiqTBuUuBPKBtxZMKMWn54sjJmHbsK67WCxMG',
    R.Id
FROM dbo.Roles AS R
WHERE R.Name = 'Admin'
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.Users
      WHERE Email = 'umeriftikhar981@gmail.com'
  );
GO