using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileImage()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _profileService.GetProfileImageAsync(userId);

                return Json(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new SpResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = ex.Message
                });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto request)
        {
            request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _profileService.UpdateProfileAsync(request);

            return Json(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile profileImage)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _profileService.UpdateProfilePictureAsync(userId, profileImage);

            return Json(response);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            try
            {
                request.UserId = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _profileService.ChangePasswordAsync(request);

                return Json(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new SpResponseDto
                {
                    ResponseCode = 400,
                    ResponseMessage = ex.Message
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var imageResponse = await _profileService.GetProfileImageAsync(userId);

                var model = new GetProfileResponseDto
                {
                    Name = User.FindFirstValue(JwtRegisteredClaimNames.Name) ?? string.Empty,
                    Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                    ProfileImagePath = imageResponse.FilePath
                };

                return PartialView("_ProfileSidebar", model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
