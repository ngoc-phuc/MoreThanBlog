using Abstraction.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class CategoryEntityConfiguration : EntityConfiguration<CategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Category");
            base.Configure(builder);

            builder.HasOne(x => x.Creator)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.CreatedBy);
        }
    }
}