using E_commerce.DTOs;
using E_commerce.DTOs.Response;

namespace E_commerce.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsDto Order { get; set; } = new();
        public List<OrderItemDto> Items { get; set; } = [];
    }
}
