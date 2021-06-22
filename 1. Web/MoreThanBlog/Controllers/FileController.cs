using System.Threading.Tasks;
using Abstraction.Service.Common;
using Core.Model.Blog;
using Core.Model.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MoreThanBlog.Controllers
{
    public class FileController : BaseController
    {
        private const string Endpoint = "files";

        private const string Upload = Endpoint;

        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Create blog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(Upload)]
        [SwaggerResponse(StatusCodes.Status200OK, "Result", typeof(FileModel))]
        public async Task<IActionResult> CreateAsync([FromForm] UploadFileModel model)
        {
            var rs = await _fileService.SaveAsync(model);
            return Ok(rs);
        }
    }
}