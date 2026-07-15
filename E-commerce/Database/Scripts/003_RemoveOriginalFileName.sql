USE ECommerce;
GO

/*==============================================================*/
/* Remove OriginalFileName From UserAvatars                     */
/*==============================================================*/

IF EXISTS
(
    SELECT 1
    FROM sys.columns
    WHERE Name = 'OriginalFileName'
      AND Object_ID = OBJECT_ID('dbo.UserAvatars')
)
BEGIN
    ALTER TABLE dbo.UserAvatars
    DROP COLUMN OriginalFileName;
END
GO