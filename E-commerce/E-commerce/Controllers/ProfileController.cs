using E_commerce.Constants;
using E_commerce.DTOs;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using E_commerce.Settings;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ITokenService _tokenService;
        private readonly JwtConfig _jwtConfig;

        public ProfileController(IProfileService profileService, 
            ITokenService tokenService, 
            IOptions<JwtConfig> jwtConfig)
        {
            _profileService = profileService;
            _tokenService = tokenService;
            _jwtConfig = jwtConfig.Value;
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
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequestDto request, IFormFile? profileImage)
        {
            request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _profileService.UpdateProfileAsync(request, profileImage);

            if (response.ResponseCode == 200 && (request.Name != null || request.Email != null))
            {
                var claims = new TokenClaimsDto
                {
                    Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    Name = request.Name ?? User.FindFirstValue(JwtRegisteredClaimNames.Name)!,
                    Email = request.Email ?? User.FindFirstValue(ClaimTypes.Email)!,
                    Role = User.FindFirstValue(ClaimTypes.Role)!,
                    ProfileImagePath = User.FindFirstValue(ClaimConstants.ProfileImagePath)
                };

                var newToken = _tokenService.GenerateToken(claims);

                Response.Cookies.Append(CookieConstants.AccessToken, newToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes)
                });
            }

            return Json(response);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var request = new ChangePasswordRequestDto
                {
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    CurrentPassword = model.CurrentPassword,
                    NewPassword = model.NewPassword
                };

                var response = await _profileService.ChangePasswordAsync(request);

                if(response.ResponseCode == 200)
                {
                    TempData["Success"] = response.ResponseMessage;

                    return RedirectToAction("Index", "Product");
                }

                ModelState.AddModelError(string.Empty, response.ResponseMessage);
                return View(new ChangePasswordViewModel());

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new ChangePasswordViewModel());
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
