using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class HomeService : IHomeService
    {
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public HomeService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public HomeViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var model = uow.Repository<Home>().GetSingle();

                var viewModel = new HomeViewModel
                {
                    MainText = _cultureService.IsCurrentCultureWelsh() ? model.MainText_W : model.MainText_E,
                    Conductor = model.Conductor,
                    Accompanist = model.Accompanist
                };

                return viewModel;
            }
        }

        public Home GetForEdit()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Home>().GetSingle();
            }
        }

        public void Save(Home model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Home>().Update(model);

                uow.Commit();
            }
        }
    }
}