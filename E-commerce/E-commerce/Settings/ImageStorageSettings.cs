namespace E_commerce.Settings
{
    public class ImageStorageSettings
    {
        public string TempFolder { get; set; } = string.Empty;
        public string AvatarsFolder { get; set; } = string.Empty;
        public long MaxFileSizeBytes { get; set; }
        public int UploadTokenExpiryMinutes { get; set; }
    }
}
