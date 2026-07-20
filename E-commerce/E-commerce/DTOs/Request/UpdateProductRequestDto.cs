namespace E_commerce.DTOs.Request
{
    public class UpdateProductRequestDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
