using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WatchWeb.Common.Constant;
using WatchWeb.Model.EFContext;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AuthService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<BaseResponse<AdminLoginDto>> LoginAdmin(LoginRequest model)
        {
            try
            {
                var user = await _dataContext.UserAccount.Include(z => z.UserRole)
                                       .Where(x => x.Name == model.UserName && x.Active &&
                                               x.Password == GetMD5(model.Password)).FirstOrDefaultAsync();

                var output = _mapper.Map<AdminLoginDto>(user);
                if (user == null)
                {
                    return new BaseResponse<AdminLoginDto>().Failed(MessageResponseConstant.LOGIN_FAILED, output);
                }

                return new BaseResponse<AdminLoginDto>().Success(MessageResponseConstant.LOGIN_SUCCESSFULLY, output);
            }
            catch (Exception ex)
            {

                throw;
            }

           
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public Task Register()
        {
            throw new NotImplementedException();
        }

        #region Private Method
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }



        #endregion Private Method
    }
}
