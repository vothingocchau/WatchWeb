using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Roles;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.IServices
{
    public interface IRoleService
    {
        public Task<BaseResponse<string>> CreateAsync(CreateRoleRequest request);
        public Task<BaseResponse<string>> UpdateAsync(UpdateRoleRequest request);
        public Task<BaseResponse<PageResponse<List<RoleSimpleDto>>>> GetAllAsync(BasePaginationRequest request);
        public Task<BaseResponse<RoleDetailDto>> GetDetailAsync(int id);
    }
}
