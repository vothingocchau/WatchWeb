using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WatchWeb.Common.Constant;
using WatchWeb.Common.Enums;
using WatchWeb.Model.EFContext;
using WatchWeb.Model.Entities;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Products;
using WatchWeb.Service.Models.Request.Users;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IUploadService _uploadService;

        public ProductService(
            IMapper mapper, DataContext dataContext, IUploadService uploadService)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _uploadService = uploadService;
        }


        public async Task<BaseResponse<CreateProductRequest>> CreateAsync(CreateProductRequest request)
        {
            var result = new BaseResponse<CreateProductRequest>();
            var product = _mapper.Map<Product>(request);
            await _dataContext.Product.AddAsync(product);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, request);
        }

        public async Task<PaginationProductResponse> GetAllAsync(BasePaginationRequest request)
        {
            try
            {
                var query = _dataContext.Product.Where(x => !x.IsDeleted).Skip((request.PageIndex * request.PageSize) - request.PageSize)
                                           .Take(request.PageSize);

                if (!string.IsNullOrEmpty(request.KeyWord))
                    query = query.Where(x => x.Name.Contains(request.KeyWord));

                var totalCount = await query.CountAsync();
                var outputs = _mapper.Map<List<ProductSimpleDto>>(await query.ToListAsync());

                return new PaginationProductResponse
                {
                    TotalCount = totalCount,
                    Data = outputs,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public async Task<BaseResponse<ProductDetailDto>> GetDetailAsync(int id)
        {
            var query = await _dataContext.Product.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return new BaseResponse<ProductDetailDto>().Failed(MessageResponseConstant.NOTFOUND, null);

            var output = _mapper.Map<ProductDetailDto>(query);
            await _uploadService.AppendFileUrl<ProductDetailDto, DocumentOutputDto>(DocumentTypeConstant.PRODUCT,
               x => x.Id,
               (x, file) => x.ImageUrl = file?.Url,
               output);

            return new BaseResponse<ProductDetailDto>().Success(MessageResponseConstant.SUCCESSFULLY, output);
        }

        public async Task<BaseResponse<UpdateProductRequest>> UpdateAsync(UpdateProductRequest request)
        {
            var result = new BaseResponse<UpdateProductRequest>();

            var entity = await _dataContext.Product.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync();
            if (entity == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, request);

            var product = _mapper.Map<Product>(request);
            _dataContext.Product.Update(product);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, request);
        }


        public async Task<BaseResponse<string>> Active(int id)
        {
            var result = new BaseResponse<string>();
            var query = await _dataContext.Product.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            if (query.Status == (int)CategoryEnumStatus.INACTIVE)
                query.Status = (int)CategoryEnumStatus.ACTIVE;
            else query.Status = (int)CategoryEnumStatus.INACTIVE;
            _dataContext.Product.Update(query);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<BaseResponse<string>> Delete(int id)
        {
            var result = new BaseResponse<string>();
            var query = await _dataContext.Product.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            query.IsDeleted = true;
            _dataContext.Product.Update(query);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<BaseResponse<UpdateProductRequest>> GetDetailForUpdateAsync(int id)
        {
            var result = new BaseResponse<UpdateProductRequest>();
            var query = await _dataContext.Product.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, new UpdateProductRequest());

            var output = _mapper.Map<UpdateProductRequest>(query);
            output.CategoryId = await _dataContext.ProductCategory.Where(x => x.ProductId == id).Select(x => x.CategoryId).ToListAsync();

            await _uploadService.AppendFileUrl<UpdateProductRequest, DocumentOutputDto>(DocumentTypeConstant.PRODUCT,
               x => x.Id,
               (x, file) => x.ImageUrl = file?.Url,
               output);

            return result.Success(MessageResponseConstant.SUCCESSFULLY, output);
        }
    }
}
