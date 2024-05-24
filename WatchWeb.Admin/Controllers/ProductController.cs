using Microsoft.AspNetCore.Mvc;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Request.Products;

namespace WatchWeb.Admin.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(CreateProductRequest request)
        {
            return LocalRedirect("product");
        }
    }
}
