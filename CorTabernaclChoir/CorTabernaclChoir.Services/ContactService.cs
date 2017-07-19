using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class ContactService : IContactService
    {
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public ContactService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public ContactViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var model = uow.Repository<Contact>().GetSingle();

                return new ContactViewModel
                {
                    MainText = _cultureService.IsCurrentCultureWelsh()
                        ? model.MainText_W
                        : model.MainText_E
                };
            }
        }

        public Contact GetForEdit()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Contact>().GetSingle();
            }
        }

        public void Save(Contact model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Contact>().Update(model);

                uow.Commit();
            }
        }
    }
}