namespace E_commerce.DTOs.Response
{
    public class GetProfileResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImagePath { get; set; }
    }
}
