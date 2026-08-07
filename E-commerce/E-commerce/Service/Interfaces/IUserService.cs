using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<SpResponseDto> LogoutAsync(string refreshToken);
        Task<GetUsersResponseDto> GetAllUsersAsync();
        Task<SpResponseDto> SoftDeleteUserAsync(int userId, int currentUserId);
    }
}
