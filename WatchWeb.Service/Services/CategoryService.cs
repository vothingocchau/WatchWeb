using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WatchWeb.Common.Constant;
using WatchWeb.Common.Enums;
using WatchWeb.Model.EFContext;
using WatchWeb.Model.Entities;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Category;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public CategoryService(DataContext dataContext, IMapper mapper,
            IUploadService uploadService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<BaseResponse<string>> CreateAsync(CreateCategoryRequest request)
        {
            var result = new BaseResponse<string>();
            var category = _mapper.Map<Category>(request);
            await _dataContext.Category.AddAsync(category);
            await _dataContext.SaveChangesAsync();
            await _uploadService.UploadImageAsync(request.ImageUrl, DocumentTypeConstant.CATEGORY, category.Id);

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<PaginationCategoryReponse> GetAllAsync(BasePaginationRequest request)
        {
            var query = _dataContext.Category.Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request.KeyWord))
                query = query.Where(x => x.Name.Contains(request.KeyWord));

            var totalCount = await query.CountAsync();
            var data = await query.ToListAsync();
            var outputs = data.Select(c => new CategorySimpleDto
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status,
                ParentId = c.ParentId,
                Parent = c.ParentId.HasValue ? _mapper.Map<CategorySimpleDto>(data.Where(x => x.Id == c.ParentId).FirstOrDefault()) : null,
            }).Skip((request.PageIndex * request.PageSize) - request.PageSize)
                                    .Take(request.PageSize).ToList();

            await _uploadService.AppendFileUrls<CategorySimpleDto, DocumentOutputDto>(DocumentTypeConstant.CATEGORY,
                x => x.Id,
                (x, files) => x.ImageUrl = files.Select(x => x.Url).FirstOrDefault(),
                outputs.ToArray());

            return new PaginationCategoryReponse
            {
                TotalCount = totalCount,
                Data = outputs,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        public async Task<List<CategoryParent>> GetListForCreateUpdate()
        {
            var output = await _dataContext.Category.Where(x => !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<CategoryParent>>(output);
        }

        public async Task<List<CategorySimpleDto>> GetListForCreateUpdateProduct()
        {
            var output = await _dataContext.Category.Where(x => !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<CategorySimpleDto>>(output);
        }

        public async Task<BaseResponse<UpdateCategoryRequest>> GetDetailForUpdateAsync(int id)
        {
            var result = new BaseResponse<UpdateCategoryRequest>();
            var query = await _dataContext.Category.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, new UpdateCategoryRequest());

            var output = _mapper.Map<UpdateCategoryRequest>(query);

            await _uploadService.AppendFileUrl<UpdateCategoryRequest, DocumentOutputDto>(DocumentTypeConstant.CATEGORY,
               x => x.Id,
               (x, file) => x.ImageUrl = file?.Url,
               output);

            return result.Success(MessageResponseConstant.SUCCESSFULLY, output);
        }

        public async Task<BaseResponse<CategorySimpleDto>> GetAsync(int id)
        {
            var query = await _dataContext.Category.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return new BaseResponse<CategorySimpleDto>().Failed(MessageResponseConstant.NOTFOUND, null);

            var output = _mapper.Map<CategorySimpleDto>(query);
            await _uploadService.AppendFileUrl<CategorySimpleDto, DocumentOutputDto>(DocumentTypeConstant.CATEGORY,
             x => x.Id,
             (x, file) => x.ImageUrl = file?.Url,
             output);

            return new BaseResponse<CategorySimpleDto>().Success(MessageResponseConstant.SUCCESSFULLY, output);
        }

        public async Task<BaseResponse<string>> UpdateAsync(UpdateCategoryRequest request)
        {
            var result = new BaseResponse<string>();
            var category = _mapper.Map<Category>(request);
            _dataContext.Category.Update(category);
            await _dataContext.SaveChangesAsync();
            if (request.ImageUrl != null)
            {
                await _uploadService.RemoveOldImageAsync(category.Id, DocumentTypeConstant.CATEGORY);
                await _uploadService.UploadImageAsync(request.ImageUrl, DocumentTypeConstant.CATEGORY, category.Id);
            }
            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }


        private List<CategorySimpleDto> GetChildrens(List<Category> categories, long parentId)
        {
            return categories.Where(x => x.ParentId == parentId).Select(c => new CategorySimpleDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                Childrens = GetChildrens(categories, c.Id)
            }).ToList();
        }

        public async Task<BaseResponse<string>> Active(int id)
        {
            var result = new BaseResponse<string>();
            var query = await _dataContext.Category.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            if (query.Status == (int)CategoryEnumStatus.INACTIVE)
                query.Status = (int)CategoryEnumStatus.ACTIVE;
            else query.Status = (int)CategoryEnumStatus.INACTIVE;
            _dataContext.Category.Update(query);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<BaseResponse<string>> Delete(int id)
        {
            var result = new BaseResponse<string>();
            var query = await _dataContext.Category.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            query.IsDeleted = true;
            _dataContext.Category.Update(query);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }
    }
}
