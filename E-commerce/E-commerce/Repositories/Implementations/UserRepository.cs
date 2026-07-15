using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using System.Data;

namespace E_commerce.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto request)
        {
            using var connection = _context.CreateConnection();
            var response = await connection.QuerySingleAsync<CreateUserResponseDto>(
                StoredProcedures.CreateUser, 
                new { 
                    Email = request.Email,
                    Name = request.Name,
                    PasswordHash = request.PasswordHash
                }, 
                commandType: CommandType.StoredProcedure);

            return response;
        }

        public async Task<ApiResponseDto> CheckEmailExistsAsync(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.CheckEmailExists, 
                new { Email = email }, 
                commandType: CommandType.StoredProcedure);

            return sql;
        }

        public async Task<ApiResponseDto> CreateUserAvatarAsync(CreateUserAvatarRequestDto request)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.CreateUserAvatar,
                new
                {
                    request.UserId,
                    request.StoredFileName,
                    request.FileExtension,
                    request.MimeType,
                    request.Width,
                    request.Height,
                    request.FileSizeBytes
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetUserById,
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<ApiResponseDto>();

            if (response.ResponseCode != 200)
            {
                return null;
            }

            return await multi.ReadSingleOrDefaultAsync<User>();
        }
    }
}
