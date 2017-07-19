using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class JoinService : IJoinService
    {
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public JoinService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public JoinViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var model = uow.Repository<Join>().GetSingle();

                var viewModel = _cultureService.IsCurrentCultureWelsh()
                    ? new JoinViewModel
                    {
                        MainText = model.MainText_W,
                        RehearsalTimes = model.RehearsalTimes_W,
                        Location = model.Location_W,
                        Auditions = model.Auditions_W,
                        Concerts = model.Concerts_W,
                        Subscriptions = model.Subscriptions_W
                    }
                    : new JoinViewModel
                    {
                        MainText = model.MainText_E,
                        RehearsalTimes = model.RehearsalTimes_E,
                        Location = model.Location_E,
                        Auditions = model.Auditions_E,
                        Concerts = model.Concerts_E,
                        Subscriptions = model.Subscriptions_E
                    };

                return viewModel;
            }
        }

        public Join GetForEdit()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Join>().GetSingle();
            }
        }

        public void Save(Join model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Join>().Update(model);

                uow.Commit();
            }
        }
    }
}