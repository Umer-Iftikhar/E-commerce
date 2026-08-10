using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using System.Data;

namespace E_commerce.Service.Implementations
{
    public class CartService : ICartService
    {
        private readonly DapperContext _context;
        public CartService(DapperContext context)
        {
            _context = context;
        }
        public async Task<SpResponseDto> AddToCartAsync(int userId, int productId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.AddToCart,
                new
                {
                    UserId = userId,
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure
            );

            var response = await multi.ReadFirstOrDefaultAsync<SpResponseDto>();

            if (response is null)
            {
                throw new InvalidOperationException("Stored procedure \"AddToCartAsync\" returned no response.");
            }
            if (response.ResponseCode != 200)
            {
                throw new InvalidOperationException(response.ResponseMessage);
            }

            return response;

        }

        public async Task<GetCartResponseDto> GetCartAsync(int userId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetCart,
                new
                {
                    UserId = userId,
                },
                commandType: CommandType.StoredProcedure
            );

            var response = await multi.ReadFirstAsync<GetCartResponseDto>();
            var items = (await multi.ReadAsync<CartItemDto>()).ToList();

            if (response.ResponseCode != 200)
            {
                throw new InvalidOperationException(response.ResponseMessage);
            }

            return new GetCartResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                Items = items
            };
        }

        public async Task<SpResponseDto> RemoveFromCartAsync(int userId, int cartItemId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.RemoveFromCart,
                new
                {
                    UserId = userId,
                    CartItemId = cartItemId
                },
                commandType: CommandType.StoredProcedure
            );
            var response = await multi.ReadFirstOrDefaultAsync<SpResponseDto>();

            if (response is null)
            {
                throw new InvalidOperationException("Stored procedure \"RemoveFromCart\" did not return a response.");
            }

            if (response.ResponseCode != 200)
            {
                throw new InvalidOperationException(response.ResponseMessage);
            }
            return response;
        }

        public async Task<SpResponseDto> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.UpdateCartItemQuantity,
                new
                {
                    UserId = userId,
                    CartItemId = cartItemId,
                    Quantity = quantity
                },
                commandType: CommandType.StoredProcedure
            );

            var response = await multi.ReadFirstOrDefaultAsync<SpResponseDto>();

            if (response is null)
            {
                throw new InvalidOperationException(
                    "Stored procedure \"UpdateCartItemQuantity\" did not return a response.");
            }

            if (response.ResponseCode != 200)
            {
                throw new InvalidOperationException(response.ResponseMessage);
            }

            return response;
        }
    }
}
