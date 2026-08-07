using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace E_commerce.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly DapperContext _context;

        public CategoryService(DapperContext context)
        {
            _context = context;
        }

        public async Task<SpResponseDto> CreateCategoryAsync(CreateCategoryRequestDto request)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstAsync<SpResponseDto>(
                StoredProcedures.CreateCategory,
                request,
                commandType: CommandType.StoredProcedure);
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

        public async Task<SpResponseDto> UpdateCategoryAsync(UpdateCategoryRequestDto request)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstAsync<SpResponseDto>(
                StoredProcedures.UpdateCategory,
                request,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<SpResponseDto> SoftDeleteCategoryAsync(int categoryId)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstAsync<SpResponseDto>(
                StoredProcedures.SoftDeleteCategory,
                new
                {
                    CategoryId = categoryId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<SpResponseDto> RestoreCategoryAsync(int categoryId)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstAsync<SpResponseDto>(
                StoredProcedures.RestoreCategory,
                new
                {
                    CategoryId = categoryId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
