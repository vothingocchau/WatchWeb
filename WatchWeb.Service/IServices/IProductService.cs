using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Products;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.IServices
{
    public interface IProductService
    {
        public Task<BaseResponse<string>> CreateAsync(CreateProductRequest request);
        public Task<BaseResponse<string>> UpdateAsync(UpdateProductRequest request);
        public Task<BaseResponse<PageResponse<List<ProductSimpleDto>>>> GetAllAsync(BasePaginationRequest request);
        public Task<BaseResponse<ProductDetailDto>> GetDetailAsync(int id);
    }
}
