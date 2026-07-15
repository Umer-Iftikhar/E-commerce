
using E_commerce.DTOs.Response;
using E_commerce.Helpers;
using E_commerce.Services.Interfaces;
using E_commerce.Settings;
using SixLabors.ImageSharp;
using Microsoft.Extensions.Options;

    namespace E_commerce.Services.Implementations
    {
        public class ImageService : IImageService
        {
            private readonly IWebHostEnvironment _env;
            private readonly ImageStorageSettings _settings;

            public ImageService(
                IWebHostEnvironment env,
                IOptions<ImageStorageSettings> options)
            {
                _env = env;
                _settings = options.Value;
            }

            public async Task<MoveImageResponseDto> MoveToPermanentStorageAsync(string tempFileName)
            {
                string? permanentFilePath = null;   
                try
                {
                    var extension = ImageHelper.GetExtension(tempFileName);

                    if (!ImageHelper.IsSupportedExtension(extension))
                    {
                        return new MoveImageResponseDto
                        {
                            ResponseCode = 400,
                            ResponseMessage = "Unsupported image type."
                        };
                    }

                    var tempFilePath = Path.Combine(_env.WebRootPath, _settings.TempFolder, tempFileName);

                    if (!File.Exists(tempFilePath))
                    {
                        return new MoveImageResponseDto
                        {
                            ResponseCode = 404,
                            ResponseMessage = "Temporary image not found."
                        };
                    }

                    var storedFileName = ImageHelper.GenerateStoredFileName(extension);

                    var avatarsFolder = Path.Combine(
                        _env.WebRootPath,
                        _settings.AvatarsFolder);

                    Directory.CreateDirectory(avatarsFolder);

                    permanentFilePath = Path.Combine(avatarsFolder, storedFileName);

                    File.Move(tempFilePath, permanentFilePath);

                    var imageInfo = await Image.IdentifyAsync(permanentFilePath);

                    if (imageInfo is null)
                    {
                        File.Delete(permanentFilePath);
                        return new MoveImageResponseDto
                        {
                            ResponseCode = 500,
                            ResponseMessage = "Unable to read image metadata."
                        };
                    }

                    var fileInfo = new FileInfo(permanentFilePath);

                    return new MoveImageResponseDto
                    {
                        ResponseCode = 200,
                        ResponseMessage = "Image moved successfully.",
                        StoredFileName = storedFileName,
                        FileExtension = extension,
                        MimeType = ImageHelper.GetMimeType(extension),
                        Width = imageInfo.Width,
                        Height = imageInfo.Height,
                        FileSizeBytes = (int)fileInfo.Length
                    };
                }
                catch (Exception ex)
                {

                    if (!string.IsNullOrWhiteSpace(permanentFilePath) && File.Exists(permanentFilePath))
                    {
                        File.Delete(permanentFilePath);
                    }
                    return new MoveImageResponseDto
                    {
                        ResponseCode = 500,
                        ResponseMessage = ex.Message
                    };
                }
            }
        }
    }

