using System;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public SocialMediaService(Func<IUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Add(SocialMediaViewModel model, ImageFile logo)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<SocialMediaAccount>().Insert(new SocialMediaAccount
                {
                    Name = model.Name,
                    Url = model.Url,
                    ImageFile = logo
                });

                uow.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<SocialMediaAccount>().Delete(id);
                uow.Commit();
            }
        }

        public void Edit(SocialMediaViewModel model, ImageFile logo)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var entity = uow.Repository<SocialMediaAccount>().GetById(model.Id);

                entity.Name = model.Name;
                entity.Url = model.Url;

                if (logo != null)
                {
                    entity.ImageFile = logo;
                }
                
                uow.Commit();
            }
        }

        public SocialMediaViewModel Get(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var entity = uow.Repository<SocialMediaAccount>().GetById(id);

                return new SocialMediaViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Url = entity.Url,
                    ImageFileId = entity.ImageFileId
                };
            }
        }
    }
}
