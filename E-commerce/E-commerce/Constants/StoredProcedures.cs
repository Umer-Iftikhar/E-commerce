namespace E_commerce.Constants
{
    public class StoredProcedures
    {
        // Users
        public const string CreateUser = "dbo.CreateUser";
        public const string GetUserByEmail = "dbo.GetUserByEmail";
        public const string GetUserById = "dbo.GetUserById";


        // Refresh Tokens
        public const string SaveRefreshToken = "dbo.SaveRefreshToken";
        public const string RevokeRefreshToken = "dbo.RevokeRefreshToken";
        public const string RotateRefreshToken = "dbo.RotateRefreshToken";


        // Categories
        public const string GetAllCategories = "dbo.GetAllCategories";


        // Products
        public const string GetProducts = "dbo.GetProducts";
        public const string GetProductById = "dbo.GetProductById";


        // Cart
        public const string AddToCart = "dbo.AddToCart";
        public const string GetCart = "dbo.GetCart";
        public const string RemoveFromCart = "dbo.RemoveFromCart";
        public const string UpdateCartItemQuantity = "dbo.UpdateCartItemQuantity";


        // Orders
        public const string CreateOrder = "dbo.CreateOrder";
        public const string GetOrders = "dbo.GetOrders";    
        public const string GetOrderDetails = "dbo.GetOrderDetails";


        // User Profile
        public const string GetPasswordHash = "dbo.GetPasswordHash";
        public const string UpdateProfile = "dbo.UpdateProfile";
        public const string UpdateProfilePicture = "dbo.UpdateProfilePicture";
        public const string ChangePassword = "dbo.ChangePassword";
        public const string GetProfileImage = "dbo.GetProfileImage";
    }
}
