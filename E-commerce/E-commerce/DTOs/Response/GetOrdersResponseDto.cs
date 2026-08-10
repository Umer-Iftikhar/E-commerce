namespace E_commerce.DTOs.Response
{
    public class GetOrdersResponseDto : SpResponseDto
    {
        public List<OrderListItemDto> Orders { get; set; } = [];
    }
}
