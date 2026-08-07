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
        private readonly IMapper _mapper;

        public CategoriesController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAllCategoriesAsync();

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

            var request = _mapper.Map<CreateCategoryRequestDto>(model);

            var response = await _categoryService.CreateCategoryAsync(request);

            if (response.ResponseCode != 200)
            {
                ModelState.AddModelError(string.Empty, response.ResponseMessage);
                return View(model);
            }

            TempData["Success"] = response.ResponseMessage;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);

            if (response.ResponseCode != 200)
            {
                TempData["Error"] = response.ResponseMessage;
                return RedirectToAction(nameof(Index));
            }

            var model = _mapper.Map<UpdateCategoryViewModel>(response.Category);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = _mapper.Map<UpdateCategoryRequestDto>(model);

            var response = await _categoryService.UpdateCategoryAsync(request);

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
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.SoftDeleteCategoryAsync(id);

            TempData[response.ResponseCode == 200 ? "Success" : "Error"] =
                response.ResponseMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var response = await _categoryService.RestoreCategoryAsync(id);

            TempData[response.ResponseCode == 200 ? "Success" : "Error"] =
                response.ResponseMessage;

            return RedirectToAction(nameof(Index));
        }
    }
}
