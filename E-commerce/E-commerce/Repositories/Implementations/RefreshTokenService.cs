using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Response;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using System.Data;

namespace E_commerce.Repositories.Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DapperContext _context;

        public RefreshTokenRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetRefreshToken,
                new { Token = token },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<ApiResponseDto>();

            if (response.ResponseCode != 200)
            {
                return null;
            }

            return await multi.ReadSingleOrDefaultAsync<RefreshToken>();
        }

        public async Task<ApiResponseDto> SaveRefreshTokenAsync(
            int userId,
            string token,
            DateTime expiresAt)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.SaveRefreshToken,
                new
                {
                    UserId = userId,
                    Token = token,
                    ExpiresAt = expiresAt
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ApiResponseDto> RevokeRefreshTokenAsync(string token)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.RevokeRefreshToken,
                new { Token = token },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ApiResponseDto> RevokeAllUserTokensAsync(int userId)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.RevokeAllUserTokens,
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }
    }
}
