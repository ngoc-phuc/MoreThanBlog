using System.Linq;
using System.Net.Http;
using Abstraction.Service.UserService;
using Core.Constants;
using Core.Errors;
using Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MoreThanBlog.Filter
{
    public class SystemApiAuthFilter : IAuthorizationFilter
    {
        private readonly IUserService _userService;
        private IHttpContextAccessor _httpContextAccessor;

        public SystemApiAuthFilter(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAllowAnonymous = SystemApiAuthHelper.IsAllowAnonymous(context);

            if (isAllowAnonymous)
            {
                return;
            }

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            try
            {
                var userIdClaim =
                    _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserId);

                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    throw new MoreThanBlogException(nameof(ErrorCode.UserNotFound), ErrorCode.UserNotFound);
                }
                var userInfo = _userService.GetUserProfile(userIdClaim.Value);
                LoggedInUser.Current = userInfo;

                if (!userInfo.IsActive)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
            }
            catch (System.Exception ex)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
        }
    }
}