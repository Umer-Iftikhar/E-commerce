using E_commerce.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response.ResponseCode != 200)
            {
                ModelState.AddModelError(string.Empty, response.ResponseMessage);
                return View();
            }
            return View(response.Product);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string? searchTerm, int? categoryId)
        {
            var response = await _productService.GetProductsAsync(searchTerm, categoryId);
            if (response.ResponseCode != 200)
            {
                return NotFound();
            }

            return Json(new
            {
                success = true,
                products = response.Products
            });
        }
    }
}
