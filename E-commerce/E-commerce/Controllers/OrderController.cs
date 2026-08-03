using E_commerce.DTOs.Request;
using E_commerce.Enums;
using E_commerce.Service.Interfaces;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICheckoutService _checkoutService;

        public OrderController(ICartService cartService, ICheckoutService checkoutService)
        {
            _cartService = cartService;
            _checkoutService = checkoutService;
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            string name = User.FindFirstValue(JwtRegisteredClaimNames.Name)!;
            string email = User.FindFirstValue(ClaimTypes.Email)!;

            try
            {
                var cart = await _cartService.GetCartAsync(userId);
                
                if (!cart.Items.Any())
                {
                    TempData["Error"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Product");
                }

                var model = new CheckoutViewModel
                {
                    Name = name,
                    Email = email,
                    Address = string.Empty,
                    PhoneNumber = string.Empty,
                    PaymentMethodId = (int)PaymentMethod.CashOnDelivery,
                    Items = cart.Items
                };

                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Product");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([FromBody] CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var request = new CreateOrderRequestDto
            {
                UserId = userId,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                PaymentMethodId = model.PaymentMethodId
            };

            var response = await _checkoutService.CreateOrderAsync(request);

            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var request = new GetOrdersRequestDto
            {
                UserId = userId
            };

            try
            {
                var response = await _checkoutService.GetOrdersAsync(request);

                var model = new OrdersViewModel
                {
                    Orders = response.Orders
                };

                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var request = new GetOrderDetailsRequestDto
            {
                UserId = userId,
                OrderId = id
            };

            try
            {
                var response = await _checkoutService.GetOrderDetailsAsync(request);

                var model = new OrderDetailsViewModel
                {
                    Order = response.Order!,
                    Items = response.Items
                };

                return PartialView("_OrderDetails", model);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(GetOrders));
            }
        }
    }
}
