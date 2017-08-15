using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class AboutService : IAboutService
    {
        private readonly IMapper _mapper;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public AboutService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public AboutViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var model = uow.Repository<About>().GetSingle();

                return _mapper.Map<About, AboutViewModel>(model);
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