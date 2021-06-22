using System.Threading;
using System.Threading.Tasks;
using Core.Model.Blog;
using Core.Model.Common;

namespace Abstraction.Service.Blog
{
    public interface IBlogService
    {
        Task<string> CreateAsync(AddBlogModel model, CancellationToken cancellationToken = default);

        Task UpdateAsync(string id, AddBlogModel model, CancellationToken cancellationToken = default);

        Task<PagedResponseModel<BlogModel>> FilterAsync(FilterBlogRequestModel model,
            CancellationToken cancellationToken = default);

        Task<BlogModel> GetAsync(string id, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}