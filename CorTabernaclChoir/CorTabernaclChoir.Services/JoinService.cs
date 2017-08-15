using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class JoinService : IJoinService
    {
        private readonly IMapper _mapper;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public JoinService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public JoinViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var model = uow.Repository<Join>().GetSingle();

                return _mapper.Map<Join,JoinViewModel>(model);
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