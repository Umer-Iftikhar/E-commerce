using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<CreateProductResponseDto> CreateProductAsync(CreateProductRequestDto request);
        Task<ApiResponseDto> AddProductImageAsync(AddProductImageRequestDto request);
        Task<ApiResponseDto> UpdateProductAsync(UpdateProductRequestDto request);
        Task<ApiResponseDto> SoftDeleteProductAsync(int productId);
        Task<List<ProductListItemDto>> GetAllProductsAsync();
        Task<GetProductResponseDto> GetProductByIdAsync(int productId);
        Task<List<ProductListItemDto>> GetProductsByCategoryAsync(int categoryId);
    }
}
