using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateAndSaveAsync(int userId);
        Task<RefreshResponseDto?> RefreshAsync(string refreshToken);
    }
}
