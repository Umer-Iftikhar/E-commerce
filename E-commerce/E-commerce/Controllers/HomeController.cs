using E_commerce.Service.Interfaces;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace E_commerce.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly IProductService _productService;

//        public HomeController(IProductService productService)
//        {
//            _productService = productService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            var productsResponse = await _productService.GetProductsAsync(null, null, null);
//            if (productsResponse.ResponseCode != 200)
//            {
//                return View("Error");
//            }

//            //var categoriesResponse = await _productService.GetAllCategoriesAsync();

//            if (categoriesResponse.ResponseCode != 200)
//            {
//                return View("Error");
//            }

//            //var viewModel = new HomeViewModel
//            //{
//            //    Products = productsResponse.Products,
//            //    Categories = categoriesResponse.Categories
//            //};

//            //return View(viewModel);
//            return View();
//        }
//    }
//}
