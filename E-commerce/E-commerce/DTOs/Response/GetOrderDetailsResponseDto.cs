
namespace E_commerce.DTOs.Response
{
    public class GetOrderDetailsResponseDto : SpResponseDto
    {
        public OrderDetailsDto? Order { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
