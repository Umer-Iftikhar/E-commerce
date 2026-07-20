using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Repositories.Interfaces;
using System.Data;

namespace E_commerce.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<ApiResponseDto> AddProductImageAsync(AddProductImageRequestDto request)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.AddProductImage,
                request,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<CreateProductResponseDto> CreateProductAsync(CreateProductRequestDto request)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<CreateProductResponseDto>(
                StoredProcedures.CreateProduct,
                request,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<ProductListItemDto>> GetAllProductsAsync()
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetAllProducts,
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<ApiResponseDto>();

            if (response.ResponseCode != 200)
            {
                return [];
            }

            return (await multi.ReadAsync<ProductListItemDto>()).ToList();
        }

        public async Task<GetProductResponseDto> GetProductByIdAsync(int productId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetProductById,
                new { ProductId = productId },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<ApiResponseDto>();

            var result = new GetProductResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage
            };

            if (response.ResponseCode != 200)
            {
                return result;
            }

            result.Product = await multi.ReadSingleOrDefaultAsync<ProductDetailDto>();

            if (result.Product is not null)
            {
                result.Product.Images = (await multi.ReadAsync<ProductImageDto>()).ToList();
            }

            return result;
        }

        public async Task<List<ProductListItemDto>> GetProductsByCategoryAsync(int categoryId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetProductsByCategory,
                new
                {
                    CategoryId = categoryId
                },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<ApiResponseDto>();

            if (response.ResponseCode != 200)
            {
                return [];
            }

            return (await multi.ReadAsync<ProductListItemDto>()).ToList();
        }

        public async Task<ApiResponseDto> SoftDeleteProductAsync(int productId)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.SoftDeleteProduct,
                new
                {
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ApiResponseDto> UpdateProductAsync(UpdateProductRequestDto request)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.UpdateProduct,
                request,
                commandType: CommandType.StoredProcedure);
        }
    }
}
