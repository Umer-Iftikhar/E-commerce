namespace E_commerce.DTOs.Request
{
    public class CreateImageUploadAttemptRequestDto
    {
        public Guid UploadToken { get; set; }
        public string TempFileName { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
