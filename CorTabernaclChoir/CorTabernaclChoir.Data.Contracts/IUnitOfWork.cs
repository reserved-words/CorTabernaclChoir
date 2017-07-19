using System;

namespace CorTabernaclChoir.Data.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;

        void Commit();
    }
}