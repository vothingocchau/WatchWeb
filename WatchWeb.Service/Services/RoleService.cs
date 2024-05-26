using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using WatchWeb.Common.Constant;
using WatchWeb.Model.EFContext;
using WatchWeb.Model.Entities;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Roles;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;


        public RoleService(DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<BaseResponse<string>> CreateAsync(CreateRoleRequest request)
        {
            var entity = _mapper.Map<Role>(request);
            await _dataContext.Role.AddAsync(entity);

            if (request.Permission.Any())
            {
                foreach (var permissionId in request.Permission)
                {
                    _dataContext.RolePermission.Add(new RolePermission { RoleId = entity.Id, PermissionId = permissionId });
                }
            }

            await _dataContext.SaveChangesAsync();
            return new BaseResponse<string>().Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<BaseResponse<PageResponse<List<RoleSimpleDto>>>> GetAllAsync(BasePaginationRequest request)
        {
            try
            {
                var query = _dataContext.Role.Skip((request.PageIndex * request.PageSize) - request.PageSize)
                                             .Take(request.PageSize);

                if (!string.IsNullOrEmpty(request.KeyWord))
                    query = query.Where(x => x.Name.Contains(request.KeyWord));

                var totalCount = await query.CountAsync();
                var outputs = _mapper.Map<List<RoleSimpleDto>>(await query.ToListAsync());

                return new BaseResponse<PageResponse<List<RoleSimpleDto>>>().Success(MessageResponseConstant.SUCCESSFULLY, new PageResponse<List<RoleSimpleDto>>
                {
                    TotalCount = totalCount,
                    Data = outputs,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                });
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<RoleSimpleDto>> GetAllForCreateUser()
        {
            var query = _dataContext.Role.Where(_ => true);
            var outputs = _mapper.Map<List<RoleSimpleDto>>(await query.ToListAsync());

            return outputs;
        }

        public async Task<BaseResponse<RoleDetailDto>> GetDetailAsync(int id)
        {
            var query = await _dataContext.Role.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (query == null)
                return new BaseResponse<RoleDetailDto>().Failed(MessageResponseConstant.NOTFOUND, null);

            var output = _mapper.Map<RoleDetailDto>(query);
            output.Permission = await _dataContext.RolePermission.Where(x => x.RoleId == id).Select(x => x.PermissionId).ToListAsync();

            return new BaseResponse<RoleDetailDto>().Success(MessageResponseConstant.SUCCESSFULLY, output);
        }

        public async Task<BaseResponse<string>> UpdateAsync(UpdateRoleRequest request)
        {
            var entity = await _dataContext.Role.Include(x => x.RolePermission).Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (entity == null)
                return new BaseResponse<string>().Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            entity.Name = request.Name;
            entity.Status = request.Status;
            var currentPermission = await _dataContext.RolePermission.Where(x => x.RoleId == entity.Id)
                                                                     .Select(z => z.PermissionId).ToListAsync();

            var newPermissionIds = request.Permission.Except(currentPermission).ToList();
            var permissionIdsToDelete = currentPermission.Except(request.Permission).ToList();

            foreach (var permissionId in permissionIdsToDelete)
            {
                var rolePermissionToDelete = entity.RolePermission.FirstOrDefault(rp => rp.PermissionId == permissionId);
                if (rolePermissionToDelete != null)
                {
                    _dataContext.RolePermission.Remove(rolePermissionToDelete);
                }
            }

            foreach (var permissionId in newPermissionIds)
            {
                _dataContext.RolePermission.Add(new RolePermission { RoleId = entity.Id, PermissionId = permissionId });
            }

            _dataContext.Role.Update(entity);
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<string>().Success(MessageResponseConstant.SUCCESSFULLY, entity.Name);
        }
    }
}
