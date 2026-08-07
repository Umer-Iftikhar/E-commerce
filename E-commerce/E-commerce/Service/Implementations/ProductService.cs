    using Dapper;
    using E_commerce.Constants;
    using E_commerce.Data;
    using E_commerce.DTOs;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
    using E_commerce.Service.Interfaces;
using E_commerce.Services.Interfaces;
using System.Data;

namespace E_commerce.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly DapperContext _context;
        private readonly IImageService _imageService;
        public ProductService(DapperContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
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

        public async Task<SpResponseDto> CreateProductAsync(CreateProductRequestDto request)
        {
            ImagePathResponseDto? image = null;

            if (request.CoverImage is not null)
            {
                image = await _imageService.SaveProductImageAsync(request.CoverImage);

                if (image.ResponseCode != 200)
                {
                    return image;
                }
            }

            try
            {
                using var connection = _context.CreateConnection();

                var response = await connection.QueryFirstAsync<SpResponseDto>(
                    StoredProcedures.CreateProduct,
                    new
                    {
                        request.CategoryId,
                        request.Name,
                        request.Description,
                        request.Price,
                        request.Stock,
                        CoverImagePath = image?.FilePath
                    },
                    commandType: CommandType.StoredProcedure);

                if (response.ResponseCode != 200)
                {
                    await _imageService.DeleteImageAsync(image?.FilePath);
                }

                return response;
            }
            catch
            {
                await _imageService.DeleteImageAsync(image?.FilePath);
                throw;
            }
        }

        public async Task<SpResponseDto> UpdateProductAsync(UpdateProductRequestDto request)
        {
            var existingProduct = await GetProductByIdAsync(request.ProductId);
            if (existingProduct.ResponseCode != 200)
            {
                return existingProduct;
            }

            string? oldImagePath = existingProduct.Product?.CoverImagePath;
            string? newImagePath = null;

            if (request.CoverImage is not null)
            {
                var imageResponse = await _imageService.SaveProductImageAsync(request.CoverImage);

                if (imageResponse.ResponseCode != 200)
                {
                    return imageResponse;
                }

                newImagePath = imageResponse.FilePath;
            }

            try
            {
                using var connection = _context.CreateConnection();

                var response = await connection.QueryFirstAsync<SpResponseDto>(
                    StoredProcedures.UpdateProduct,
                    new
                    {
                        request.ProductId,
                        request.CategoryId,
                        request.Name,
                        request.Description,
                        request.Price,
                        request.Stock,
                        CoverImagePath = newImagePath
                    },
                    commandType: CommandType.StoredProcedure);

                if (response.ResponseCode == 200)
                {
                    if (newImagePath is not null)
                    {
                        await _imageService.DeleteImageAsync(oldImagePath);
                    }
                }
                else
                {
                    if (newImagePath is not null)
                    {
                        await _imageService.DeleteImageAsync(newImagePath);
                    }
                }

                return response;
            }
            catch
            {
                if (newImagePath is not null)
                {
                    await _imageService.DeleteImageAsync(newImagePath);
                }
                throw;
            }
        }
        
        public async Task<SpResponseDto> SoftDeleteProductAsync(int productId)
        {
            using var connection = _context.CreateConnection();

            var response = await connection.QueryFirstAsync<SoftDeleteProductResponseDto>(
                StoredProcedures.SoftDeleteProduct,
                new { ProductId = productId },
                commandType: CommandType.StoredProcedure);

            if (response.ResponseCode == 200)
            {
                await _imageService.DeleteImageAsync(response.CoverImagePath);
            }

            return response;
        }

        public async Task<GetProductsResponseDto> GetAllProductsAdminAsync()
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetAllProductsAdmin,
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadFirstAsync<SpResponseDto>();

            var products = (await multi.ReadAsync<ProductListItemDto>()).ToList();

            return new GetProductsResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                Products = products
            };
        }

        public async Task<SpResponseDto> RestoreProductAsync(int productId)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstAsync<SpResponseDto>(
                StoredProcedures.RestoreProduct,
                new
                {
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
