using E_commerce.DTOs.Response;
using E_commerce.Models;
using E_commerce.Services.Interfaces;
using E_commerce.Settings;
using Microsoft.Extensions.Options;

namespace E_commerce.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ImageStorageSettings _imageStorageSettings;

        public ImageService(
            IWebHostEnvironment environment,
            IOptions<ImageStorageSettings> imageStorageSettings)
        {
            _environment = environment;
            _imageStorageSettings = imageStorageSettings.Value;
        }

        public Task DeleteImageAsync(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return Task.CompletedTask;
            }

            var fullPath = Path.Combine(_environment.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }

        private async Task<ImagePathResponseDto> SaveAvatarInternalAsync(IFormFile image, int? userId)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png")
            {
                return new ImagePathResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Invalid image format"
                };
            }

            if (image.Length > _imageStorageSettings.MaxFileSizeBytes)
            {
                return new ImagePathResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Image size exceeds limit"
                };
            }

            var fileName = $"{Guid.NewGuid()}{extension}";

            string folderPath;
            string relativePath;

            if (userId.HasValue)
            {
                folderPath = Path.Combine(_environment.WebRootPath, _imageStorageSettings.AvatarsFolder, userId.Value.ToString());

                relativePath = Path.Combine(_imageStorageSettings.AvatarsFolder, userId.Value.ToString(), fileName);
            }
            else
            {
                folderPath = Path.Combine(_environment.WebRootPath, _imageStorageSettings.AvatarsFolder);

                relativePath = Path.Combine(_imageStorageSettings.AvatarsFolder, fileName);
            }

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return new ImagePathResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "Image Saved Successfully",
                FilePath = relativePath.Replace("\\", "/")
            };
        }

        public async Task<ImagePathResponseDto> SaveAvatarAsync(IFormFile image, int userId)
        {
            return await SaveAvatarInternalAsync(image, userId);
        }

        public async Task<ImagePathResponseDto> SaveAvatarAsync(IFormFile image)
        {
            return await SaveAvatarInternalAsync(image, null);
        }
    }
}