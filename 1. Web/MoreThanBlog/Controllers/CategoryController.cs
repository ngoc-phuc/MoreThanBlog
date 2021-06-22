using System.Threading.Tasks;
using Abstraction.Service.CategoryService;
using Core.Model.Category;
using Core.Model.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MoreThanBlog.Controllers
{
    public class CategoryController : BaseController
    {
        private const string Endpoint = "categories";

        private const string Filter = Endpoint;
        private const string Create = Endpoint;
        private const string Update = Endpoint + "/{id}";
        private const string Delete = Endpoint + "/{id}";
        private const string Get = Endpoint + "/{id}";

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Filter categories
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(Filter)]
        [SwaggerResponse(StatusCodes.Status201Created, "Result", typeof(PagedResponseModel<CategoryModel>))]
        public async Task<IActionResult> FilterAsync([FromQuery] FilterCategoryRequestModel model)
        {
            var rs = await _categoryService.FilterAsync(model);
            return Ok(rs);
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(Create)]
        [SwaggerResponse(StatusCodes.Status200OK, "Result", typeof(string))]
        public async Task<IActionResult> CreateAsync([FromBody] AddCategoryModel model)
        {
            var rs = await _categoryService.CreateAsync(model);
            return Ok(rs);
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut(Update)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] AddCategoryModel model)
        {
            await _categoryService.UpdateAsync(id, model);
            return NoContent();
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Delete)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(Get)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var rs = await _categoryService.GetAsync(id);
            return Ok(rs);
        }
    }
}