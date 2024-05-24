using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Category;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.IServices
{
    public interface ICategoryService
    {
        public Task<BaseResponse<string>> CreateAsync(CreateCategoryRequest request);
        public Task<BaseResponse<string>> UpdateAsync(UpdateCategoryRequest request);
        public Task<PaginationCategoryReponse> GetAllAsync(BasePaginationRequest request);
        public Task<BaseResponse<CategorySimpleDto>> GetAsync(int id);
        public Task<List<CategoryParent>> GetListForCreateUpdate();
        public Task<BaseResponse<UpdateCategoryRequest>> GetDetailForUpdateAsync(int id);
        public Task<BaseResponse<string>> Active(int id); 
        public Task<BaseResponse<string>> Delete(int id); 
    }
}
