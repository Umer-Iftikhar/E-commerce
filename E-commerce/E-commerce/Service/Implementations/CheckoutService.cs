using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using System.Data;

namespace E_commerce.Service.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly DapperContext _context;
        public CheckoutService(DapperContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.CreateOrder,
                new
                {
                    request.UserId,
                    request.PhoneNumber,
                    request.Address,
                    request.PaymentMethodId
                },
                commandType: CommandType.StoredProcedure
            );

            var response = await multi.ReadFirstAsync<SpResponseDto>();

            var productIds = (await multi.ReadAsync<int>()).ToList();

            return new CreateOrderResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                InvalidProductIds = productIds
            };
        }
    }
}
