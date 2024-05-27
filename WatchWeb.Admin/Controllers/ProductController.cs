using Microsoft.AspNetCore.Mvc;
using WatchWeb.Common.Constant;
using WatchWeb.Common.Helper;
using WatchWeb.Model.Entities;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Products;

namespace WatchWeb.Admin.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(BasePaginationRequest request)
        {
            var product = await _productService.GetAllAsync(request);
            return View(product);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var req = new CreateProductRequest();
            req.Categories = await _categoryService.GetListForCreateUpdateProduct();
            return View(req);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductRequest request, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                    request.ImageUrl = ImageHelper.ConvertImageToBase64(image);
                request.CreateDate = DateTime.UtcNow;
                request.CreateUser = Convert.ToInt32(HttpContext.User.FindFirst(SeasionConstant.USER_ID)?.Value);
                var result = await _productService.CreateAsync(request);
                result.Data.Categories = await _categoryService.GetListForCreateUpdateProduct();
                ViewData[ViewDataConstant.RESULT] = result.Message;
                if (result.IsSuccess)
                    return LocalRedirect("/product");
                return View(result.Data);
            }
            var req = new CreateProductRequest();
            req.Categories = await _categoryService.GetListForCreateUpdateProduct();
            return View(req);
        }

        [HttpGet("update")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetDetailForUpdateAsync(id);
            product.Data.Categories = await _categoryService.GetListForCreateUpdateProduct();
            return View(product.Data);
        }

        [HttpPost("update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateProductRequest request, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                    request.ImageUrl = ImageHelper.ConvertImageToBase64(image);
                var result = await _productService.UpdateAsync(request);
                result.Data.Categories = await _categoryService.GetListForCreateUpdateProduct();
                ViewData[ViewDataConstant.RESULT] = result.Message;
                if (result.IsSuccess)
                    return LocalRedirect("/product");
                return View(result.Data);
            }
            var req = new UpdateProductRequest();
            req.Categories = await _categoryService.GetListForCreateUpdateProduct();
            return View(req);
        }

        [Route("active")]
        public async Task<IActionResult> Active(int id)
        {
            var result = await _productService.Active(id);
            if (result.IsSuccess)
            {
                ViewData["Success"] = result.Message;
                return Json(result);
            }
            return Json(result);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (result.IsSuccess)
            {
                ViewData["Success"] = result.Message;
                return Json(result);
            }
            return Json(result);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var product = await _productService.GetDetailAsync(id);
            return View(product);
        }

        [HttpGet("activemodal")]
        public IActionResult ActivePartial(string id, string status)
        {
            ViewData["id"] = id;
            ViewData["status"] = status;
            ViewData["action"] = "active";
            ViewData["controller"] = "category";
            return PartialView("_parttialActiveProductModal");
        }


        [HttpGet("deletemodal")]
        public IActionResult DeletePartial(string id, string name)
        {
            ViewData["id"] = id;
            ViewData["name"] = name;
            ViewData["action"] = "delete";
            ViewData["controller"] = "category";
            return PartialView("_partialDeleteProductModal");
        }

        [HttpGet("detailmodal")]
        public async Task<IActionResult> DetailPartial(string id)
        {
            var req = await _productService.GetDetailAsync(Convert.ToInt32(id));
            return PartialView("_partialDetailProductModal", req.Data);
        }
    }
}
