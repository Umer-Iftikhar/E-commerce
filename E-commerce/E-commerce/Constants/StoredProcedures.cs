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


        // Refresh Tokens
        public const string SaveRefreshToken = "dbo.SaveRefreshToken";
        public const string RevokeRefreshToken = "dbo.RevokeRefreshToken";
        public const string RevokeAllUserTokens = "dbo.RevokeAllUserTokens";
        public const string RotateRefreshToken = "dbo.RotateRefreshToken";


        // Categories
        public const string CreateCategory = "dbo.CreateCategory";
        public const string GetAllCategories = "dbo.GetAllCategories";


        // Products
        public const string GetProducts = "dbo.GetProducts";
        public const string GetProductById = "dbo.GetProductById";

        public const string CreateProduct = "dbo.CreateProduct";
        public const string AddProductImage = "dbo.AddProductImage";
        public const string UpdateProduct = "dbo.UpdateProduct";
        public const string SoftDeleteProduct = "dbo.SoftDeleteProduct";
        public const string GetProductsByCategory = "dbo.GetProductsByCategory";
    }
}
