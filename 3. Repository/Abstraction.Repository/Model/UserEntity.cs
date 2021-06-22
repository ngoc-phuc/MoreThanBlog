using System;
using System.Collections.Generic;

namespace Abstraction.Repository.Model
{
    public class UserEntity : MoreThanBlogEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarUrl { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTimeOffset PasswordLastUpdatedTime { get; set; }
        public string Phone { get; set; }

        public DateTimeOffset? EmailConfirmedTime { get; set; }

        public string EmailConfirmToken { get; set; }

        public DateTimeOffset? EmailConfirmTokenExpireTime { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<CategoryEntity> Categories { get; set; }

        public virtual ICollection<BlogEntity> Blogs { get; set; }
    }
}