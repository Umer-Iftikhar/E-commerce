//using Dapper;
//using E_commerce.Constants;
//using E_commerce.Data;
//using E_commerce.DTOs.Response;
//using E_commerce.Repositories.Interfaces;
//using System.Data;

//namespace E_commerce.Repositories.Implementations
//{
//    public class CategoryRepository : ICategoryRepository
//    {
//        private readonly DapperContext _context;

//        public CategoryRepository(DapperContext context)
//        {
//            _context = context;
//        }
//        public async Task<CreateCategoryResponseDto> CreateCategoryAsync(string name)
//        {
//            using var connection = _context.CreateConnection();

//            return await connection.QuerySingleAsync<CreateCategoryResponseDto>(
//            StoredProcedures.CreateCategory,
//            new
//            {
//                Name = name
//            },
//            commandType: CommandType.StoredProcedure);
//        }
//    }
//}
