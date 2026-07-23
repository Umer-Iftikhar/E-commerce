    using Dapper;
    using E_commerce.Constants;
    using E_commerce.Data;
    using E_commerce.DTOs;
    using E_commerce.DTOs.Response;
    using E_commerce.Service.Interfaces;
    using System.Data;

namespace E_commerce.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly DapperContext _context;
        public ProductService(DapperContext context)
        {
            _context = context;
        }
        public async Task<GetProductsResponseDto> GetProductsAsync(string? searchTerm, int? categoryId, DateOnly? createdDate)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetProducts,
                new
                {
                    SearchTerm = searchTerm,
                    CategoryId = categoryId,
                    CreatedDate = createdDate?.ToDateTime(TimeOnly.MinValue)
                },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadFirstAsync<SpResponseDto>();

            var product = (await multi.ReadAsync<ProductListItemDto>()).ToList();

            return new GetProductsResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                Products = product
            };
        }

        public async Task<GetProductResponseDto> GetProductByIdAsync(int productId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetProductById,
                new
                {
                    ProductId = productId,
                },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadFirstAsync<SpResponseDto>();

            var product = await multi.ReadFirstOrDefaultAsync<ProductDetailDto>();

            return new GetProductResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                Product = product
            };
        }

        public async Task<GetCategoriesResponseDto> GetAllCategoriesAsync()
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetAllCategories,
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadFirstAsync<SpResponseDto>();

            var categories = (await multi.ReadAsync<CategoryListItemDto>()).ToList();

            return new GetCategoriesResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                Categories = categories
            };
        }
    }
}
