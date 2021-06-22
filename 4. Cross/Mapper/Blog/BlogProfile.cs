using System.Linq;
using Abstraction.Repository.Model;
using AutoMapper;
using Core.Model.Blog;
using Core.Model.Category;

namespace Mapper.Blog
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<AddBlogModel, BlogEntity>().IgnoreAllNonExisting();

            CreateMap<BlogEntity, BlogModel>().IgnoreAllNonExisting()
                .ForMember(x => x.CreatedBy, y => y.MapFrom(z => $"{z.Creator.FirstName} {z.Creator.LastName}"))
                .ForMember(x => x.MainImageUrl, y => y.MapFrom(z => z.MainImage.Slug))
                .ForMember(x => x.Categories, y => y.MapFrom(z => z.BlogCategories.Where(x => x.DeletedTime == null)
                    .Select(x => new SortCategoryModel
                    {
                        Id = x.CategoryId,
                        Name = x.Category.Name
                    }).ToList()));
        }
    }
}