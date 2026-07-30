using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface ICheckoutService
    {
        Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto dto);
    }
}
