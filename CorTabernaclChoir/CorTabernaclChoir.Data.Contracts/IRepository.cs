using System;
using System.Linq;
using System.Linq.Expressions;

namespace CorTabernaclChoir.Data.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Including<TProperty>(Expression<Func<T, TProperty>> property);

        IQueryable<T> GetAll();

        T GetById(int id);

        T GetSingle();

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int id);
    }
}