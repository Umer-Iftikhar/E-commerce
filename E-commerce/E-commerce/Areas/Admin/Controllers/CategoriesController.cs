using E_commerce.DTOs.Request;
using E_commerce.Service.Interfaces;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAllCategoriesAdminAsync();

            if (response.ResponseCode != 200)
            {
                TempData["Error"] = response.ResponseMessage;
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new CreateCategoryRequestDto
            {
                Name = model.Name
            };

            var response = await _categoryService.CreateCategoryAsync(request);

            if (response.ResponseCode != 200)
            {
                ModelState.AddModelError(string.Empty, response.ResponseMessage);
                return View(model);
            }

            TempData["Success"] = response.ResponseMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    ResponseCode = 400,
                    ResponseMessage = "Invalid request."
                });
            }

            var request = new UpdateCategoryRequestDto
            {
                CategoryId = model.Id,
                Name = model.Name
            };

            var response = await _categoryService.UpdateCategoryAsync(request);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.SoftDeleteCategoryAsync(id);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var response = await _categoryService.RestoreCategoryAsync(id);

            return Json(response);
        }
    }
}
