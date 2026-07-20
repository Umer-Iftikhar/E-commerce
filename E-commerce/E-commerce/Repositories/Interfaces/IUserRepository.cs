using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Models;

namespace E_commerce.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponseDto> CheckEmailExistsAsync(string email);
        Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto request);
        Task<ApiResponseDto> CreateUserAvatarAsync(CreateUserAvatarRequestDto request);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<ApiResponseDto> AssignRoleToUserAsync(int userId, string roleName);
    }
}
