using E_commerce.DTOs.Response;
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

        public async Task<ImagePathResponseDto> SaveAvatarAsync(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
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

            var folderPath = Path.Combine(_environment.WebRootPath, _imageStorageSettings.AvatarsFolder);

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var relativePath = Path.Combine(_imageStorageSettings.AvatarsFolder,fileName).Replace("\\", "/");

            return new ImagePathResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "Image Saved Successfully",
                FilePath = relativePath
            };
        }
    }
}