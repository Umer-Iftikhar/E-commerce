namespace E_commerce.DTOs.Response
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string StoredFileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public int FileSizeBytes { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
