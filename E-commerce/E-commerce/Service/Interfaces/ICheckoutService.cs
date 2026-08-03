using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface ICheckoutService
    {
        Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto dto);
        Task<GetOrdersResponseDto> GetOrdersAsync(GetOrdersRequestDto request);
        Task<GetOrderDetailsResponseDto> GetOrderDetailsAsync(GetOrderDetailsRequestDto request);
    }
}
