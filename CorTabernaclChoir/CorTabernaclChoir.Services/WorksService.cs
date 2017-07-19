using System;
using System.Linq;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class WorksService : IWorksService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public WorksService(Func<IUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Save(Work model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                if (model.Id > 0)
                {
                    uow.Repository<Work>().Update(model);
                }
                else
                {
                    uow.Repository<Work>().Insert(model);
                }
                
                uow.Commit();
            }
        }

        public WorksViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var years = uow.Repository<Work>()
                    .GetAll()
                    .GroupBy(w => w.Year)
                    .Select(w => new YearList {
                        Year = w.Key,
                        Works = w.Select(wk => new ChoralWork { Id = wk.Id, Composer = wk.Composer, Title = wk.Title }).ToList()
                    })
                    .OrderBy(y => y.Year)
                    .ToList();
                
                return new WorksViewModel { Years = years };
            }
        }

        public Work GetForEdit(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Work>().GetById(id);
            }
        }

        public void Delete(Work model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Work>().Delete(model);

                uow.Commit();
            }
        }
    }
}