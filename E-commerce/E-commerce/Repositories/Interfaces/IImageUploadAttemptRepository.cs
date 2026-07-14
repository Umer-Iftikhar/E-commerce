using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Repositories.Interfaces
{
    public interface IImageUploadAttemptRepository
    {
        Task<ApiResponseDto> CreateImageUploadAttemptAsync(CreateImageUploadAttemptRequestDto request);
        Task<ApiResponseDto> MarkUploadCompletedAsync(Guid uploadToken);
        Task<ApiResponseDto> MarkUploadExpiredAsync(Guid uploadToken);
        Task<GetExpiredImageUploadsResponseDto> GetExpiredUploadsAsync();
        Task<GetImageUploadAttemptResponseDto> GetUploadAttemptByTokenAsync(Guid uploadToken);
    }
}
