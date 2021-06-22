using Abstraction.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class FileEntityConfiguration : EntityConfiguration<FileEntity>
    {
        public override void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable("File");

            base.Configure(builder);

            builder.HasIndex(x => x.Slug);

            builder.HasIndex(x => x.Hash);
        }
    }
}