namespace E_commerce.DTOs.Response
{
    public class GetDashboardStatsResponseDto :SpResponseDto
    {
        public DashboardStatsDto Stats { get; set; } = new();
        public List<LowStockProductDto> LowStockProducts { get; set; } = [];
    }
}
