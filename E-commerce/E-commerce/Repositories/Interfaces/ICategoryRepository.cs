using E_commerce.DTOs.Response;

namespace E_commerce.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<CreateCategoryResponseDto> CreateCategoryAsync(string name);
    }
}
