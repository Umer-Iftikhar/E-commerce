using E_commerce.Constants;
using E_commerce.DTOs.Internal;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Repositories.Interfaces;
using E_commerce.Service.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageUploadAttemptRepository _imageUploadAttemptRepository;
        private readonly IImageService _imageService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public UserService(
            IUserRepository userRepository,
            IImageUploadAttemptRepository imageUploadAttemptRepository,
            IImageService imageService,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _imageUploadAttemptRepository = imageUploadAttemptRepository;
            _imageService = imageService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<AuthenticationResponseDto> LoginAsync(LoginRequestDto request)
        {
            // 1. Retrieve user by email
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Invalid email or password."
                };
            }

            // 2. Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Invalid email or password."
                };
            }

            // 3. Ensure account is active
            if (!user.IsActive || user.IsDeleted)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = 401,
                    ResponseMessage = "Invalid email or password."
                };
            }

            if (string.IsNullOrWhiteSpace(user.Role))
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = 500,
                    ResponseMessage = "User account has no assigned role."
                };
            }

            // 4. Generate access token
            var claims = new TokenClaimsDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            var accessToken = _tokenService.GenerateToken(claims);

            // 5. Generate and save refresh token
            var refreshToken = await _refreshTokenService.GenerateAndSaveAsync(user.Id);

            if (refreshToken is null)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = 500,
                    ResponseMessage = "Failed to generate refresh token."
                };
            }

            // 6. Return authentication response
            return new AuthenticationResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "User logged in successfully.",

                Id = user.Id,
                Name = user.Name,
                Email = user.Email,

                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<ApiResponseDto> LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return new ApiResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Refresh token is required."
                };
            }

            var response = await _refreshTokenService.RevokeAsync(refreshToken);

            return response;
        }

        public async Task<AuthenticationResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // 1. Check email
            var emailResponse = await _userRepository.CheckEmailExistsAsync(request.Email);

            if (emailResponse.ResponseCode == 409)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = emailResponse.ResponseCode,
                    ResponseMessage = emailResponse.ResponseMessage
                };
            }

            if (emailResponse.ResponseCode != 200)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = 500,
                    ResponseMessage = "Unexpected error during email validation."
                };
            }

            // 2. Validate upload token
            var uploadAttemptResponse =
                await _imageUploadAttemptRepository.GetUploadAttemptByTokenAsync(request.UploadToken);

            if (uploadAttemptResponse.ResponseCode != 200)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = uploadAttemptResponse.ResponseCode,
                    ResponseMessage = uploadAttemptResponse.ResponseMessage
                };
            }

            // 3. Move image from temporary storage to permanent storage
            var imageResponse =
                await _imageService.MoveToPermanentStorageAsync(uploadAttemptResponse.Upload!.TempFileName);

            if (imageResponse.ResponseCode != 200)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = imageResponse.ResponseCode,
                    ResponseMessage = imageResponse.ResponseMessage
                };
            }

            // 4. Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 5. Create user
            var createUserResponse = await _userRepository.CreateUserAsync(
                new CreateUserRequestDto
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = passwordHash
                });

            if (createUserResponse.ResponseCode != 200)
            {
                // NOTE (v1):
                // DEFERRED: Image has already been moved to permanent storage.
                // Future work: implement DeletePermanentImageAsync() so the moved
                // image can be removed if user creation fails.

                return new AuthenticationResponseDto
                {
                    ResponseCode = createUserResponse.ResponseCode,
                    ResponseMessage = createUserResponse.ResponseMessage
                };
            }

            // 6. Assign default Customer role
            var roleResponse = await _userRepository.AssignRoleToUserAsync(
                createUserResponse.UserId,
                Roles.Customer);

            if (roleResponse.ResponseCode != 200)
            {
                // NOTE (v1):
                // DEFERRED: User has been created but no role was assigned.
                // Future work: implement compensating actions to remove the user
                // if role assignment fails.

                return new AuthenticationResponseDto
                {
                    ResponseCode = roleResponse.ResponseCode,
                    ResponseMessage = roleResponse.ResponseMessage
                };
            }

            // 7. Create avatar metadata
            var avatarResponse = await _userRepository.CreateUserAvatarAsync(
                new CreateUserAvatarRequestDto
                {
                    UserId = createUserResponse.UserId,
                    StoredFileName = imageResponse.StoredFileName,
                    FileExtension = imageResponse.FileExtension,
                    MimeType = imageResponse.MimeType,
                    Width = imageResponse.Width,
                    Height = imageResponse.Height,
                    FileSizeBytes = imageResponse.FileSizeBytes
                });

            if (avatarResponse.ResponseCode != 200)
            {
                // NOTE (v1):
                // DEFERRED: User, role and image already exist.
                // Future work: remove user, role assignment and permanent image
                // if avatar creation fails.

                return new AuthenticationResponseDto
                {
                    ResponseCode = avatarResponse.ResponseCode,
                    ResponseMessage = avatarResponse.ResponseMessage
                };
            }

            // 8. Mark upload attempt as completed
            var uploadCompleteResponse =
                await _imageUploadAttemptRepository.MarkUploadCompletedAsync(request.UploadToken);

            if (uploadCompleteResponse.ResponseCode != 200)
            {
                // NOTE (v1):
                // DEFERRED: User, role and avatar already exist.
                // Upload attempt will eventually be handled by the cleanup service.

                return new AuthenticationResponseDto
                {
                    ResponseCode = uploadCompleteResponse.ResponseCode,
                    ResponseMessage = uploadCompleteResponse.ResponseMessage
                };
            }

            // 9. Generate access token
            var claims = new TokenClaimsDto
            {
                Id = createUserResponse.UserId,
                Name = request.Name,
                Email = request.Email,
                Role = Roles.Customer
            };

            var accessToken = _tokenService.GenerateToken(claims);

            // 10. Generate and save refresh token
            var refreshToken =
                await _refreshTokenService.GenerateAndSaveAsync(createUserResponse.UserId);

            if (refreshToken is null)
            {
                return new AuthenticationResponseDto
                {
                    ResponseCode = 500,
                    ResponseMessage = "Failed to generate refresh token."
                };
            }

            // 11. Return authentication response
            return new AuthenticationResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "Registration successful.",

                Id = createUserResponse.UserId,
                Name = request.Name,
                Email = request.Email,

                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public Task<RefreshResponseDto> RefreshAsync(string refreshToken)
        {
            return _refreshTokenService.RefreshAsync(refreshToken);
        }
    }
}
