using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface ICartService
    {
        Task<SpResponseDto> AddToCartAsync(int userId, int productId);
    }
}
