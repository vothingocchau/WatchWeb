using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using WatchWeb.Common.Constant;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Roles;
using WatchWeb.Service.Services;

namespace WatchWeb.Admin.Controllers
{
    [Route("role")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService,
            IMapper mapper) 
        {
            _roleService = roleService;
            _mapper = mapper;
        
        }

        public async Task<IActionResult> Index(BasePaginationRequest request)
        {
            var outputs = await _roleService.GetAllAsync(request);
            return View(outputs.Data.Data);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var request = new CreateRoleRequest();
            request.Permissions = await _roleService.GetAllPermission();
            return View(request);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateRoleRequest request)
        {
            var result = await _roleService.CreateAsync(request);
            ViewData[ViewDataConstant.RESULT] = result.Message;
            if (result.IsSuccess)
            {
                return LocalRedirect("/role");
            }
            return View();
        }


        [HttpGet("update")]
        public async Task<IActionResult> Update(int id)
        {
            var roleDetail = await _roleService.GetDetailAsync(id);
            var output = _mapper.Map<UpdateRoleRequest>(roleDetail.Data);
            output.Permissions = await _roleService.GetAllPermission();
            return View(output);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(UpdateRoleRequest request)
        {
            var result = await _roleService.UpdateAsync(request);
            ViewData[ViewDataConstant.RESULT] = result.Message;
            if (result.IsSuccess)
            {
                return LocalRedirect("/role");
            }
            return View();
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var roleDetail = await _roleService.GetDetailAsync(id);
            return View(roleDetail.Data);
        }

        [HttpGet("detailmodal")]
        public async Task<IActionResult> DetailPartial(string id)
        {
            var roleDetail = await _roleService.GetDetailAsync(Convert.ToInt32(id));
            roleDetail.Data.Permissions = await _roleService.GetAllPermission();
            return PartialView("_partialDetailPermissionModal", roleDetail.Data);
        }
    }
}
