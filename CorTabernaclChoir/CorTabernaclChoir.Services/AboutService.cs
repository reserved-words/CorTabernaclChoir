using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class AboutService : IAboutService
    {
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public AboutService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public AboutViewModel Get()
        {
            var isCurrentCultureWelsh = _cultureService.IsCurrentCultureWelsh();

            using (var uow = _unitOfWorkFactory())
            {
                var model = uow.Repository<About>().GetSingle();

                var viewModel = new AboutViewModel
                {
                    AboutChoir = isCurrentCultureWelsh ? model.AboutChoir_W : model.AboutChoir_E,
                    AboutConductor = isCurrentCultureWelsh ? model.AboutMusicalDirector_W : model.AboutMusicalDirector_E,
                    AboutAccompanist = isCurrentCultureWelsh ? model.AboutAccompanist_W : model.AboutAccompanist_E
                };

                return viewModel;
            }
        }

        public About GetForEdit()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<About>().GetSingle();
            }
        }

        public void Save(About model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<About>().Update(model);

                uow.Commit();
            }
        }
    }
}