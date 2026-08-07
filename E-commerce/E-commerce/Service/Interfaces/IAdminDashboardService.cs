using E_commerce.DTOs.Response;

namespace E_commerce.Service.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<GetDashboardStatsResponseDto> GetDashboardStatsAsync();
    }
}
