using E_commerce.DTOs.Request;
using E_commerce.Service.Interfaces;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        private async Task<List<SelectListItem>> GetCategorySelectListAsync()
        {
            var response = await _categoryService.GetAllCategoriesAsync();

            if (response.ResponseCode != 200)
            {
                return [];
            }

            return response.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAllProductsAdminAsync();

            if (response.ResponseCode != 200)
            {
                TempData["Error"] = response.ResponseMessage;
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductViewModel
            {
                Categories = await GetCategorySelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectListAsync();
                return View(model);
            }

            var request = new CreateProductRequestDto
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CoverImage = model.CoverImage
            };

            var response = await _productService.CreateProductAsync(request);

            if (response.ResponseCode != 200)
            {
                ModelState.AddModelError(string.Empty, response.ResponseMessage);

                model.Categories = await GetCategorySelectListAsync();
                return View(model);
            }

            TempData["Success"] = response.ResponseMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);

            if (response.ResponseCode != 200)
            {
                TempData["Error"] = response.ResponseMessage;
                return RedirectToAction(nameof(Index));
            }

            var product = response.Product!;

            var model = new UpdateProductViewModel
            {
                ProductId = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CurrentCoverImagePath = product.CoverImagePath,
                Categories = await GetCategorySelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] UpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectListAsync();
                return View(model);
            }

            var request = new UpdateProductRequestDto
            {
                ProductId = model.ProductId,
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CoverImage = model.CoverImage
            };

            var response = await _productService.UpdateProductAsync(request);

            if (response.ResponseCode != 200)
            {
                ModelState.AddModelError(string.Empty, response.ResponseMessage);

                model.Categories = await GetCategorySelectListAsync();

                return View(model);
            }
            TempData["Success"] = response.ResponseMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.SoftDeleteProductAsync(id);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var response = await _productService.RestoreProductAsync(id);

            return Json(response);
        }
    }
}
