using System.Linq;
using Abstraction.Repository.Model;
using AutoMapper;
using Core.Model.Category;

namespace Mapper.Category
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddCategoryModel, CategoryEntity>().IgnoreAllNonExisting();

            CreateMap<CategoryEntity, CategoryModel>().IgnoreAllNonExisting()
                .ForMember(x => x.CreatedBy, y => y.MapFrom(z => $"{z.Creator.FirstName} {z.Creator.LastName}"))
                .ForMember(x => x.BlogCount, y => y.MapFrom(z => z.BlogCategories.Count(x => x.DeletedTime == null)));
        }
    }
}