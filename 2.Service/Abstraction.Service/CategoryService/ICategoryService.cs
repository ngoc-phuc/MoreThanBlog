using System.Threading;
using System.Threading.Tasks;
using Core.Model.Category;
using Core.Model.Common;

namespace Abstraction.Service.CategoryService
{
    public interface ICategoryService
    {
        Task<string> CreateAsync(AddCategoryModel model, CancellationToken cancellationToken = default);

        Task<PagedResponseModel<CategoryModel>> FilterAsync(FilterCategoryRequestModel model,
            CancellationToken cancellationToken = default);

        Task<CategoryModel> GetAsync(string id, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(string id, AddCategoryModel model, CancellationToken cancellationToken = default);
    }
}