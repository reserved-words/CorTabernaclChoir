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

        public void Add(SocialMediaViewModel model, byte[] logo)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<SocialMediaAccount>().Insert(new SocialMediaAccount
                {
                    Name = model.Name,
                    Url = model.Url,
                    ImageFile = new ImageFile { File = logo }
                });

                uow.Commit();
            }
        }
    }
}
