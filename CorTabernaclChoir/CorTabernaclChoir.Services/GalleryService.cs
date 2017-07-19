using System;
using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Common;

namespace CorTabernaclChoir.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public GalleryService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService)
        {
            _cultureService = cultureService;
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
                        .Select(im => new Image
                        {
                            Id = im.Id,
                            Caption = string.Format(Resources.GalleryImageCaption,
                                _cultureService.IsCurrentCultureWelsh()
                                    ? im.Caption_W
                                    : im.Caption_E,
                                im.Year)
                        })
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