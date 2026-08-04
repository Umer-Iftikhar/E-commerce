using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IProfileService
    {
        Task<string?> GetPasswordHashAsync(int userId);
        Task<ImagePathResponseDto> GetProfileImageAsync(int userId);
        Task<SpResponseDto> UpdateProfileAsync(UpdateProfileRequestDto request);
        Task<SpResponseDto> UpdateProfilePictureAsync(int userId, IFormFile image);
        Task<SpResponseDto> ChangePasswordAsync(ChangePasswordRequestDto request);
    }
}
