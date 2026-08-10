using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface ICartService
    {
        Task<SpResponseDto> AddToCartAsync(int userId, int productId);
        Task<GetCartResponseDto> GetCartAsync(int userId);
        Task<SpResponseDto> RemoveFromCartAsync(int userId, int cartItemId);
        Task<SpResponseDto> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity);
    }
}
