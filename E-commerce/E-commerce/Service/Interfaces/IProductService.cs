using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IProductService
    {
        Task<GetProductsResponseDto> GetProductsAsync(string? searchTerm, int? categoryId);
        Task<GetProductResponseDto> GetProductByIdAsync(int productId);
    }
}
