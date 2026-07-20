namespace E_commerce.Constants
{
    public class StoredProcedures
    {
        // Users
        public const string CheckEmailExists = "dbo.CheckEmailExists";
        public const string CreateUser = "dbo.CreateUser";
        public const string GetUserByEmail = "dbo.GetUserByEmail";
        public const string GetUserById = "dbo.GetUserById";
        public const string AssignRoleToUser = "AssignRoleToUser";


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


        // Categories
        public const string CreateCategory = "dbo.CreateCategory";


        // Products
        public const string CreateProduct = "dbo.CreateProduct";
        public const string AddProductImage = "dbo.AddProductImage";
        public const string UpdateProduct = "dbo.UpdateProduct";
        public const string SoftDeleteProduct = "dbo.SoftDeleteProduct";
        public const string GetAllProducts = "dbo.GetAllProducts";
        public const string GetProductById = "dbo.GetProductById";
        public const string GetProductsByCategory = "dbo.GetProductsByCategory";
    }
}
