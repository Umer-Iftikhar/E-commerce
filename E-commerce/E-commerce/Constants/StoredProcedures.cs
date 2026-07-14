namespace E_commerce.Constants
{
    public class StoredProcedures
    {
        // Users
        public const string CheckEmailExists = "dbo.CheckEmailExists";
        public const string CreateUser = "dbo.CreateUser";
        public const string GetUserByEmail = "dbo.GetUserByEmail";
        public const string GetUserById = "dbo.GetUserById";


        // User Avatars
        public const string CreateUserAvatar = "dbo.CreateUserAvatar";


        // Image Upload Attempts
        public const string CreateImageUploadAttempt = "dbo.CreateImageUploadAttempt";
        public const string GetImageUploadAttemptByToken = "dbo.GetImageUploadAttemptByToken";
        public const string MarkImageUploadCompleted = "dbo.MarkImageUploadCompleted";
        public const string MarkImageUploadExpired = "dbo.MarkImageUploadExpired";
        public const string GetExpiredImageUploads = "dbo.GetExpiredImageUploads";


        // Refresh Tokens
        public const string SaveRefreshToken = "dbo.SaveRefreshToken";
        public const string GetRefreshToken = "dbo.GetRefreshToken";
        public const string RevokeRefreshToken = "dbo.RevokeRefreshToken";
        public const string RevokeAllUserTokens = "dbo.RevokeAllUserTokens";
    }
}
