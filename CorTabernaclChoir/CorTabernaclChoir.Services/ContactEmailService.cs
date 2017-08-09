using System;
using System.Collections.Generic;
using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class ContactEmailService : IEmailService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public ContactEmailService(Func<IUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public List<ContactEmail> GetAddresses()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<ContactEmail>().GetAll().ToList();
            }
        }

        public void AddAddress(ContactEmail email)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<ContactEmail>().Insert(email);
                uow.Commit();
            }
        }

        public void RemoveAddress(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<ContactEmail>().Delete(id);
                uow.Commit();
            }
        }
    }
}