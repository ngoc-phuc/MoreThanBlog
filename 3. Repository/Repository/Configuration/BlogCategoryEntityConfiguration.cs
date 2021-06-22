using Abstraction.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class BlogCategoryEntityConfiguration : EntityConfiguration<BlogCategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<BlogCategoryEntity> builder)
        {
            builder.ToTable("BlogCategory");
            base.Configure(builder);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.BlogCategories)
                .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.Blog)
                .WithMany(x => x.BlogCategories)
                .HasForeignKey(x => x.BlogId);
        }
    }
}