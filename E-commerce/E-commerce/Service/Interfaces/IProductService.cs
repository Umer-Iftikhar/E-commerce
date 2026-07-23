using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IProductService
    {
        Task<GetProductsResponseDto> GetProductsAsync(string? searchTerm, int? categoryId, DateOnly? createdDate);
        Task<GetProductResponseDto> GetProductByIdAsync(int productId);
        Task<GetCategoriesResponseDto> GetAllCategoriesAsync();
    }
}
