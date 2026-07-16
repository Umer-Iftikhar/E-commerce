using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Helpers;
using E_commerce.Repositories.Interfaces;
using E_commerce.Service.Interfaces;
using E_commerce.Settings;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;

namespace E_commerce.Service.Implementations
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ImageStorageSettings _settings;
        private readonly IImageUploadAttemptRepository _repository;

        public ImageUploadService(IWebHostEnvironment env, IOptions<ImageStorageSettings> options, IImageUploadAttemptRepository repository)
        {
            _env = env;
            _settings = options.Value;
            _repository = repository;
        }

        public async Task<UploadImageResponseDto> UploadAsync(IFormFile image)
        {
            // 1. Validate null/empty
            if (image == null)
            {
                return new UploadImageResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Image file is required."
                };
            }

            // 2. Validate file size
            if (image.Length > _settings.MaxFileSizeBytes)
            { 
                return new UploadImageResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = $"File size exceeds the maximum limit of {_settings.MaxFileSizeBytes} bytes."
                };
            }

            // 3. Get extension and validate
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!ImageHelper.IsSupportedExtension(extension))
            {
                return new UploadImageResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Only JPG, JPEG, and PNG files are allowed."
                };
            }

            // 4. Open stream
            using var stream = image.OpenReadStream();

            // 5. Read magic bytes
            byte[] header = new byte[4];

            await stream.ReadAsync(header);

            if (!IsValidMagicBytes(header))
            {
                return new UploadImageResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Invalid image file."
                };
            }

            // 6. Reset stream position
            stream.Position = 0;

            // 7. Image.IdentifyAsync()

            var imageInfo = await Image.IdentifyAsync(stream);
            if (imageInfo is null)
            {
                return new UploadImageResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = "Invalid image file."
                };
            }

            // 8. Reset stream position
            stream.Position = 0;

            // 9. Generate stored filename
            var storedFileName = ImageHelper.GenerateStoredFileName(extension);

            // 10. Build temp path
            var tempFolder = Path.Combine(_env.WebRootPath, _settings.TempFolder);

            // 11. Create temp directory
            Directory.CreateDirectory(tempFolder);
            var tempFilePath = Path.Combine(tempFolder, storedFileName);

            // 12. Copy stream to file
            using var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);

            // 13. Generate upload token
            var uploadToken = new CreateImageUploadAttemptRequestDto
            {
                UploadToken = Guid.NewGuid(),
                TempFileName = storedFileName,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_settings.UploadTokenExpiryMinutes)
            };

            // 14. Create ImageUploadAttempt
            var uploadAttemptResponse = await _repository.CreateImageUploadAttemptAsync(uploadToken);

            if (uploadAttemptResponse.ResponseCode != 200)
            {
                // Compensation:
                // File exists but database record was not created.
                // Remove orphaned temp file.
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                return new UploadImageResponseDto
                {
                    ResponseCode = uploadAttemptResponse.ResponseCode,
                    ResponseMessage = uploadAttemptResponse.ResponseMessage
                };
            }

            // 15. Return UploadImageResponseDto
            return new UploadImageResponseDto
            {
                ResponseCode = 200,
                ResponseMessage = "Image uploaded successfully.",

                UploadToken = uploadToken.UploadToken,
                Width = imageInfo.Width,
                Height = imageInfo.Height,
                FileSizeBytes = (int)image.Length,
                MimeType = ImageHelper.GetMimeType(extension),
                FileExtension = extension
            };
        }

        private static bool IsValidMagicBytes(byte[] header)
        {
            // JPEG
            if (header.Length >= 3 &&
                header[0] == 0xFF &&
                header[1] == 0xD8 &&
                header[2] == 0xFF)
            {
                return true;
            }

            // PNG
            if (header.Length >= 4 &&
                header[0] == 0x89 &&
                header[1] == 0x50 &&
                header[2] == 0x4E &&
                header[3] == 0x47)
            {
                return true;
            }

            return false;
        }
    }
}
