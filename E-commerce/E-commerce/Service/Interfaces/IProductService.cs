using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IProductService
    {
        Task<GetProductsResponseDto> GetAllProductsAsync();
        Task<GetProductResponseDto> GetProductByIdAsync(int productId);
    }
}
