namespace E_commerce.DTOs.Response
{
    public class UploadImageResponseDto : ApiResponseDto
    {
        public Guid UploadToken { get; set; }
        public string FileExtension { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public int FileSizeBytes { get; set; }
    }
}
