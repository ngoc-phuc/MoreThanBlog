using Microsoft.AspNetCore.Mvc;
using MoreThanBlog.Filter;

namespace MoreThanBlog.Controllers
{
    [ModelValidationFilter]
    [ServiceFilter(typeof(SystemApiAuthFilter))]
    public class BaseController : ControllerBase
    {
    }
}