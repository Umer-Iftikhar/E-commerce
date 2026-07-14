namespace E_commerce.DTOs.Response
{
    public class RefreshResponseDto : ApiResponseDto
    {
        public string? AccessToken { get; set; } 
        public string? RefreshToken { get; set; } 
    }
}
