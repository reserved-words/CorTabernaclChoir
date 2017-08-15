using System;
using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IMapper _mapper;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public GalleryService(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public GalleryViewModel Get()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return new GalleryViewModel
                {
                    Images = uow.Repository<GalleryImage>()
                        .GetAll()
                        .ToList()
                        .Select(im => _mapper.Map<GalleryImage,Image>(im))
                        .ToList()
                };
            }
        }

        public void Save(GalleryImage model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                if (model.Id > 0)
                {
                    uow.Repository<GalleryImage>().Update(model);
                }
                else
                {
                    uow.Repository<GalleryImage>().Insert(model);
                }

                uow.Commit();
            }
        }

        public GalleryImage GetForEdit(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<GalleryImage>().GetById(id);
            }
        }

        public void Delete(GalleryImage model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<GalleryImage>().Delete(model);

                uow.Commit();
            }
        }
    }
}