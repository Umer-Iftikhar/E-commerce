using E_commerce.DTOs.Response;

namespace E_commerce.Services.Interfaces
{
    public interface IImageService
    {
        Task<ImagePathResponseDto> SaveAvatarAsync(IFormFile image);
        Task<ImagePathResponseDto> SaveAvatarAsync(IFormFile image, int userId);
        Task DeleteImageAsync(string? relativePath);
        Task<ImagePathResponseDto> SaveProductImageAsync(IFormFile image);
    }
}
