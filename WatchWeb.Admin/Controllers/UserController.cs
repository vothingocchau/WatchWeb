using Microsoft.AspNetCore.Mvc;
using WatchWeb.Common.Constant;
using WatchWeb.Common.Helper;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Users;

namespace WatchWeb.Admin.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(BasePaginationRequest request)
        {
            var user = await _userService.GetAllAsync(request);
            return View(user);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var req = new CreateUserRequest();
            req.UserRoles = await _roleService.GetAllForCreateUser();
            return View(req);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserRequest request, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                    request.ImageUrl = ImageHelper.ConvertImageToBase64(image);
                var result = await _userService.CreateAsync(request);
                result.Data.UserRoles = await _roleService.GetAllForCreateUser();
                ViewData[ViewDataConstant.RESULT] = result.Message;
                if (result.IsSuccess)
                    return LocalRedirect("/user");
                return View(result.Data);
            }
            var req = new CreateUserRequest();
            req.UserRoles = await _roleService.GetAllForCreateUser();
            return View(req);
        }

        [HttpGet("update")]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userService.GetDetailForUpdateAsync(id);
            user.Data.UserRoles = await _roleService.GetAllForCreateUser();
            return View(user.Data);
        }

        [HttpPost("update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateUserRequest request, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                    request.ImageUrl = ImageHelper.ConvertImageToBase64(image);
                var result = await _userService.UpdateAsync(request);
                result.Data.UserRoles = await _roleService.GetAllForCreateUser();
                ViewData[ViewDataConstant.RESULT] = result.Message;
                if (result.IsSuccess)
                    return LocalRedirect("/user");
                return View(result.Data);
            }
            var req = new UpdateUserRequest();
            req.UserRoles = await _roleService.GetAllForCreateUser();
            return View(req);
        }

        [Route("active")]
        public async Task<IActionResult> Active(int id)
        {
            var result = await _userService.Active(id);
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
            var result = await _userService.Delete(id);
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
            var category = await _userService.GetAsync(id);
            return View(category);
        }

        [HttpGet("activemodal")]
        public IActionResult ActivePartial(string id, string status)
        {
            ViewData["id"] = id;
            ViewData["status"] = status;
            ViewData["action"] = "active";
            ViewData["controller"] = "category";
            return PartialView("_parttialActiveUserModal");
        }


        [HttpGet("deletemodal")]
        public IActionResult DeletePartial(string id, string name)
        {
            ViewData["id"] = id;
            ViewData["name"] = name;
            ViewData["action"] = "delete";
            ViewData["controller"] = "category";
            return PartialView("_partialDeleteUserModal");
        }

        [HttpGet("detailmodal")]
        public async Task<IActionResult> DetailPartial(string id)
        {
            var req = await _userService.GetAsync(Convert.ToInt32(id));
            return PartialView("_partialDetailUserModal", req.Data);
        }
    }
}
