using E_commerce.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetProductsAsync(null, null);
            if (response.ResponseCode != 200)
            {
                return View("Error");
            }
            return View(response.Products);
        }
    }
}
