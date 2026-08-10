using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _cartService.AddToCartAsync(userId, request.ProductId);

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

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _cartService.GetCartAsync(userId);

                return PartialView("_Cart", response);
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

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _cartService.RemoveFromCartAsync(userId, cartItemId);

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

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var response = await _cartService.UpdateCartItemQuantityAsync(userId, cartItemId, quantity);

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
    }
}
