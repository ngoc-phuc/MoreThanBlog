using System.Threading.Tasks;
using Abstraction.Service.Blog;
using Core.Model.Blog;
using Core.Model.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MoreThanBlog.Controllers
{
    public class BlogController : BaseController
    {
        private const string Endpoint = "blogs";

        private const string Filter = Endpoint;
        private const string Create = Endpoint;
        private const string Update = Endpoint + "/{id}";
        private const string Delete = Endpoint + "/{id}";
        private const string Get = Endpoint + "/{id}";

        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// Filter blogs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(Filter)]
        [SwaggerResponse(StatusCodes.Status201Created, "Result", typeof(PagedResponseModel<BlogModel>))]
        public async Task<IActionResult> FilterAsync([FromQuery] FilterBlogRequestModel model)
        {
            var rs = await _blogService.FilterAsync(model);
            return Ok(rs);
        }

        /// <summary>
        /// Create blog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(Create)]
        [SwaggerResponse(StatusCodes.Status200OK, "Result", typeof(string))]
        public async Task<IActionResult> CreateAsync([FromBody] AddBlogModel model)
        {
            var rs = await _blogService.CreateAsync(model);
            return Ok(rs);
        }

        /// <summary>
        /// Update blog
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut(Update)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] AddBlogModel model)
        {
            await _blogService.UpdateAsync(id, model);
            return NoContent();
        }

        /// <summary>
        /// Delete blog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Delete)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _blogService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get blog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(Get)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var rs = await _blogService.GetAsync(id);
            return Ok(rs);
        }
    }
}