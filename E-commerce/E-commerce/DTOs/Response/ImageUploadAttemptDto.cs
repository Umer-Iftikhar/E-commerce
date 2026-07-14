using E_commerce.Enums;

namespace E_commerce.DTOs.Response
{
    public class ImageUploadAttemptDto
    {
        public Guid UploadToken { get; set; }
        public string TempFileName { get; set; } = string.Empty;
        public UploadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
