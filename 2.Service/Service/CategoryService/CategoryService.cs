using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Repository;
using Abstraction.Repository.Model;
using Abstraction.Service.CategoryService;
using Core.Errors;
using Core.Model.Category;
using Core.Model.Common;
using Microsoft.EntityFrameworkCore;

namespace Service.CategoryService
{
    public class CategoryService : BaseService.Service, ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork,
            AutoMapper.IMapper mapper) : base(unitOfWork, mapper)
        {
            _categoryRepository = unitOfWork.GetRepository<CategoryEntity>();
        }

        public async Task<string> CreateAsync(AddCategoryModel model, CancellationToken cancellationToken = default)
        {
            CheckDuplicateName(model.Name);

            var entity = _mapper.Map<CategoryEntity>(model);

            _categoryRepository.Add(entity);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task<PagedResponseModel<CategoryModel>> FilterAsync(FilterCategoryRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _categoryRepository.Get();

            if (!string.IsNullOrWhiteSpace(model.Terms))
            {
                query = query.Where(x => x.Name.Contains(model.Terms) || x.Desc.Contains(model.Terms));
            }

            return new PagedResponseModel<CategoryModel>
            {
                Total = await query.CountAsync(cancellationToken: cancellationToken),
                Items = await _mapper.ProjectTo<CategoryModel>( query
                        .Skip(model.Skip)
                        .Take(model.Take)).ToListAsync(cancellationToken: cancellationToken)
            };
        }

        public async Task<CategoryModel> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _mapper.ProjectTo<CategoryModel>(
                    _categoryRepository.Get(x => x.Id == id))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            _categoryRepository.DeleteWhere(x => x.Id == id);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(string id, AddCategoryModel model, CancellationToken cancellationToken = default)
        {
           CheckDuplicateName(model.Name, id);

           _categoryRepository.Update(new CategoryEntity
           {
               Id = id,
               Name = model.Name,
               Desc = model.Desc,
               IsActive = model.IsActive
           }, x => x.Name, x => x.Desc, x => x.IsActive);

           await UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        #region Utilities

        private void CheckDuplicateName(string name, string id = null)
        {
            var query = _categoryRepository.Get(x => x.Name == name.Trim());

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(x => x.Id != id);
            }

            if (query.Any())
            {
                throw new MoreThanBlogException(nameof(ErrorCode.DuplicateName), ErrorCode.DuplicateName);
            }
        }

        #endregion
    }
}