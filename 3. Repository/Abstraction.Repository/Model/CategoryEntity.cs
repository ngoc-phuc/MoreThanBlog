using System.Collections.Generic;

namespace Abstraction.Repository.Model
{
    public class CategoryEntity : MoreThanBlogEntity
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public bool IsActive { get; set; }

        public virtual UserEntity Creator { get; set; }

        public virtual ICollection<BlogCategoryEntity> BlogCategories { get; set; }
    }
}