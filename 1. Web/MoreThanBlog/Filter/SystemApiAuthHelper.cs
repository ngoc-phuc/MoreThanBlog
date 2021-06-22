using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MoreThanBlog.Filter
{
    public static class SystemApiAuthHelper
    {
        public static bool IsAllowAnonymous(AuthorizationFilterContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                return true;
            }

            if (IsAllowAnonymous(controllerActionDescriptor))
            {
                return true;
            }

            return false;
        }

        private static bool IsAllowAnonymous(ControllerActionDescriptor controllerActionDescriptor)
        {
            // If action have any AllowAnonymousAttribute => Allow Anonymous
            bool isActionAllowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
            if (isActionAllowAnonymous) return true;

            bool isControllerAllowAnonymous = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
            if (isControllerAllowAnonymous) return true;

            return false;
        }
    }
}