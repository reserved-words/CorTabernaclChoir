using CorTabernaclChoir.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorTabernaclChoir.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        private bool _disposed;

        public void Commit()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (!_repositories.Keys.Contains(typeof(TEntity)))
            {
                _repositories.Add(typeof(TEntity), new Repository<TEntity>(_context));
            }

            return _repositories[typeof(TEntity)] as IRepository<TEntity>;
        }
    }
}