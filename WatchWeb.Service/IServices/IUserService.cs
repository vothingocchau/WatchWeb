using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Category;
using WatchWeb.Service.Models.Request.Users;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.IServices
{
    public interface IUserService
    {
        public Task<BaseResponse<CreateUserRequest>> CreateAsync(CreateUserRequest request);
        public Task<BaseResponse<UpdateUserRequest>> UpdateAsync(UpdateUserRequest request);
        public Task<PaginationUserReponse> GetAllAsync(BasePaginationRequest request);
        public Task<BaseResponse<UserDetailDto>> GetAsync(int id);
        public Task<BaseResponse<UpdateUserRequest>> GetDetailForUpdateAsync(int id);
        public Task<BaseResponse<string>> Active(int id);
        public Task<BaseResponse<string>> Delete(int id);
    }
}
