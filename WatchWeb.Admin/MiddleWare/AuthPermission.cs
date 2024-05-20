using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WatchWeb.Common.Constant;
using WatchWeb.Common.Helpers;
using WatchWeb.Model;

namespace WatchWeb.Admin.MiddleWare
{
    public class AuthPermission : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public AuthPermission(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var dbContext = DbContextHelper.GetDbContext(serviceProvider);
            var httpContext = context.HttpContext;
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            var claimsIdentity = (ClaimsIdentity)httpContext.User.Identity;
            var claims = claimsIdentity.Claims;
            var userId = claims.Where(x => x.Type == SeasionConstant.USER_ID).Select(c => c.Value).FirstOrDefault();
            var roleIds = claims.Where(x => x.Type == SeasionConstant.ROLE_ID).Select(c => c.Value).ToList();
            var isPassed = dbContext.UserRole.Where(x => roleIds.Select(int.Parse).Contains(x.RoleId) && x.UserAccountId == Convert.ToInt32(userId)).FirstOrDefault();
            if (isPassed == null)
            {
                context.Result = new ViewResult { ViewName = "NotFound" };
                return;
            }

            var listPermission = dbContext.RolePermission
                .Include(x=>x.Permission)
                .Where(x => roleIds.Select(int.Parse).Contains(x.RoleId))
                .Select(z=>z.Permission).ToList();
            if (listPermission.Any(x => x.Name == PermissionSeed.Admin)) return;

            if (listPermission.Any(x => x.Name == _requiredRole))
            {
                context.Result = new ViewResult { ViewName = "NotFound" };
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
