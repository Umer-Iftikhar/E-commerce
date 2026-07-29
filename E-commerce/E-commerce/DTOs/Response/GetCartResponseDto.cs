namespace E_commerce.DTOs.Response
{
    public class GetCartResponseDto : SpResponseDto
    {
        public List<CartItemDto> Items { get; set; } = [];
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
