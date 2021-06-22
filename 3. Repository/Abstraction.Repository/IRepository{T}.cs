using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abstraction.Repository.Model;

namespace Abstraction.Repository
{
    public interface IRepository<TEntity> where TEntity : MoreThanBlogEntity
    {
        IQueryable<TEntity> Include(
            params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> predicate = null,
            bool isIncludeDeleted = false,
            params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Add(TEntity entity);

        List<TEntity> AddRange(params TEntity[] entities);

        void Update(
            TEntity entity,
            params Expression<Func<TEntity, object>>[] changedProperties);

        void Update(TEntity entity);

        void Delete(TEntity entity, bool isPhysicalDelete = false);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate, bool isPhysicalDelete = false);
    }
}