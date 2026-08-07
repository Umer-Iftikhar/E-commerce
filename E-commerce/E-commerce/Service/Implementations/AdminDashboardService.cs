using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using System.Data;

namespace E_commerce.Service.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly DapperContext _context;
        public AdminDashboardService(DapperContext context)
        {
            _context = context;
        }
        public async Task<GetDashboardStatsResponseDto> GetDashboardStatsAsync()
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetDashboardStats,
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<SpResponseDto>();

            var dashboard = new GetDashboardStatsResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage
            };

            if (dashboard.ResponseCode != 200)
            {
                return dashboard;
            }

            dashboard.Stats = await multi.ReadSingleAsync<DashboardStatsDto>();
            dashboard.LowStockProducts = (await multi.ReadAsync<LowStockProductDto>()).ToList();

            return dashboard;
        }
    }
}
