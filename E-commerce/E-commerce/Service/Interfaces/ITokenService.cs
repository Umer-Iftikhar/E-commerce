
using E_commerce.DTOs;

namespace E_commerce.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(TokenClaimsDto claims);
    }
}
