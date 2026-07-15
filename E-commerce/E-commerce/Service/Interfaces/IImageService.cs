using E_commerce.DTOs.Response;

namespace E_commerce.Services.Interfaces
{
    public interface IImageService
    {
        Task<MoveImageResponseDto> MoveToPermanentStorageAsync(string tempFileName);

    }
}
