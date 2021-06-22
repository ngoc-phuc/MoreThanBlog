using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Model.File;

namespace Abstraction.Service.Common
{
    public interface IFileService
    {
        Task<FileModel> SaveAsync(UploadFileModel model, CancellationToken cancellationToken = default);

        Task<FileModel> GetAsync(string slug, CancellationToken cancellationToken = default);

        Task<FileModel> GetDetailAsync(string id, CancellationToken cancellationToken = default);

        Task DeleteAsync(string slug, CancellationToken cancellationToken = default);
    }
}
