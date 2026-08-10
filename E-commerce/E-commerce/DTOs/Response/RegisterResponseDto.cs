namespace E_commerce.DTOs.Response
{
    public class RegisterResponseDto : SpResponseDto
    {
        public int UserId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
