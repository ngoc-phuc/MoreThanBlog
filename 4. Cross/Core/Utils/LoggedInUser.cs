using System;

namespace Core.Utils
{
    public class LoggedInUser
    {
        public static LoggedInUser Current { get; set; }

        // Properties

        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsActive { get; set; }

        public string FullName => FirstName + " " + LastName;

        // Audit
        public DateTimeOffset CreatedTime { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}