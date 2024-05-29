using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WatchWeb.Customer.Models;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Request;

namespace WatchWeb.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public HomeController(ILogger<HomeController> logger, 
            IProductService productService,
            ICategoryService categoryService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllForCustomer(new BasePaginationRequest
            {
                PageIndex = 1,
                PageSize = 12,
            });
            return View(products.Data);
        }

        [Route("category")]
        public async Task<IActionResult> GetCategory()
        {
            var category = await _categoryService.GetAllForUserAsync();
            return PartialView("_partialCategory", category);
        }

        [Route("feature-product")]
        public async Task<IActionResult> GetFeatures()
        {
            var products = await _productService.GetFeatureProduct();
            return PartialView("_partialFeatureProduct", products);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
