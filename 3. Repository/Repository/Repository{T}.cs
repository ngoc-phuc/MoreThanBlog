using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Abstraction.Repository;
using Abstraction.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : MoreThanBlogEntity
    {
        public Microsoft.EntityFrameworkCore.DbContext Context;

        public virtual DbSet<T> DbSet => Context.Set<T>();

        public Repository(Microsoft.EntityFrameworkCore.DbContext context)
        {
            Context = context;
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = DbSet.AsNoTracking();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);

            }
            return query;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, bool isIncludeDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = DbSet.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            includeProperties = includeProperties?.Distinct().ToArray();

            if (includeProperties?.Any() == true)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            // NOTE: Query Filter (query.IgnoreQueryFilters()), it affect to load data business logic.
            // Currently not flexible, please check https://github.com/aspnet/EntityFrameworkCore/issues/8576
            query = isIncludeDeleted ? query.IgnoreQueryFilters() : query.Where(x => x.DeletedTime == null);

            return query;
        }

        public T Add(T entity)
        {
            entity.DeletedTime = null;

            entity.LastUpdatedTime = entity.CreatedTime = DateTimeOffset.UtcNow;

            entity = DbSet.Add(entity).Entity;

            return entity;
        }

        public List<T> AddRange(params T[] entities)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;

            List<T> listAddedEntity = new List<T>();

            foreach (var entity in entities)
            {
                entity.CreatedTime = dateTimeUtcNow;

                var addedEntity = Add(entity);

                listAddedEntity.Add(addedEntity);
            }

            return listAddedEntity;
        }

        public void Update(T entity, params Expression<Func<T, object>>[] changedProperties)
        {
            TryAttach(entity);

            changedProperties = changedProperties?.Distinct().ToArray();

            entity.LastUpdatedTime = DateTimeOffset.UtcNow;

            if (changedProperties?.Any() == true)
            {
                Context.Entry(entity).Property(x => x.LastUpdatedTime).IsModified = true;

                foreach (var property in changedProperties)
                {
                    Context.Entry(entity).Property(property).IsModified = true;
                }
            }
            else
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Update(T entity)
        {
            TryAttach(entity);

            entity.LastUpdatedTime = DateTimeOffset.UtcNow;

            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity, bool isPhysicalDelete = false)
        {
            TryAttach(entity);

            if (!isPhysicalDelete)
            {
                entity.DeletedTime = DateTimeOffset.UtcNow;

                Context.Entry(entity).Property(x => x.DeletedTime).IsModified = true;
            }
            else
            {
                DbSet.Remove(entity);
            }
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate, bool isPhysicalDelete = false)
        {
            var entities = Get(predicate, isPhysicalDelete).AsEnumerable();

            foreach (var entity in entities)
            {
                Delete(entity, isPhysicalDelete);
            }
        }

        #region Helper

        protected void TryAttach(T entity)
        {
            try
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    DbSet.Attach(entity);
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion
    }
}
