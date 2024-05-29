using Microsoft.AspNetCore.Mvc;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Request;

namespace WatchWeb.Customer.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        { 
            _productService = productService;
        }

        [Route("search")]
        public async Task<IActionResult> Index(BasePaginationRequest request)
        {
            var products = await _productService.GetAllForCustomer(request);
            return View(products);
        }


        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetDetailForUserAsync(id);
            return View(product);
        }
    }
}
