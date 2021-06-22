using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Repository.Model;

namespace Abstraction.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get;}

        IRepository<T> GetRepository<T>() where T : MoreThanBlogEntity;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int ExecuteSqlRaw(string sql, params object[] parameters);

        IQueryable<T> FromSqlRaw<T>(string sql, params object[] parameters) where T : class;
    }
}