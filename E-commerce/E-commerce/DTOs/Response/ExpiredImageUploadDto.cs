namespace E_commerce.DTOs.Response
{
    public class ExpiredImageUploadDto
    {
        public Guid UploadToken { get; set; }
        public string TempFileName { get; set; } = string.Empty;
    }
}
