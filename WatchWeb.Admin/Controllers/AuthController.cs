using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WatchWeb.Admin.MiddleWare;
using WatchWeb.Common.Constant;
using WatchWeb.Model.EFContext;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Request;

namespace WatchWeb.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect("/home");
            }
            if (Request.Query[SeasionConstant.RETURN_URL].Any())
            {
                HttpContext.Session.SetString(SeasionConstant.RETURN_URL, Request.Query[SeasionConstant.RETURN_URL]);
            }

            return View();
        }


        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var output = await _authService.LoginAdmin(model);
            ViewData[ViewDataConstant.RESULT] = output.Message;
            if (output.Data != null)
            {
                if (model.IsRemember)
                {
                    CookieOptions cookie = new CookieOptions();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("RememberMe", model.UserName, cookie);
                }
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(AddClaim(output.Data)));
                if (HttpContext.Session.GetString(SeasionConstant.RETURN_URL) != null)
                {
                    return LocalRedirect(HttpContext.Session.GetString(SeasionConstant.RETURN_URL));
                }
                return LocalRedirect("/home");
            }
          
            return View();
        }


        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/login");
        }


        #region Private Method
        private ClaimsIdentity AddClaim(AdminLoginDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(SeasionConstant.USER_ID,user.UserId.ToString()),
                new Claim(SeasionConstant.USER_NAME,user.Name.ToString())
            };
            foreach(var role in user.RoleIds)
            {
               claims.Add(new Claim(SeasionConstant.ROLE_ID, role.ToString()));
            }
            return new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
        }
        #endregion Private Method
    }
}
