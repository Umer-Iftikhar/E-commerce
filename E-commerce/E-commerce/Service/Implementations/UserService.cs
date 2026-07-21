using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using E_commerce.Services.Interfaces;
using E_commerce.Settings;
using Microsoft.Extensions.Options;
using System.Data;

namespace E_commerce.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IImageService _imageService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ImageStorageSettings _imageStorageSettings;
        private readonly DapperContext _context;
        private readonly JwtConfig _jwtConfig;
        private readonly IWebHostEnvironment _environment;

        public UserService(
            IImageService imageService,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService,
            IOptions<ImageStorageSettings> options,
            DapperContext context,
            IOptions<JwtConfig> jwtConfig,
            IWebHostEnvironment environment)
        {
            _imageService = imageService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _imageStorageSettings = options.Value; 
            _context = context;
            _jwtConfig = jwtConfig.Value;
            _environment = environment;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetUserByEmail,
                new { Email = request.Email },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<SpResponseDto>();

            if (response.ResponseCode != 200)
            {
                return new LoginResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Invalid Email Or Password"
                };
            }

            var user = await multi.ReadSingleOrDefaultAsync<UserDto>();

            if (user is null)
            {
                return new LoginResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Invalid Email Or Password"
                };
            }

            if (!user.IsActive || user.IsDeleted)
            {
                return new LoginResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Account Is Not Active"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash))
            {
                return new LoginResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Invalid Email Or Password"
                };
            }

            var refreshToken = await _refreshTokenService.GenerateAndSaveAsync(user.Id);

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return new LoginResponseDto
                {
                    ResponseCode = 500,
                    ResponseMessage = "Failed To Generate Refresh Token"
                };
            }

            var claims = new TokenClaimsDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.RoleName
            };

            var accessToken = _tokenService.GenerateToken(claims);

            return new LoginResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "Login Successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<SpResponseDto> LogoutAsync(string refreshToken)
        {
            return await _refreshTokenService.RevokeAsync(refreshToken);
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            string? profileImagePath = null;

            if (request.ProfileImage is not null)
            {
                var imageResponse = await _imageService.SaveAvatarAsync(request.ProfileImage);

                if (imageResponse.ResponseCode != 200)
                {
                    return new LoginResponseDto
                    {
                        ResponseCode = imageResponse.ResponseCode,
                        ResponseMessage = imageResponse.ResponseMessage
                    };
                }

                profileImagePath = imageResponse.FilePath;
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            using var connection = _context.CreateConnection();

            var userResponse = await connection.QuerySingleAsync<RegisterResponseDto>(
                StoredProcedures.CreateUser,
                new
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    ProfileImagePath = profileImagePath
                },
                commandType: CommandType.StoredProcedure);

            if (userResponse.ResponseCode != 200)
            {
                return new LoginResponseDto
                {
                    ResponseCode = userResponse.ResponseCode,
                    ResponseMessage = userResponse.ResponseMessage
                };
            }

            var refreshToken = await _refreshTokenService.GenerateAndSaveAsync(userResponse.UserId);

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return new LoginResponseDto
                {
                    ResponseCode = 500,
                    ResponseMessage = "Failed To Generate Refresh Token"
                };
            }

            var claims = new TokenClaimsDto
            {
                Id = userResponse.UserId,
                Name = request.Name,
                Email = request.Email,
                Role = userResponse.RoleName
            };

            var accessToken = _tokenService.GenerateToken(claims);

            return new LoginResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "Registration Successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        //public Task<RefreshResponseDto> RefreshAsync(string refreshToken)
        //{
        //    return _refreshTokenService.RefreshAsync(refreshToken);
        //}
    }
}
