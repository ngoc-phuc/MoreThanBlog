using Abstraction.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class BlogEntityConfiguration : EntityConfiguration<BlogEntity>
    {
        public override void Configure(EntityTypeBuilder<BlogEntity> builder)
        {
            builder.ToTable("Blog");
            base.Configure(builder);

            builder.HasOne(x => x.Creator)
                .WithMany(x => x.Blogs)
                .HasForeignKey(x => x.CreatedBy);

            builder.HasOne(x => x.MainImage)
                .WithMany(x => x.Blogs)
                .HasForeignKey(x => x.MainImageId);
        }
    }
}