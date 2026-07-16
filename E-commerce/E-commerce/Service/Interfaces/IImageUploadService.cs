using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IImageUploadService
    {
        Task<UploadImageResponseDto> UploadAsync(IFormFile image);
    }
}
