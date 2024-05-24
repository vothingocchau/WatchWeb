using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WatchWeb.Common.Constant;
using WatchWeb.Model.EFContext;
using WatchWeb.Model.Entities;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Products;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;


        public ProductService(
            IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }


        public async Task<BaseResponse<string>> CreateAsync(CreateProductRequest request)
        {
            var result = new BaseResponse<string>();
            var product = _mapper.Map<Product>(request);
            await _dataContext.Product.AddAsync(product);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<BaseResponse<PageResponse<List<ProductSimpleDto>>>> GetAllAsync(BasePaginationRequest request)
        {
            var query = _dataContext.Product.Skip((request.PageIndex * request.PageSize) - request.PageSize)
                                            .Take(request.PageSize);

            if (!string.IsNullOrEmpty(request.KeyWord))
                query = query.Where(x => x.Name.Contains(request.KeyWord));

            var totalCount = await query.CountAsync();
            var outputs = _mapper.Map<List<ProductSimpleDto>>(await query.ToListAsync());

            return new BaseResponse<PageResponse<List<ProductSimpleDto>>>().Success(MessageResponseConstant.SUCCESSFULLY, new PageResponse<List<ProductSimpleDto>>
            {
                TotalCount = totalCount,
                Data = outputs,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            });
        }

        public async Task<BaseResponse<ProductDetailDto>> GetDetailAsync(int id)
        {
            var query = await _dataContext.Product.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (query == null)
                return new BaseResponse<ProductDetailDto>().Failed(MessageResponseConstant.NOTFOUND, null);

            var output = _mapper.Map<ProductDetailDto>(query);

            return new BaseResponse<ProductDetailDto>().Success(MessageResponseConstant.SUCCESSFULLY, output);
        }

        public async Task<BaseResponse<string>> UpdateAsync(UpdateProductRequest request)
        {
            var result = new BaseResponse<string>();

            var entity = await _dataContext.Role.Include(x => x.RolePermission).Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (entity == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            var product = _mapper.Map<Product>(request);
            _dataContext.Product.Update(product);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }
    }
}
