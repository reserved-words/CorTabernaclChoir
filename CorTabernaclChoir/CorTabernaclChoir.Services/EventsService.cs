using System;
using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Common.Delegates;

namespace CorTabernaclChoir.Services
{
    public class EventsService : IEventsService
    {
        private readonly IMapper _mapper;
        private readonly ICultureService _cultureService;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly GetCurrentTime _getCurrentTime;
        private readonly int _itemsPerPage;

        public EventsService(Func<IUnitOfWork> unitOfWorkFactory, ICultureService cultureService, IAppSettingsService appSettingsService,
            IMapper mapper, GetCurrentTime getCurrentTime)
        {
            _cultureService = cultureService;
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
            _getCurrentTime = getCurrentTime;

            _itemsPerPage = appSettingsService.NumberOfItemsPerPage;
        }

        public EventsViewModel GetAll(int page)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var totalItems = uow.Repository<Event>()
                    .Including(n => n.PostImages)
                    .Count();

                var maximumPageNumber = (double)totalItems / _itemsPerPage;

                return new EventsViewModel
                {
                    PageNo = page,
                    Items = uow.Repository<Event>()
                        .Including(n => n.PostImages)
                        .OrderByDescending(n => n.Date)
                        .Skip((page - 1) * _itemsPerPage)
                        .Take(_itemsPerPage)
                        .Select(n => _mapper.Map<Event,EventViewModel>(n))
                        .ToList(),
                    PreviousPage = page == 1 ? (int?)null : page - 1,
                    NextPage = (page >= maximumPageNumber) ? (int?)null : (page + 1)
                };
            }
        }

        public EventViewModel GetById(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var entity = uow.Repository<Event>()
                    .Including(e => e.PostImages)
                    .Single(e => e.Id == id);

                return _mapper.Map<Event, EventViewModel>(entity);
            }
        }

        public int Save(EditEventViewModel model)
        {
            var post = _mapper.Map<EditEventViewModel, Event>(model);

            using (var uow = _unitOfWorkFactory())
            {
                if (model.Id > 0)
                {
                    uow.Repository<Event>().Update(post);

                    foreach (var image in model.PostImages.Where(im => im.MarkForDeletion))
                    {
                        uow.Repository<PostImage>().Delete(image.Id);
                    }
                }
                else
                {
                    model.Published = _getCurrentTime();
                    uow.Repository<Event>().Insert(post);
                }

                uow.Commit();

                return model.Id;
            }
        }

        public EditEventViewModel GetForEdit(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                throw new NotImplementedException();
            }
        }

        public void Delete(Event model)
        {
            using (var uow = _unitOfWorkFactory())
            {
                uow.Repository<Event>().Delete(model);

                uow.Commit();
            }
        }

        public int SaveImage(int postId, string fileExtension)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var postImage = new PostImage
                {
                    PostId = postId,
                    FileExtension = fileExtension
                };

                uow.Repository<PostImage>()
                    .Insert(postImage);

                uow.Commit();

                return postImage.Id;
            }
        }

        public void DeleteImage(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var image = uow.Repository<PostImage>().GetById(id);

                image.Post.PostImages.Remove(image);

                uow.Repository<PostImage>().Delete(id);

                uow.Commit();
            }
        }
    }
}