using E_commerce.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageUploadService _imageUploadService;

        public ImageController(IImageUploadService imageUploadService)
        {
            _imageUploadService = imageUploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            var response = await _imageUploadService.UploadAsync(image);

            return Json(response);
        }
    }
}
