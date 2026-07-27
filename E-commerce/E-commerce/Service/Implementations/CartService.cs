using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
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

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.Int32);
            parameters.Add("@ProductId", productId, DbType.Int32);

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.AddToCart,
                parameters,
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadFirstOrDefaultAsync<SpResponseDto>();

            if (response is null)
            {
                throw new InvalidOperationException("Stored procedure returned no response.");
            }

            return response;

        }
    }
}
