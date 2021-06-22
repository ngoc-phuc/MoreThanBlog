using System.Threading.Tasks;
using Abstraction.Service.UserService;
using Core.Model.User;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MoreThanBlog.Controllers
{
    [Route(Endpoint)]
    public class UserController : BaseController
    {
        private const string Endpoint = "user";

        private const string Login = "login";

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(Login)]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, "Result", typeof(JwtTokenResultModel))]
        public async Task<IActionResult> CreateUserAsync([FromBody] LoginModel model)
        {
            var rs = await _userService.LoginAsync(model);
            return Ok(rs);
        }
    }
}