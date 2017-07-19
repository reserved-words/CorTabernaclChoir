using CorTabernaclChoir.Data.Contracts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CorTabernaclChoir.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbContext Context;
        internal DbSet<TEntity> DbSet;

        public Repository(DbContext context)
        {
            Context = context;

            DbSet = context.Set<TEntity>();
        }
        
        public void Delete(int id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public TEntity GetSingle()
        {
            return DbSet.Single();
        }

        public IQueryable<TEntity> Including<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            return DbSet.Include(property);
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var dbEntity = Context.Entry(entity);

            if (dbEntity.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            dbEntity.State = EntityState.Modified;
        }
    }
}