using Microsoft.AspNetCore.Http;

namespace Core.Model.File
{
    public class UploadFileModel
    {
        public IFormFile File { get; set; }

        public string Folder { get; set; }
    }
}