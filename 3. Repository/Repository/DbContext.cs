using Abstraction.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Configuration;

namespace Repository
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public readonly int CommandTimeoutInSecond = 20 * 60;

        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        #region DbSet

        public DbSet<UserEntity> User { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            var config =
                new ConfigurationBuilder()
                    .AddJsonFile("connectionconfig.json", false, true)
                    .Build();

            var connectionString =
                config.GetConnectionString("Development");

            optionsBuilder.UseSqlServer(connectionString);

            //optionsBuilder.EnableSensitiveDataLogging(EnvHelper.IsDevelopment());

            // Force all query is No Tracking to boost-up performance
            // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityConfiguration).Assembly);
        }
    }
}