namespace Abstraction.Repository.Model
{
    public class BlogCategoryEntity : MoreThanBlogEntity
    {
        public string BlogId { get; set; }

        public string CategoryId { get; set; }

        public virtual CategoryEntity Category { get; set; }

        public virtual BlogEntity Blog { get; set; }
    }
}