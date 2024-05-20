using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.IServices
{
    public interface IAuthService
    {
        public Task<BaseResponse<AdminLoginDto>> LoginAdmin(LoginRequest model);
        public Task Logout();
        public Task Register();
    }
}
