using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Repository;
using Abstraction.Repository.Model;
using Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private bool disposed;
        private Dictionary<Type, object> repositories;
        public Microsoft.EntityFrameworkCore.DbContext Context => _context;

        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this); // use to clean memory
        }

        public int ExecuteSqlRaw(string sql, params object[] parameters) => _context.Database.ExecuteSqlRaw(sql, parameters);

        public IQueryable<T> FromSqlRaw<T>(string sql, params object[] parameters) where T : class => _context.Set<T>().FromSqlRaw(sql, parameters);

        public IRepository<T> GetRepository<T>() where T : MoreThanBlogEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<T>(_context);
            }

            return (IRepository<T>)repositories[type];
        }

        public int SaveChanges()
        {
            try
            {
                StandardizeEntities();
                var result =  _context.SaveChanges();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message, ex);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                StandardizeEntities();
                var result = await _context.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message, ex);
            }
        }

        #region Helper

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }

                    // dispose the db context.
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        private void StandardizeEntities()
        {
            var listState = new List<EntityState>
            {
                EntityState.Added,
                EntityState.Modified,
                EntityState.Deleted
            };

            var listEntry = Context.ChangeTracker.Entries()
                .Where(x => x.Entity is MoreThanBlogEntity && listState.Contains(x.State))
                .Select(x => x).ToList();

            var dateTimeNow = DateTimeOffset.UtcNow;

            foreach (var entry in listEntry)
            {
                if (entry.Entity is MoreThanBlogEntity baseEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        baseEntity.DeletedTime = null;
                        baseEntity.LastUpdatedTime = baseEntity.CreatedTime = dateTimeNow;
                        baseEntity.CreatedBy = LoggedInUser.Current?.Id;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        baseEntity.LastUpdatedTime = dateTimeNow;
                        baseEntity.LastUpdatedBy = LoggedInUser.Current?.Id;
                    }
                    else
                    {
                        baseEntity.DeletedTime = dateTimeNow;
                        baseEntity.DeletedBy = LoggedInUser.Current?.Id;
                    }
                }
            }
        }

        #endregion
    }
}