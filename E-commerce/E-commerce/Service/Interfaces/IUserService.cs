using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthenticationResponseDto> LoginAsync(LoginRequestDto request);
        Task<ApiResponseDto> LogoutAsync(string refreshToken);
    }
}
