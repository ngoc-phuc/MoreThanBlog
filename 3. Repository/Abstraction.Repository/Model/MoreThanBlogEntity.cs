using System;

namespace Abstraction.Repository.Model
{
    public abstract class MoreThanBlogEntity
    {
        public string Id { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        public string DeletedBy { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }


        protected MoreThanBlogEntity()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
}