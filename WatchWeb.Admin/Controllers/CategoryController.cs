using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WatchWeb.Common.Helper;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Category;

namespace WatchWeb.Admin.Controllers
{
    [Route("category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(BasePaginationRequest request)
        {
            var categories = await _categoryService.GetAllAsync(request);
            return View(categories);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var request = new CreateCategoryRequest();
            request.CategoryParent = await _categoryService.GetListForCreateUpdate();
            return View(request);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCategoryRequest request, IFormFile image)
        {
          
            if (image != null && image.Length > 0)
                request.ImageUrl = ImageHelper.ConvertImageToBase64(image);
            await _categoryService.CreateAsync(request);
            return LocalRedirect("/category");
        }

        [HttpGet("update")]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetDetailForUpdateAsync(id);
            category.Data.CategoryParent = await _categoryService.GetListForCreateUpdate();
            return View(category.Data);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(UpdateCategoryRequest request, IFormFile image)
        {
            if (image != null && image.Length > 0)
                request.ImageUrl = ImageHelper.ConvertImageToBase64(image);
            await _categoryService.UpdateAsync(request);
            return LocalRedirect("/category");
        }

        [Route("active")]
        public async Task<IActionResult> Active(int id)
        {
            var result = await _categoryService.Active(id);
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
            var result = await _categoryService.Delete(id);
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
            var category = await _categoryService.GetAsync(id);
            return View(category);
        }

        [HttpGet("activemodal")]
        public IActionResult ActivePartial(string id, string status)
        {
            ViewData["id"] = id;
            ViewData["status"] = status;
            ViewData["action"] = "active";
            ViewData["controller"] = "category";
            return PartialView("_parttialActiveModal");
        }


        [HttpGet("deletemodal")]
        public IActionResult DeletePartial(string id, string name)
        {
            ViewData["id"] = id;
            ViewData["name"] = name;
            ViewData["action"] = "delete";
            ViewData["controller"] = "category";
            return PartialView("_partialDeleteModal");
        }

        [HttpGet("detailmodal")]
        public async Task<IActionResult> DetailPartial(string id)
        {
            var req = await _categoryService.GetAsync(Convert.ToInt32(id));
            return PartialView("_patialDetailModal", req.Data);
        }
    }
}
