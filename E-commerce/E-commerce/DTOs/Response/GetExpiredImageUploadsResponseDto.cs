namespace E_commerce.DTOs.Response
{
    public class GetExpiredImageUploadsResponseDto : ApiResponseDto
    {
        public IReadOnlyList<ExpiredImageUploadDto> ExpiredUploads { get; set; } = [];
    }
}
