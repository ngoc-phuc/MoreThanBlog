using System;

namespace Core.Model.Category
{
    public class CategoryModel
    {
        public string Id { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public bool IsActive { get; set; }

        public int BlogCount { get; set; }
    }
}