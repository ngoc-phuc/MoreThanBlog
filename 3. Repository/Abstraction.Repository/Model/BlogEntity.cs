using System.Collections.Generic;

namespace Abstraction.Repository.Model
{
    public class BlogEntity : MoreThanBlogEntity
    {
        public string Title { get; set; }

        public string Desc { get; set; }

        public string Content { get; set; }

        public int ReadTime { get; set; } // minutes

        public string MainImageId { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<BlogCategoryEntity> BlogCategories { get; set; }

        public virtual FileEntity MainImage { get; set; }

        public virtual UserEntity Creator { get; set; }
    }
}