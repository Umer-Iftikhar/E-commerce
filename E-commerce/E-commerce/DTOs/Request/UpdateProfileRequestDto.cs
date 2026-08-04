namespace E_commerce.DTOs.Request
{
    public class UpdateProfileRequestDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
