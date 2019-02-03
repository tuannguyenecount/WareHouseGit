using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Warehouse.Core.Entities;
namespace Warehouse.Core.DataAccess.EntityFramework
{
    /// <summary>
    /// EntityRepositoryBase
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>

    public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public virtual void Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return filter != null ?  context.Set<TEntity>().FirstOrDefault(filter) : context.Set<TEntity>().FirstOrDefault();
            }
        }

        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return filter != null ?  context.Set<TEntity>().SingleOrDefault(filter) : context.Set<TEntity>().SingleOrDefault();
            }
        }

        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public virtual void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return filter != null ? context.Set<TEntity>().Count(filter) : context.Set<TEntity>().Count();
            }
        }

    }
}
