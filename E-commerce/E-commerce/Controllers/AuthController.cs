using E_commerce.Constants;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using E_commerce.Settings;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace E_commerce.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtConfig _jwtConfig;
        public AuthController(IUserService userService, IOptions<JwtConfig> options)
        {
            _userService = userService;
            _jwtConfig = options.Value;
        }

        private void SetAuthenticationCookies(AuthenticationResponseDto response)
        {
            Response.Cookies.Append(CookieConstants.AccessToken, response.AccessToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes)
            });

            Response.Cookies.Append(CookieConstants.RefreshToken, response.RefreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryDays)
            });
        }

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.UploadToken == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.UploadToken),"Please upload an image.");

                return View(model);
            }

            var request = new RegisterRequestDto
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                UploadToken = model.UploadToken
            };

            var response = await _userService.RegisterAsync(request);

            if (response.ResponseCode != 200)
            {
                ModelState.AddModelError(string.Empty, response.ResponseMessage);
                return View(model);
            }

            SetAuthenticationCookies(response);

            TempData["SuccessMessage"] = "Registration successful. Welcome!";
            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password
            };

            var response = await _userService.LoginAsync(request);

            if (response.ResponseCode != 200)
            {
                ModelState.AddModelError(string.Empty, response.ResponseMessage);
                return View(model);
            }

            SetAuthenticationCookies(response);

            TempData["SuccessMessage"] = "Login successful.";
            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Logout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await _userService.LogoutAsync(refreshToken);
            }

            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");

            return RedirectToAction(nameof(Login));
        }

        #endregion
    }
}
