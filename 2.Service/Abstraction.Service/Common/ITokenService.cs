using System.Security.Claims;
using System.Threading.Tasks;
using Core.Utils;

namespace Abstraction.Service.Common
{
    public interface ITokenService
    {
        Task<JwtTokenResultModel> RequestTokenAsync(ClaimsIdentity identity);
    }
}