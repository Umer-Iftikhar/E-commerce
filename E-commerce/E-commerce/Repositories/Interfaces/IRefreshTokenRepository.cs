using E_commerce.DTOs.Response;
using E_commerce.Models;

namespace E_commerce.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task<ApiResponseDto> SaveRefreshTokenAsync(int userId,string token,DateTime expiresAt);
        Task<ApiResponseDto> RevokeRefreshTokenAsync(string token);
        Task<ApiResponseDto> RevokeAllUserTokensAsync(int userId);
    }
}
