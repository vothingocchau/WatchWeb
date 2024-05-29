using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Products;
using WatchWeb.Service.Models.Request.Users;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.IServices
{
    public interface IProductService
    {
        public Task<BaseResponse<CreateProductRequest>> CreateAsync(CreateProductRequest request);
        public Task<BaseResponse<UpdateProductRequest>> UpdateAsync(UpdateProductRequest request);
        public Task<PaginationProductResponse> GetAllAsync(BasePaginationRequest request);
        public Task<PaginationProductResponse> GetAllForCustomer(BasePaginationRequest request);
        public Task<BaseResponse<ProductDetailDto>> GetDetailAsync(int id);
        public Task<BaseResponse<string>> Active(int id);
        public Task<BaseResponse<string>> Delete(int id);
        public Task<BaseResponse<UpdateProductRequest>> GetDetailForUpdateAsync(int id);
		public Task<List<ProductSimpleDto>> GetFeatureProduct();
        public Task<ProductDetailDto> GetDetailForUserAsync(int id);
    }
}
