namespace E_commerce.Models
{
    public class UserAvatar
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public int FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
