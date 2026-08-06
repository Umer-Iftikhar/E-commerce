using E_commerce.Service.Interfaces;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productsResponse = await _productService.GetProductsAsync(null, null, null);

            if (productsResponse.ResponseCode != 200)
            {
                return View("Error");
            }

            var categoriesResponse = await _categoryService.GetAllCategoriesAsync();

            if (categoriesResponse.ResponseCode != 200)
            {
                return View("Error");
            }

            var viewModel = new ProductIndexViewModel
            {
                Products = productsResponse.Products,
                Categories = categoriesResponse.Categories
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response.ResponseCode != 200)
            {
                return NotFound();
            }
            return View(response.Product);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string? searchTerm, int? categoryId, DateOnly? createdDate)
        {
            var response = await _productService.GetProductsAsync(searchTerm, categoryId, createdDate);
            if (response.ResponseCode != 200)
            {
                return BadRequest(new
                {
                    success = false,
                    message = response.ResponseMessage
                });
            }

            return PartialView("_ProductList", response.Products);
        }
    }
}
