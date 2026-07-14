namespace E_commerce.Constants
{
    public class StoredProcedures
    {
        public const string CheckEmailExists = "dbo.CheckEmailExists";
        public const string CreateUser = "dbo.CreateUser";

        // User Avatars
        public const string CreateUserAvatar = "dbo.CreateUserAvatar";

        // Image Upload Attempts
        public const string CreateImageUploadAttempt = "dbo.CreateImageUploadAttempt";
        public const string GetImageUploadAttemptByToken = "dbo.GetImageUploadAttemptByToken";
        public const string MarkImageUploadCompleted = "dbo.MarkImageUploadCompleted";
        public const string MarkImageUploadExpired = "dbo.MarkImageUploadExpired";
        public const string GetExpiredImageUploads = "dbo.GetExpiredImageUploads";
    }
}
