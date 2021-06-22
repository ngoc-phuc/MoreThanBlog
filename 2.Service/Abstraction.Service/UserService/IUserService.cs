using Core.Model.User;
using System.Threading;
using System.Threading.Tasks;
using Core.Utils;

namespace Abstraction.Service.UserService
{
    public interface IUserService
    {
        Task<JwtTokenResultModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);

        Task InitAdminAccountAsync(CancellationToken cancellation = default);

        LoggedInUser GetUserProfile(string userId);
    }
}