using System;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class ImageService : IImageService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public ImageService(Func<IUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public ImageFile Get(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<ImageFile>().GetById(id);
            }
        }
    }
}