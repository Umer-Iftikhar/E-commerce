using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using E_commerce.Services.Interfaces;
using System.Data;

namespace E_commerce.Service.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly DapperContext _context;
        private readonly IImageService _imageService;

        public ProfileService(
            DapperContext context,
            IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<string?> GetPasswordHashAsync(int userId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetPasswordHash,
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<SpResponseDto>();

            if (response.ResponseCode != 200)
            {
                throw new InvalidOperationException(response.ResponseMessage);
            }

            return await multi.ReadSingleOrDefaultAsync<string>();
        }

        public async Task<ImagePathResponseDto> GetProfileImageAsync(int userId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetProfileImage,
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<SpResponseDto>();

            if (response.ResponseCode != 200)
            {
                throw new InvalidOperationException(response.ResponseMessage);
            }

            var image = await multi.ReadSingleOrDefaultAsync<ImagePathResponseDto>();

            if (image is null)
            {
                return new ImagePathResponseDto
                {
                    ResponseCode = response.ResponseCode,
                    ResponseMessage = response.ResponseMessage,
                    FilePath = null
                };
            }

            image.ResponseCode = response.ResponseCode;
            image.ResponseMessage = response.ResponseMessage;

            return image;
        }

        public async Task<SpResponseDto> UpdateProfileAsync(UpdateProfileRequestDto request, IFormFile? image = null)
        {
            string? oldImagePath = null;
            string? newImagePath = null;

            if (image is not null)
            {
                var currentImage = await GetProfileImageAsync(request.UserId);
                oldImagePath = currentImage.FilePath;

                var imageResponse = await _imageService.SaveAvatarAsync(image, request.UserId);
                if (imageResponse.ResponseCode != 200)
                {
                    return imageResponse;
                }

                newImagePath = imageResponse.FilePath;
                request.ProfileImagePath = newImagePath;
            }

            using var connection = _context.CreateConnection();

            var response = await connection.QuerySingleAsync<SpResponseDto>(
                StoredProcedures.UpdateProfile,
                new
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    Email = request.Email,
                    ProfileImagePath = request.ProfileImagePath,
                },
                commandType: CommandType.StoredProcedure);

            if (image is not null)
            {
                if (response.ResponseCode == 200)
                {
                    await _imageService.DeleteImageAsync(oldImagePath);
                }
                else
                {
                    await _imageService.DeleteImageAsync(newImagePath);
                }
            }

            return response;
        }


        public async Task<SpResponseDto> ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            var currentHash = await GetPasswordHashAsync(request.UserId);


            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, currentHash))
            {
                return new SpResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Current password is incorrect."
                };
            }

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<SpResponseDto>(
                StoredProcedures.ChangePassword,
                new
                {
                    request.UserId,
                    PasswordHash = newPasswordHash
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}