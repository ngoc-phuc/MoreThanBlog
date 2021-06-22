using System.Collections.Generic;

namespace Abstraction.Repository.Model
{
    public class FileEntity : MoreThanBlogEntity
    {
        public string Slug { get; set; }

        public string Folder { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string MimeType { get; set; }

        public long ContentLength { get; set; }

        public string Hash { get; set; }

        public long? EntityRecordId { get; set; }

        // Image

        public bool IsImage { get; set; }

        public bool IsCompressedImage { get; set; }

        public string ImageDominantHexColor { get; set; }

        public int ImageWidthPx { get; set; } = -1;

        public int ImageHeightPx { get; set; } = -1;

        public virtual ICollection<BlogEntity> Blogs { get; set; }
    }
}