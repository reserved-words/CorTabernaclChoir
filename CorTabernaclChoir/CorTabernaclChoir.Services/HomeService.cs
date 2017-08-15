using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class HomeService : IHomeService
    {
        private readonly IMapper _mapper;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public HomeService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public HomeViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var model = uow.Repository<Home>().GetSingle();
                
                return _mapper.Map<Home,HomeViewModel>(model);
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