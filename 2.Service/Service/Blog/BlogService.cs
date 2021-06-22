using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Repository;
using Abstraction.Repository.Model;
using Abstraction.Service.Blog;
using AutoMapper;
using Core.Errors;
using Core.Model.Blog;
using Core.Model.Common;
using Microsoft.EntityFrameworkCore;

namespace Service.Blog
{
    public class BlogService : BaseService.Service, IBlogService
    {
        private readonly IRepository<BlogEntity> _blogRepository;
        private readonly IRepository<BlogCategoryEntity> _blogCategoryRepository;

        public BlogService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _blogRepository = UnitOfWork.GetRepository<BlogEntity>();
            _blogCategoryRepository = UnitOfWork.GetRepository<BlogCategoryEntity>();
        }

        public async Task<string> CreateAsync(AddBlogModel model, CancellationToken cancellationToken = default)
        {
            CheckDuplicateTitle(model.Title);

            var entity = _mapper.Map<BlogEntity>(model);

            entity.BlogCategories = model.CategoryIds?.Select(x => new BlogCategoryEntity
            {
                CategoryId = x
            }).ToList();

            _blogRepository.Add(entity);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateAsync(string id, AddBlogModel model, CancellationToken cancellationToken = default)
        {
            CheckDuplicateTitle(model.Title, id);

            CheckExist(id);

            var entity = _mapper.Map<BlogEntity>(model);
            entity.Id = id;

            _blogRepository.Update(entity, x => x.Title, x => x.Content, x => x.Desc, 
                x => x.IsActive, x => x.MainImageId, x => x.ReadTime);

            _blogCategoryRepository.DeleteWhere(x => x.BlogId == id, true);

            _blogCategoryRepository.AddRange(model.CategoryIds.Select(x => new BlogCategoryEntity
            {
                BlogId = id,
                CategoryId = x
            }).ToArray());

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResponseModel<BlogModel>> FilterAsync(FilterBlogRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _blogRepository.Get();

            if (!string.IsNullOrWhiteSpace(model.Terms))
            {
                query = query.Where(x => x.Title.Contains(model.Terms) || x.Desc.Contains(model.Terms));
            }

            return new PagedResponseModel<BlogModel>
            {
                Total = await query.CountAsync(cancellationToken: cancellationToken),
                Items = await query
                    .Include(x => x.Creator)
                    .Include(x => x.MainImage)
                    .Include(x => x.BlogCategories)
                    .ThenInclude(x => x.Category).Skip(model.Skip)
                    .Take(model.Take)
                    .Select(x => _mapper.Map<BlogModel>(x))
                    .ToListAsync(cancellationToken: cancellationToken)
            };
        }

        public async Task<BlogModel> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _blogRepository.Get(x => x.Id == id)
                .Include(x => x.Creator)
                .Include(x => x.MainImage)
                .Include(x => x.BlogCategories)
                .ThenInclude(x => x.Category)
                .Select(x => _mapper.Map<BlogModel>(x))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            _blogRepository.DeleteWhere(x => x.Id == id);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        #region Utilities

        private void CheckDuplicateTitle(string title, string id = null)
        {
            var query = _blogRepository.Get(x => x.Title == title.Trim());

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(x => x.Id != id);
            }

            if (query.Any())
            {
                throw new MoreThanBlogException(nameof(ErrorCode.DuplicateTitle), ErrorCode.DuplicateTitle);
            }
        }

        private void CheckExist(string id)
        {
            if (!_blogRepository.Get(x => x.Id == id).Any())
            {
                throw new MoreThanBlogException(nameof(ErrorCode.BlogNotFound), ErrorCode.BlogNotFound);
            }
        }

        #endregion
    }
}