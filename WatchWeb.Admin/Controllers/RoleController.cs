using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Roles;

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

            return LocalRedirect("/role");
        }


        [HttpGet("update")]
        public async Task<IActionResult> Update(int id)
        {
            var roleDetail = await _roleService.GetDetailAsync(id);
            var output = _mapper.Map<UpdateRoleRequest>(roleDetail.Data);
            return View(output);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(UpdateRoleRequest request)
        {
            return LocalRedirect("/role");
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail(int id)
        {
            return View();
        }
    }
}
