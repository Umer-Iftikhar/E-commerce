using E_commerce.DTOs.Internal;
using E_commerce.DTOs.Response;
using E_commerce.Repositories.Interfaces;
using E_commerce.Service.Interfaces;
using E_commerce.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace E_commerce.Service.Implementations
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly JwtConfig _jwtConfig;

        public RefreshTokenService(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            ITokenService tokenService,
            IOptions<JwtConfig> options)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _jwtConfig = options.Value;
        }

        public async Task<string?> GenerateAndSaveAsync(int userId)
        {
            var refreshToken = GenerateRefreshToken();

            var expiresAt = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryDays);

            var response = await _refreshTokenRepository.SaveRefreshTokenAsync(userId, refreshToken, expiresAt);

            if (response.ResponseCode != 200)
            {
                return null;
            }

            return refreshToken;
        }

        public async Task<RefreshResponseDto> RefreshAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);

            if (storedToken is null)
            {
                return new RefreshResponseDto
                {
                    ResponseCode = 404,
                    ResponseMessage = "Refresh Token Not Found"
                };
            }

            if (storedToken.IsRevoked)
            {
                return new RefreshResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Refresh Token Has Been Revoked"
                };
            }

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
            {
                return new RefreshResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Refresh Token Has Expired"
                };
            }

            var revokeResponse = await _refreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);

            if (revokeResponse.ResponseCode != 200)
            {
                return new RefreshResponseDto
                {
                    ResponseCode = revokeResponse.ResponseCode,
                    ResponseMessage = revokeResponse.ResponseMessage
                };
            }

            var newRefreshToken = await GenerateAndSaveAsync(storedToken.UserId);

            if (string.IsNullOrWhiteSpace(newRefreshToken))
            {
                return new RefreshResponseDto
                {
                    ResponseCode = 500,
                    ResponseMessage = "Failed To Save Refresh Token"
                };
            }

            var user = await _userRepository.GetUserByIdAsync(storedToken.UserId);

            if (user is null)
            {
                return new RefreshResponseDto
                {
                    ResponseCode = 404,
                    ResponseMessage = "User Not Found"
                };
            }

            var claims = new TokenClaimsDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            var accessToken = _tokenService.GenerateToken(claims);

            return new RefreshResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "Token Refreshed Successfully",
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        private static string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
