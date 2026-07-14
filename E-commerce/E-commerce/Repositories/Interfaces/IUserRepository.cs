using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;

namespace E_commerce.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponseDto> CheckEmailExistsAsync(string email);
        Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto request);
    }
}
