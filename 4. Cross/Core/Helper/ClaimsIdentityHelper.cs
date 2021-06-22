using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;

namespace Core.Helper
{
    public static class ClaimsIdentityHelper
    {
        public static string GetUserId(this IIdentity identity)
        {
            var userIdClaim = (identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == Constants.ClaimTypes.UserId);
            if (string.IsNullOrEmpty(userIdClaim?.Value))
            {
                throw new AuthenticationException();
            }

            return userIdClaim.Value;
        }

        public static string GetUserName(this IIdentity identity)
        {
            return (identity as ClaimsIdentity)?.Claims.FirstOrDefault(c => c.Type == Constants.ClaimTypes.UserName)?.Value;
        }
    }
}
