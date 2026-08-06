using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<GetCategoriesResponseDto> GetAllCategoriesAsync();
        Task<SpResponseDto> CreateCategoryAsync(CreateCategoryRequestDto request);
        Task<SpResponseDto> UpdateCategoryAsync(UpdateCategoryRequestDto request);
        Task<SpResponseDto> SoftDeleteCategoryAsync(int categoryId);
        Task<SpResponseDto> RestoreCategoryAsync(int categoryId);
    }
}
