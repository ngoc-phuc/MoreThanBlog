using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Repository;
using Abstraction.Repository.Model;
using Abstraction.Service.Common;
using AutoMapper;
using Core.Errors;
using Core.Helper;
using Core.Model.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Service.Common
{
    public class FileService : BaseService.Service, IFileService
    {
        private readonly IRepository<FileEntity> _fileRepository;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IWebHostEnvironment hostingEnvironment) : base(unitOfWork, mapper)
        {
            _fileRepository = unitOfWork.GetRepository<FileEntity>();

            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<FileModel> SaveAsync(UploadFileModel model, CancellationToken cancellationToken = default)
        {
            //create directory
            var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "image", model.Folder, DateTimeOffset.UtcNow.ToString("yyyyMMdd"), UniqueId.CreateRandomId());
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            //store file
            var filePath = Path.Combine(directoryPath, Path.GetFileNameWithoutExtension(model.File.FileName).UrlFriendly() + Path.GetExtension(model.File.FileName));

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await model.File.CopyToAsync(fileStream, cancellationToken);

            filePath = filePath.Replace(_hostingEnvironment.WebRootPath, string.Empty).Replace("\\", "/");

            var entity = _fileRepository.Add(new FileEntity
            {
                Name = model.File.Name,
                Extension = model.File.ContentType,
                Folder = model.Folder,
                Slug = filePath
            });

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return await GetDetailAsync(entity.Id, cancellationToken);
        }

        public async Task<FileModel> GetAsync(string slug, CancellationToken cancellationToken = default)
        {
            slug = slug?.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(slug))
            {
                return null;
            }

            var fileModel =
                await _fileRepository
                    .Get(x => x.Slug == slug)
                    .Select(x => _mapper.Map<FileModel>(x))
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            return fileModel;
        }

        public async Task<FileModel> GetDetailAsync(string id, CancellationToken cancellationToken = default)
        {
            var file = await _fileRepository.Get(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

            // Get attachment
            var result = new FileModel()
            {
                Id = file.Id,
                Slug = file.Slug,
                Name = file.Name,
                Extension = file.Extension
            };


            return result;
        }

        public async Task DeleteAsync(string slug, CancellationToken cancellationToken = default)
        {
            CheckSlugExist(slug);
            slug = slug?.Trim().ToLowerInvariant();

            var fileEntity =
                await _fileRepository
                    .Get(x => x.Slug == slug)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            if (fileEntity != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _fileRepository.Delete(fileEntity);

                // Save Change
                await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
            }
        }

        private void CheckSlugExist(string slug)
        {
            var isExisted = _fileRepository.Get().Any(x => x.Slug.Equals(slug));

            if (!isExisted)
            {
                throw new MoreThanBlogException(nameof(ErrorCode.FileNotExist), ErrorCode.FileNotExist);
            }
        }
    }
}