using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WatchWeb.Common.Constant;
using WatchWeb.Model.EFContext;
using WatchWeb.Model.Entities;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Request.Category;
using WatchWeb.Service.Models.Request.Users;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public UserService(DataContext dataContext, IMapper mapper, IUploadService uploadService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<BaseResponse<string>> Active(int id)
        {
            var result = new BaseResponse<string>();
            var query = await _dataContext.UserAccount.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            if (query.Active)
                query.Active = false;
            else query.Active = true;
            _dataContext.UserAccount.Update(query);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<BaseResponse<CreateUserRequest>> CreateAsync(CreateUserRequest request)
        {
            try
            {
                var result = new BaseResponse<CreateUserRequest>();
                var user = _mapper.Map<UserAccount>(request);
                user.TypeAccount = UserTypeAccountConstant.STAFF;

                var checkCreate = await ValidateCreateUser(request);
                if (!checkCreate.IsSuccess)
                {
                    return result.Failed(checkCreate.Message, request);
                }

                await _dataContext.UserAccount.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                await _uploadService.UploadImageAsync(request.ImageUrl, DocumentTypeConstant.USER, user.Id);

                var userRole = new List<UserRole>();
                request.ListRoleIds.ForEach(x =>
                {
                    userRole.Add(new UserRole { RoleId = x, UserAccountId = user.Id });
                });

                await _dataContext.UserRole.AddRangeAsync(userRole);
                await _dataContext.SaveChangesAsync();

                return result.Success(MessageResponseConstant.SUCCESSFULLY, request);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<BaseResponse<string>> Delete(int id)
        {
            var result = new BaseResponse<string>();
            var query = await _dataContext.UserAccount.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);

            query.IsDeleted = true;
            _dataContext.UserAccount.Update(query);
            await _dataContext.SaveChangesAsync();

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        public async Task<PaginationUserReponse> GetAllAsync(BasePaginationRequest request)
        {
            var query = _dataContext.UserAccount.Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request.KeyWord))
                query = query.Where(x => x.Name.Contains(request.KeyWord));

            var totalCount = await query.CountAsync();
            var data = await query.Skip((request.PageIndex * request.PageSize) - request.PageSize)
                                    .Take(request.PageSize).ToListAsync();
            var outputs = _mapper.Map<List<UserSimpleDto>>(data);

            await _uploadService.AppendFileUrls<UserSimpleDto, DocumentOutputDto>(DocumentTypeConstant.USER,
                x => x.Id,
                (x, files) => x.ImageUrl = files.Select(x => x.Url).FirstOrDefault(),
                outputs.ToArray());

            return new PaginationUserReponse
            {
                TotalCount = totalCount,
                Data = outputs,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        public async Task<BaseResponse<UserDetailDto>> GetAsync(int id)
        {
            var query = await _dataContext.UserAccount.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return new BaseResponse<UserDetailDto>().Failed(MessageResponseConstant.NOTFOUND, null);

            var output = _mapper.Map<UserDetailDto>(query);
            await _uploadService.AppendFileUrl<UserSimpleDto, DocumentOutputDto>(DocumentTypeConstant.USER,
             x => x.Id,
             (x, file) => x.ImageUrl = file?.Url,
             output);

            output.Roles = await _dataContext.UserRole.Include(x=>x.Role).Where(x=>x.UserAccountId == id).Select(z=>z.Role.Name).ToListAsync();

            return new BaseResponse<UserDetailDto>().Success(MessageResponseConstant.SUCCESSFULLY, output);
        }

        public async Task<BaseResponse<UpdateUserRequest>> GetDetailForUpdateAsync(int id)
        {
            var result = new BaseResponse<UpdateUserRequest>();
            var query = await _dataContext.UserAccount.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (query == null)
                return result.Failed(MessageResponseConstant.NOTFOUND, new UpdateUserRequest());

            var output = _mapper.Map<UpdateUserRequest>(query);
            output.ListRoleIds = await _dataContext.UserRole.Where(x => x.UserAccountId == id).Select(x => x.RoleId).ToListAsync();

            await _uploadService.AppendFileUrl<UpdateUserRequest, DocumentOutputDto>(DocumentTypeConstant.USER,
               x => x.Id,
               (x, file) => x.ImageUrl = file?.Url,
               output);

            return result.Success(MessageResponseConstant.SUCCESSFULLY, output);
        }

        public async Task<BaseResponse<UpdateUserRequest>> UpdateAsync(UpdateUserRequest request)
        {
            try
            {
                var result = new BaseResponse<UpdateUserRequest>();
                var entity = await _dataContext.UserAccount.Include(x => x.UserRole).Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync();
                if (entity == null)
                    return result.Failed(MessageResponseConstant.NOTFOUND, request);


                var checkUpdate = await ValidateUpdateUser(request);
                if (checkUpdate.IsSuccess)
                {
                    return result.Failed(checkUpdate.Message, request);
                }

                entity.Name = request.Name;
                entity.Email = request.Email;
                entity.Phone = request.Phone;
                entity.Active = request.Active;
                entity.Gender = request.Gender;
                _dataContext.UserAccount.Update(entity);
                await _dataContext.SaveChangesAsync();
                if (request.ImageUrl != null)
                {
                    await _uploadService.RemoveOldImageAsync(entity.Id, DocumentTypeConstant.USER);
                    await _uploadService.UploadImageAsync(request.ImageUrl, DocumentTypeConstant.USER, entity.Id);
                }

                var currentRole = await _dataContext.UserRole.Where(x => x.UserAccountId == entity.Id)
                                             .Select(z => z.RoleId).ToListAsync();

                var newRoleIds = request.ListRoleIds.Except(currentRole).ToList();
                var roleIdsToDelete = currentRole.Except(request.ListRoleIds).ToList();

                foreach (var roleId in roleIdsToDelete)
                {
                    var rolePermissionToDelete = entity.UserRole.FirstOrDefault(rp => rp.RoleId == roleId);
                    if (rolePermissionToDelete != null)
                    {
                        _dataContext.UserRole.Remove(rolePermissionToDelete);
                    }
                }

                foreach (var roleId in newRoleIds)
                {
                    _dataContext.UserRole.Add(new UserRole { RoleId = roleId, UserAccountId = entity.Id });
                }
                return result.Success(MessageResponseConstant.SUCCESSFULLY, request);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }


        private async Task<BaseResponse<bool>> ValidateCreateUser(CreateUserRequest request)
        {
            var result = new BaseResponse<bool>();
            var checkUserExist = await _dataContext.UserAccount.Where(x => x.UserName == request.UserName && !x.IsDeleted).FirstOrDefaultAsync();
            if (checkUserExist != null)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_USERNAME_EXIST, false);
            }

            var checkEmailExist = await _dataContext.UserAccount.Where(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();
            if (checkEmailExist != null)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_EMAIL_EXIST, false);
            }


            var checkPhoneExist = await _dataContext.UserAccount.Where(x => x.Phone == request.Phone && !x.IsDeleted).FirstOrDefaultAsync();
            if (checkPhoneExist != null)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_PHONE_EXIST, false);
            }

            return result.Success(MessageResponseConstant.SUCCESSFULLY, true);
        }

        private async Task<BaseResponse<bool>> ValidateUpdateUser(UpdateUserRequest request)
        {
            var result = new BaseResponse<bool>();

            var checkEmailExist = await _dataContext.UserAccount.Where(x => x.Email == request.Email && x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync();
            if (checkEmailExist != null)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_EMAIL_EXIST, false);
            }


            var checkPhoneExist = await _dataContext.UserAccount.Where(x => x.Phone == request.Phone && x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync();
            if (checkPhoneExist != null)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_PHONE_EXIST, false);
            }

            return result.Success(MessageResponseConstant.SUCCESSFULLY, true);
        }
    }
}
