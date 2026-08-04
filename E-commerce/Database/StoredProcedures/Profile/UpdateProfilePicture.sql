USE ECommerce;
GO

CREATE OR ALTER PROCEDURE dbo.UpdateProfilePicture
(
	@UserId INT,
	@ProfileImagePath VARCHAR(1000)
)
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY

		UPDATE dbo.Users
        SET ProfileImagePath = @ProfileImagePath
        WHERE Id = @UserId;

		SELECT
            200 AS ResponseCode,
            'Profile picture updated successfully.' AS ResponseMessage;
	END TRY

	BEGIN CATCH
		
		SELECT 500 AS ResponseCode,
			ERROR_MESSAGE() AS ResponseMessage;

	END CATCH
END
GO