namespace E_commerce.DTOs.Request
{
    public class UpdateCategoryRequestDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
