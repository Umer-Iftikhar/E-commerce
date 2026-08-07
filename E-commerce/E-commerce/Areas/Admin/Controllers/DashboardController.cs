using E_commerce.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;
        public DashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _adminDashboardService.GetDashboardStatsAsync();

            if (response.ResponseCode != 200)
            {
                TempData["Error"] = response.ResponseMessage;
            }

            return View(response);
        }
    }
}
