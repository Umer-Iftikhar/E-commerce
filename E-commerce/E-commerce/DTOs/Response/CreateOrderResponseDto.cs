namespace E_commerce.DTOs.Response
{
    public class CreateOrderResponseDto : SpResponseDto
    {
        public List<int> InvalidProductIds { get; set; } = [];
    }
}

