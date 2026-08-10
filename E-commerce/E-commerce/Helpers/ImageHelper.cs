using E_commerce.Constants;

namespace E_commerce.Helpers
{
    public static class ImageHelper
    {
        private readonly static Dictionary<string, string> SupportedImageTypes = new()
        {
            { ImageConstants.Jpeg, "image/jpeg" },
            { ImageConstants.Jpg, "image/jpeg" },
            { ImageConstants.Png, "image/png" }
        };

        public static string GenerateStoredFileName(string extension)
        {
            return $"{Guid.NewGuid()}{extension.ToLowerInvariant()}";
        }

        public static string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName).ToLowerInvariant();
        }

        public static bool IsSupportedExtension(string extension)
        {
            return SupportedImageTypes.ContainsKey(extension.ToLowerInvariant());
        }

        public static string GetMimeType(string extension)
        {
            return SupportedImageTypes.TryGetValue(extension.ToLowerInvariant(), out var mimeType) ? mimeType : ImageConstants.DefaultMimeType;
        }
    }
}
