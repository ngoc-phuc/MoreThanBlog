using System.Collections.Generic;

namespace Core.Model.Blog
{
    public class AddBlogModel
    {
        public string Title { get; set; }

        public string Desc { get; set; }

        public string Content { get; set; }

        public int ReadTime { get; set; } // minutes

        public string MainImageId { get; set; }

        public bool IsActive { get; set; }

        public List<string> CategoryIds { get; set; }
    }
}